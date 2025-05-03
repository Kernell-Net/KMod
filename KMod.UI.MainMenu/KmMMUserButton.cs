using System;
using System.Collections;
using MelonLoader;
using KMod.VRChat;
using TMPro;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Elements.Controls;

namespace KMod.UI.MainMenu
{
	public class KmMMUserButton
	{
		private ToolTip _tooltip;

		public string Tooltip
		{
			get
			{
				return (_tooltip != null) ? _tooltip._localizableString.Key : "";
			}
			set
			{
				if (!(_tooltip == null))
				{
					LocalizableString localizableString = LocalizableStringExtensions.Localize(value);
					_tooltip._localizableString = localizableString;
				}
			}
		}

		public KmMMUserButton(string name, string tooltip, Action onClick, Sprite icon, Transform parent)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(MMenuPrefabs.MMUserDetailButton.transform, parent).gameObject;
			gameObject.name = "MMUButton_" + name;
			TextMeshProUGUI txt = gameObject.transform.Find("Text_ButtonName").GetComponent<TextMeshProUGUI>();
			MelonCoroutines.Start(Wait());
			txt.richText = true;
			txt.text = name;
			Il2CppArrayBase<ToolTip> components = gameObject.GetComponents<ToolTip>();
			if (components.Length > 0)
			{
				_tooltip = components[0];
				for (int i = 1; i < components.Length; i++)
				{
					UnityEngine.Object.DestroyImmediate(components[i]);
				}
			}
			if (_tooltip != null)
			{
				LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip);
				_tooltip._localizableString = localizableString;
				_tooltip._alternateLocalizableString = localizableString;
			}
			gameObject.transform.Find("Text_ButtonName/Icon").GetComponent<Image>().overrideSprite = icon;
			Button component = gameObject.GetComponent<Button>();
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener((Action)onClick.Invoke);
			IEnumerator Wait()
			{
				while (object.Equals(txt.gameObject.activeInHierarchy, false))
				{
					yield return null;
				}
				txt.text = name;
			}
		}
	}
}
