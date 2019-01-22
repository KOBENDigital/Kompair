using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Koben.Kompair.Models
{
	public class KompairDocumentType : IEquatable<KompairDocumentType>
	{
		public int ParentId { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string Alias { get; set; }
		public ICollection<KompairPropertyGroup> Groups { get; set; } = new List<KompairPropertyGroup>();
		[JsonConverter(typeof(StringEnumConverter))]
		public MatchStatus MatchStatus { get; set; }

		public bool Equals(KompairDocumentType other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return string.Equals(Name, other.Name) && string.Equals(Alias, other.Alias);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((KompairDocumentType) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (Name != null ? Name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Alias != null ? Alias.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Groups != null ? Groups.GetHashCode() : 0);
				return hashCode;
			}
		}
	}
}
