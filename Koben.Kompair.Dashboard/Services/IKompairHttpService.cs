using System;
using System.Threading.Tasks;
using Koben.Kompair.DTOs;

namespace Koben.Kompair.Dashboard.Services
{
	public interface IKompairHttpService
	{
		Task<KompairDocumentTypesAndEditors> GetTargetDocumentTypes(Uri targetUrl);
	}
}
