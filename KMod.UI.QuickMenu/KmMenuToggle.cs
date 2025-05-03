using System;
using MelonLoader;
using KMod.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.Localization;
using TMPro;
using UnhollowerRuntimeLib;

namespace KMod.UI.QuickMenu
{
    public class KmMenuToggle : UiElement
    {
        // Core components
        private readonly Button _button;
        private readonly TextMeshProUGUI _text;
        private readonly StyleElement _styleElement;
        private readonly Image _iconOn;
        private readonly Image _iconOff;
        private readonly GameObject _iconsContainer;
        
        // State
        private bool _value;
        private bool _thinMode;
        private readonly Action<bool> _onToggle;
        private Color _textColor = Color.white;
        
        // Property accessors
        public bool Value
        {
            get => _value;
            set => SetValue(value);
        }
        
        public bool ThinMode
        {
            get => _thinMode;
            set
            {
                if (_thinMode == value) return;
                _thinMode = value;
                
                if (_iconsContainer != null)
                    _iconsContainer.SetActive(!_thinMode);
                
                UpdateText();
            }
        }
        
        public bool Interactable
        {
            get => _button.interactable;
            set
            {
                if (_button.interactable == value) return;
                _button.interactable = value;
                _styleElement?.Method_Private_Void_Boolean_Boolean_0(value);
            }
        }

        public KmMenuToggle(
            string text,
            string tooltip,
            Action<bool> onToggle,
            Transform parent,
            bool defaultValue = false,
            Sprite iconOn = null,
            Sprite iconOff = null,
            string color = "#FFFFFF",
            bool thinMode = false)
            : base(QMMenuPrefabs.TogglePrefab, parent, $"Toggle_{text}")
        {
            // Validate state
            if (GameObject == null)
            {
                MelonLogger.Error($"Failed to create toggle: {text}");
                return;
            }
            
            // Store parameters
            _onToggle = onToggle;
            _value = defaultValue;
            _thinMode = thinMode;
            
            // Parse color
            if (ColorUtility.TryParseHtmlString(color, out Color parsedColor))
                _textColor = parsedColor;
            
            // Get UI components
            _button = GameObject.GetComponent<Button>();
            _text = GameObject.GetComponentInChildren<TextMeshProUGUI>();
            _styleElement = GameObject.GetComponent<StyleElement>();
            _iconsContainer = RectTransform?.Find("Icons")?.gameObject;
            _iconOn = GetIcon("Icons/Icon_On");
            _iconOff = GetIcon("Icons/Icon_Off");
            
            // Kmmove blocking graphic
            UnityEngine.Object.DestroyImmediate(GameObject.GetComponent<UIInvisibleGraphic>());
            
            // Setup appearance
            SetupIcons(iconOn, iconOff);
            SetupTooltip(tooltip);
            UpdateText(text);
            
            // Configure button
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityEngine.Events.UnityAction>(
                new Action(ToggleValue)));
            
            // Initialize state
            _iconsContainer?.SetActive(!_thinMode);
            UpdateVisuals();
        }
        
        private Image GetIcon(string path)
        {
            Transform iconTransform = RectTransform?.Find(path);
            return iconTransform?.GetComponent<Image>();
        }
        
        private void SetupIcons(Sprite iconOn, Sprite iconOff)
        {
            if (_iconOn != null)
            {
                _iconOn.sprite = iconOn ?? GetDefaultSprite(true);
                _iconOn.overrideSprite = _iconOn.sprite;
            }
            
            if (_iconOff != null)
            {
                _iconOff.sprite = iconOff ?? GetDefaultSprite(false);
                _iconOff.overrideSprite = _iconOff.sprite;
            }
        }
        
        private void SetupTooltip(string tooltip)
        {
            if (string.IsNullOrEmpty(tooltip)) return;
            
            var tooltipComponent = GameObject.GetComponent<UiToggleTooltip>();
            if (tooltipComponent != null)
            {
                var locString = LocalizableStringExtensions.Localize(tooltip);
                tooltipComponent._localizableString = locString;
                tooltipComponent._alternateLocalizableString = locString;
            }
        }
        
        private void UpdateText(string text = null)
        {
            if (_text == null) return;
            
            // Update text content if provided
            if (text != null)
                _text.text = $"<color=#{ColorUtility.ToHtmlStringRGB(_textColor)}>{text}</color>";
            
            // Adjust text position and size for thin mode
            if (_thinMode)
            {
                _text.transform.localPosition = Vector3.zero;
                _text.fontSize = 24f;
                _text.enableAutoSizing = true;
                _text.fontSizeMin = 18f;
                _text.fontSizeMax = 24f;
            }
            else
            {
                _text.transform.localPosition = new Vector3(-100f, 0f, _text.transform.localPosition.z);
                _text.fontSize = 20f;
                _text.enableAutoSizing = false;
            }
        }
        
        private void UpdateVisuals()
        {
            if (_thinMode) return;
            
            // Update icon visibility based on toggle state
            if (_iconOn != null)
            {
                Color color = _iconOn.color;
                color.a = _value ? 1f : 0.1f;
                _iconOn.color = color;
            }
            
            if (_iconOff != null)
            {
                Color color = _iconOff.color;
                color.a = _value ? 0.1f : 1f;
                _iconOff.color = color;
            }
        }
        
        private Sprite GetDefaultSprite(bool isOn)
        {
            // Use cached sprites if available
            if (isOn && MenuEx.OnIconSprite != null)
                return MenuEx.OnIconSprite;
            if (!isOn && MenuEx.OffIconSprite != null)
                return MenuEx.OffIconSprite;
                
            // Find sprites by name
            string spriteName = isOn ? "ON_Icon" : "OFF_Icon";
            foreach (var sprite in Resources.FindObjectsOfTypeAll<Sprite>())
            {
                if (sprite.name.Equals(spriteName, StringComparison.OrdinalIgnoreCase))
                    return sprite;
            }
            
            return null;
        }
        
        public void ToggleValue()
        {
            SetValue(!_value);
        }
        
        public void SetValue(bool value, bool triggerCallback = true)
        {
            bool changed = _value != value;
            _value = value;
            
            UpdateVisuals();
            
            if (changed && triggerCallback)
                _onToggle?.Invoke(_value);
        }
    }
}