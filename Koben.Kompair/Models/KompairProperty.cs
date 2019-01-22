using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Koben.SanityCheck.Models
{
	public class KompairProperty : IEquatable<KompairProperty>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Alias { get; set; }
		public string PropertyGroupName { get; set; }
		public string DocumentTypeAlias { get; set; }
		public KompairDataType DataType { get; set; }
		public int SortOrder { get; set; }
		public KompairPropertyEditor Editor { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public MatchStatus MatchStatus { get; set; }

		public bool Equals(KompairProperty other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return string.Equals(Name, other.Name) && string.Equals(Alias, other.Alias) && DataType.Equals(other.DataType) && SortOrder == other.SortOrder && Editor.Equals(other.Editor);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((KompairProperty) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (Name != null ? Name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Alias != null ? Alias.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (DataType != null ? DataType.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ SortOrder;
				hashCode = (hashCode * 397) ^ (Editor != null ? Editor.GetHashCode() : 0);
				return hashCode;
			}
		}
	}
}