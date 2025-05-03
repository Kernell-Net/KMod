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
	public class KmMMText : KmMMSectionElement
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

		public KmMMText(string title, string menutext, string tooltip, Transform parent = null, int fontSize = 30, bool separator = true, string color = "#ffffff")
			: base(MMenuPrefabs.MMLabelPrefab, parent, sizefitter: true, separator)
		{
			KmMMText reMMText = this;
			Object.Destroy(base.gameObject.GetComponent<Toggle>());
			Object.Destroy(base.RightItemContainer.Find("Cell_MM_OnOffSwitch").gameObject);
			_textComponent = base.LeftItemContainer.Find("Title").GetComponent<TextMeshProUGUI>();
			MelonCoroutines.Start(Wait());
			_textComponent.fontSize = fontSize;
			_textComponent.richText = true;
			GameObject gameObject = Object.Instantiate(MMenuPrefabs.MMLabelTextPrefab, base.RightItemContainer);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.richText = true;
			component.text = "<color=" + color + ">" + menutext + "</color>";
			component.alignment = TextAlignmentOptions.Right;
			component.fontSize = fontSize;
			LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip);
			ToolTip component2 = base.gameObject.GetComponent<ToolTip>();
			component2._localizableString = localizableString;
			component2._alternateLocalizableString = localizableString;
			IEnumerator Wait()
			{
				while (object.Equals(reMMText._textComponent.gameObject.activeInHierarchy, false))
				{
					yield return null;
				}
				reMMText.Text = "<color=" + color + ">" + title + "</color>";
			}
		}
	}
}
