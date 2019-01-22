using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Koben.SanityCheck.Models
{
	public class KompairPropertyGroup : IEquatable<KompairPropertyGroup>
	{
		public int Id { get; set; }
		public string DocumentTypeAlias { get; set; }
		public string Name { get; set; }
		public int SortOrder { get; set; }
		public ICollection<KompairProperty> Properties { get; set; } = new List<KompairProperty>();
		[JsonConverter(typeof(StringEnumConverter))]
		public MatchStatus MatchStatus { get; set; }

		public bool Equals(KompairPropertyGroup other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return string.Equals(Name, other.Name) && SortOrder == other.SortOrder;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((KompairPropertyGroup) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (Name != null ? Name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ SortOrder;
				hashCode = (hashCode * 397) ^ (Properties != null ? Properties.GetHashCode() : 0);
				return hashCode;
			}
		}
	}
}