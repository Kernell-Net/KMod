using System;
using System.Collections;
using System.Collections.Generic;
using MelonLoader;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.DataModel;
using VRC.Localization;
using VRC.UI;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Menus;

namespace KMod.VRChat
{
	public static class PopupManagerExtensions
	{
		private enum QMConfirmButtons
		{
			Button_Yes,
			Button_YesAlt,
			Button_No
		}

		public enum KeyBoardType
		{
			Standard,
			Numeric,
			Search
		}

		private static VRCInputField _KeyboardComponent;

		private static GameObject keyboardGameObject;

		private static GameObject QMCofirmPopupObj;

		private static readonly List<UnityAction> QMConfirmActionsActive;

		private static bool QMConfirmButtonIsReady;

		static PopupManagerExtensions()
		{
			QMConfirmActionsActive = new List<UnityAction>();
			QMConfirmButtonIsReady = true;
			MelonCoroutines.Start(wait());
			static IEnumerator wait()
			{
				while ((object)MenuEx.QMenuParent == null)
				{
					yield return null;
				}
				Transform originalButtons = MenuEx.QMenuParent.Find("Modal_ConfirmDialog/MenuPanel/Buttons");
				while ((object)originalButtons == null)
				{
					yield return null;
				}
				QMCofirmPopupObj = UnityEngine.Object.Instantiate(originalButtons.gameObject, originalButtons.parent);
				QMCofirmPopupObj.gameObject.SetActive(value: false);
			}
		}

		public static void Alert1Box(string title, string content, Action middleBtnAction = null, string middleBtnText = "Okay", Sprite icon = null)
		{
			MelonCoroutines.Start(TriggerQMConfirm(title, content, middleBtnAction ?? ((Action)delegate
			{
			}), null, null, middleBtnText, "", "", icon));
		}

		public static void Alert2Box(string title, string content, Action leftBtnAction = null, Action rightBtnAction = null, string leftBtnText = "Yes", string rightBtnText = "No", Sprite icon = null)
		{
			if (rightBtnText == "" && rightBtnAction == null)
			{
				MelonCoroutines.Start(TriggerQMConfirm(title, content, leftBtnAction ?? ((Action)delegate
				{
				}), null, null, leftBtnText, "", "", icon));
			}
			else
			{
				MelonCoroutines.Start(TriggerQMConfirm(title, content, null, leftBtnAction ?? ((Action)delegate
				{
				}), rightBtnAction ?? ((Action)delegate
				{
				}), "", leftBtnText, rightBtnText, icon));
			}
		}

		public static void Alert3Box(string title, string content, Action leftBtnAction = null, Action middleBtnAction = null, Action rightBtnAction = null, string leftBtnText = "Yes", string middleBtnText = "Maybe", string rightBtnText = "No", Sprite icon = null)
		{
			MelonCoroutines.Start(TriggerQMConfirm(title, content, middleBtnAction ?? ((Action)delegate
			{
			}), leftBtnAction ?? ((Action)delegate
			{
			}), rightBtnAction ?? ((Action)delegate
			{
			}), middleBtnText, leftBtnText, rightBtnText, icon));
		}

		private static IEnumerator TriggerQMConfirm(string title, string content, Action middleBtnAction = null, Action leftBtnAction = null, Action rightBtnAction = null, string middleBtnText = "Maybe", string leftBtnText = "Yes", string rightBtnText = "No", Sprite icon = null)
		{
			while (object.Equals(QMConfirmButtonIsReady, false))
			{
				yield return null;
			}
			yield return new WaitForSeconds(0.3f);
			MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0("ConfirmDialog", null, param_3: false, UIPage.EnumNPublicSealedvaNoLeRiBoIn6vUnique.None);
			Transform confirmmObj = MenuEx.QMenuParent.Find("Modal_ConfirmDialog/MenuPanel");
			Transform iconObj = confirmmObj.Find("QMHeader_H2/LeftItemContainer/Icon");
			MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique something = confirmmObj.parent.GetComponent<MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique>();
			int buttons = 0;
			MelonCoroutines.Start(QMConfirmIgnoredWait(confirmmObj, iconObj, something));
			confirmmObj.Find("Buttons").gameObject.SetActive(value: false);
			QMCofirmPopupObj.SetActive(value: true);
			if (leftBtnAction != null)
			{
				buttons++;
			}
			if (middleBtnAction != null)
			{
				buttons++;
			}
			if (rightBtnAction != null)
			{
				buttons++;
			}
			switch (buttons)
			{
				case 1:
					SetUpQMConfirmButton(confirmmObj, iconObj, something, QMConfirmButtons.Button_Yes, middleBtnText, middleBtnAction);
					QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(QMConfirmButtons), QMConfirmButtons.Button_YesAlt)).gameObject.SetActive(value: false);
					QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(QMConfirmButtons), QMConfirmButtons.Button_No)).gameObject.SetActive(value: false);
					break;
				case 2:
					SetUpQMConfirmButton(confirmmObj, iconObj, something, QMConfirmButtons.Button_Yes, leftBtnText, leftBtnAction);
					SetUpQMConfirmButton(confirmmObj, iconObj, something, QMConfirmButtons.Button_No, rightBtnText, rightBtnAction);
					QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(QMConfirmButtons), QMConfirmButtons.Button_YesAlt)).gameObject.SetActive(value: false);
					break;
				case 3:
					SetUpQMConfirmButton(confirmmObj, iconObj, something, QMConfirmButtons.Button_Yes, leftBtnText, leftBtnAction);
					SetUpQMConfirmButton(confirmmObj, iconObj, something, QMConfirmButtons.Button_YesAlt, middleBtnText, middleBtnAction);
					SetUpQMConfirmButton(confirmmObj, iconObj, something, QMConfirmButtons.Button_No, rightBtnText, rightBtnAction);
					break;
				default:
					middleBtnAction = delegate
					{
					};
					SetUpQMConfirmButton(confirmmObj, iconObj, something, QMConfirmButtons.Button_Yes, middleBtnText, middleBtnAction);
					QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(QMConfirmButtons), QMConfirmButtons.Button_YesAlt)).gameObject.SetActive(value: false);
					QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(QMConfirmButtons), QMConfirmButtons.Button_No)).gameObject.SetActive(value: false);
					break;
			}
			if (icon != null)
			{
				iconObj.gameObject.SetActive(value: true);
				ImageEx iconCom = iconObj.GetComponent<ImageEx>();
				((Image)(object)iconCom).overrideSprite = icon;
			}
			Transform titleObj = confirmmObj.Find("QMHeader_H2/LeftItemContainer/Text_Title");
			TextMeshProUGUI titleCom = titleObj.GetComponent<TextMeshProUGUI>();
			titleCom.text = title;
			Transform contentObj = confirmmObj.Find("Text_Body");
			TextMeshProUGUI contentCom = contentObj.GetComponent<TextMeshProUGUI>();
			contentCom.text = content;
		}

		private static IEnumerator QMConfirmIgnoredWait(Transform confirmmObj, Transform iconObj, MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique something)
		{
			while (object.Equals(something.Method_Public_UIPage_0().prop_Boolean_2, true))
			{
				yield return null;
			}
			QMConfirmPopupCleanUp(confirmmObj, iconObj, something);
		}

		private static void SetUpQMConfirmButton(Transform confirmmObj, Transform iconObj, MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique something, QMConfirmButtons qmConfirmButtons, string BtnText, Action BtnAction)
		{
			Transform transform = QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(QMConfirmButtons), qmConfirmButtons));
			transform.gameObject.SetActive(value: true);
			Transform transform2 = transform.Find("Text_MM_H3");
			TextMeshProUGUI component = transform2.GetComponent<TextMeshProUGUI>();
			Button component2 = transform.GetComponent<Button>();
			component.text = BtnText;
			UnityAction call = DelegateSupport.ConvertDelegate<UnityAction>((Action)delegate
			{
				QMConfirmPopupCleanUp(confirmmObj, iconObj, something, closePopup: true);
			});
			component2.onClick.RemoveListener(call);
			component2.onClick.AddListener(call);
			foreach (UnityAction item in QMConfirmActionsActive)
			{
				component2.onClick.RemoveListener(item);
			}
			QMConfirmActionsActive.Add(BtnAction);
			component2.onClick.AddListener(BtnAction);
		}

		private static void QMConfirmPopupCleanUp(Transform confirmmObj, Transform iconObj, MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique something, bool closePopup = false)
		{
			if (closePopup)
			{
				something.Method_Private_Void_0();
			}
			confirmmObj.Find("Buttons").gameObject.SetActive(value: true);
			QMCofirmPopupObj.SetActive(value: false);
			if (iconObj.gameObject.activeInHierarchy)
			{
				ImageEx component = iconObj.GetComponent<ImageEx>();
				((Image)(object)component).overrideSprite = null;
				iconObj.gameObject.SetActive(value: false);
				QMConfirmButtonIsReady = true;
			}
		}

		public static void InputPopup(string Title, Action<string> EndString, Action<string> KmalTimeString = null, Action OnClose = null, KeyBoardType keyBoardType = KeyBoardType.Standard, string Placeholder = "Enter Text", string OkButton = "OK", string CancelButton = "Cancel", bool MultiLine = true, int CharLimit = 0, bool KeepOpen = false, bool KmadOnly = false)
		{
			if (_KeyboardComponent == null)
			{
				keyboardGameObject = new GameObject("KMod_KeyBoard");
				UnityEngine.Object.DontDestroyOnLoad(keyboardGameObject);
				_KeyboardComponent = keyboardGameObject.AddComponent<VRCInputField>();
			}
			try
			{
				KeyboardData keyboardData = new KeyboardData();
				KeyboardData keyboardData2 = keyboardData.Method_Public_KeyboardData_LocalizableString_LocalizableString_String_LocalizableString_LocalizableString_0(LocalizableStringExtensions.Localize(Title), LocalizableStringExtensions.Localize(Placeholder), "", LocalizableStringExtensions.Localize(OkButton), LocalizableStringExtensions.Localize(CancelButton));
				KeyboardData keyboardData3 = keyboardData2.Method_Public_KeyboardData_Action_1_String_Action_1_String_Action_Boolean_PDM_0(KmalTimeString, EndString, OnClose, KeepOpen);
				KeyboardData keyboardData4 = keyboardData3.Method_Public_KeyboardData_EnumPublicSealedvaStNuSe4vUnique_Boolean_PDM_0((EnumPublicSealedvaStNuSe4vUnique)keyBoardType, true);
				KeyboardData keyboardData5 = keyboardData4.Method_Public_KeyboardData_InputType_ContentType_Int32_Boolean_Boolean_InterfacePublicAbstractBoStVoAc1VoAcSt1BoUnique_PDM_0(TMP_InputField.InputType.Standard, TMP_InputField.ContentType.Standard, CharLimit, MultiLine, KmadOnly);
				_KeyboardComponent.Method_Private_Void_PDM_0();
			}
			catch
			{
			}
		}
	}
}
