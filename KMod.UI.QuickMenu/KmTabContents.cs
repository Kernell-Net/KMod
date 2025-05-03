using KMod.VRChat;
using UnityEngine;
using UnityEngine.UI;

namespace KMod.UI.QuickMenu
{
	public class KmTabContents : UiElement
	{
		public KmTabContents(string title, string color = "#ffffff", Transform parent = null)
			: base(QMMenuPrefabs.TabContentPrefab, parent, "Contents_" + title)
		{
			Object.DestroyImmediate(base.GameObject.GetComponent<MonoBehaviourPublicObGa_lObLo_hOb_eLoUnique>());
			base.GameObject.GetComponent<Canvas>().enabled = true;
			base.GameObject.GetComponent<GraphicRaycaster>().enabled = true;
		}

		public KmTabContents(Transform transform)
			: base(transform)
		{
		}
	}
}
