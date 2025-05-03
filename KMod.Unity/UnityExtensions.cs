using System;
using System.Runtime.CompilerServices;
using Il2CppSystem;
using Il2CppSystem.Collections;
using UnityEngine;

namespace KMod.Unity
{
	public static class UnityExtensions
	{
		public const float MaxAllowedValueTop = 34028230f;

		public const float MaxAllowedValueBottom = -34028230f;

	
		public static string GetPath(Transform current)
		{
			if (current.parent == null)
			{
				return "/" + current.name;
			}
			return GetPath(current.parent) + "/" + current.name;
		}

	
		public static T[] GetComponentsInDirectChildren<T>(GameObject gameObject)
		{
			int num = 0;
			IEnumerator enumerator = gameObject.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Il2CppSystem.Object current = enumerator.Current;
					Transform transform = current.Cast<Transform>();
					if (transform.GetComponent<T>() != null)
					{
						num++;
					}
				}
			}
			finally
			{
				if (enumerator is System.IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
			T[] array = new T[num];
			num = 0;
			IEnumerator enumerator2 = gameObject.transform.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Il2CppSystem.Object current2 = enumerator2.Current;
					Transform transform2 = current2.Cast<Transform>();
					if (transform2.GetComponent<T>() != null)
					{
						array[num++] = transform2.GetComponent<T>();
					}
				}
			}
			finally
			{
				if (enumerator2 is System.IDisposable disposable2)
				{
					disposable2.Dispose();
				}
			}
			return array;
		}

	
		public static bool IsAbsurd(float f)
		{
			return !(f > -34028230f) || !(f < 34028230f);
		}

	
		public static bool IsBad(Vector3 v3)
		{
			return float.IsNaN(v3.x) || float.IsNaN(v3.y) || float.IsNaN(v3.z) || float.IsInfinity(v3.x) || float.IsInfinity(v3.y) || float.IsInfinity(v3.z);
		}

	
		public static bool IsAbsurd(Vector3 v3)
		{
			return !(v3.x > -34028230f) || !(v3.x < 34028230f) || !(v3.y > -34028230f) || !(v3.y < 34028230f) || !(v3.z > -34028230f) || !(v3.z < 34028230f);
		}

	
		public static void Clamp(Vector3 v3)
		{
			v3.x = Mathf.Clamp(v3.x, -512000f, 512000f);
			v3.y = Mathf.Clamp(v3.y, -512000f, 512000f);
			v3.z = Mathf.Clamp(v3.z, -512000f, 512000f);
		}

	
		public static void Clamp(Quaternion v3)
		{
			v3.x = Mathf.Clamp(v3.x, -512000f, 512000f);
			v3.y = Mathf.Clamp(v3.y, -512000f, 512000f);
			v3.z = Mathf.Clamp(v3.z, -512000f, 512000f);
			v3.w = Mathf.Clamp(v3.w, -512000f, 512000f);
		}

	
		public static string ToCleanString(Vector3 v3, string format = "F4")
		{
			return v3.ToString(format).Replace(" ", string.Empty).Trim('(', ')');
		}

	
		public static float RoundAmount(float i, float nearestFactor)
		{
			return (float)System.Math.Round(i / nearestFactor) * nearestFactor;
		}

	
		public static Vector3 RoundAmount(Vector3 i, float nearestFactor)
		{
			return new Vector3(RoundAmount(i.x, nearestFactor), RoundAmount(i.y, nearestFactor), RoundAmount(i.z, nearestFactor));
		}

	
		public static Vector2 RoundAmount(Vector2 i, float nearestFactor)
		{
			return new Vector2(RoundAmount(i.x, nearestFactor), RoundAmount(i.y, nearestFactor));
		}
	}
}
