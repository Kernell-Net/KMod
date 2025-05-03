using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MelonLoader;
using KMod.Unity;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI;
using VRC.UI.Controls;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Menus;

namespace KMod.VRChat
{
    public static class MenuEx
    {
        private static readonly Dictionary<int, object> _cache = new Dictionary<int, object>(40);
        private static bool _wasPatched;
        private static bool _isInitializing;
        
        private const int CACHE_QM_INSTANCE = 0;
        private const int CACHE_USER_INTERFACE = 1;
        private const int CACHE_APPLICATION = 2;
        private const int CACHE_ACTION_MENU = 3;
        private const int CACHE_MM_INSTANCE = 4;
        private const int CACHE_MM_PARENT = 5;
        private const int CACHE_QM_PARENT = 6;
        private const int CACHE_QM_TABS = 7;
        private const int CACHE_MM_TABS = 8;
        private const int CACHE_QM_STATE = 9;
        private const int CACHE_MM_STATE = 10;
        private const int CACHE_QM_SELECTED_USER = 11;
        private const int CACHE_QM_DASHBOARD = 12;
        private const int CACHE_MM_DASHBOARD = 13;
        private const int CACHE_MM_TARGET = 14;
        private const int CACHE_QM_NOTIFICATION = 15;
        private const int CACHE_QM_HERE = 16;
        private const int CACHE_QM_CAMERA = 17;
        private const int CACHE_QM_AUDIO = 18;
        private const int CACHE_QM_SETTINGS = 19;
        private const int CACHE_QM_DEVTOOLS = 20;
        private const int CACHE_QM_LEFT_WING = 21;
        private const int CACHE_QM_RIGHT_WING = 22;
        private const int CACHE_SPRITE_ON = 23;
        private const int CACHE_SPRITE_OFF = 24;
        private const int CACHE_MM_SETTINGS = 25;
        private const int CACHE_MM_REMOD = 26;

        static MenuEx()
        {
            if (!_isInitializing)
            {
                _isInitializing = true;
                Initialize();
            }
        }

        public static QuickMenu QMInstance
        {
            get
            {
                if (!_cache.TryGetValue(CACHE_QM_INSTANCE, out var val) || val == null)
                {
                    QuickMenu instance = userInterface?.GetComponentInChildren<QuickMenu>(true);
                    _cache[CACHE_QM_INSTANCE] = instance;
                    return instance;
                }
                return (QuickMenu)val;
            }
        }

        public static GameObject userInterface
        {
            get
            {
                if (!_cache.TryGetValue(CACHE_USER_INTERFACE, out var val) || val == null)
                {
                    StartUILoader();
                    return null;
                }
                return (GameObject)val;
            }
        }

        public static GameObject _application
        {
            get
            {
                if (!_cache.TryGetValue(CACHE_APPLICATION, out var val) || val == null)
                {
                    StartUILoader();
                    return null;
                }
                return (GameObject)val;
            }
        }

        private static void StartUILoader()
        {
            if (!_isInitializing)
            {
                _isInitializing = true;
                MelonCoroutines.Start(WaitForUI());
            }
        }

        private static IEnumerator WaitForUI()
        {
            EnableDisableListener.KmgisterSafe();
            while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null)
                yield return null;

            var objects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var obj in objects)
            {
                string name = obj.name;
                if (name.Contains("UserInterface"))
                    _cache[CACHE_USER_INTERFACE] = obj;
                else if (name.Contains("_Application"))
                    _cache[CACHE_APPLICATION] = obj;
                
                if (_cache.ContainsKey(CACHE_USER_INTERFACE) && _cache.ContainsKey(CACHE_APPLICATION))
                    break;
            }
        }

        public static ActionMenuController ActionMenuInstance
        {
            get
            {
                if (!_cache.TryGetValue(CACHE_ACTION_MENU, out var val) || val == null)
                {
                    if (ActionMenuController.field_Public_Static_ActionMenuController_0 == null)
                    {
                        MelonCoroutines.Start(WaitForActionMenu());
                    }
                    else
                    {
                        _cache[CACHE_ACTION_MENU] = ActionMenuController.prop_ActionMenuController_0;
                    }
                    Patch();
                    return (ActionMenuController)_cache[CACHE_ACTION_MENU];
                }
                return (ActionMenuController)val;
            }
        }

        private static IEnumerator WaitForActionMenu()
        {
            while (ActionMenuController.field_Public_Static_ActionMenuController_0 == null)
                yield return null;
            _cache[CACHE_ACTION_MENU] = ActionMenuController.prop_ActionMenuController_0;
        }

        public static MainMenu MMInstance => GetOrFindComponent<MainMenu>(CACHE_MM_INSTANCE, () => userInterface?.GetComponentInChildren<MainMenu>(true));

        public static GameObject MMenuParent
        {
            get
            {
                if (!_cache.TryGetValue(CACHE_MM_PARENT, out var val) || val == null)
                {
                    var mmenu = MMInstance;
                    if (mmenu != null)
                    {
                        Transform parent = mmenu.transform.Find("Container/MMParent");
                        _cache[CACHE_MM_PARENT] = parent;
                        return parent?.gameObject;
                    }
                    return null;
                }
                return ((Transform)val)?.gameObject;
            }
        }

        public static Transform QMenuParent => GetOrFindTransform(CACHE_QM_PARENT, () => QMInstance?.transform.Find("CanvasGroup/Container/Window/QMParent"));
        public static Transform QMenuTabs => GetOrFindTransform(CACHE_QM_TABS, () => QMInstance?.transform.Find("CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup"));
        public static Transform MMenuTabs => GetOrFindTransform(CACHE_MM_TABS, () => MMInstance?.transform.Find("Container/PageButtons/HorizontalLayoutGroup"));
        public static MenuStateController QMenuStateCtrl => GetOrFindComponent<MenuStateController>(CACHE_QM_STATE, () => QMInstance?.GetComponent<MenuStateController>());
        public static MenuStateController MMenuStateCtrl => GetOrFindComponent<MenuStateController>(CACHE_MM_STATE, () => MMInstance?.GetComponent<MenuStateController>());
        
        public static SelectedUserMenuQM QMSelectedUserLocal
        {
            get
            {
                if (!_cache.TryGetValue(CACHE_QM_SELECTED_USER, out var val) || val == null)
                {
                    var user = QMenuParent?.Find("Menu_SelectedUser_Local")?.GetComponent<SelectedUserMenuQM>();
                    _cache[CACHE_QM_SELECTED_USER] = user;
                    return user;
                }
                return (SelectedUserMenuQM)val;
            }
        }
        
        public static Transform QMDashboardMenu => GetOrFindTransform(CACHE_QM_DASHBOARD, () => QMenuParent?.Find("Menu_Dashboard"));
        public static Transform MMDashboardMenu => GetOrFindTransform(CACHE_MM_DASHBOARD, () => MMenuParent?.transform.Find("Menu_Dashboard"));
        public static Transform MMTargetboardMenu => GetOrFindTransform(CACHE_MM_TARGET, () => MMenuParent?.transform.Find("Menu_UserDetail"));
        public static Transform QMNotificationMenu => GetOrFindTransform(CACHE_QM_NOTIFICATION, () => QMenuParent?.Find("Menu_Notifications"));
        public static Transform QMHereMenu => GetOrFindTransform(CACHE_QM_HERE, () => QMenuParent?.Find("Menu_Here"));
        public static Transform QMCameraMenu => GetOrFindTransform(CACHE_QM_CAMERA, () => QMenuParent?.Find("Menu_Camera"));
        public static Transform QMAudioSettingsMenu => GetOrFindTransform(CACHE_QM_AUDIO, () => QMenuParent?.Find("Menu_QM_AudioSettings"));
        public static Transform QMSettingsMenu => GetOrFindTransform(CACHE_QM_SETTINGS, () => QMenuParent?.Find("Menu_QM_GeneralSettings"));
        public static Transform QMDevToolsMenu => GetOrFindTransform(CACHE_QM_DEVTOOLS, () => QMenuParent?.Find("Menu_DevTools"));
        
        public static GameObject QMLeftWing
        {
            get
            {
                if (!_cache.TryGetValue(CACHE_QM_LEFT_WING, out var val) || val == null)
                {
                    GameObject wing = QMInstance?.transform.Find("CanvasGroup/Container/Window/Wing_Left")?.gameObject;
                    _cache[CACHE_QM_LEFT_WING] = wing;
                    return wing;
                }
                return (GameObject)val;
            }
        }
        
        public static GameObject QMRightWing
        {
            get
            {
                if (!_cache.TryGetValue(CACHE_QM_RIGHT_WING, out var val) || val == null)
                {
                    GameObject wing = QMInstance?.transform.Find("CanvasGroup/Container/Window/Wing_Right")?.gameObject;
                    _cache[CACHE_QM_RIGHT_WING] = wing;
                    return wing;
                }
                return (GameObject)val;
            }
        }

        public static Sprite OnIconSprite
        {
            get
            {
                if (!_cache.TryGetValue(CACHE_SPRITE_ON, out var val) || val == null)
                {
                    Sprite sprite = null;
                    try
                    {
                        sprite = Resources.FindObjectsOfTypeAll<Sprite>()
                            .FirstOrDefault(x => x.name == "Toggle_ON" || x.name == "ON");

                        if (sprite == null && QMNotificationMenu != null)
                        {
                            var iconTransform = QMNotificationMenu.Find("Panel_NoNotifications_Message/Icon");
                            if (iconTransform != null)
                            {
                                var rawImage = iconTransform.GetComponent<ImageEx>();
                                if (rawImage != null)
                                    sprite = ((Image)(object)rawImage).sprite;
                            }
                        }

                        if (sprite == null)
                        {
                            var texture = new Texture2D(64, 64);
                            var pixels = texture.GetPixels();
                            for (var i = 0; i < pixels.Length; ++i)
                                pixels[i] = Color.white;
                            texture.SetPixels(pixels);
                            texture.Apply();
                            sprite = Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
                        }
                    }
                    catch (Exception e)
                    {
                        MelonLogger.Warning($"Failed to load OnIconSprite: {e}");
                    }
                    _cache[CACHE_SPRITE_ON] = sprite;
                    return sprite;
                }
                return (Sprite)val;
            }
        }

        public static Sprite OffIconSprite
        {
            get
            {
                if (!_cache.TryGetValue(CACHE_SPRITE_OFF, out var val) || val == null)
                {
                    Sprite sprite = null;
                    try
                    {
                        sprite = Resources.FindObjectsOfTypeAll<Sprite>()
                            .FirstOrDefault(x => x.name == "Toggle_OFF" || x.name == "OFF");

                        if (sprite == null && QMNotificationMenu != null)
                        {
                            var iconTransform = QMNotificationMenu.Find("Panel_Notification_Tabs/Button_ClearNotifications/Text_FieldContent/Icon");
                            if (iconTransform != null)
                            {
                                var rawImage = iconTransform.GetComponent<ImageEx>();
                                if (rawImage != null)
                                    sprite = ((Image)(object)rawImage).sprite;
                            }
                        }

                        if (sprite == null)
                        {
                            var texture = new Texture2D(64, 64);
                            var pixels = texture.GetPixels();
                            for (var i = 0; i < pixels.Length; ++i)
                                pixels[i] = Color.gray;
                            texture.SetPixels(pixels);
                            texture.Apply();
                            sprite = Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
                        }
                    }
                    catch (Exception e)
                    {
                        MelonLogger.Warning($"Failed to load OffIconSprite: {e}");
                    }
                    _cache[CACHE_SPRITE_OFF] = sprite;
                    return sprite;
                }
                return (Sprite)val;
            }
        }

        public static GameObject MMSettingsMenu => GetOrFindGameObject(CACHE_MM_SETTINGS, () => MMenuParent?.transform.Find("Menu_Settings")?.gameObject);
        public static GameObject MMReMod => GetOrFindGameObject(CACHE_MM_REMOD, () => MMenuParent?.transform.Find("Menu_ReMod")?.gameObject);

        public static Transform QMWingMenuContent(GameObject qmwing)
        {
            return qmwing?.transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup");
        }
        
        private static Transform GetOrFindTransform(int cacheKey, Func<Transform> finder)
        {
            if (!_cache.TryGetValue(cacheKey, out var val) || val == null)
            {
                Transform found = finder();
                _cache[cacheKey] = found;
                return found;
            }
            return (Transform)val;
        }
        
        private static GameObject GetOrFindGameObject(int cacheKey, Func<GameObject> finder)
        {
            if (!_cache.TryGetValue(cacheKey, out var val) || val == null)
            {
                GameObject found = finder();
                _cache[cacheKey] = found;
                return found;
            }
            return (GameObject)val;
        }
        
        private static T GetOrFindComponent<T>(int cacheKey, Func<T> finder) where T : Component
        {
            if (!_cache.TryGetValue(cacheKey, out var val) || val == null)
            {
                T found = finder();
                _cache[cacheKey] = found;
                return found;
            }
            return (T)val;
        }

        public static IEnumerable<Type> TryGetTypes(Assembly asm)
        {
            try { return asm.GetTypes(); }
            catch (ReflectionTypeLoadException ex)
            {
                try { return asm.GetExportedTypes(); }
                catch { return ex.Types.Where(t => t != null); }
            }
            catch { return Array.Empty<Type>(); }
        }

        public static void Patch()
        {
            if (!_wasPatched)
            {
                _wasPatched = true;
                KmModPatches.Patch();
            }
        }

        public static IEnumerator WaitForUInPatch()
        {
            Patch();
            while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null) yield return null;
            while (userInterface == null) yield return null;
            while (QMInstance == null) yield return null;
            while (MMInstance == null) yield return null;
            while (ActionMenuInstance == null) yield return null;
        }

        public static void Initialize()
        {
            MelonCoroutines.Start(InitializeRoutine());
        }

        private static IEnumerator InitializeRoutine()
        {
            yield return WaitForUInPatch();
            var _ = OnIconSprite;
            var __ = OffIconSprite;
            MelonLogger.Msg("MenuEx initialized successfully");
            _isInitializing = false;
        }
        
        public static void KmsetCache()
        {
            _cache.Clear();
            _isInitializing = false;
            _wasPatched = false;
        }
    }
}