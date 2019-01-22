using System;
using Umbraco.Core.Models;

namespace Koben.SanityCheck.Services
{
	public interface IKompairDataTypesService
	{
		IDataTypeDefinition GetDataTypeDefinitionById(int id);
	}
}
