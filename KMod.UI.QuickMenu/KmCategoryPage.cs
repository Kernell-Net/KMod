        using System;
        using System.Collections;
        using System.Linq;
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
            public class KmCategoryPage : UiElement
            {
                private readonly bool _isRoot;
                private readonly Transform _container;
                private static bool _fixedLaunchpad;

                private static int SiblingIndex => MenuEx.QMenuParent.transform.Find("Modal_AddMessage").GetSiblingIndex();

                public UIPage UiPage { get; }

                public event System.Action OnOpen;
                public event System.Action OnClose;

                public KmCategoryPage(string text, bool isRoot = false, string color = "#ffffff")
                    : base(QMMenuPrefabs.CategoryPagePrefab, MenuEx.QMenuParent, "Menu_" + text, defaultState: false)
                {
                    KmCategoryPage reCategoryPage = this;

                    if (!_fixedLaunchpad)
                    {
                        FixLaunchpadScrolling();
                        _fixedLaunchpad = true;
                    }

                    // Configure scroll rect
                    VRCScrollRect scrollRect = base.RectTransform.GetComponentInChildren<VRCScrollRect>();
                    scrollRect.content.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
                    scrollRect.field_Public_Boolean_0 = true;
                    scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
                    scrollRect.m_VerticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
                    scrollRect.enabled = true;
                    scrollRect.verticalScrollbar = scrollRect.transform.Find("Scrollbar").GetComponent<Scrollbar>();

                    // Fix the mask
                    VRCRectMask2D mask = scrollRect.viewport.GetComponent<VRCRectMask2D>();
                    mask.enabled = true;
                    mask.prop_Boolean_0 = true;

                    // Kmmove the UIPage from the prefab and set sibling index
                    UnityEngine.Object.DestroyImmediate(base.GameObject.GetComponent<UIPage>());
                    base.RectTransform.SetSiblingIndex(SiblingIndex);

                    _isRoot = isRoot;

                    // Set header
                    Transform headerTransform = base.RectTransform.GetChild(0);
                    headerTransform.transform.Find("RightItemContainer/Button_QM_Expand").gameObject.SetActive(false);

                    // Setup title text
                    TextMeshProUGUI titleText = headerTransform.GetComponentInChildren<TextMeshProUGUI>();
                    MelonCoroutines.Start(WaitForActive());
                    titleText.text = $"<color={color}>{text}</color>";
                    titleText.richText = true;

                    // Show back button if not root
                    if (!_isRoot)
                    {
                        headerTransform.Find("LeftItemContainer/Button_Back").gameObject.SetActive(true);
                    }

                    // Set container to the scroll rect content and clear any existing children
                    _container = scrollRect.content;
                    Il2CppSystem.Collections.IEnumerator enumerator = _container.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            Il2CppSystem.Object current = enumerator.Current;
                            Transform child = current.Cast<Transform>();
                            if (child != null)
                            {
                                UnityEngine.Object.Destroy(child.gameObject);
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

                    // Create our new UIPage
                    UiPage = base.GameObject.AddComponent<UIPage>();
                    UiPage.field_Public_String_0 = "QuickMenuReMod" + UiElement.GetCleanName(text);
                    UiPage.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
                    UiPage.field_Private_List_1_UIPage_0.Add(UiPage);

                    UiPage.GetComponent<Canvas>().enabled = true;
                    UiPage.GetComponent<CanvasGroup>().enabled = true;
                    UiPage.GetComponent<UIPage>().enabled = true;
                    UiPage.GetComponent<GraphicRaycaster>().enabled = true;
                    UiPage.gameObject.active = false;

                    // Kmgister the page in QMenuStateCtrl
                    MenuEx.QMenuStateCtrl.field_Private_Dictionary_2_String_UIPage_0.Add(UiPage.field_Public_String_0, UiPage);

                    // If root, add it to the array of pages
                    if (isRoot)
                    {
                        var list = Enumerable.ToList(MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0);
                        list.Add(UiPage);
                        MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0 = list.ToArray();
                    }

                    // Hook up OnOpen/OnClose events
                    EnableDisableListener edListener = base.GameObject.AddComponent<EnableDisableListener>();
                    edListener.OnEnableEvent += () => { OnOpen?.Invoke(); };
                    edListener.OnDisableEvent += () => { OnClose?.Invoke(); };

                    // Wait for the GameObject to become active before updating title text
                    IEnumerator WaitForActive()
                    {
                        while (!reCategoryPage.GameObject.activeInHierarchy)
                        {
                            yield return null;
                        }
                        titleText.text = $"<color={color}>{text}</color>";
                    }
                }

                public KmCategoryPage(Transform transform)
                    : base(transform)
                {
                    UiPage = base.GameObject.GetComponent<UIPage>();
                    _isRoot = MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0.Contains(UiPage);
                    _container = base.RectTransform.GetComponentInChildren<VRCScrollRect>().content;
                }

                public void Open()
                {
                    UiPage.gameObject.active = true;
                    try
                    {
                     /*   MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_TransitionType_0(
                            UiPage.field_Public_String_0,
                            null,
                            param_3: false,
                            UIPage.TransitionType.None
                        );*/
                     
                     MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(UiPage.field_Public_String_0,null,false,UIPage.EnumNPublicSealedvaNoLeRiBoIn6vUnique.None);
                    }
                    catch (System.Exception)
                    {
                    }
                }

                public KmMenuCategory AddCategory(
                    string title,
                    bool collapsible = true,
                    string color = "#ffffff",
                    bool skipLayoutGroup = false
                )
                {
                    var existing = GetCategory(title);
                    if (existing != null) return existing;

                    Transform parentTransform = skipLayoutGroup ? _container.parent : _container;
                    return new KmMenuCategory(title, parentTransform, collapsible, color);
                }

                public KmMenuCategory AddCategory(string title, string color)
                {
                    return AddCategory(title, collapsible: true, color: color, skipLayoutGroup: false);
                }
                public void ClearCategories()
                {
                    // Clears all child elements from the container
                    for (int i = _container.childCount - 1; i >= 0; i--)
                    {
                        Transform child = _container.GetChild(i);
                        UnityEngine.Object.Destroy(child.gameObject);
                    }
                }

                public KmMenuCategory GetCategory(string name)
                {
                    Transform transform = _container.Find("Header_" + UiElement.GetCleanName(name));
                    if (transform == null)
                    {
                        return null;
                    }
                    var headerElement = new KmMenuHeader(transform);
                    var container = new KmMenuButtonContainer(_container.Find("Buttons_" + UiElement.GetCleanName(name)));
                    return new KmMenuCategory(headerElement, container);
                }

                public KmMenuSliderCategory AddSliderCategory(
                    string title,
                    bool collapsible = true,
                    string color = "#ffffff",
                    bool skipLayoutGroup = false
                )
                {
                    var existing = GetSliderCategory(title);
                    if (existing != null) return existing;

                    Transform parentTransform = skipLayoutGroup ? _container.parent : _container;
                    return new KmMenuSliderCategory(title, parentTransform, collapsible, color);
                }

                public KmMenuSliderCategory AddSliderCategory(string title, string color)
                {
                    return AddSliderCategory(title, collapsible: true, color: color, skipLayoutGroup: false);
                }

                public KmMenuSliderCategory GetSliderCategory(string name)
                {
                    Transform transform = _container.Find("Header_" + UiElement.GetCleanName(name));
                    if (transform == null)
                    {
                        return null;
                    }
                    var headerElement = new KmMenuHeader(transform);
                    var container = new KmMenuSliderContainer(_container.Find("Sliders_" + UiElement.GetCleanName(name)));
                    return new KmMenuSliderCategory(headerElement, container);
                }

                public KmNewMenuCategory AddNewCategory(
                    string title,
                    bool collapsible = true,
                    string color = "#ffffff",
                    bool skipLayoutGroup = false
                )
                {
                    var existing = GetNewCategory(title);
                    if (existing != null) return existing;

                    Transform parentTransform = skipLayoutGroup ? _container.parent : _container;
                    return new KmNewMenuCategory(title, parentTransform, collapsible, color);
                }

                public KmNewMenuCategory AddNewCategory(string title, string color)
                {
                    return AddNewCategory(title, collapsible: true, color: color, skipLayoutGroup: false);
                }

                public KmNewMenuCategory GetNewCategory(string name)
                {
                    Transform transform = _container.Find("Header_" + UiElement.GetCleanName(name));
                    if (transform == null)
                    {
                        return null;
                    }
                    var headerElement = new KmMenuHeader(transform);
                    var container = new KmNewMenuContainer(_container.Find("Sliders_" + UiElement.GetCleanName(name)));
                    return new KmNewMenuCategory(headerElement, container);
                }

                public static KmCategoryPage Create(string text, bool isRoot, string color = "#ffffff")
                {
                    return new KmCategoryPage(text, isRoot, color);
                }

                /// <summary>
                /// Clears all child elements (subcategories/buttons) from the container.
                /// </summary>
                public void ClearSubCategories()
                {
                    for (int i = _container.childCount - 1; i >= 0; i--)
                    {
                        Transform child = _container.GetChild(i);
                        UnityEngine.Object.Destroy(child.gameObject);
                    }
                }

                private static void FixLaunchpadScrolling()
                {
                    UIPage uiPage = MenuEx.QMDashboardMenu.GetComponent<UIPage>();
                    VRCScrollRect scrollRect = uiPage.GetComponentInChildren<VRCScrollRect>();
                    scrollRect.content.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
                    scrollRect.enabled = true;
                    scrollRect.verticalScrollbar = scrollRect.transform.Find("Scrollbar").GetComponent<Scrollbar>();
                    scrollRect.viewport.GetComponent<RectMask2D>().enabled = true;
                }
            }
        }
