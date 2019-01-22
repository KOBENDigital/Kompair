using System;
using Koben.SanityCheck.DTOs;
using Koben.SanityCheck.Models;

namespace Koben.SanityCheck.Services
{
	public class KompairDataService : IKompairDataService
	{
		private IKompairContentTypesService _ContentTypesService;
		private IKompairDataTypesService _DataTypesService;

		public KompairDataService(IKompairContentTypesService contentTypesService, IKompairDataTypesService dataTypesService)
		{
			_ContentTypesService = contentTypesService ?? throw new ArgumentNullException(nameof(contentTypesService));
			_DataTypesService = dataTypesService ?? throw new ArgumentNullException(nameof(dataTypesService));
		}

		public KompairDocumentTypesAndEditors GetDocumentTypesForComparison()
		{
			var allTypes = _ContentTypesService.GetAllContentTypes();

			var typesAndEditors = new KompairDocumentTypesAndEditors();

			foreach (var type in allTypes)
			{
				KompairDocumentType documentType = new KompairDocumentType
				{
					Id = type.Id,
					ParentId = type.ParentId,
					Alias = type.Alias
				};

				foreach (var typeGroup in type.PropertyGroups)
				{
					KompairPropertyGroup group = new KompairPropertyGroup
					{
						Id = typeGroup.Id,
						DocumentTypeAlias = documentType.Alias,
						Name = typeGroup.Name,
						SortOrder = typeGroup.SortOrder
					};

					foreach (var groupProperty in typeGroup.PropertyTypes)
					{
						var dataType = _DataTypesService.GetDataTypeDefinitionById(groupProperty.DataTypeDefinitionId);

						if (!typesAndEditors.PropertyEditors.TryGetValue(groupProperty.PropertyEditorAlias, out KompairPropertyEditor editor))
						{
							editor = new KompairPropertyEditor
							{
								Alias = groupProperty.PropertyEditorAlias
							};

							typesAndEditors.PropertyEditors.Add(editor.Alias, editor);
						}

						KompairProperty property = new KompairProperty
						{
							Id = groupProperty.Id,
							Alias = groupProperty.Alias,
							Name = groupProperty.Name,
							PropertyGroupName = group.Name,
							DocumentTypeAlias = documentType.Alias,
							SortOrder = groupProperty.SortOrder,
							DataType = new KompairDataType
							{
								Id = groupProperty.DataTypeDefinitionId,
								Name = dataType.Name,
								ValueType = dataType.DatabaseType.ToString()
							},
							Editor = editor
						};

						group.Properties.Add(property);
					}

					documentType.Groups.Add(group);
				}

				typesAndEditors.DocumentTypes.Add(documentType.Alias, documentType);
			}

			return typesAndEditors;
		}
	}
}