using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Core;

namespace Koben.Kompair.Services
{
	public class KompairContentTypesService : IKompairContentTypesService
	{
		public IEnumerable<IContentType> GetAllContentTypes(params int[] ids)
		{
			var contentTypeService = ApplicationContext.Current.Services.ContentTypeService;

			return contentTypeService.GetAllContentTypes(ids);
		}
	}
}
