using System;
using System.Collections.Generic;
using System.Linq;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Controls;

namespace KMod.UI.QuickMenu
{
	public class KmTab : UiElement
	{
		private readonly Transform _container;

		protected TextMeshProUGUI TextComponent;

		public static List<KmTabContents> tabcontentslist = new List<KmTabContents>();

		public string Title
		{
			get
			{
				return TextComponent.text;
			}
			set
			{
				TextComponent.text = value;
			}
		}

		public KmTab(string title, string color = "#ffffff", Transform parent = null)
			: base(QMMenuPrefabs.TabPrefab, parent, "Tab_" + title)
		{
			UnityEngine.Object.DestroyImmediate(base.GameObject.GetComponent<MonoBehaviourPublicObGaObBoBuObObObUnique>());
			TextComponent = base.GameObject.GetComponentInChildren<TextMeshProUGUI>();
			TextComponent.text = "<color=" + color + ">" + title + "</color>";
			TextComponent.richText = true;
			KmTabContents tabContents = new KmTabContents(title, color, parent.parent.parent.GetChild(2));
			_container = tabContents.RectTransform;
			tabContents.GameObject.SetActive(value: false);
			Enumerable.First(tabcontentslist).GameObject.SetActive(value: true);
			base.RectTransform.GetChild(1).gameObject.transform.Find("NewBadge").gameObject.SetActive(value: false);
			base.RectTransform.parent.parent.GetChild(1).gameObject.SetActive(value: false);
			Button component = base.GameObject.GetComponent<Button>();
			component.onClick = new Button.ButtonClickedEvent();
			component.onClick.AddListener((Action)delegate
			{
				foreach (KmTabContents item in tabcontentslist)
				{
					item.GameObject.SetActive(value: false);
				}
				if (tabContents.GameObject.name.Contains(title))
				{
					tabContents.GameObject.SetActive(value: true);
				}
			});
		}

		public KmTab(Transform transform)
			: base(transform)
		{
		}

		public KmMenuCategory AddCategory(string title, bool collapsible = true)
		{
			return GetCategory(title) ?? new KmMenuCategory(title, _container.GetChild(0).GetChild(0), collapsible);
		}

		public KmMenuCategory AddCategory(string title)
		{
			return GetCategory(title) ?? new KmMenuCategory(title, _container.GetChild(0).GetChild(0));
		}

		public KmMenuCategory AddCategory(string title, bool collapsible = true, string color = "#ffffff")
		{
			return GetCategory(title) ?? new KmMenuCategory(title, _container.GetChild(0).GetChild(0), collapsible, color);
		}

		public KmMenuCategory AddCategory(string title, string color = "#ffffff")
		{
			return GetCategory(title) ?? new KmMenuCategory(title, _container.GetChild(0).GetChild(0), collapsible: true, color);
		}

		public KmMenuCategory GetCategory(string name)
		{
			Transform transform = _container.GetChild(0).GetChild(0).Find("Header_" + UiElement.GetCleanName(name));
			if (transform == null)
			{
				return null;
			}
			KmMenuHeader headerElement = new KmMenuHeader(transform);
			KmMenuButtonContainer container = new KmMenuButtonContainer(_container.GetChild(0).GetChild(0).Find("Buttons_" + UiElement.GetCleanName(name)));
			return new KmMenuCategory(headerElement, container);
		}

		public KmMenuSliderCategory AddSliderCategory(string title)
		{
			return AddSliderCategory(title, collapsible: true);
		}

		public KmMenuSliderCategory AddSliderCategory(string title, bool collapsible = true)
		{
			return GetSliderCategory(title) ?? new KmMenuSliderCategory(title, _container.GetChild(0).GetChild(0), collapsible);
		}

		public KmMenuSliderCategory AddSliderCategory(string title, string color = "#ffffff")
		{
			return AddSliderCategory(title, collapsible: true, color);
		}

		public KmMenuSliderCategory AddSliderCategory(string title, bool collapsible = true, string color = "#ffffff")
		{
			return GetSliderCategory(title) ?? new KmMenuSliderCategory(title, _container.GetChild(0).GetChild(0), collapsible, color);
		}

		public KmMenuSliderCategory GetSliderCategory(string name)
		{
			Transform transform = _container.GetChild(0).GetChild(0).Find("Header_" + UiElement.GetCleanName(name));
			if (transform == null)
			{
				return null;
			}
			KmMenuHeader headerElement = new KmMenuHeader(transform);
			KmMenuSliderContainer container = new KmMenuSliderContainer(_container.GetChild(0).GetChild(0).Find("Sliders_" + UiElement.GetCleanName(name)));
			return new KmMenuSliderCategory(headerElement, container);
		}

		public KmNewMenuCategory AddNewCategory(string title)
		{
			return AddNewCategory(title, collapsible: true);
		}

		public KmNewMenuCategory AddNewCategory(string title, bool collapsible = true)
		{
			return GetNewCategory(title) ?? new KmNewMenuCategory(title, _container.GetChild(0).GetChild(0), collapsible);
		}

		public KmNewMenuCategory AddNewCategory(string title, string color = "#ffffff")
		{
			return AddNewCategory(title, collapsible: true, color);
		}

		public KmNewMenuCategory AddNewCategory(string title, bool collapsible = true, string color = "#ffffff")
		{
			return GetNewCategory(title) ?? new KmNewMenuCategory(title, _container.GetChild(0).GetChild(0), collapsible, color);
		}

		public KmNewMenuCategory GetNewCategory(string name)
		{
			Transform transform = _container.GetChild(0).GetChild(0).Find("Header_" + UiElement.GetCleanName(name));
			if (transform == null)
			{
				return null;
			}
			KmMenuHeader headerElement = new KmMenuHeader(transform);
			KmNewMenuContainer container = new KmNewMenuContainer(_container.GetChild(0).GetChild(0).Find("Sliders_" + UiElement.GetCleanName(name)));
			return new KmNewMenuCategory(headerElement, container);
		}
	}
}
