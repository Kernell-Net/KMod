using System;
using UnityEngine;

namespace KMod.UI.Wings
{
	public class KmMirroredWingButton
	{
		private readonly KmWingButton _leftButton;

		private readonly KmWingButton _rightButton;

		public KmMirroredWingButton(string text, string tooltip, Action onClick, Transform leftParent, Transform rightParent, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
		{
			_leftButton = new KmWingButton(text, tooltip, onClick, leftParent, sprite, arrow, background, separator);
			_rightButton = new KmWingButton(text, tooltip, onClick, rightParent, sprite, arrow, background, separator);
		}

		public void Destroy()
		{
			UnityEngine.Object.DestroyImmediate(_leftButton.GameObject);
			UnityEngine.Object.DestroyImmediate(_rightButton.GameObject);
		}
	}
}
