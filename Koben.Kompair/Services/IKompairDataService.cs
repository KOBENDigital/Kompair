using Koben.SanityCheck.DTOs;

namespace Koben.SanityCheck.Services
{
	public interface IKompairDataService
	{
		KompairDocumentTypesAndEditors GetDocumentTypesForComparison();
	}
}