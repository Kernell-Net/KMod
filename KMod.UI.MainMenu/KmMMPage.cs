using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using KMod.UI.MainMenu.Header;
using KMod.Unity;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Menus;

namespace KMod.UI.MainMenu
{
    public class KmMMPage : UiElement
    {
        private readonly bool _isRoot;
        private readonly Transform _container;
        private Sprite _pageIcon;

        public UIPage UiPage { get; }
        public GameObject MenuObject { get; private set; }
        internal TextMeshProUGUI MenuTitleText { get; private set; }

        public event Action OnOpen;
        public event Action OnClose;

        /// <summary>
        /// Creates a new custom menu page
        /// </summary>
        internal KmMMPage(string text, Sprite icon, bool isRoot = false, string color = "#ffffff")
            : base(null, null, "Menu_" + text, defaultState: false)
        {
            try
            {
                // Check if required references exist
                if (MenuEx.MMSettingsMenu == null)
                {
                    Debug.LogError("[KernellClient] MenuEx.MMSettingsMenu is null, cannot create page");
                    return;
                }

                if (MenuEx.QMenuParent == null)
                {
                    Debug.LogError("[KernellClient] MenuEx.QMenuParent is null, cannot create page");
                    return;
                }

                if (MenuEx.MMenuStateCtrl == null)
                {
                    Debug.LogError("[KernellClient] MenuEx.MMenuStateCtrl is null, cannot create page");
                    return;
                }

                // Initialize with proper references
                base.GameObject = new GameObject("Menu_" + UiElement.GetCleanName(text));
                base.GameObject.transform.SetParent(MenuEx.QMenuParent.transform, false);

                KmMMPage reMMPage = this;
                _pageIcon = icon;
                string cleanName = UiElement.GetCleanName(text);

                // Create the menu by cloning the settings menu
                MenuObject = UnityEngine.Object.Instantiate(MenuEx.MMSettingsMenu, MenuEx.MMSettingsMenu.transform.parent);
                if (MenuObject == null)
                {
                    Debug.LogError("[KernellClient] Failed to instantiate MenuObject");
                    return;
                }
                
                MenuObject.name = "Menu_" + cleanName;
                MenuObject.transform.SetSiblingIndex(19);

                // Assign base GameObject
                UnityEngine.Object.Destroy(base.GameObject);
                base.GameObject = MenuObject;

                // Kmmove unnecessary components
                SettingsCategory settingsPage = MenuObject.GetComponent<SettingsCategory>();
                if (settingsPage != null)
                {
                    UnityEngine.Object.DestroyImmediate(settingsPage);
                }
                
                // Find and remove search field if it exists
                GameObject searchField = FindComponentInObject(MenuObject, "Field_MM_TextSearchField");
                if (searchField != null)
                {
                    UnityEngine.Object.DestroyImmediate(searchField);
                }

                // Set up the UI page component
                UiPage = MenuObject.AddComponent<UIPage>();
                UiPage.field_Public_String_0 = "MainMenuReMod" + cleanName;
                UiPage.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
                UiPage.field_Private_List_1_UIPage_0.Add(UiPage);

                // Enable required components
                Canvas canvas = UiPage.GetComponent<Canvas>();
                if (canvas != null) canvas.enabled = true;
                
                CanvasGroup canvasGroup = UiPage.GetComponent<CanvasGroup>();
                if (canvasGroup != null) canvasGroup.enabled = true;
                
                UiPage.enabled = true;
                
                GraphicRaycaster raycaster = UiPage.GetComponent<GraphicRaycaster>();
                if (raycaster != null) raycaster.enabled = true;
                
                UiPage.gameObject.SetActive(false);

                // Kmgister the page with the menu controller
                try
                {
                    MenuEx.MMenuStateCtrl.field_Private_Dictionary_2_String_UIPage_0.Add(UiPage.field_Public_String_0, UiPage);
                    
                    // Add the page to the array
                    if (MenuEx.MMenuStateCtrl.field_Public_ArrayOf_UIPage_0 != null)
                    {
                        System.Collections.Generic.List<UIPage> pageList = Enumerable.ToList(MenuEx.MMenuStateCtrl.field_Public_ArrayOf_UIPage_0);
                        pageList.Add(UiPage);
                        MenuEx.MMenuStateCtrl.field_Public_ArrayOf_UIPage_0 = pageList.ToArray();
                    }
                    else
                    {
                        Debug.LogError("[KernellClient] MenuEx.MMenuStateCtrl.field_Public_ArrayOf_UIPage_0 is null");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[KernellClient] Failed to register page with menu controller: {ex.Message}");
                }

                // Set up enable/disable listeners for events
                EnableDisableListener enableDisableListener = base.GameObject.AddComponent<EnableDisableListener>();
                enableDisableListener.OnEnableEvent += delegate
                {
                    if (reMMPage.OnOpen != null)
                    {
                        reMMPage.OnOpen();
                    }
                };
                enableDisableListener.OnDisableEvent += delegate
                {
                    if (reMMPage.OnClose != null)
                    {
                        reMMPage.OnClose();
                    }
                };

                // Clean up existing category containers
                CleanupCategoryContainers();

                // Set up the title
                MenuTitleText = FindTitleText();
                if (MenuTitleText != null)
                {
                    MenuTitleText.richText = true;
                }

                // Clean up header buttons
                SetupHeaderButtons();

                // Hide default title
                GameObject defaultTitle = FindComponentInObject(MenuObject, "Text_Title");
                if (defaultTitle != null)
                {
                    defaultTitle.SetActive(false);
                }

                // Start coroutine to set title text after the GameObject is active
                MelonCoroutines.Start(SetTitleWhenActive(text, color));
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error creating KmMMPage: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Creates a KmMMPage from an existing transform
        /// </summary>
        public KmMMPage(Transform transform)
            : base(transform)
        {
            try
            {
                if (transform == null)
                {
                    Debug.LogError("[KernellClient] Cannot create KmMMPage from null transform");
                    return;
                }

                UiPage = base.GameObject.GetComponent<UIPage>();
                
                if (MenuEx.QMenuStateCtrl != null && MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0 != null)
                {
                    _isRoot = MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0.Contains(UiPage);
                }
                else
                {
                    _isRoot = false;
                }
                
                GameObject scrollRect = FindComponentInObject(base.GameObject, "Scrollrect");
                if (scrollRect != null)
                {
                    ScrollRect component = scrollRect.GetComponent<ScrollRect>();
                    if (component != null)
                    {
                        _container = component.content;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error creating KmMMPage from transform: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the full path of a transform in the hierarchy
        /// </summary>
        private string GetFullPath(Transform transform)
        {
            if (transform == null) return "null";
            
            string path = transform.name;
            Transform parent = transform.parent;
            
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            
            return path;
        }

        /// <summary>
        /// Kmcursively searches for a GameObject by name in an object
        /// </summary>
        private GameObject FindComponentInObject(GameObject parent, string name)
        {
            if (parent == null || string.IsNullOrEmpty(name)) return null;

            // Check this object first
            if (parent.name == name)
                return parent;
                
            // Check direct children
            Transform direct = parent.transform.Find(name);
            if (direct != null)
                return direct.gameObject;
                
            // Search recursively
            foreach (Transform child in parent.transform)
            {
                if (child == null) continue;
                
                // Skip QM elements
                if (child.name.Contains("QM") || child.name.Contains("QuickMenu"))
                    continue;
                    
                GameObject found = FindComponentInObject(child.gameObject, name);
                if (found != null)
                {
                    //Debug.Log($"[KernellClient] Found {name} at: {GetFullPath(found.transform)}");
                    return found;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Cleans up all category containers in the menu
        /// </summary>
        private void CleanupCategoryContainers()
        {
            try
            {
                if (MenuObject == null) return;
                
                // Find all VerticalLayoutGroup components that might contain categories
                VerticalLayoutGroup[] layoutGroups = MenuObject.GetComponentsInChildren<VerticalLayoutGroup>(true);
                
                if (layoutGroups == null) return;
                
                foreach (var group in layoutGroups)
                {
                    if (group == null || group.transform == null) continue;
                    
                    // Skip if this is a header's parent
                    if (group.transform.Find("DynamicSidePanel_Header") != null)
                        continue;
                        
                    // Clean children except headers
                    for (int i = group.transform.childCount - 1; i >= 0 ; i--)
                    {
                        Transform child = group.transform.GetChild(i);
                        if (child != null && child.name != "DynamicSidePanel_Header")
                        {
                            UnityEngine.Object.Destroy(child.gameObject);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error cleaning category containers: {ex.Message}");
            }
        }

        /// <summary>
        /// Finds the title text component
        /// </summary>
        private TextMeshProUGUI FindTitleText()
        {
            try
            {
                if (MenuObject == null) return null;
                
                GameObject titleTextObj = FindComponentInObject(MenuObject, "Text_Name");
                return titleTextObj != null ? titleTextObj.GetComponent<TextMeshProUGUI>() : null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error finding title text: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Sets up the header buttons by removing logout and exit buttons
        /// </summary>
        private void SetupHeaderButtons()
        {
            try
            {
                if (MenuTitleText == null) return;
                
                Transform parent = MenuTitleText.transform.parent;
                if (parent == null) return;

                // Kmmove logout and exit buttons
                GameObject logoutButton = FindComponentInObject(parent.gameObject, "Button_Logout");
                if (logoutButton != null)
                {
                    UnityEngine.Object.Destroy(logoutButton);
                }

                GameObject exitButton = FindComponentInObject(parent.gameObject, "Button_Exit");
                if (exitButton != null)
                {
                    UnityEngine.Object.Destroy(exitButton);
                }

                // Adjust separator position
                GameObject separator = FindComponentInObject(parent.gameObject, "Separator");
                if (separator != null)
                {
                    RectTransform rectTransform = separator.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.anchoredPosition = new Vector2(0f, -100f);
                    }
                }

                // Set layout element height
                LayoutElement layoutElement = parent.GetComponent<LayoutElement>();
                if (layoutElement != null)
                {
                    layoutElement.minHeight = 100f;
                }

                // Update icon if it exists
                GameObject iconObj = FindComponentInObject(parent.gameObject, "Icon");
                if (iconObj != null && _pageIcon != null)
                {
                    Image iconImage = iconObj.GetComponent<Image>();
                    if (iconImage != null)
                    {
                        iconImage.sprite = _pageIcon;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error setting up header buttons: {ex.Message}");
            }
        }

        /// <summary>
        /// Coroutine to set the title text once the GameObject is active
        /// </summary>
        private IEnumerator SetTitleWhenActive(string text, string color)
        {
            while (MenuObject != null && !MenuObject.activeInHierarchy)
            {
                yield return null;
            }
            
            try
            {
                if (MenuTitleText != null)
                {
                    MenuTitleText.text = "<color=" + color + ">" + text + "</color>";
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error setting title text: {ex.Message}");
            }
        }

        /// <summary>
        /// Opens this menu page
        /// </summary>
        public void Open()
        {
            try
            {
                if (UiPage == null)
                {
                    Debug.LogError("[KernellClient] Cannot open page: UiPage is null");
                    return;
                }
                
                UiPage.gameObject.SetActive(true);
                
                if (MenuEx.QMenuStateCtrl != null)
                {
                MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(UiPage.field_Public_String_0,null,false, UIPage.EnumNPublicSealedvaNoLeRiBoIn6vUnique.None);
                }
                
                OnOpen?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error opening page: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the container for category buttons
        /// </summary>
        internal Transform GetCategoryButtonContainer()
        {
            try
            {
                if (MenuObject == null) return null;
                
                // First try to find a container that looks like a category button container
                VerticalLayoutGroup[] layoutGroups = MenuObject.GetComponentsInChildren<VerticalLayoutGroup>(true);
                
                if (layoutGroups != null)
                {
                    foreach (var group in layoutGroups)
                    {
                        if (group == null || group.transform == null || group.transform.parent == null) continue;
                        
                        // Check for characteristics of a category button container
                        if (group.transform.parent.name.Contains("Viewport") && 
                            group.transform.parent.name.Contains("Navigation"))
                        {
                            return group.transform;
                        }
                    }
                }
                
                // Fallback to direct search
                GameObject container = FindComponentInObject(MenuObject, "VerticalLayoutGroup");
                if (container == null)
                {
                    Debug.LogError("[KernellClient] Could not find category button container");
                }
                
                return container?.transform;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error getting category button container: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gets the container for category child elements
        /// </summary>
        internal Transform GetCategoryChildContainer()
        {
            try
            {
                if (MenuObject == null) return null;
                
                // Try to find a container that looks like a category content container
                VerticalLayoutGroup[] layoutGroups = MenuObject.GetComponentsInChildren<VerticalLayoutGroup>(true);
                
                if (layoutGroups != null)
                {
                    foreach (var group in layoutGroups)
                    {
                        if (group == null || group.transform == null || group.transform.parent == null) continue;
                        
                        // Check for characteristics of a content container
                        if (group.transform.parent.name.Contains("Viewport") && 
                            !group.transform.parent.name.Contains("Navigation"))
                        {
                            return group.transform;
                        }
                    }
                }
                
                // If not found, fallback to trying to find by name content
                Transform contentContainer = FindContentContainer();
                if (contentContainer != null)
                {
                    return contentContainer;
                }
                
                // Last resort: return the same as the button container
                return GetCategoryButtonContainer();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error getting category child container: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Tries to find the content container by looking for specific components
        /// </summary>
        private Transform FindContentContainer()
        {
            try
            {
                if (MenuObject == null) return null;
                
                // Look for typical container names
                string[] containerNames = new string[] { "Content_Header", "Content", "ScrollRect_Content" };
                
                foreach (string name in containerNames)
                {
                    GameObject container = FindComponentInObject(MenuObject, name);
                    if (container != null)
                    {
                        return container.transform;
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error finding content container: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gets the category title text component
        /// </summary>
        internal TextMeshProUGUI GetCategoryTitle()
        {
            try
            {
                if (MenuObject == null) return null;
                
                GameObject titleObj = FindComponentInObject(MenuObject, "Text_Title");
                if (titleObj == null)
                {
                    Debug.LogError("[KernellClient] Could not find category title");
                    return null;
                }
                
                return titleObj.GetComponent<TextMeshProUGUI>();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error getting category title: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gets the side panel header transform
        /// </summary>
        internal Transform GetSidePanelHeader()
        {
            try
            {
                if (MenuObject == null) return null;
                
                GameObject headerObj = FindComponentInObject(MenuObject, "DynamicSidePanel_Header");
                if (headerObj == null)
                {
                    Debug.LogError("[KernellClient] Could not find side panel header");
                    return null;
                }
                
                return headerObj.transform;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error getting side panel header: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Adds a header button to this page
        /// </summary>
        public KmMMHeaderButton AddHeaderButton(string tooltip, Action onClick, Sprite icon = null)
        {
            try
            {
                if (MenuObject == null)
                {
                    Debug.LogError("[KernellClient] Cannot add header button: MenuObject is null");
                    return null;
                }
                
                return new KmMMHeaderButton(tooltip, icon, this, onClick);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error adding header button: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Adds a sidebar header button to this page
        /// </summary>
        public KmMMSidebarHeaderButton AddSidebarHeaderButton(string text, string tooltip, Action onClick, Sprite icon = null, string color = "#ffffff")
        {
            try
            {
                if (MenuObject == null)
                {
                    Debug.LogError("[KernellClient] Cannot add sidebar header button: MenuObject is null");
                    return null;
                }
                
                return new KmMMSidebarHeaderButton(this, text, tooltip, icon, onClick, color);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error adding sidebar header button: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gets a menu page by name
        /// </summary>
        public KmMMPage GetMenuPage(string name)
        {
            try
            {
                string cleanName = UiElement.GetCleanName("Menu_" + name);
                GameObject pageObj = null;
                
                // Try to find in MMenuParent first
                if (MenuEx.MMenuParent != null)
                {
                    Transform direct = MenuEx.MMenuParent.transform.Find(cleanName);
                    if (direct != null)
                    {
                        pageObj = direct.gameObject;
                    }
                }
                
                // If not found, try settings menu
                if (pageObj == null && MenuEx.MMSettingsMenu != null)
                {
                    Transform direct = MenuEx.MMSettingsMenu.transform.Find(cleanName);
                    if (direct != null)
                    {
                        pageObj = direct.gameObject;
                    }
                }
                
                // Last attempt - search recursively
                if (pageObj == null && MenuEx.MMenuParent != null)
                {
                    pageObj = FindComponentInObject(MenuEx.MMenuParent, cleanName);
                }
                
                return (pageObj != null) ? new KmMMPage(pageObj.transform) : null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error getting menu page: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Adds a category to this page and returns it
        /// </summary>
        public KmMMCategory AddnGetMenuCategory(string title, string tooltip, Sprite icon = null, string color = "#ffffff")
        {
            try
            {
                if (MenuObject == null)
                {
                    Debug.LogError("[KernellClient] Cannot add menu category: MenuObject is null");
                    return null;
                }
                
                return new KmMMCategory(this, title, tooltip, icon, null, color);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error adding menu category: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Creates a new menu page
        /// </summary>
        public static KmMMPage Create(string text, Sprite icon, bool isRoot)
        {
            try
            {
                // Check if required references exist
                if (MenuEx.MMSettingsMenu == null)
                {
                    Debug.LogError("[KernellClient] Cannot create page: MenuEx.MMSettingsMenu is null");
                    return null;
                }
                
                if (MenuEx.QMenuParent == null)
                {
                    Debug.LogError("[KernellClient] Cannot create page: MenuEx.QMenuParent is null");
                    return null;
                }
                
                if (MenuEx.MMenuStateCtrl == null)
                {
                    Debug.LogError("[KernellClient] Cannot create page: MenuEx.MMenuStateCtrl is null");
                    return null;
                }
                
                return new KmMMPage(text, icon, isRoot);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[KernellClient] Error creating page: {ex.Message}");
                return null;
            }
        }
    }
}