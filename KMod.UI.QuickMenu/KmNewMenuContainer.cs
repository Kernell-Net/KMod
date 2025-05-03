using System;
using Il2CppSystem;
using Il2CppSystem.Collections;
using KMod.VRChat;
using UnityEngine;
using UnityEngine.UI;

namespace KMod.UI.QuickMenu
{
	public class KmNewMenuContainer : UiElement
	{
		public KmNewMenuContainer(string name, Transform parent = null)
			: base(QMMenuPrefabs.NewContainerPrefab, (parent == null) ? QMMenuPrefabs.NewContainerPrefab.transform.parent : parent, "NewUI_" + name)
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
			VerticalLayoutGroup component = base.GameObject.GetComponent<VerticalLayoutGroup>();
			component.m_Padding = new RectOffset(64, 64, 0, 0);
		}

		public KmNewMenuContainer(Transform transform)
			: base(transform)
		{
		}
	}
}
