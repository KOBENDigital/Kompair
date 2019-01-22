using Koben.Kompair.DTOs;

namespace Koben.Kompair.Services
{
	public interface IKompairDataService
	{
		KompairDocumentTypesAndEditors GetDocumentTypesForComparison();
	}
}