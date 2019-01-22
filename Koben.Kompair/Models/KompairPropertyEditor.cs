using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Koben.SanityCheck.Models
{
	public class KompairPropertyEditor : IEquatable<KompairPropertyEditor>
	{
		public string Alias { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public MatchStatus MatchStatus { get; set; }

		public bool Equals(KompairPropertyEditor other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return string.Equals(Alias, other.Alias);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((KompairPropertyEditor) obj);
		}

		public override int GetHashCode()
		{
			return (Alias != null ? Alias.GetHashCode() : 0);
		}
	}
}