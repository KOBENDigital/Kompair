using System.Linq;
using Koben.Kompair.DTOs;
using Koben.Kompair.Models;

namespace Koben.Kompair.Services
{
	public class KompairService : IKompairService
	{
		public KompairResults Compare(KompairDocumentTypesAndEditors source, KompairDocumentTypesAndEditors target)
		{
			AreEqual(source, target);
			AreEqual(target, source);

			return new KompairResults(source, target);
		}

		public bool AreEqual(KompairDocumentTypesAndEditors source, KompairDocumentTypesAndEditors target)
		{
			bool equal = true;

			foreach (var sourceDocumentType in source.DocumentTypes)
			{
				if (!target.DocumentTypes.TryGetValue(sourceDocumentType.Key, out KompairDocumentType targetDocumentType))
				{
					sourceDocumentType.Value.MatchStatus = MatchStatus.None;
					equal = false;

					foreach (var propertyGroup in sourceDocumentType.Value.Groups)
					{
						propertyGroup.MatchStatus = MatchStatus.None;

						foreach (var property in propertyGroup.Properties)
						{
							property.MatchStatus = MatchStatus.None;
						}
					}

					continue;
				}

				MatchStatus documentTypeMatchStatus = MatchStatus.Complete;

				foreach (var sourceGroup in sourceDocumentType.Value.Groups)
				{
					KompairPropertyGroup targetGroup = targetDocumentType.Groups.FirstOrDefault(g => g.Name == sourceGroup.Name);

					if (targetGroup == null)
					{
						sourceGroup.MatchStatus = MatchStatus.None;
						documentTypeMatchStatus = MatchStatus.Partial;
						equal = false;
						continue;
					}

					MatchStatus propertyGroupMatchStatus = MatchStatus.Complete;

					if (!sourceGroup.Equals(targetGroup))
					{
						propertyGroupMatchStatus = MatchStatus.Partial;
						documentTypeMatchStatus = MatchStatus.Partial;
						equal = false;
					}

					foreach (var sourceProperty in sourceGroup.Properties)
					{
						KompairProperty targetProperty =
							targetGroup.Properties.FirstOrDefault(p => p.Alias == sourceProperty.Alias);

						if (targetProperty == null)
						{
							sourceProperty.MatchStatus = MatchStatus.None;
							documentTypeMatchStatus = MatchStatus.Partial;
							propertyGroupMatchStatus = MatchStatus.Partial;
							equal = false;
							continue;
						}

						if (!sourceProperty.Equals(targetProperty))
						{
							sourceProperty.MatchStatus = MatchStatus.Partial;
							documentTypeMatchStatus = MatchStatus.Partial;
							propertyGroupMatchStatus = MatchStatus.Partial;
							equal = false;
							continue;
						}

						sourceProperty.MatchStatus = MatchStatus.Complete;
					}

					sourceGroup.MatchStatus = propertyGroupMatchStatus;
				}

				sourceDocumentType.Value.MatchStatus = documentTypeMatchStatus;
			}

			foreach (var sourcePropertyEditor in source.PropertyEditors)
			{
				if (!target.PropertyEditors.TryGetValue(sourcePropertyEditor.Key, out KompairPropertyEditor targetPropertyEditor))
				{
					sourcePropertyEditor.Value.MatchStatus = MatchStatus.None;
					equal = false;
					continue;
				}

				sourcePropertyEditor.Value.MatchStatus = sourcePropertyEditor.Value.Equals(targetPropertyEditor) ? MatchStatus.Complete : MatchStatus.Partial;
			}

			return equal;
		}
	}
}