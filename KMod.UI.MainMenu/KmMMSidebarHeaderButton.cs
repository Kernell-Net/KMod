using System;
using System.Collections;
using MelonLoader;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Elements.Controls;

namespace KMod.UI.MainMenu
{
	public class KmMMSidebarHeaderButton : UiElement
	{
		private TextMeshProUGUI _textComponent;

		public string Text
		{
			get
			{
				return _textComponent.text;
			}
			set
			{
				_textComponent.text = value;
			}
		}

		public KmMMSidebarHeaderButton(KmMMPage menu, string text, string tooltip, Sprite icon, Action onClick, string color = "#ffffff")
			: base(MMenuPrefabs.MMSideBarHeaderButtonPrefab, menu.GetSidePanelHeader(), "sb_btn_" + text)
		{
			KmMMSidebarHeaderButton reMMSidebarHeaderButton = this;
			UnityEngine.Object.Destroy(base.GameObject.GetComponent<LogoutButton>());
			_textComponent = base.GameObject.transform.Find("Background_Field/Text_FieldContent").GetComponent<TextMeshProUGUI>();
			MelonCoroutines.Start(Wait());
			_textComponent.richText = true;
			if (icon == null)
			{
				base.GameObject.transform.Find("Background_Field/Text_FieldContent").SetAsFirstSibling();
			}
			base.GameObject.transform.Find("Background_Field/Icon").GetComponent<Image>().overrideSprite = icon;
			base.GameObject.transform.Find("Background_Field/Icon").gameObject.SetActive(icon != null);
			LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip);
			ToolTip component = base.GameObject.GetComponent<ToolTip>();
			if (component != null)
			{
				component._alternateLocalizableString = localizableString;
				component._localizableString = localizableString;
			}
			base.GameObject.GetComponent<Button>().onClick.AddListener((Action)onClick.Invoke);
			Transform parent = menu.MenuTitleText.transform.parent;
			parent.GetComponent<LayoutElement>().minHeight += 95f;
			parent.Find("Separator").GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 95f);
			IEnumerator Wait()
			{
				while (object.Equals(reMMSidebarHeaderButton.GameObject.activeInHierarchy, false))
				{
					yield return null;
				}
				reMMSidebarHeaderButton.Text = "<color=" + color + ">" + text + "</color>";
			}
		}
	}
}
