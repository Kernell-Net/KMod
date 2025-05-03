using System;
using UnityEngine;

namespace KMod.UI.Wings
{
	public class KmMirroredWingToggle
	{
		private readonly KmWingToggle _leftToggle;

		private readonly KmWingToggle _rightToggle;

		public bool Interactable
		{
			get
			{
				return _leftToggle.Interactable;
			}
			set
			{
				_leftToggle.Interactable = value;
				_rightToggle.Interactable = value;
			}
		}

		public KmMirroredWingToggle(string text, string tooltip, Action<bool> onToggle, Transform leftParent, Transform rightParent, bool defaultValue = false)
		{
			KmMirroredWingToggle reMirroredWingToggle = this;
			_leftToggle = new KmWingToggle(text, tooltip, delegate(bool b)
			{
				reMirroredWingToggle._rightToggle?.Toggle(b, callback: false);
				onToggle(b);
			}, leftParent, defaultValue);
			_rightToggle = new KmWingToggle(text, tooltip, delegate(bool b)
			{
				reMirroredWingToggle._leftToggle.Toggle(b, callback: false);
				onToggle(b);
			}, rightParent, defaultValue);
		}

		public void Toggle(bool b, bool callback = true)
		{
			_leftToggle.Toggle(b, callback);
			_rightToggle.Toggle(b, callback);
		}
	}
}
