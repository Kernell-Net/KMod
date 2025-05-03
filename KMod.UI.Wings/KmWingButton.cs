using System;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace KMod.UI.Wings
{
	public class KmWingButton : UiElement
	{
		private static GameObject _wingButtonPrefab;

		private readonly Image _iconImage;

		private readonly StyleElement _styleElement;

		private readonly Button _button;

		private static GameObject WingButtonPrefab
		{
			get
			{
				if (_wingButtonPrefab == null)
				{
					_wingButtonPrefab = MenuEx.QMLeftWing.transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Profile").gameObject;
				}
				return _wingButtonPrefab;
			}
		}

		public Sprite Sprite
		{
			get
			{
				return _iconImage.sprite;
			}
			set
			{
				if (value != null)
				{
					_iconImage.sprite = value;
					_iconImage.overrideSprite = value;
				}
				_iconImage.gameObject.SetActive(value != null);
			}
		}

		public bool Interactable
		{
			get
			{
				return _button.interactable;
			}
			set
			{
				_button.interactable = value;
				_styleElement.Method_Private_Void_Boolean_Boolean_0(value);
			}
		}

		public KmWingButton(string text, string tooltip, Action onClick, Sprite sprite = null, bool left = true, bool arrow = true, bool background = true, bool separator = false)
			: this(text, tooltip, onClick, (left ? MenuEx.QMLeftWing : MenuEx.QMRightWing).transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"), sprite, arrow, background, separator)
		{
		}

		public KmWingButton(string text, string tooltip, Action onClick, Transform parent, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
			: base(WingButtonPrefab, parent, "Button_" + text)
		{
			Transform transform = base.RectTransform.Find("Container").transform;
			transform.Find("Background").gameObject.SetActive(background);
			transform.Find("Icon_Arrow").gameObject.SetActive(arrow);
			base.RectTransform.Find("Separator").gameObject.SetActive(separator);
			_iconImage = transform.Find("Icon").GetComponent<Image>();
			Sprite = sprite;
			TextMeshProUGUI componentInChildren = transform.GetComponentInChildren<TextMeshProUGUI>();
			componentInChildren.text = text;
			componentInChildren.richText = true;
			_styleElement = base.GameObject.GetComponent<StyleElement>();
			_button = base.GameObject.GetComponent<Button>();
			_button.onClick = new Button.ButtonClickedEvent();
			_button.onClick.AddListener((Action)onClick.Invoke);
			if (sprite == null && !arrow)
			{
				transform.gameObject.AddComponent<HorizontalLayoutGroup>();
				componentInChildren.enableAutoSizing = true;
			}
		}

		public static void Create(string text, string tooltip, Action onClick, Sprite sprite = null, WingSide wingSide = WingSide.Both, bool arrow = true, bool background = true, bool separator = true)
		{
			if ((wingSide & WingSide.Left) == WingSide.Left)
			{
				new KmWingButton(text, tooltip, onClick, sprite, left: true, arrow, background, separator);
			}
			if ((wingSide & WingSide.Right) == WingSide.Right)
			{
				new KmWingButton(text, tooltip, onClick, sprite, left: false, arrow, background, separator);
			}
		}

		public static void Create(string text, string tooltip, Action onClick, Transform parent, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = true)
		{
			new KmWingButton(text, tooltip, onClick, parent, sprite, arrow, background, separator);
		}
	}
}
