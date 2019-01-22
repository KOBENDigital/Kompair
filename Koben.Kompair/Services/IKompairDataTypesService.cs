using System;
using Umbraco.Core.Models;

namespace Koben.Kompair.Services
{
	public interface IKompairDataTypesService
	{
		IDataTypeDefinition GetDataTypeDefinitionById(int id);
	}
}
