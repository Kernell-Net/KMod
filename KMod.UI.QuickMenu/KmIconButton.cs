using System;
using KMod.Unity;
using KMod.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;

namespace KMod.UI.QuickMenu
{
	public class KmIconButton
	{
		public UIPage UiPage { get; }

		public event Action OnOpen;

		public event Action OnClose;

		public KmIconButton(KmMenuPage menu, Sprite icon, string parent = "Dashboard")
		{
			Transform original = MenuEx.QMDashboardMenu.transform.Find("Header_H1/RightItemContainer/Button_QM_Expand");
			Transform parent2 = MenuEx.QMenuParent.transform.Find("Menu_" + parent).transform.Find("Header_H1/RightItemContainer/");
			GameObject gameObject = UnityEngine.Object.Instantiate(original, parent2).gameObject;
			gameObject.transform.Find("Icon").GetComponent<Image>().overrideSprite = icon;
			Button component = gameObject.GetComponent<Button>();
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener((Action)menu.Open);
			EnableDisableListener enableDisableListener = gameObject.AddComponent<EnableDisableListener>();
			enableDisableListener.OnEnableEvent += delegate
			{
				if (this.OnOpen != null)
				{
					this.OnOpen();
				}
			};
			enableDisableListener.OnDisableEvent += delegate
			{
				if (this.OnClose != null)
				{
					this.OnClose();
				}
			};
		}
	}
}
