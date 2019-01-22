using System;

namespace Koben.Kompair
{
	public static class EnumExtensions
	{
		public static bool TryParse<T>(this string value, bool ignoreCase, out T parsedEnum)
			where T : struct, IComparable, IFormattable, IConvertible
		{
			return Enum.TryParse(value, ignoreCase, out parsedEnum);
		}
	}
}