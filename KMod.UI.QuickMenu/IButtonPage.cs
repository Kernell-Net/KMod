using System;
using UnityEngine;

namespace KMod.UI.QuickMenu
{
	public interface IButtonPage
	{
		KmMenuButton AddButton(string text, string tooltip, Action onClick, Sprite sprite = null, string color = "#ffffff");

		KmMenuButton AddSpacer(Sprite sprite = null);

		KmMenuPage AddMenuPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff");

		KmCategoryPage AddCategoryPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff");

		KmTabbedPage AddTabbedPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff");

		KmMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue = false, string color = "#ffffff");

		KmMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, string color = "#ffffff");

		KmMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue, Sprite iconOn, Sprite iconOff, string color = "#ffffff");

		KmMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, Sprite iconOn, Sprite iconOff, string color = "#ffffff");

		KmMenuPage GetMenuPage(string name);

		KmCategoryPage GetCategoryPage(string name);

		KmTabbedPage GetTabbedPage(string name);

		KmMenuPage ToMenuPage(string name, string tooltip = "", Sprite sprite = null);

		KmCategoryPage ToCategoryPage(string name, string tooltip = "", Sprite sprite = null);

		void AddCategoryPage(string text, string tooltip, Action<KmCategoryPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff");

		void AddMenuPage(string text, string tooltip, Action<KmMenuPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff");

		void AddTabbedPage(string text, string tooltip, Action<KmTabbedPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff");
	}
}
