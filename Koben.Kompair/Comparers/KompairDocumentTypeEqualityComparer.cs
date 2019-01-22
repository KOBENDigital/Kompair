using System.Collections.Generic;
using Koben.SanityCheck.Models;

namespace Koben.SanityCheck.Comparers
{
	public sealed class KompairDocumentTypeEqualityComparer : IEqualityComparer<KompairDocumentType>
	{
		public bool Equals(KompairDocumentType x, KompairDocumentType y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (ReferenceEquals(x, null)) return false;
			if (ReferenceEquals(y, null)) return false;
			if (x.GetType() != y.GetType()) return false;
			return x.Equals(y);
		}

		public int GetHashCode(KompairDocumentType obj)
		{
			return obj.GetHashCode();
		}
	}
}