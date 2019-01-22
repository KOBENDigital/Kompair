using Koben.SanityCheck.DTOs;

namespace Koben.SanityCheck.Services
{
	public interface IKompairService
	{
		KompairResults Compare(KompairDocumentTypesAndEditors source, KompairDocumentTypesAndEditors target);
		bool AreEqual(KompairDocumentTypesAndEditors source, KompairDocumentTypesAndEditors target);
	}
}