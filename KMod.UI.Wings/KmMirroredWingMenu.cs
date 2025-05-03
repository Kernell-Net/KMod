using System;
using KMod.VRChat;
using UnityEngine;

namespace KMod.UI.Wings
{
	public class KmMirroredWingMenu
	{
		private KmWingMenu _leftMenu;

		private KmWingMenu _rightMenu;

		public bool Active
		{
			get
			{
				return _leftMenu.Active && _rightMenu.Active;
			}
			set
			{
				_leftMenu.Active = value;
				_rightMenu.Active = value;
			}
		}

		public KmMirroredWingMenu(string text, string tooltip, Transform leftParent, Transform rightParent, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
		{
			_leftMenu = new KmWingMenu(text);
			_rightMenu = new KmWingMenu(text, left: false);
			KmWingButton.Create(text, tooltip, _leftMenu.Open, leftParent, sprite, arrow, background, separator);
			KmWingButton.Create(text, tooltip, _rightMenu.Open, rightParent, sprite, arrow, background, separator);
		}

		public static KmMirroredWingMenu Create(string text, string tooltip, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
		{
			return new KmMirroredWingMenu(text, tooltip, MenuEx.QMLeftWing.transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"), MenuEx.QMRightWing.transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"), sprite, arrow, background, separator);
		}

		public KmMirroredWingButton AddButton(string text, string tooltip, Action onClick, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
		{
			if (_leftMenu == null || _rightMenu == null)
			{
				throw new NullReferenceException("This wing menu has been destroyed.");
			}
			return new KmMirroredWingButton(text, tooltip, onClick, _leftMenu.Container, _rightMenu.Container, sprite, arrow, background, separator);
		}

		public KmMirroredWingToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue)
		{
			if (_leftMenu == null || _rightMenu == null)
			{
				throw new NullReferenceException("This wing menu has been destroyed.");
			}
			return new KmMirroredWingToggle(text, tooltip, onToggle, _leftMenu.Container, _rightMenu.Container, defaultValue);
		}

		public KmMirroredWingMenu AddSubMenu(string text, string tooltip, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
		{
			if (_leftMenu == null || _rightMenu == null)
			{
				throw new NullReferenceException("This wing menu has been destroyed.");
			}
			return new KmMirroredWingMenu(text, tooltip, _leftMenu.Container, _rightMenu.Container, sprite, arrow, background, separator);
		}

		public void Destroy()
		{
			_leftMenu.Destroy();
			_rightMenu.Destroy();
			_leftMenu = null;
			_rightMenu = null;
		}
	}
}
