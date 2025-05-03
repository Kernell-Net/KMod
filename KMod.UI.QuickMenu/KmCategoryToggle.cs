using System;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements.Controls;

namespace KMod.UI.QuickMenu
{
	public class KmCategoryToggle : UiElement
	{
		private readonly TextMeshProUGUI _text;

		private readonly Toggle _toggleComponent = null;

		private StyleElement _toggleStyleElement = null;

		public string Text
		{
			get
			{
				return _text.text;
			}
			set
			{
				_text.SetText(value);
			}
		}

		public bool Interactable
		{
			get
			{
				return _toggleComponent.interactable;
			}
			set
			{
				_toggleComponent.interactable = value;
				if (_toggleStyleElement != null)
				{
					_toggleStyleElement.OnEnable();
				}
			}
		}

		public KmCategoryToggle(string title, string tooltip, Action<bool> onToggle, Transform parent, bool defaultValue = false, string color = "#ffffff")
			: base(QMMenuPrefabs.MenuCategoryTogglePrefab, parent, "Toggle_" + title)
		{
			_text = base.GameObject.GetComponentInChildren<TextMeshProUGUI>();
			_text.text = "<color=" + color + ">" + title + "</color>";
			_text.richText = true;
		}

		public static KmCategoryToggle Create(string title, string tooltip, Action<bool> onToggle, Transform parent, bool defaultValue = false, string color = "#ffffff")
		{
			return new KmCategoryToggle(title, tooltip, onToggle, parent, defaultValue, color);
		}
	}
}
