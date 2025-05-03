using System;
using UnityEngine;

namespace KMod.UI.QuickMenu
{
	public class KmNewMenuCategory
	{
		public readonly KmMenuHeader Header;

		private readonly KmNewMenuContainer _newContainer;

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
				return _newContainer.Active;
			}
			set
			{
				Header.Active = value;
				_newContainer.Active = value;
			}
		}

		public KmNewMenuCategory(string title, Transform parent = null, bool collapsible = true, string color = "#ffffff")
		{
			if (collapsible)
			{
				KmMenuHeaderCollapsible reMenuHeaderCollapsible = new KmMenuHeaderCollapsible("<color=" + color + ">" + title + "</color>", parent);
				reMenuHeaderCollapsible.OnToggle = (Action<bool>)Delegate.Combine(reMenuHeaderCollapsible.OnToggle, (Action<bool>)delegate(bool b)
				{
					if (_newContainer != null)
					{
						_newContainer.GameObject.SetActive(b);
					}
				});
				Header = reMenuHeaderCollapsible;
			}
			else
			{
				KmMenuHeader header = new KmMenuHeader("<color=" + color + ">" + title + "</color>", parent);
				Header = header;
			}
			_newContainer = new KmNewMenuContainer("<color=" + color + ">" + title + "</color>", parent);
			new KmNewUIBackground(title, _newContainer.RectTransform);
		}

		public KmNewMenuCategory(KmMenuHeader headerElement, KmNewMenuContainer container)
		{
			Header = headerElement;
			_newContainer = container;
		}

		public KmCategoryToggle AddCategoryToggle(string title, string tooltip, Action<bool> onToggle, bool defaultValue = false, string color = "#ffffff")
		{
			return new KmCategoryToggle(title, tooltip, onToggle, _newContainer.RectTransform);
		}

		public KmToggleSlider AddSlider(string text, string tooltip, Action<float> onSlide, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new KmToggleSlider(text, tooltip, onSlide, _newContainer.RectTransform, defaultValue, minValue, maxValue, color);
		}

		public KmToggleSlider AddSlider(string text, string tooltip, ConfigValue<float> configValue, bool reset = false, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new KmToggleSlider(text, tooltip, configValue.SetValue, _newContainer.RectTransform, configValue, minValue, maxValue, color);
		}
	}
}
