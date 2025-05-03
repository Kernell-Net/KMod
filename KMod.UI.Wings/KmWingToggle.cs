using System;
using KMod.VRChat;
using UnityEngine;

namespace KMod.UI.Wings
{
	public class KmWingToggle
	{
		private readonly KmWingButton _button;

		private readonly Action<bool> _onToggle;

		private bool _state;

		public bool Interactable
		{
			get
			{
				return _button.Interactable;
			}
			set
			{
				_button.Interactable = value;
			}
		}

		public KmWingToggle(string text, string tooltip, Action<bool> onToggle, Transform parent, bool defaultValue = false)
		{
			_onToggle = onToggle;
			_button = new KmWingButton(text, tooltip, delegate
			{
				Toggle(!_state);
			}, parent, GetCurrentIcon(), arrow: false);
			Toggle(defaultValue);
		}

		private Sprite GetCurrentIcon()
		{
			return _state ? MenuEx.OnIconSprite : MenuEx.OffIconSprite;
		}

		public void Toggle(bool b, bool callback = true)
		{
			if (_state != b)
			{
				_state = b;
				_button.Sprite = GetCurrentIcon();
				if (callback)
				{
					_onToggle(_state);
				}
			}
		}
	}
}
