using System;
using KMod.Managers;
using KMod.UI.ActionMenu;
using UnityEngine;

namespace KMod.ActionMenu
{
	public class ActionMenuToggle
	{
		private Sprite _onImage;

		private Sprite _offImage;
		
		public bool State { get; set; }

		public PedalOption currentPedalOption => actionButton.currentPedalOption;

		private ActionMenuButton actionButton { get; }

		internal Sprite onImage
		{
			get
			{
				if (_onImage == null)
				{
					_onImage = null;
				}
				return _onImage;
			}
		}

		internal Sprite offImage
		{
			get
			{
				if (_offImage == null)
				{
					_offImage = null;
				}
				return _offImage;
			}
		}

		public ActionMenuToggle(ActionMenuPage basePage, string text, Action<bool> action, bool state = false, Sprite onicon = null, Sprite officon = null)
		{
			ActionMenuToggle actionMenuToggle = this;
			if (onicon != null)
			{
				_onImage = onicon;
			}
			if (officon != null)
			{
				_offImage = officon;
			}
			State = state;
			actionButton = new ActionMenuButton(basePage, text, delegate
			{
				actionMenuToggle.State = !actionMenuToggle.State;
				action(actionMenuToggle.State);
				if (actionMenuToggle.actionButton != null)
				{
					actionMenuToggle.actionButton.SetIcon(actionMenuToggle.State ? actionMenuToggle.onImage : actionMenuToggle.offImage);
				}
			}, state ? onicon : officon);
		}

		public void SetState(bool newState)
		{
			State = newState;
			actionButton.SetIcon(newState ? onImage : offImage);
		}
	}
}
