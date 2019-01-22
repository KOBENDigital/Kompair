using System;

namespace Koben.SanityCheck.Models
{
	public class KompairDataType : IEquatable<KompairDataType>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ValueType { get; set; }

		public bool Equals(KompairDataType other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return string.Equals(Name, other.Name) && string.Equals(ValueType, other.ValueType);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((KompairDataType) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (ValueType != null ? ValueType.GetHashCode() : 0);
			}
		}
	}
}