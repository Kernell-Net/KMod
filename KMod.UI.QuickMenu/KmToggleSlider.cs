using System;
using KMod.Unity;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace KMod.UI.QuickMenu
{
    public class KmToggleSlider : UiElement
    {
        private readonly Slider _slider;
        private readonly StyleElement _style;
        private readonly TextMeshProUGUI _valueText;
        private readonly string _colorHex;
        private readonly Action<float> _onSlideCallback;

        public bool Interactable
        {
            get => _slider.interactable;
            set
            {
                if (_slider.interactable == value) return;
                _slider.interactable = value;
                _style?.Method_Private_Void_Boolean_Boolean_0(value);
            }
        }

        public KmToggleSlider(string title, string tooltip, Action<float> onSlide, Transform parent, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
            : base(QMMenuPrefabs.SliderTogglePrefab, parent, "SliderToggle_" + title)
        {
            _colorHex = color;
            _onSlideCallback = onSlide;

            var titleText = RectTransform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
            titleText.text = $"<color={color}>{title}</color>";
            titleText.richText = true;

            _valueText = RectTransform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            _valueText.richText = true;
            
            _slider = GameObject.GetComponentInChildren<Slider>();
            _style = _slider.GetComponent<StyleElement>();
            
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
            
            _slider.onValueChanged = new Slider.SliderEvent();
            _slider.onValueChanged.AddListener((UnityAction<float>)OnValueChanged);
            _slider.m_OnValueChanged = _slider.onValueChanged;
            
            Slide(defaultValue, false);
            
            EnableDisableListener.KmgisterSafe();
        }

        private void OnValueChanged(float value)
        {
            UpdateValueText(value);
            _onSlideCallback?.Invoke(value);
        }

        private void UpdateValueText(float value)
        {
            _valueText.text = $"<color={_colorHex}>{value:F}</color>";
        }

        public void Slide(float value, bool triggerCallback = true)
        {
            if (!triggerCallback)
                UpdateValueText(value);
                
            _slider.Set(value, triggerCallback);
        }

        public void SetNewMaxValue(float value)
        {
            if (_slider.maxValue != value)
                _slider.maxValue = value;
        }

        public void SetNewMinValue(float value)
        {
            if (_slider.minValue != value)
                _slider.minValue = value;
        }

        public float CurrentValue() => _slider.value;
    }
}