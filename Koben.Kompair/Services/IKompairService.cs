using Koben.Kompair.DTOs;

namespace Koben.Kompair.Services
{
	public interface IKompairService
	{
		KompairResults Compare(KompairDocumentTypesAndEditors source, KompairDocumentTypesAndEditors target);
		bool AreEqual(KompairDocumentTypesAndEditors source, KompairDocumentTypesAndEditors target);
	}
}