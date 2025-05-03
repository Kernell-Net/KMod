using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace KMod
{
	internal static class AssemblyExtensions
	{
	
		public static IEnumerable<Type> TryGetTypes(Assembly asm)
		{
			try
			{
				return asm.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				try
				{
					return asm.GetExportedTypes();
				}
				catch
				{
					return Enumerable.Where(ex.Types, (Type t) => t != null);
				}
			}
			catch
			{
				return Enumerable.Empty<Type>();
			}
		}
	}
}
