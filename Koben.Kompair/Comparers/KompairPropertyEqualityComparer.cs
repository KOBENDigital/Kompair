using System.Collections.Generic;
using Koben.Kompair.Models;

namespace Koben.Kompair.Comparers
{
	public sealed class KompairPropertyEqualityComparer : IEqualityComparer<KompairProperty>
	{
		public bool Equals(KompairProperty x, KompairProperty y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (ReferenceEquals(x, null)) return false;
			if (ReferenceEquals(y, null)) return false;
			if (x.GetType() != y.GetType()) return false;
			return x.Equals(y);
		}

		public int GetHashCode(KompairProperty obj)
		{
			return obj.GetHashCode();
		}
	}
}