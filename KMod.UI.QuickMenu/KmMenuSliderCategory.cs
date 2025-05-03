using System;
using UnityEngine;

namespace KMod.UI.QuickMenu
{
	public class KmMenuSliderCategory
	{
		public readonly KmMenuHeader Header;

		private readonly KmMenuSliderContainer _sliderContainer;

		public string Title
		{
			get
			{
				return Header.Title;
			}
			set
			{
				Header.Title = value;
			}
		}

		public bool Active
		{
			get
			{
				return _sliderContainer.Active;
			}
			set
			{
				Header.Active = value;
				_sliderContainer.Active = value;
			}
		}

		public KmMenuSliderCategory(string title, Transform parent = null, bool collapsible = true, string color = "#ffffff")
		{
			if (collapsible)
			{
				KmMenuHeaderCollapsible reMenuHeaderCollapsible = new KmMenuHeaderCollapsible("<color=" + color + ">" + title + "</color>", parent);
				reMenuHeaderCollapsible.OnToggle = (Action<bool>)Delegate.Combine(reMenuHeaderCollapsible.OnToggle, (Action<bool>)delegate(bool b)
				{
					if (_sliderContainer != null)
					{
						_sliderContainer.GameObject.SetActive(b);
					}
				});
				Header = reMenuHeaderCollapsible;
			}
			else
			{
				KmMenuHeader header = new KmMenuHeader("<color=" + color + ">" + title + "</color>", parent);
				Header = header;
			}
			_sliderContainer = new KmMenuSliderContainer("<color=" + color + ">" + title + "</color>", parent);
		}

		public KmMenuSliderCategory(KmMenuHeader headerElement, KmMenuSliderContainer container)
		{
			Header = headerElement;
			_sliderContainer = container;
		}

		public KmMenuSlider AddSlider(string text, string tooltip, Action<float> onSlide, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new KmMenuSlider(text, tooltip, onSlide, _sliderContainer.RectTransform, defaultValue, minValue, maxValue, color);
		}

		public KmMenuSlider AddSlider(string text, string tooltip, ConfigValue<float> configValue, bool reset = false, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new KmMenuSlider(text, tooltip, configValue.SetValue, _sliderContainer.RectTransform, configValue, minValue, maxValue, color);
		}
	}
}
