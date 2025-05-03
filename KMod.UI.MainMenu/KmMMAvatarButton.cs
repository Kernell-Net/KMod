using System;
using System.Collections;
using MelonLoader;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI;

namespace KMod.UI.MainMenu
{
	public class KmMMAvatarButton
	{
		public Button btn;

		public ImageEx background;
		
		
		public KmMMAvatarButton(string name, string tooltip, Action onClick, Sprite icon, Transform parent, string color = "#ffffff")
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(MMenuPrefabs.MMAvatarButton.transform, parent).gameObject;
			gameObject.name = "MMAviButton_" + UiElement.GetCleanName(name);
			TextMeshProUGUI txt = gameObject.transform.Find("Text_ButtonName").GetComponent<TextMeshProUGUI>();
			MelonCoroutines.Start(Wait());
			txt.richText = true;
			background = gameObject.transform.Find("Background_Button").GetComponent<ImageEx>();
			HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleRight;
			ContentSizeFitter contentSizeFitter = gameObject.transform.Find("Background_Button").gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			ContentSizeFitter contentSizeFitter2 = gameObject.AddComponent<ContentSizeFitter>();
			HorizontalLayoutGroup horizontalLayoutGroup2 = gameObject.transform.Find("Background_Button").gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup2.childControlWidth = true;
			horizontalLayoutGroup2.childControlHeight = true;
			horizontalLayoutGroup2.padding = new RectOffset(25, 25, 17, 17);
			contentSizeFitter2.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter2.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			gameObject.transform.Find("Text_ButtonName/Icon").gameObject.SetActive(icon != null);
			UnityEngine.Object.Instantiate(gameObject.transform.Find("Text_ButtonName").gameObject, gameObject.transform.Find("Background_Button"));
			UnityEngine.Object.Destroy(gameObject.transform.Find("Text_ButtonName").gameObject);
			btn = gameObject.GetComponent<Button>();
			btn.onClick.RemoveAllListeners();
			btn.onClick.AddListener((Action)onClick.Invoke);
			IEnumerator Wait()
			{
				while (object.Equals(txt.gameObject.activeInHierarchy, false))
				{
					yield return null;
				}
				txt.text = "<color=" + color + ">" + name + "</color>";
			}
		}
	}
}
