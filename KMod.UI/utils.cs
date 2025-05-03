using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KMod.UI
{
	internal static class utils
	{
	
		public static void DestroyChildren(Transform transform)
		{
			DestroyChildren(transform, null);
		}

	
		public static void DestroyChildren(Transform transform, Func<Transform, bool> exclude)
		{
			for (int num = transform.childCount - 1; num >= 0; num--)
			{
				if (exclude == null || exclude(transform.GetChild(num)))
				{
					UnityEngine.Object.DestroyImmediate(transform.GetChild(num).gameObject);
				}
			}
		}
	}
}
