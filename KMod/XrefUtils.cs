using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MelonLoader;
using UnhollowerRuntimeLib.XrefScans;

namespace KMod
{
	public static class XrefUtils
	{
		public static bool CheckMethod(MethodInfo method, string match)
		{
			try
			{
				foreach (XrefInstance item in XrefScanner.XrefScan(method))
				{
					if (item.Type == XrefType.Global && item.ReadAsObject().ToString().Contains(match))
					{
						return true;
					}
				}
				return false;
			}
			catch
			{
			}
			return false;
		}

		public static bool CheckUsedBy(MethodInfo method, string methodName, Type type = null)
		{
			foreach (XrefInstance item in XrefScanner.UsedBy(method))
			{
				if (item.Type != XrefType.Method)
				{
					continue;
				}
				try
				{
					if ((type == null || item.TryResolve().DeclaringType == type) && item.TryResolve().Name.Contains(methodName))
					{
						return true;
					}
				}
				catch
				{
				}
			}
			return false;
		}

		public static bool CheckUsing(MethodInfo method, string methodName, Type type = null)
		{
			foreach (XrefInstance item in XrefScanner.XrefScan(method))
			{
				if (item.Type != XrefType.Method)
				{
					continue;
				}
				try
				{
					if ((type == null || item.TryResolve().DeclaringType == type) && item.TryResolve().Name.Contains(methodName))
					{
						return true;
					}
				}
				catch
				{
				}
			}
			return false;
		}

	
		public static void DumpXRefs(Type type)
		{
			MelonLogger.Msg(type.Name + " XRefs:");
			foreach (MethodInfo declaredMethod in AccessTools.GetDeclaredMethods(type))
			{
				DumpXRefs(declaredMethod, 1);
			}
		}

	
		public static void DumpXRefs(MethodInfo method, int depth = 0)
		{
			string text = new string('\t', depth);
			MelonLogger.Msg(text + method.Name + " XRefs:");
			foreach (XrefInstance item in XrefScanner.XrefScan(method))
			{
				if (item.Type == XrefType.Global)
				{
					MelonLogger.Msg("\tString = " + item.ReadAsObject());
					continue;
				}
				MethodBase methodBase = item.TryResolve();
				if (methodBase != null)
				{
					MelonLogger.Msg(text + "\tMethod -> " + methodBase.DeclaringType?.Name + "." + methodBase.Name);
				}
			}
		}

	
		internal static bool XRefScanForMethod(MethodBase methodBase, string methodName = null, string reflectedType = null)
		{
			bool flag = false;
			foreach (XrefInstance item in XrefScanner.XrefScan(methodBase))
			{
				if (item.Type != XrefType.Method)
				{
					continue;
				}
				MethodBase methodBase2 = item.TryResolve();
				if (!(methodBase2 == null))
				{
					if (!string.IsNullOrEmpty(methodName))
					{
						flag = !string.IsNullOrEmpty(methodBase2.Name) && methodBase2.Name.IndexOf(methodName, StringComparison.OrdinalIgnoreCase) >= 0;
					}
					if (!string.IsNullOrEmpty(reflectedType))
					{
						flag = !string.IsNullOrEmpty(methodBase2.ReflectedType?.Name) && methodBase2.ReflectedType.Name.IndexOf(reflectedType, StringComparison.OrdinalIgnoreCase) >= 0;
					}
					if (flag)
					{
						return true;
					}
				}
			}
			return false;
		}

	
		internal static int XRefCount(MethodBase methodBase)
		{
			int num = 0;
			foreach (XrefInstance item in XrefScanner.XrefScan(methodBase))
			{
				if (item.Type == XrefType.Method)
				{
					MethodBase methodBase2 = item.TryResolve();
					if (!(methodBase2 == null))
					{
						num++;
					}
				}
			}
			return num;
		}
	}
}
