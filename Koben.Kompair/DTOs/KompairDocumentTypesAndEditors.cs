using System.Collections.Generic;
using Koben.Kompair.Models;

namespace Koben.Kompair.DTOs
{
	public class KompairDocumentTypesAndEditors
	{
		public Dictionary<string, KompairDocumentType> DocumentTypes { get; set; } = new Dictionary<string, KompairDocumentType>();
		public Dictionary<string, KompairPropertyEditor> PropertyEditors { get; set; } = new Dictionary<string, KompairPropertyEditor>();
	}
}
