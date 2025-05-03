using UnityEngine;
using MelonLoader;

namespace KMod.VRChat
{
    public static class MMenuPrefabs
    {
        private static GameObject[] _prefabs = new GameObject[16];
        private static Transform _mainMenuRoot;
        private static bool _initialized = false;
        
        public static Transform MainMenuRoot
        {
            get
            {
                if (_mainMenuRoot == null)
                {
                    _mainMenuRoot = GameObject.Find("UserInterface/Canvas_MainMenu(Clone)/Container/MMParent")?.transform 
                        ?? GameObject.Find("MMParent")?.transform;
                    
                    if (_mainMenuRoot == null)
                    {
                        foreach (var transform in GameObject.FindObjectsOfType<Transform>())
                        {
                            if (transform.name.Contains("MMParent") && !transform.name.Contains("QM"))
                            {
                                _mainMenuRoot = transform;
                                break;
                            }
                        }
                    }
                }
                return _mainMenuRoot;
            }
        }
        
        private static string GetPath(Transform transform)
        {
            string path = transform.name;
            Transform parent = transform.parent;
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            return path;
        }
        
        private static GameObject FindInHierarchy(Transform root, string name, string altName)
        {
            if (root == null) return null;
            
            Transform direct = root.Find(name);
            if (direct != null) return direct.gameObject;
            
            if (root.name == name || root.name == altName) return root.gameObject;
            
            for (int i = 0; i < root.childCount; i++)
            {
                Transform child = root.GetChild(i);
                if (child.name.Contains("QM") || child.name.Contains("QuickMenu")) continue;
                
                if (child.name == name || child.name == altName) return child.gameObject;
                
                for (int j = 0; j < child.childCount; j++)
                {
                    Transform subchild = child.GetChild(j);
                    if (subchild.name == name || subchild.name == altName) return subchild.gameObject;
                    
                    GameObject deepSearch = FindRecursively(subchild, name, altName, 3);
                    if (deepSearch != null) return deepSearch;
                }
            }
            
            return null;
        }
        
        private static GameObject FindRecursively(Transform parent, string name, string altName, int depth)
        {
            if (depth <= 0 || parent == null) return null;
            
            if (parent.name == name || parent.name == altName) return parent.gameObject;
            
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.name == name || child.name == altName) return child.gameObject;
                
                GameObject found = FindRecursively(child, name, altName, depth - 1);
                if (found != null) return found;
            }
            
            return null;
        }
        
        private static GameObject GetComponent(int index, string name, string altName)
        {
            if (!_initialized) InitializePrefabs();
            
            if (_prefabs[index] == null)
            {
                _prefabs[index] = FindInHierarchy(MainMenuRoot, name, altName);
            }
            
            return _prefabs[index];
        }
        
        private static void InitializePrefabs()
        {
            if (_initialized || MainMenuRoot == null) return;
            
            _initialized = true;
            
            System.Action[] initActions = new System.Action[]
            {
                () => _prefabs[0] = FindInHierarchy(MainMenuRoot, "Button_ToggleNavPanel", "HeaderButton"),
                () => _prefabs[1] = FindInHierarchy(MainMenuRoot, "Field_MM_SortBy", "DropdownField"),
                () => _prefabs[2] = FindInHierarchy(MainMenuRoot, "AlwaysShowVisualAide", "Toggle_MM"),
                () => _prefabs[3] = FindInHierarchy(MainMenuRoot, "Separator", "Separator_MM"),
                () => _prefabs[4] = FindInHierarchy(MainMenuRoot, "Menu_Settings", "TabButton"),
                () => _prefabs[5] = FindInHierarchy(MainMenuRoot, "Cell_MM_Audio & Voice", "CategoryButton"),
                () => _prefabs[6] = FindInHierarchy(MainMenuRoot, "AudioAndVoice", "CategoryContainer"),
                () => _prefabs[7] = FindInHierarchy(MainMenuRoot, "ShapeFollowsCameraRotation", "Selector_MM"),
                () => _prefabs[8] = FindInHierarchy(MainMenuRoot, "Button_Logout", "SidebarHeaderButton"),
                () => _prefabs[9] = FindInHierarchy(MainMenuRoot, "FalloffForwardShift", "Slider_MM"),
                () => _prefabs[10] = FindInHierarchy(MainMenuRoot, "EarmuffMode", "CategorySection"),
                () => _prefabs[11] = FindInHierarchy(MainMenuRoot, "Background_Info", "SectionBackground"),
                () => _prefabs[12] = FindInHierarchy(MainMenuRoot, "JoinBtn", "UserDetailButton"),
                () => _prefabs[13] = FindInHierarchy(MainMenuRoot, "ViewAvatarDetails", "AvatarButton"),
                () => _prefabs[14] = FindInHierarchy(MainMenuRoot, "AlwaysShowVisualAide", "Label_MM"),
                () => _prefabs[15] = FindInHierarchy(MainMenuRoot, "Title", "LabelText")
            };

            foreach (var action in initActions) action();
        }

        public static GameObject MMHeaderButtonPrefab => GetComponent(0, "Button_ToggleNavPanel", "HeaderButton");
        public static GameObject MMDropdownPrefab => GetComponent(1, "Field_MM_SortBy", "DropdownField");
        public static GameObject MMTogglePrefab => GetComponent(2, "AlwaysShowVisualAide", "Toggle_MM");
        public static GameObject MMSeparatorPrefab => GetComponent(3, "Separator", "Separator_MM");
        public static GameObject MMTabButtonPrefab => GetComponent(4, "Menu_Settings", "TabButton");
        public static GameObject MMCategoryButtonPrefab => GetComponent(5, "Cell_MM_Audio & Voice", "CategoryButton");
        public static GameObject MMCategoryContainerPrefab => GetComponent(6, "AudioAndVoice", "CategoryContainer");
        public static GameObject MMSelectorPrefab => GetComponent(7, "ShapeFollowsCameraRotation", "Selector_MM");
        public static GameObject MMSideBarHeaderButtonPrefab => GetComponent(8, "Button_Logout", "SidebarHeaderButton");
        public static GameObject MMSliderPrefab => GetComponent(9, "FalloffForwardShift", "Slider_MM");
        public static GameObject MMCategorySectionPrefab => GetComponent(10, "EarmuffMode", "CategorySection");
        public static GameObject MMCategorySectionBackGroundPrefab => GetComponent(11, "Background_Info", "SectionBackground");
        public static GameObject MMUserDetailButton => GetComponent(12, "JoinBtn", "UserDetailButton");
        public static GameObject MMAvatarButton => GetComponent(13, "ViewAvatarDetails", "AvatarButton");
        public static GameObject MMSeparatorprefab => MMSeparatorPrefab;
        public static GameObject MMLabelPrefab => GetComponent(14, "AlwaysShowVisualAide", "Label_MM");
        public static GameObject MMLabelTextPrefab => GetComponent(15, "Title", "LabelText");
        
        public static void KmsetCache()
        {
            _initialized = false;
            _mainMenuRoot = null;
            for (int i = 0; i < _prefabs.Length; i++)
            {
                _prefabs[i] = null;
            }
        }
    }
}