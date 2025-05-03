using System;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI;
using VRC.UI.Core.Styles;

namespace KMod.UI.MainMenu
{
    public class KmMMToggle
    {
        private readonly Toggle _toggle;
        private readonly StyleElement _style;
        private readonly TextMeshProUGUI _text;
        private readonly UiToggleTooltip _tooltip;
        private readonly ImageEx _onImg;
        private readonly ImageEx _offImg;
        private readonly RectTransform _handle;
        private readonly float _handleWidth;
        private readonly Transform _onContainer;
        private readonly Transform _offContainer;
        private readonly GameObject _onText;
        private readonly GameObject _offText;
        private readonly bool _defaultState;
        private bool _value;

        public GameObject ToggleObject { get; }

        public bool Value
        {
            get => _value;
            set => Toggle(value);
        }

        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }

        public string Tooltip
        {
            get => _tooltip?._localizableString.Key ?? string.Empty;
            set
            {
                if (_tooltip != null)
                    _tooltip._localizableString = LocalizableStringExtensions.Localize(value);
            }
        }

        public bool Interactable
        {
            get => _toggle.interactable;
            set
            {
                _toggle.interactable = value;
                _style.OnEnable();
            }
        }

        public KmMMToggle(string title, string tooltip, Action<bool> onToggle, bool defaultState = false, 
                        Transform parent = null, bool separator = true, Sprite iconOn = null, 
                        Sprite iconOff = null, string color = "#ffffff")
        {
            _defaultState = defaultState;
            
            ToggleObject = UnityEngine.Object.Instantiate(MMenuPrefabs.MMTogglePrefab, parent);
            
            _toggle = ToggleObject.GetComponent<Toggle>();
            _style = ToggleObject.GetComponent<StyleElement>();
            
            _text = ToggleObject.transform.Find("LeftItemContainer/Title").GetComponent<TextMeshProUGUI>();
            _text.richText = true;
            _text.text = $"<color={color}>{title}</color>";
            
            Transform switchTransform = ToggleObject.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch");
            _onContainer = switchTransform.Find("On_Container");
            _offContainer = switchTransform.Find("Off_Container");
            _onText = _onContainer.Find("On_Text").gameObject;
            _offText = _offContainer.Find("Off_Text").gameObject;
            
            _onImg = _onContainer.GetComponent<ImageEx>();
            _offImg = _offContainer.GetComponent<ImageEx>();
            
            if (iconOn) ((Image)(object)_onImg).overrideSprite = iconOn;
            if (iconOff) ((Image)(object)_offImg).overrideSprite = iconOff;
            
            _handle = switchTransform.Find("Handle").GetComponent<RectTransform>();
            _handleWidth = _handle.rect.width;
            
            _toggle.onValueChanged = new Toggle.ToggleEvent();
            _toggle.onValueChanged.AddListener((UnityAction<bool>)UpdateToggleState);
            
            if (onToggle != null)
                _toggle.onValueChanged.AddListener(onToggle);
            
            LocalizableString localizedTooltip = LocalizableStringExtensions.Localize(tooltip);
            _tooltip = ToggleObject.GetComponent<UiToggleTooltip>();
            _tooltip._alternateLocalizableString = localizedTooltip;
            _tooltip._localizableString = localizedTooltip;
            
            if (separator)
                UnityEngine.Object.Instantiate(MMenuPrefabs.MMSeparatorprefab, parent);
            
            KmModPatches.QuickMenuFirstOpened += () => Toggle(defaultState, false);
        }

        private void UpdateToggleState(bool value)
        {
            _value = value;
            _onContainer.gameObject.SetActive(value);
            _offContainer.gameObject.SetActive(!value);
            _onText.SetActive(value);
            _offText.SetActive(!value);
            _handle.localPosition += new Vector3(value ? (_handleWidth * 2f) : (-_handleWidth * 2f), 0f, 0f);
        }

        public void Toggle(bool value, bool callback = true)
        {
            if (_value == value && callback) return;
            
            _value = value;
            
            if (callback)
            {
                _toggle.Set(value, true);
            }
            else
            {
                _toggle.isOn = value;
                _onContainer.gameObject.SetActive(value);
                _offContainer.gameObject.SetActive(!value);
                _onText.SetActive(value);
                _offText.SetActive(!value);
                
                if (_defaultState || value)
                {
                    _handle.localPosition += new Vector3(value ? (_handleWidth * 2f) : (-_handleWidth * 2f), 0f, 0f);
                }
            }
        }
    }
}