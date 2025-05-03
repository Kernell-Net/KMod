using System;
using System.Collections;
using MelonLoader;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using VRC.UI.Elements.Controls;

namespace KMod.UI.QuickMenu
{
	public class KmMenuHeaderCollapsible : KmMenuHeader
	{
		public Action<bool> OnToggle;

		public KmMenuHeaderCollapsible(string title, Transform parent)
			: base(QMMenuPrefabs.MenuCategoryHeaderCollapsiblePrefav, (parent == null) ? QMMenuPrefabs.MenuCategoryHeaderCollapsiblePrefav.transform.parent : parent, "Header_" + title)
		{
			KmMenuHeaderCollapsible reMenuHeaderCollapsible = this;
			TextComponent = base.GameObject.GetComponentInChildren<TextMeshProUGUI>();
			MelonCoroutines.Start(Wait());
			TextComponent.richText = true;
			FoldoutToggle component = base.GameObject.GetComponent<FoldoutToggle>();
			component.Method_Public_Void_String_Boolean_0("UI.KmMod." + UiElement.GetCleanName(title));
			component.Method_Public_Void_UnityAction_1_Boolean_0((Action<bool>)delegate(bool b)
			{
				if (reMenuHeaderCollapsible.OnToggle != null)
				{
					reMenuHeaderCollapsible.OnToggle(b);
				}
			});
			IEnumerator Wait()
			{
				while (object.Equals(reMenuHeaderCollapsible.GameObject.activeInHierarchy, false))
				{
					yield return null;
				}
				reMenuHeaderCollapsible.TextComponent.text = title;
			}
		}

		public KmMenuHeaderCollapsible(Transform transform)
			: base(transform)
		{
			TextComponent = base.GameObject.GetComponentInChildren<TextMeshProUGUI>();
		}
	}
}
