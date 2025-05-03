using System.Collections;
using MelonLoader;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Controls;

namespace KMod.UI.QuickMenu
{
	public class KmMenuHeader : UiElement
	{
		protected TextMeshProUGUI TextComponent;

		public string Title
		{
			get
			{
				return TextComponent.text;
			}
			set
			{
				MelonCoroutines.Start(Wait());
				IEnumerator Wait()
				{
					while (object.Equals(base.GameObject.activeInHierarchy, false))
					{
						yield return null;
					}
					TextComponent.text = value;
				}
			}
		}

		public KmMenuHeader(string title, Transform parent)
			: base(QMMenuPrefabs.MenuCategoryHeaderPrefab, (parent == null) ? QMMenuPrefabs.MenuCategoryHeaderPrefab.transform.parent : parent, "Header_" + title)
		{
			KmMenuHeader reMenuHeader = this;
			TextComponent = base.GameObject.GetComponentInChildren<TextMeshProUGUI>();
			MelonCoroutines.Start(Wait());
			TextComponent.richText = true;
			TextComponent.transform.parent.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;
			IEnumerator Wait()
			{
				while (object.Equals(reMenuHeader.GameObject.activeInHierarchy, false))
				{
					yield return null;
				}
				reMenuHeader.TextComponent.text = title;
			}
		}

		public KmMenuHeader(Transform transform)
			: base(transform)
		{
			TextComponent = base.GameObject.GetComponentInChildren<TextMeshProUGUI>();
		}

		protected KmMenuHeader(GameObject original, Transform parent, Vector3 pos, string name, bool defaultState = true)
			: base(original, parent, pos, name, defaultState)
		{
		}

		protected KmMenuHeader(GameObject original, Transform parent, string name, bool defaultState = true)
			: base(original, parent, name, defaultState)
		{
		}
	}
}
