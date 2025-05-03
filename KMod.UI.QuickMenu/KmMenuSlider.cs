using System;
using KMod.Unity;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Core.Styles;
using VRC.UI.Elements.Controls;

namespace KMod.UI.QuickMenu
{
    public class KmMenuSlider : UiElement
    {
        private UnityEngine.UI.Slider _sliderComponent;
        private ToolTip _tooltip;
        private StyleElement _styleElement;
        private readonly TextMeshProUGUI _valueText;
        private readonly string _colorHex;

        public string Tooltip
        {
            get => (_tooltip != null) ? _tooltip._localizableString.Key : "";
            set
            {
                if (_tooltip != null)
                {
                    var localizableString = LocalizableStringExtensions.Localize(value);
                    _tooltip._localizableString = localizableString;
                    _tooltip._alternateLocalizableString = localizableString;
                }
            }
        }

        public bool Interactable
        {
            get => _sliderComponent.interactable;
            set
            {
                if (_sliderComponent.interactable == value) return; // Skip if unchanged
                
                _sliderComponent.interactable = value;
                _styleElement?.Method_Private_Void_Boolean_Boolean_0(value);
            }
        }

        public float Value
        {
            get => _sliderComponent.value;
            set => Slide(value);
        }

        public float MinValue
        {
            get => _sliderComponent.minValue;
            set => _sliderComponent.minValue = value;
        }

        public float MaxValue
        {
            get => _sliderComponent.maxValue;
            set => _sliderComponent.maxValue = value;
        }

        public KmMenuSlider(string text, string tooltip, Action<float> onSlide, Transform parent, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
            : base(QMMenuPrefabs.SliderPrefab, parent, "Slider_" + text)
        {
            // Store color for reuse
            _colorHex = color;

            // Configure label text
            SetupLabelText(text);
            
            // Configure value text
            _valueText = SetupValueText(defaultValue);
            
            // Configure slider component
            SetupSlider(defaultValue, minValue, maxValue, onSlide);
            
            // Clean up unused components
            CleanupUnusedComponents();
            
            // Configure tooltip
            SetupTooltip(tooltip);
            
            // Set initial value
            Slide(defaultValue, false);
            
            // Kmgister event listener
            EnableDisableListener.KmgisterSafe();
        }

        private void SetupLabelText(string text)
        {
            var labelTextComponent = base.RectTransform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
            labelTextComponent.text = $"<color={_colorHex}>{text}</color>";
            labelTextComponent.richText = true;
            labelTextComponent.enableAutoSizing = true;
        }

        private TextMeshProUGUI SetupValueText(float defaultValue)
        {
            var valueTextComponent = base.RectTransform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            valueTextComponent.richText = true;
            valueTextComponent.text = GetFormattedValueText(defaultValue);
            return valueTextComponent;
        }

        private void SetupSlider(float defaultValue, float minValue, float maxValue, Action<float> onSlide)
        {
            _sliderComponent = base.GameObject.GetComponentInChildren<UnityEngine.UI.Slider>();
            _styleElement = _sliderComponent.GetComponent<StyleElement>();
            
            // Configure slider range
            _sliderComponent.minValue = minValue;
            _sliderComponent.maxValue = maxValue;
            _sliderComponent.value = defaultValue;
            
            // Configure slider events
            _sliderComponent.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            
            if (onSlide != null)
            {
                _sliderComponent.onValueChanged.AddListener(new Action<float>(onSlide));
            }
            
            _sliderComponent.onValueChanged.AddListener(new Action<float>(UpdateValueText));
            _sliderComponent.m_OnValueChanged = _sliderComponent.onValueChanged;
        }

        private void UpdateValueText(float value)
        {
            if (_valueText != null)
            {
                _valueText.text = GetFormattedValueText(value);
            }
        }

        private string GetFormattedValueText(float value)
        {
            return $"<color={_colorHex}>{value:F}</color>";
        }

        private void CleanupUnusedComponents()
        {
            Transform toggleButton = base.GameObject.transform.Find("RightItemContainer/Cell_MM_ToggleButton");
            if (toggleButton != null)
            {
                UnityEngine.Object.DestroyImmediate(toggleButton.gameObject);
            }
        }

        private void SetupTooltip(string tooltipText)
        {
            var localizableString = LocalizableStringExtensions.Localize(tooltipText);
            
            // Set tooltip on value text (if available)
            if (_valueText != null)
            {
                _valueText.text = localizableString._fallbackText;
            }
            
            // Setup tooltip component
            _tooltip = _sliderComponent.GetComponent<ToolTip>();
            if (_tooltip != null)
            {
                _tooltip._localizableString = localizableString;
                _tooltip._alternateLocalizableString = localizableString;
            }
        }

        // Public methods for backward compatibility
        public void Slide(float value, bool callback = true)
        {
            _sliderComponent.Set(value, callback);
        }

        public void SetNewMaxValue(float value)
        {
            MaxValue = value;
        }

        public void SetNewMinValue(float value)
        {
            MinValue = value;
        }

        public float CurrentValue()
        {
            return Value;
        }

        // Fluent interface methods for chaining
        public KmMenuSlider WithValue(float value)
        {
            Value = value;
            return this;
        }

        public KmMenuSlider WithMinValue(float value)
        {
            MinValue = value;
            return this;
        }

        public KmMenuSlider WithMaxValue(float value)
        {
            MaxValue = value;
            return this;
        }

        public KmMenuSlider WithInteractable(bool interactable)
        {
            Interactable = interactable;
            return this;
        }
    }
}