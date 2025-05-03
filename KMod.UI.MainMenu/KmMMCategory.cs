using System;
using System.Collections;
using MelonLoader;
using KMod.Unity;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI;
using VRC.UI.Elements.Controls;

namespace KMod.UI.MainMenu
{
	public class KmMMCategory : UiElement
	{
		protected TextMeshProUGUI ButtonText;

		protected KmMMPage Menu;

		public GameObject ButtonObj { get; private set; }

		public GameObject ContainerObj { get; protected set; }

		public event Action onOpen;

		public event Action onClose;

		public KmMMCategory(KmMMPage menu, string btnText, string tooltip, Sprite Icon = null, Transform parent = null, string color = "#ffffff")
			: base(MMenuPrefabs.MMCategoryButtonPrefab, parent ?? menu.GetCategoryButtonContainer(), btnText)
		{
			KmMMCategory reMMCategory = this;
			Menu = menu;
			ButtonObj = base.GameObject;
			ButtonObj.name = "CategoryBtn-" + UiElement.GetCleanName(btnText);
			((Image)(object)ButtonObj.transform.Find("Icon").GetComponent<ImageEx>()).overrideSprite = Icon;
			ButtonText = ButtonObj.transform.Find("Mask/Text_Name").GetComponent<TextMeshProUGUI>();
			MelonCoroutines.Start(Wait());
			ButtonText.richText = true;
			ContainerObj = UnityEngine.Object.Instantiate(MMenuPrefabs.MMCategoryContainerPrefab, menu.GetCategoryChildContainer());
			ContainerObj.name = "CatetgoryChild-" + UiElement.GetCleanName(btnText);
			for (int num = ContainerObj.transform.childCount - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(ContainerObj.transform.GetChild(num).gameObject);
			}
			EnableDisableListener enableDisableListener = ContainerObj.AddComponent<EnableDisableListener>();
			enableDisableListener.OnEnableEvent += delegate
			{
				if (reMMCategory.onOpen != null)
				{
					reMMCategory.onOpen();
				}
			};
			enableDisableListener.OnDisableEvent += delegate
			{
				if (reMMCategory.onClose != null)
				{
					reMMCategory.onClose();
				}
			};
			ToolTip component = ButtonObj.GetComponent<ToolTip>();
			component._alternateLocalizableString = LocalizableStringExtensions.Localize(tooltip);
			component._localizableString = LocalizableStringExtensions.Localize(tooltip);
			IEnumerator Wait()
			{
				while (object.Equals(reMMCategory.GameObject.activeInHierarchy, false))
				{
					yield return null;
				}
				reMMCategory.ButtonText.text = "<color=" + color + ">" + btnText + "</color>";
			}
		}

		public KmMMCategory(Transform transform)
			: base(transform)
		{
			Menu = new KmMMPage(transform.parent.parent.parent.parent.parent.parent);
		}

		public KmMMCategorySection AddnGetSection(string title, bool collapsible = false, string color = "#ffffff")
		{
			return new KmMMCategorySection(title, collapsible, ContainerObj.transform, this, color);
		}

		public KmMMPage GetMenu()
		{
			return Menu;
		}
	}
}
