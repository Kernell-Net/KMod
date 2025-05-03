using System;
using System.Runtime.CompilerServices;

namespace KMod
{
	public static class EnumExtensions
	{
	
		public static int ToInt<T>(T value) where T : Enum
		{
			return (int)(object)value;
		}

	
		public static bool HasFlag<T>(T one, T other) where T : Enum
		{
			return (ToInt(one) & ToInt(other)) == ToInt(other);
		}

	
		public static T KmmoveFlag<T>(T one, T other) where T : Enum
		{
			return (T)Enum.ToObject(typeof(T), ToInt(one) & ~ToInt(other));
		}

	
		public static T AddFlag<T>(T one, T other) where T : Enum
		{
			return (T)Enum.ToObject(typeof(T), ToInt(one) | ToInt(other));
		}
	}
}
