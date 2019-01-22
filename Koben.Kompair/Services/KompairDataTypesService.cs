using Umbraco.Core;
using Umbraco.Core.Models;

namespace Koben.Kompair.Services
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
