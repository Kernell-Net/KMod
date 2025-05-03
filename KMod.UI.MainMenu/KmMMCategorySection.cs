using System;
using System.Collections;
using MelonLoader;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Controls;

namespace KMod.UI.MainMenu
{
	public class KmMMCategorySection : UiElement
	{
		protected GameObject TitleContainer;

		protected TextMeshProUGUI TitleText;

		public Transform ContentArea { get; protected set; }

		internal KmMMCategory Category { get; private set; }

		internal string Title
		{
			get
			{
				return TitleText.text;
			}
			set
			{
				TitleText.text = value;
			}
		}

		public KmMMCategorySection(string title, bool collapsible = false, Transform parent = null, KmMMCategory category = null, string color = "#ffffff")
			: base(MMenuPrefabs.MMCategorySectionPrefab, parent ?? MMenuPrefabs.MMCategorySectionPrefab.transform.parent, "Section-" + title)
		{
			KmMMCategorySection reMMCategorySection = this;
			TitleContainer = base.GameObject.transform.Find("MM_Foldout/Label").gameObject;
			TitleText = TitleContainer.GetComponent<TextMeshProUGUI>();
			MelonCoroutines.Start(Wait());
			TitleText.richText = true;
			if (category != null)
			{
				Category = category;
			}
			if (!collapsible)
			{
				TitleContainer.transform.SetAsFirstSibling();
			}
			base.GameObject.transform.Find("MM_Foldout/Background_Button").GetComponent<Toggle>().enabled = collapsible;
			base.GameObject.transform.Find("MM_Foldout/Arrow").gameObject.SetActive(collapsible);
			ContentArea = base.GameObject.transform.Find("Settings_Panel_1/VerticalLayoutGroup");
			for (int num = ContentArea.childCount - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(ContentArea.GetChild(num).gameObject);
			}
			UnityEngine.Object.Instantiate(MMenuPrefabs.MMCategorySectionBackGroundPrefab, ContentArea);
			IEnumerator Wait()
			{
				while (object.Equals(reMMCategorySection.GameObject.activeInHierarchy, false))
				{
					yield return null;
				}
				reMMCategorySection.Title = "<color=" + color + ">" + title + "</color>";
			}
		}

		public KmMMSlider AddSlider(string title, string tooltip, Action<float> onSlide, Transform parent = null, bool separator = true, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new KmMMSlider(title, tooltip, onSlide, parent ?? ContentArea, separator, defaultValue, minValue, maxValue, color, this);
		}

		public KmMMSlider AddSlider(string title, string tooltip, Action<float> onSlide, Transform parent = null, bool separator = true, string color = "#ffffff")
		{
			return new KmMMSlider(title, tooltip, onSlide, parent ?? ContentArea, separator, 0f, 0f, 10f, color, this);
		}

		public KmMMSlider AddSlider(string title, string tooltip, Action<float> onSlide, Transform parent = null, string color = "#ffffff")
		{
			return new KmMMSlider(title, tooltip, onSlide, parent ?? ContentArea, separator: true, 0f, 0f, 10f, color, this);
		}

		public KmMMSlider AddSlider(string title, string tooltip, ConfigValue<float> configValue, Transform parent = null, bool separator = true, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new KmMMSlider(title, tooltip, configValue.SetValue, parent ?? ContentArea, separator, configValue, minValue, maxValue, color, this);
		}

		public KmMMSlider AddSlider(string title, string tooltip, ConfigValue<float> configValue, Transform parent = null, bool separator = true, string color = "#ffffff")
		{
			return new KmMMSlider(title, tooltip, configValue.SetValue, parent ?? ContentArea, separator, configValue, 0f, 10f, color, this);
		}

		public KmMMSlider AddSlider(string title, string tooltip, ConfigValue<float> configValue, Transform parent = null, string color = "#ffffff")
		{
			return new KmMMSlider(title, tooltip, configValue.SetValue, parent ?? ContentArea, separator: true, configValue, 0f, 10f, color, this);
		}

		public KmMMToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue = false, bool separator = true, Sprite iconOn = null, Sprite iconOff = null, string color = "#ffffff")
		{
			return new KmMMToggle(text, tooltip, onToggle, defaultValue, ContentArea, separator, iconOn, iconOff, color);
		}

		public KmMMToggle AddToggle(string text, string tooltip, Action<bool> onToggle, Sprite iconOn = null, Sprite iconOff = null, string color = "#ffffff")
		{
			return new KmMMToggle(text, tooltip, onToggle, defaultState: false, ContentArea, separator: true, iconOn, iconOff, color);
		}

		public KmMMToggle AddToggle(string text, string tooltip, Action<bool> onToggle, string color = "#ffffff")
		{
			return new KmMMToggle(text, tooltip, onToggle, defaultState: false, ContentArea, separator: true, null, null, color);
		}

		public KmMMToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, bool defaultValue = false, bool separator = true, Sprite iconOn = null, Sprite iconOff = null, string color = "#ffffff")
		{
			return new KmMMToggle(text, tooltip, configValue.SetValue, configValue, ContentArea, separator, iconOn, iconOff, color);
		}

		public KmMMToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, Sprite iconOn = null, Sprite iconOff = null, string color = "#ffffff")
		{
			return new KmMMToggle(text, tooltip, configValue.SetValue, configValue, ContentArea, separator: true, iconOn, iconOff, color);
		}

		public KmMMToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, string color = "#ffffff")
		{
			return new KmMMToggle(text, tooltip, configValue.SetValue, configValue, ContentArea, separator: true, null, null, color);
		}

		public KmMMButton AddButton(string title, string buttontext, string tooltip, Action onClick, bool separator = true, string color = "#ffffff")
		{
			return new KmMMButton(title, buttontext, tooltip, onClick, ContentArea, separator, color);
		}

		public KmMMButton AddButton(string title, string tooltip, Action onClick, bool separator = true, string color = "#ffffff")
		{
			return new KmMMButton(title, "ㅤㅤㅤ", tooltip, onClick, ContentArea, separator, color);
		}

		public KmMMText AddLabel(string leftText, string rightText, string tooltip, int fontSize, bool separator = true, string color = "#ffffff")
		{
			return new KmMMText(leftText, rightText, tooltip, ContentArea, fontSize, separator, color);
		}

		public KmMMText AddLabel(string text, string tooltip, int fontSize, bool separator = true, string color = "#ffffff")
		{
			return new KmMMText(text, "", tooltip, ContentArea, fontSize, separator, color);
		}

		public KmMMOptionSelector AddnGetOptionSelector(string title, string tooltipForward = "Next", string tooltipBackward = "Back", uint defaultOptionIndex = 0u, bool separator = true, string color = "#ffffff")
		{
			return new KmMMOptionSelector(title, tooltipForward, tooltipBackward, defaultOptionIndex, ContentArea, separator, color, this);
		}

		public KmMMOptionSelector AddnGetOptionSelector(string title, uint defaultOptionIndex = 0u, bool separator = true, string color = "#ffffff")
		{
			return new KmMMOptionSelector(title, "Next", "Back", defaultOptionIndex, ContentArea, separator, color, this);
		}

		public KmMMOptionSelector AddnGetOptionSelector(string title, bool separator = true, string color = "#ffffff")
		{
			return new KmMMOptionSelector(title, "Next", "Back", 0u, ContentArea, separator, color, this);
		}
	}
}
