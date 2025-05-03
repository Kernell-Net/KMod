using System;
using KMod.VRChat;
using UnityEngine;
using UnityEngine.UI;

namespace KMod.UI.MainMenu.Header
{
	public class KmMMHeaderButton : KmMMHeaderElement
	{
		private Button buttonComponent;

		public KmMMHeaderButton(string tooltip, Sprite icon, KmMMPage page, Action onClick)
			: base(MMenuPrefabs.MMHeaderButtonPrefab, page, tooltip)
		{
			base.gameObject.name = "Button_header";
			base.gameObject.transform.Find("Icon").GetComponent<Image>().overrideSprite = icon;
			buttonComponent = base.gameObject.GetComponent<Button>();
			buttonComponent.onClick.AddListener((Action)onClick.Invoke);
		}
	}
}
