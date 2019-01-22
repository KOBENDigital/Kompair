using System.Collections.Generic;
using System.Linq;
using Koben.Kompair.DTOs;
using Koben.Kompair.Models;

namespace Koben.Kompair
{
	public class KompairResults
	{
		public ICollection<KompairDocumentType> SourceDocumentTypes { get; }
		public ICollection<KompairDocumentType> TargetDocumentTypes { get; }
		public ICollection<KompairPropertyGroup> SourcePropertyGroups { get; }
		public ICollection<KompairPropertyGroup> TargetPropertyGroups { get; }
		public ICollection<KompairProperty> SourceProperties { get; }
		public ICollection<KompairProperty> TargetProperties { get; }
		public ICollection<KompairPropertyEditor> SourcePropertyEditors { get; }
		public ICollection<KompairPropertyEditor> TargetPropertyEditors { get; }

		public KompairResults(KompairDocumentTypesAndEditors source, KompairDocumentTypesAndEditors target)
		{
			SourceDocumentTypes = source.DocumentTypes.Values.ToList();
			TargetDocumentTypes = target.DocumentTypes.Values.ToList();
			SourcePropertyGroups = source.DocumentTypes.Values.SelectMany(v => v.Groups).ToList();
			TargetPropertyGroups = target.DocumentTypes.Values.SelectMany(v => v.Groups).ToList();
			SourceProperties = source.DocumentTypes.Values.SelectMany(v => v.Groups.SelectMany(g => g.Properties)).ToList();
			TargetProperties = target.DocumentTypes.Values.SelectMany(v => v.Groups.SelectMany(g => g.Properties)).ToList();
			SourcePropertyEditors = source.PropertyEditors.Values.ToList();
			TargetPropertyEditors = target.PropertyEditors.Values.ToList();
		}
	}
}