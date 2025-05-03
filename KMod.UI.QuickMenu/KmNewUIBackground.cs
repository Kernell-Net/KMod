using KMod.VRChat;
using UnityEngine;

namespace KMod.UI.QuickMenu
{
	public class KmNewUIBackground : UiElement
	{
		public KmNewUIBackground(string name, Transform parent = null)
			: base(QMMenuPrefabs.NewBackgroundPrefab, (parent == null) ? QMMenuPrefabs.NewBackgroundPrefab.transform.parent : parent, "BackgroundInfo_" + name)
		{
		}

		public KmNewUIBackground(Transform transform)
			: base(transform)
		{
		}
	}
}
