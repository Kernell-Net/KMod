using System;
using System.Collections.Generic;
using Il2CppSystem.Collections.Generic;
using KMod.Unity;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Menus;

namespace KMod.UI.QuickMenu
{
	public class KmRadioTogglePage : UiElement
	{
		private static GameObject _menuPrefab;

		private TextMeshProUGUI _titleText;

		private GameObject _toggleGroupRoot;

		private System.Collections.Generic.List<Tuple<string, object>> _radioElementSource = new System.Collections.Generic.List<Tuple<string, object>>();

		private System.Collections.Generic.List<KmRadioToggle> _radioElements = new System.Collections.Generic.List<KmRadioToggle>();

		private bool _isUpdated;

		private readonly Transform _container;

		private static GameObject MenuPrefab
		{
			get
			{
				if (_menuPrefab == null)
				{
					_menuPrefab = MenuEx.QMInstance.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_ChangeAudioInputDevice").gameObject;
				}
				return _menuPrefab;
			}
		}

		private static int SiblingIndex => MenuEx.QMInstance.transform.Find("CanvasGroup/Container/Window/QMParent/Modal_AddMessage").GetSiblingIndex();

		public string TitleText
		{
			set
			{
				_titleText.text = value;
			}
		}

		public UIPage UiPage { get; }

		public event Action OnOpen;

		public event Action OnClose;

		public event Action<object> OnSelect;

		public KmRadioTogglePage(string name)
			: base(MenuPrefab, MenuEx.QMenuParent, "Menu_" + name, defaultState: false)
		{
			Transform child = base.RectTransform.GetChild(0);
			_titleText = child.GetComponentInChildren<TextMeshProUGUI>();
			_titleText.text = name;
			_titleText.richText = true;
			_container = base.RectTransform.GetComponentInChildren<ScrollRect>().content;
			MonoBehaviour1Public54Ga16ObGaVoOb58TeGaUnique component = base.RectTransform.GetComponent<MonoBehaviour1Public54Ga16ObGaVoOb58TeGaUnique>();
			_toggleGroupRoot = component.gameObject;
			UnityEngine.Object.DestroyImmediate(_toggleGroupRoot.gameObject.GetComponent<MonoBehaviourPublicSt_cReGa_c_eBo_fObAcUnique>());
			UnityEngine.Object.DestroyImmediate(_toggleGroupRoot.gameObject.GetComponent<RadioButton>());
			UnityEngine.Object.DestroyImmediate(component);
			UiPage = base.GameObject.GetComponent<UIPage>();
			UiPage.field_Public_String_0 = "QuickMenuReMod" + UiElement.GetCleanName(name);
			//UiPage.field_Protected_MenuStateController_0 = MenuEx.QMenuStateCtrl;
			//UiPage.field_Private_List_1_MonoBehaviourPublicTe_lRaBoAsObAcObBoTeUnique_0 = MenuEx.QMenuStateCtrl;
			UiPage.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
			UiPage.field_Private_List_1_UIPage_0.Add(UiPage);
			MenuEx.QMenuStateCtrl.field_Private_Dictionary_2_String_UIPage_0.Add(UiPage.field_Public_String_0, UiPage);
			EnableDisableListener enableDisableListener = base.GameObject.AddComponent<EnableDisableListener>();
			enableDisableListener.OnEnableEvent += delegate
			{
				this.OnOpen?.Invoke();
			};
			enableDisableListener.OnDisableEvent += delegate
			{
				this.OnClose?.Invoke();
			};
		}

		public void Open(object selected = null)
		{
		//	MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_TransitionType_0(UiPage.field_Public_String_0, null, param_3: false, UIPage.EnumNPublicSealedvaNoLeRiBoIn6vUnique.None);
		MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(UiPage.field_Public_String_0,null,false,UIPage.EnumNPublicSealedvaNoLeRiBoIn6vUnique.None);
			if (_isUpdated)
			{
				_isUpdated = false;
				foreach (KmRadioToggle radioElement in _radioElements)
				{
					UnityEngine.Object.DestroyImmediate(radioElement.GameObject);
				}
				_radioElements.Clear();
				foreach (Tuple<string, object> item in _radioElementSource)
				{
					KmRadioToggle reRadioToggle = new KmRadioToggle(_toggleGroupRoot.transform, item.Item1, item.Item1, item.Item2);
					reRadioToggle.ToggleStateUpdated = (Action<KmRadioToggle, bool>)Delegate.Combine(reRadioToggle.ToggleStateUpdated, new Action<KmRadioToggle, bool>(OnToggleSelect));
					_radioElements.Add(reRadioToggle);
				}
			}
			if (selected == null)
			{
				return;
			}
			foreach (KmRadioToggle radioElement2 in _radioElements)
			{
				radioElement2.SetToggle(radioElement2.ToggleData.Equals(selected));
			}
		}

		private void OnToggleSelect(KmRadioToggle toggle, bool state)
		{
			foreach (KmRadioToggle radioElement in _radioElements)
			{
				if (radioElement != toggle)
				{
					radioElement.SetToggle(state: false);
				}
			}
			this.OnSelect?.Invoke(toggle.ToggleData);
		}

		public void AddItem(string name, object obj)
		{
			_radioElementSource.Add(new Tuple<string, object>(name, obj));
			_isUpdated = true;
		}

		public void ClearItems()
		{
			_radioElementSource.Clear();
			_isUpdated = true;
		}
	}
}
