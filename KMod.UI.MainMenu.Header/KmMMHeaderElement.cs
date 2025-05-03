using UnityEngine;

namespace KMod.UI.MainMenu.Header
{
	public class KmMMHeaderElement
	{
		internal GameObject gameObject { get; private set; }

		public Transform Container => gameObject.transform.parent;

		public KmMMPage Page { get; private set; }

		public KmMMHeaderElement(GameObject prefab, KmMMPage page, string tooltip)
		{
			gameObject = Object.Instantiate(prefab, page.MenuObject.transform.Find("Menu_MM_DynamicSidePanel/Panel_SectionList/ScrollRect_Navigation/ScrollRect_Content/Header_MM_H2/RightItemContainer"));
			Page = page;
		}
	}
}
