using System;
using System.Collections;
using MelonLoader;
using KMod.Unity;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Elements.Controls;

namespace KMod.UI.MainMenu
{
	public class KmMMSlider : KmMMSectionElement
	{
		private TextMeshProUGUI _textComponent;

		private SnapSliderExtendedCallbacks _sliderComponent;

		private string _color;

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

		public KmMMCategorySection Section { get; private set; }

		public KmMMSlider(string title, string tooltip, Action<float> onSlide, Transform parent = null, bool separator = true, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff", KmMMCategorySection section = null)
			: base(MMenuPrefabs.MMSliderPrefab, parent, sizefitter: false, separator)
		{
			KmMMSlider reMMSlider = this;
			_textComponent = base.gameObject.transform.Find("LeftItemContainer/Title").GetComponent<TextMeshProUGUI>();
			MelonCoroutines.Start(Wait());
			_textComponent.richText = true;
			_color = color;
			if (section != null)
			{
				Section = section;
			}
			_sliderComponent = base.gameObject.transform.Find("RightItemContainer/Slider").GetComponent<SnapSliderExtendedCallbacks>();
			TextMeshProUGUI value = base.gameObject.transform.Find("RightItemContainer/Text_MM_H3").GetComponent<TextMeshProUGUI>();
			value.richText = true;
			value.text = "<color=" + color + ">" + defaultValue.ToString("F") + "</color>";
			_sliderComponent.minValue = minValue;
			_sliderComponent.maxValue = maxValue;
			_sliderComponent.value = defaultValue;
			_sliderComponent.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
			_sliderComponent.onValueChanged.AddListener((Action<float>)onSlide.Invoke);
			_sliderComponent.onValueChanged.AddListener((Action<float>)delegate(float val)
			{
				value.text = "<color=" + color + ">" + val.ToString("F") + "</color>";
			});
			_sliderComponent.m_OnValueChanged = _sliderComponent.onValueChanged;
			ToolTip component = _sliderComponent.GetComponent<ToolTip>();
			if (component != null)
			{
				component._localizableString = LocalizableStringExtensions.Localize(tooltip);
				component._alternateLocalizableString = LocalizableStringExtensions.Localize(tooltip);
			}
			if (separator)
			{
				UnityEngine.Object.Instantiate(MMenuPrefabs.MMSeparatorprefab, parent);
			}
			Slide(defaultValue, callback: false);
			EnableDisableListener.KmgisterSafe();
			if (Section != null)
			{
				Section.Category.ButtonObj.GetComponent<Button>().onClick.AddListener((Action)delegate
				{
					MelonCoroutines.Start(reMMSlider.fix1());
				});
			}
			IEnumerator Wait()
			{
				while (object.Equals(reMMSlider._textComponent.gameObject.activeInHierarchy, false))
				{
					yield return null;
				}
				reMMSlider.Text = "<color=" + color + ">" + title + "</color>";
			}
		}

		private IEnumerator fix1()
		{
			yield return new WaitForSeconds(0.025f);
			TextMeshProUGUI value = base.gameObject.transform.Find("RightItemContainer/Text_MM_H3").GetComponent<TextMeshProUGUI>();
			value.text = "<color=" + _color + ">" + CurrentValue().ToString("F") + "</color>";
		}

		public void Slide(float value, bool callback = true)
		{
			_sliderComponent.Set(value, callback);
		}

		public void SetNewMaxValue(float value)
		{
			_sliderComponent.maxValue = value;
		}

		public void SetNewMinValue(float value)
		{
			_sliderComponent.minValue = value;
		}

		public float CurrentValue()
		{
			return _sliderComponent.value;
		}
	}
}
