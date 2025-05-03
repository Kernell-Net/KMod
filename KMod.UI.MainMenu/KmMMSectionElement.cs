using KMod.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace KMod.UI.MainMenu
{
	public class KmMMSectionElement
	{
		public Transform LeftItemContainer { get; protected set; }

		public Transform RightItemContainer { get; protected set; }

		public GameObject gameObject { get; protected set; }

		public StyleElement StyleElement { get; protected set; }

		public KmMMSectionElement(GameObject prefab, Transform container, bool sizefitter = true, bool separator = true)
		{
			gameObject = Object.Instantiate(prefab, container);
			LeftItemContainer = gameObject.transform.Find("LeftItemContainer") ?? null;
			RightItemContainer = gameObject.transform.Find("RightItemContainer") ?? null;
			StyleElement = gameObject.GetComponent<StyleElement>() ?? null;
			if (sizefitter && RightItemContainer != null)
			{
				ContentSizeFitter contentSizeFitter = RightItemContainer.gameObject.AddComponent<ContentSizeFitter>();
				contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
				contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			}
			if (separator)
			{
				Object.Instantiate(MMenuPrefabs.MMSeparatorprefab, container);
			}
		}

		public KmMMSectionElement(GameObject prefab, KmMMCategorySection section, bool sizefitter = true)
			: this(prefab, section.ContentArea, sizefitter)
		{
		}
	}
}
