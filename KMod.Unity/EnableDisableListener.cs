using System;
using MelonLoader;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace KMod.Unity
{
	[RegisterTypeInIl2Cpp]
	public class EnableDisableListener : MonoBehaviour
	{
		private static bool _registered;

		[method: HideFromIl2Cpp]
		public event Action OnEnableEvent;

		[method: HideFromIl2Cpp]
		public event Action OnDisableEvent;

		public EnableDisableListener(IntPtr obj)
			: base(obj)
		{
		}

		public void OnEnable()
		{
			this.OnEnableEvent?.Invoke();
		}

		public void OnDisable()
		{
			this.OnDisableEvent?.Invoke();
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
				ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>();
				_registered = true;
			}
			catch (Exception)
			{
				_registered = true;
			}
		}
	}
}
