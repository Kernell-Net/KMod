using System;
using KMod.UI.ActionMenu.API;
using UnityEngine;

namespace KMod.UI.ActionMenu
{
	public class ActionMenuButton
	{
		public PedalOption currentPedalOption { get; set; }

		internal Func<bool> buttonAction { get; }

		internal string buttonText { get; private set; }

		internal Texture2D buttonIcon { get; private set; }

		public ActionMenuButton(string text, Action action, Sprite icon = null)
		{
			buttonText = text;
			buttonIcon = ((icon != null) ? icon.texture : null);
			buttonAction = delegate
			{
				action();
				return true;
			};
			ActionMenuAPI._mainMenuButtons.Add(this);
		}

		public ActionMenuButton(ActionMenuPage basePage, string text, Action action, Sprite icon = null)
		{
			buttonText = text;
			buttonIcon = ((icon != null) ? icon.texture : null);
			buttonAction = delegate
			{
				action();
				return true;
			};
			basePage.buttons.Add(this);
		}

		public void SetButtonText(string newText)
		{
			buttonText = newText;
		}

		public void SetIcon(Sprite newTexture)
		{
			buttonIcon = newTexture.texture;
			if (currentPedalOption != null)
			{
				currentPedalOption.Method_Public_Virtual_Final_New_Void_Texture2D_0(newTexture.texture);
			}
		}
	}
}
