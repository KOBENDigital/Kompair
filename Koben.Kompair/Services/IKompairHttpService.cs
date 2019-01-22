using System;
using System.Threading.Tasks;
using Koben.Kompair.DTOs;

namespace Koben.Kompair.Services
{
	public interface IKompairHttpService
	{
		Task<KompairDocumentTypesAndEditors> GetTargetDocumentTypes(Uri targetUrl);
	}
}
