using System;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Menus;

namespace KMod.UI.QuickMenu
{
	public class KmRadioToggle : UiElement
	{
		private static GameObject _togglePrefab;

		public bool IsOn;

		public Action<KmRadioToggle, bool> ToggleStateUpdated;

		public object ToggleData;

		private Button _button;

		private Toggle _toggle;

		private Graphic _checkmark;

		private TMP_Text _text;

		private StyleElement _style;

		private static GameObject TogglePrefab
		{
			get
			{
				if (_togglePrefab == null)
				{
					GameObject gameObject = MenuEx.QMInstance.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_ChangeAudioInputDevice").gameObject;
					MonoBehaviour1Public54Ga16ObGaVoOb58TeGaUnique component = gameObject.GetComponent<MonoBehaviour1Public54Ga16ObGaVoOb58TeGaUnique>();
					_togglePrefab = component.field_Public_GameObject_0;
				}
				return _togglePrefab;
			}
		}

		public KmRadioToggle(Transform parent, string name, string text, object obj, bool defaultState = false)
			: base(TogglePrefab, parent, "KmRadioToggle_" + UiElement.GetCleanName(name))
		{
			UnityEngine.Object.DestroyImmediate(base.RectTransform.GetComponent<MonoBehaviour2PublicOb_l_bObSt_iStUnique>());
			UnityEngine.Object.DestroyImmediate(base.RectTransform.GetComponent<DataContext>());
			_button = base.RectTransform.GetComponent<Button>();
			_toggle = base.RectTransform.GetComponentInChildren<Toggle>(includeInactive: true);
			_checkmark = _toggle.graphic;
			_text = base.RectTransform.GetComponentInChildren<TMP_Text>(includeInactive: true);
			_style = base.RectTransform.GetComponent<StyleElement>();
			_text.text = text;
			ToggleData = obj;
			SetToggle(defaultState);
			_button.onClick.AddListener((Action)ToggleOn);
		}

		public void SetToggle(bool state)
		{
			IsOn = state;
			_checkmark.gameObject.active = IsOn;
			_toggle.Set(IsOn);
		}

		private void ToggleOn()
		{
			if (!IsOn)
			{
				IsOn = true;
				_checkmark.gameObject.active = IsOn;
				_toggle.Set(IsOn);
				ToggleStateUpdated?.Invoke(this, IsOn);
			}
		}
	}
}
