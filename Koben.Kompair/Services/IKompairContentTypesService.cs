using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Koben.Kompair.Services
{
	public interface IKompairContentTypesService
	{
		IEnumerable<IContentType> GetAllContentTypes(params int[] ids);
	}
}
