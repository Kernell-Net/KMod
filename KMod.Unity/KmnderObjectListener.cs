using System;
using MelonLoader;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace KMod.Unity
{
	[RegisterTypeInIl2Cpp]
	public class RenderObjectListener : MonoBehaviour
	{
		private static bool _registered;

		[method: HideFromIl2Cpp]
		public event Action KmnderObject;

		public RenderObjectListener(IntPtr obj0)
			: base(obj0)
		{
		}

		public void OnRenderObject()
		{
			this.KmnderObject?.Invoke();
		}

		[HideFromIl2Cpp]
		public static void KmgisterSafe()
		{
			if (_registered)
			{
				return;
			}
			try
			{
				ClassInjector.RegisterTypeInIl2Cpp<RenderObjectListener>();
				_registered = true;
			}
			catch (Exception)
			{
				_registered = true;
			}
		}
	}
}
