using System;
using Il2CppSystem;
using Il2CppSystem.Collections;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KMod.UI.QuickMenu
{
	public class KmMenuButtonContainer : UiElement
	{
		public KmMenuButtonContainer(string name, Transform parent = null)
			: base(QMMenuPrefabs.MenuCategoryContainerPrefab, (parent == null) ? QMMenuPrefabs.MenuCategoryContainerPrefab.transform.parent : parent, "Buttons_" + name)
		{
			IEnumerator enumerator = base.RectTransform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Il2CppSystem.Object current = enumerator.Current;
					Transform transform = current.Cast<Transform>();
					if (!(transform == null))
					{
						UnityEngine.Object.Destroy(transform.gameObject);
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
			GridLayoutGroup component = base.GameObject.GetComponent<GridLayoutGroup>();
			component.padding.top = 8;
			component.padding.left = 64;
		}

		public KmMenuButtonContainer(Transform transform)
			: base(transform)
		{
		}
	}
}
