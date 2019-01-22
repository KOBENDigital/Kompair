using Umbraco.Core;
using Umbraco.Core.Models;

namespace Koben.SanityCheck.Services
{
	public class KompairDataTypesService : IKompairDataTypesService
	{
		public IDataTypeDefinition GetDataTypeDefinitionById(int id)
		{
			var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
			return dataTypeService.GetDataTypeDefinitionById(id);
		}
	}
}
