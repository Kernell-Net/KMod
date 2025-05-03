using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using KMod.Unity;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;
using IEnumerator = System.Collections.IEnumerator;

namespace KMod.UI.QuickMenu
{
    public class KmTabbedPage : UiElement
    {
        private readonly bool _isRoot;
        private readonly Transform _container;
        public Transform tabContainer;
        public Transform contentContainer;
        private static int SiblingIndex => MenuEx.QMenuParent.transform.Find("Modal_AddMessage").GetSiblingIndex();
        public UIPage UiPage { get; set; }
        public event System.Action OnOpen;
        public event System.Action OnClose;

        public KmTabbedPage(string text, bool isRoot = false, string color = "#ffffff")
            : base(QMMenuPrefabs.TabbedPagePrefab, MenuEx.QMenuParent, "Menu_" + text, defaultState: false)
        {
            KmTabbedPage reTabbedPage = this;
            UnityEngine.Object.DestroyImmediate(GameObject.GetComponent<MonoBehaviour2PublicBuObBu_sOb_rGaLiOb1Unique>());
            RectTransform.SetSiblingIndex(SiblingIndex);
            string cleanName = UiElement.GetCleanName(text);
            _isRoot = isRoot;
            
            // Configure header and title
            Transform header = RectTransform.GetChild(0);
            TextMeshProUGUI titleText = header.GetComponentInChildren<TextMeshProUGUI>();
            MelonCoroutines.Start(WaitForActive());
            titleText.richText = true;
            header.transform.Find("RightItemContainer/Button_QM_Expand").gameObject.SetActive(false);
            
            // Set up tab container and clear existing tabs
            tabContainer = RectTransform.Find("Panel_Notification_Tabs/Tabs");
            ClearContainer(tabContainer);
            
            // Set up content container and clear existing content
            contentContainer = RectTransform.Find("Panel_Content/");
            ClearContainer(contentContainer);
            
            // Enable first content panel
            contentContainer.GetChild(0).gameObject.SetActive(true);
            
            // Kmmove unnecessary components
            UnityEngine.Object.DestroyImmediate(RectTransform.Find("Panel_NoNotifications_Message").gameObject);
            
            // Configure back button based on root status
            if (!_isRoot)
            {
                Button backButton = header.GetComponentInChildren<Button>(includeInactive: true);
                backButton.gameObject.SetActive(true);
            }
            
            // Set up UIPage
            ConfigureUIPage(cleanName, isRoot);
            
            // Add event listeners
            AddEventListeners();
            
            IEnumerator WaitForActive()
            {
                while (GameObject.activeInHierarchy == false)
                {
                    yield return null;
                }
                titleText.text = $"<color={color}>{text}</color>";
            }
        }
        
        private void ClearContainer(Transform container)
        {
            if (container == null) return;
            
            Il2CppSystem.Collections.IEnumerator enumerator = container.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform transform = enumerator.Current.Cast<Transform>();
                    if (transform != null)
                    {
                        UnityEngine.Object.Destroy(transform.gameObject);
                    }
                }
            }
            finally
            {
                if (enumerator is System.IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
        
        private void ConfigureUIPage(string cleanName, bool isRoot)
        {
            UiPage = GameObject.AddComponent<UIPage>();
            UiPage.field_Public_String_0 = "QuickMenuReMod" + cleanName;
            UiPage.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
            UiPage.field_Private_List_1_UIPage_0.Add(UiPage);
            
            // Enable necessary components
            UiPage.GetComponent<Canvas>().enabled = true;
            UiPage.GetComponent<CanvasGroup>().enabled = true;
            UiPage.GetComponent<UIPage>().enabled = true;
            UiPage.GetComponent<GraphicRaycaster>().enabled = true;
            UiPage.gameObject.active = false;
            
            // Kmgister with menu controller
            KmgisterWithMenuController(isRoot);
        }
        
        private void KmgisterWithMenuController(bool isRoot)
        {
            MenuEx.QMenuStateCtrl.field_Private_Dictionary_2_String_UIPage_0.Add(UiPage.field_Public_String_0, UiPage);
            if (isRoot)
            {
                System.Collections.Generic.List<UIPage> list = MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0.ToList();
                list.Add(UiPage);
                MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0 = list.ToArray();
            }
        }
        
        private void AddEventListeners()
        {
            EnableDisableListener listener = GameObject.AddComponent<EnableDisableListener>();
            listener.OnEnableEvent += () => OnOpen?.Invoke();
            listener.OnDisableEvent += () => OnClose?.Invoke();
        }

        public static KmTabbedPage Create(string text, bool isRoot, string color = "#ffffff")
        {
            return new KmTabbedPage(text, isRoot, color);
        }

        public KmTabbedPage(Transform transform)
            : base(transform)
        {
            UiPage = GameObject.GetComponent<UIPage>();
            _isRoot = MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0.Contains(UiPage);
            ScrollRect scrollRect = RectTransform.Find("Scrollrect").GetComponent<ScrollRect>();
            _container = scrollRect.content;
        }

        public void Open()
        {
            UiPage.gameObject.active = true;
            contentContainer.GetChild(0).gameObject.SetActive(true);
            MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(
                UiPage.field_Public_String_0, null, false, UIPage.EnumNPublicSealedvaNoLeRiBoIn6vUnique.None);
        }

        public KmTab AddTab(string title, string color = "#ffffff")
        {
            return new KmTab(title, color, tabContainer);
        }

        public KmTabbedPage GetTabbedPage(string name)
        {
            Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
            return (transform == null) ? null : new KmTabbedPage(transform);
        }

        public static void FixLaunchpadScrolling()
        {
            UIPage dashboardPage = MenuEx.QMDashboardMenu.GetComponent<UIPage>();
            ScrollRect scrollRect = dashboardPage.GetComponentInChildren<ScrollRect>();
            scrollRect.content.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
            scrollRect.enabled = true;
            scrollRect.verticalScrollbar = scrollRect.transform.Find("Scrollbar").GetComponent<Scrollbar>();
            scrollRect.viewport.GetComponent<RectMask2D>().enabled = true;
        }
    }
}