using System;
using System.Threading.Tasks;
using Koben.SanityCheck.DTOs;

namespace Koben.SanityCheck.Services
{
	public interface IKompairHttpService
	{
		Task<KompairDocumentTypesAndEditors> GetTargetDocumentTypes(Uri targetUrl);
	}
}
