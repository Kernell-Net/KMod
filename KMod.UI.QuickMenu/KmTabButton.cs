using System;
using KMod.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;

namespace KMod.UI.QuickMenu
{
	public class KmTabButton : UiElement
	{
		public KmTabButton(string name, string tooltip, string pageName, Sprite sprite, KmMenuPage menus)
			: base(QMMenuPrefabs.TabButtonPrefab, QMMenuPrefabs.TabButtonPrefab.transform.parent, "Page_" + name)
		{
			MenuTab menuTab = base.RectTransform.GetComponent<MenuTab>();
			menuTab.name = UiElement.GetCleanName("Page_" + pageName);
			menuTab._controlName = UiElement.GetCleanName("Page_" + pageName);
			menuTab.field_Private_MenuStateController_0 = MenuEx.QMenuStateCtrl;
			Button component = base.GameObject.GetComponent<Button>();
			component.onClick = new Button.ButtonClickedEvent();
			component.onClick.AddListener((Action)delegate
			{
				UIPage uIPage = menuTab.field_Private_MenuStateController_0.Method_Public_UIPage_String_0(menus.UiPage.field_Public_String_0);
				int contentIndex = 11;
				int num = 0;
				if (uIPage != null)
				{
					foreach (UIPage item in menuTab.field_Private_MenuStateController_0.field_Public_ArrayOf_UIPage_0)
					{
						if (item != null && item.field_Public_String_0 == uIPage.field_Public_String_0)
						{
							contentIndex = num;
							break;
						}
						num++;
					}
				}
				menuTab.field_Private_MenuStateController_0.ShowTabContent(contentIndex);
			});
			LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip);
			ToolTip component2 = base.GameObject.GetComponent<ToolTip>();
			component2._localizableString = localizableString;
			component2._alternateLocalizableString = localizableString;
			Image component3 = base.RectTransform.Find("Icon").GetComponent<Image>();
			component3.sprite = sprite;
			component3.overrideSprite = sprite;
		}

		public static KmTabButton Create(string name, string tooltip, string pageName, Sprite sprite, KmMenuPage menu)
		{
			return new KmTabButton(name, tooltip, pageName, sprite, menu);
		}
	}
}
