using System;
using System.Collections;
using System.Collections.Generic;           
using System.Linq;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using KMod;
using KMod.UI;
using KMod.UI.QuickMenu;
using KMod.Unity;
using KMod.VRChat;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Menus;
using IDisposable = System.IDisposable;
using IEnumerator = System.Collections.IEnumerator;

namespace KMod.UI.QuickMenu
{
    public enum ButtonLayoutShape
    {
        Grid,
        Horizontal,
        Vertical,
        CircleTop,
        CircleBottom,
        CircleFull,
        DiagonalRight,
        DiagonalLeft,
        SidesAndBottom,
        FivePanelLayout
    }

    public class KmMenuPage : UiElement, IButtonPage
    {
        private readonly bool _isRoot;
        private Transform _container;
        private ButtonLayoutShape _currentLayoutShape = ButtonLayoutShape.Grid;
        private bool _useThinButtons = false;
        private static int SiblingIndex => MenuEx.QMenuParent.transform.Find("Modal_AddMessage").GetSiblingIndex();
        public UIPage UiPage { get; }
        public event System.Action OnOpen;
        public event System.Action OnClose;
        private readonly System.Collections.Generic.List<UiElement> _buttons = new System.Collections.Generic.List<UiElement>();
        private readonly System.Collections.Generic.Dictionary<int, Vector3> _directPositions = new System.Collections.Generic.Dictionary<int, Vector3>();
        private float _viewportWidth = 900f;
        private float _viewportHeight = 600f;
        private float _buttonWidth = 210f;
        private float _buttonHeight = 90f;
        private float _thinButtonHeight = 70f;
        public float _horizontalOffset = 100f;
        public float _verticalOffset = -30f;
        public float _verticalSpacing = 180f;
        public float _horizontalSpacing = 20f;
        public float _topSafetyMargin = 100f;
        public float _bottomSafetyMargin = 90f;
        public float _leftSafetyMargin = 80f;
        public float _rightSafetyMargin = 80f;
        private const float Deg2Rad = 0.0174532925f;

        public ButtonLayoutShape LayoutShape 
        { 
            get => _currentLayoutShape;
            set 
            {
                if (_currentLayoutShape != value)
                {
                    _currentLayoutShape = value;
                    UpdateLayout();
                }
            }
        }

        public bool UseThinButtons
        {
            get => _useThinButtons;
            set
            {
                if (_useThinButtons != value)
                {
                    _useThinButtons = value;
                    UpdateExistingButtonsThinMode();
                    UpdateLayout();
                }
            }
        }

        public KmMenuPage(string text, bool isRoot = false, string color = "#ffffff")
            : base(QMMenuPrefabs.MenuPagePrefab, MenuEx.QMenuParent, "Menu_" + text, defaultState: false)
        {
            KmMenuPage reMenuPage = this;
            UnityEngine.Object.DestroyImmediate(GameObject.GetComponent<MonoBehaviour1PublicBuToBuToUnique>());
            RectTransform.SetSiblingIndex(SiblingIndex);
            string cleanName = UiElement.GetCleanName(text);
            _isRoot = isRoot;
            Transform header = RectTransform.GetChild(0);
            TextMeshProUGUI titleText = header.GetComponentInChildren<TextMeshProUGUI>();
            MelonCoroutines.Start(WaitForActive());
            titleText.richText = true;
            if (!_isRoot)
            {
                header.Find("LeftItemContainer/Button_Back").gameObject.SetActive(true);
            }
            header.name = "Header_H1";
            CleanExistingLayout();
            UiPage = GameObject.AddComponent<UIPage>();
            UiPage.field_Public_String_0 = "Page_" + cleanName;
            UiPage.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
            UiPage.field_Private_List_1_UIPage_0.Add(UiPage);
            UiPage.GetComponent<Canvas>().enabled = true;
            UiPage.GetComponent<CanvasGroup>().enabled = true;
            UiPage.GetComponent<UIPage>().enabled = true;
            UiPage.GetComponent<GraphicRaycaster>().enabled = true;
            UiPage.gameObject.active = false;
            VRCScrollRect scrollRect = RectTransform.Find("Scrollrect").GetComponent<VRCScrollRect>();
            _container = scrollRect.content;
            ConfigureContainer(scrollRect);
            ConfigureScrolling(scrollRect);
            KmgisterWithMenuController(isRoot);
            EnableDisableListener enableDisableListener = GameObject.AddComponent<EnableDisableListener>();
            enableDisableListener.OnEnableEvent += () =>
            {
                OnOpen?.Invoke();
                MelonCoroutines.Start(MeasureViewport());
            };
            enableDisableListener.OnDisableEvent += () =>
            {
                OnClose?.Invoke();
            };

            IEnumerator WaitForActive()
            {
                while (!reMenuPage.GameObject.activeInHierarchy) yield return null;
                titleText.text = $"<color={color}>{text}</color>";
            }

            IEnumerator MeasureViewport()
            {
                yield return new WaitForSeconds(0.05f);
                RectTransform viewport = scrollRect.viewport;
                if (viewport != null)
                {
                    Rect rect = viewport.rect;
                    _viewportWidth = rect.width;
                    _viewportHeight = rect.height;
                }
                UpdateLayout();
            }
        }

        public KmMenuPage(Transform transform)
            : base(transform)
        {
            UiPage = GameObject.GetComponent<UIPage>();
            _isRoot = MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0.Contains(UiPage);
            ScrollRect scrollRect = RectTransform.Find("Scrollrect").GetComponent<ScrollRect>();
            _container = scrollRect.content;
            RectTransform containerRect = _container as RectTransform;
            if (containerRect != null) containerRect.pivot = new Vector2(0f, 1f);
            RectTransform viewport = scrollRect.viewport;
            if (viewport != null)
            {
                Rect rect = viewport.rect;
                _viewportWidth = rect.width;
                _viewportHeight = rect.height;
            }
        }

        private void CleanExistingLayout()
        {
            Transform oldButtonContainer = RectTransform.Find("Scrollrect/Viewport/VerticalLayoutGroup/Buttons");
            if (oldButtonContainer != null)
            {
                Il2CppSystem.Collections.IEnumerator enumerator = oldButtonContainer.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        Transform child = enumerator.Current as Transform;
                        if (child != null) UnityEngine.Object.Destroy(child.gameObject);
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable) disposable.Dispose();
                }
            }
        }

        private void ConfigureContainer(VRCScrollRect scrollRect)
        {
            Transform verticalLayoutGroup = GameObject.transform.Find("Scrollrect/Viewport/VerticalLayoutGroup");
            if (verticalLayoutGroup != null)
            {
                VerticalLayoutGroup existingLayout = verticalLayoutGroup.GetComponent<VerticalLayoutGroup>();
                if (existingLayout != null) UnityEngine.Object.DestroyImmediate(existingLayout);
            }
            Transform oldButtons = _container.Find("Buttons");
            if (oldButtons != null) UnityEngine.Object.DestroyImmediate(oldButtons.gameObject);
            Transform oldSpacer = _container.Find("Spacer_8pt");
            if (oldSpacer != null) UnityEngine.Object.DestroyImmediate(oldSpacer.gameObject);
            GameObject customButtonContainer = new GameObject("CustomButtonContainer");
            customButtonContainer.transform.SetParent(_container);
            customButtonContainer.transform.localPosition = Vector3.zero;
            customButtonContainer.transform.localRotation = Quaternion.identity;
            customButtonContainer.transform.localScale = Vector3.one;
            ContentSizeFitter fitter = customButtonContainer.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            RectTransform containerRect = customButtonContainer.GetComponent<RectTransform>();
            if (containerRect == null) containerRect = customButtonContainer.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0, 1);
            containerRect.anchorMax = new Vector2(0, 1);
            containerRect.pivot = new Vector2(0, 1);
            _container = customButtonContainer.transform;
        }

        private void ConfigureScrolling(VRCScrollRect scrollRect)
        {
            Transform scrollbar = scrollRect.transform.Find("Scrollbar");
            scrollbar.gameObject.SetActive(true);
            scrollRect.field_Public_Boolean_0 = true;
            scrollRect.enabled = true;
            scrollRect.verticalScrollbar = scrollbar.GetComponent<Scrollbar>();
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
            VRCRectMask2D mask = scrollRect.viewport.GetComponent<VRCRectMask2D>();
            mask.enabled = true;
            mask.prop_Boolean_0 = true;
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

        public void Open()
        {
            UiPage.gameObject.active = true;
            MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(UiPage.field_Public_String_0, null, param_3: false, UIPage.EnumNPublicSealedvaNoLeRiBoIn6vUnique.None);
        }

        private void UpdateLayout()
        {
            if (_buttons.Count == 0) return;
            _directPositions.Clear();
            switch (_currentLayoutShape)
            {
                case ButtonLayoutShape.Grid: CalculateGridLayout(); break;
                case ButtonLayoutShape.Horizontal: CalculateHorizontalLayout(); break;
                case ButtonLayoutShape.Vertical: CalculateVerticalLayout(); break;
                case ButtonLayoutShape.CircleTop: CalculateCircleTopLayout(); break;
                case ButtonLayoutShape.CircleBottom: CalculateCircleBottomLayout(); break;
                case ButtonLayoutShape.CircleFull: CalculateCircleFullLayout(); break;
                case ButtonLayoutShape.DiagonalRight: CalculateDiagonalRightLayout(); break;
                case ButtonLayoutShape.DiagonalLeft: CalculateDiagonalLeftLayout(); break;
                case ButtonLayoutShape.SidesAndBottom: CalculateSidesAndBottomLayout(); break;
                case ButtonLayoutShape.FivePanelLayout: CalculateFivePanelLayout(); break;
            }
            ApplyButtonPositions();
            UpdateContentSize();
        }

        private void CalculateGridLayout()
        {
            float buttonHeight = _useThinButtons ? _thinButtonHeight : _buttonHeight;
            int columns = Mathf.Max(1, Mathf.FloorToInt((_viewportWidth - _leftSafetyMargin - _rightSafetyMargin - _horizontalOffset + _horizontalSpacing) / (_buttonWidth + _horizontalSpacing)));
            for (int i = 0; i < _buttons.Count; i++)
            {
                int row = i / columns;
                int col = i % columns;
                float x = _leftSafetyMargin + _horizontalOffset + col * (_buttonWidth + _horizontalSpacing);
                float y = -(_topSafetyMargin + _verticalOffset + row * _verticalSpacing);
                _directPositions[i] = new Vector3(x, y, 0f);
            }
        }

        private void CalculateHorizontalLayout()
        {
            float y = -(_topSafetyMargin + _verticalOffset);
            for (int i = 0; i < _buttons.Count; i++)
            {
                float x = _leftSafetyMargin + _horizontalOffset + i * (_buttonWidth + _horizontalSpacing);
                _directPositions[i] = new Vector3(x, y, 0f);
            }
        }

        private void CalculateVerticalLayout()
        {
            float x = _leftSafetyMargin + _horizontalOffset + (_viewportWidth - _leftSafetyMargin - _rightSafetyMargin - _buttonWidth) / 2;
            for (int i = 0; i < _buttons.Count; i++)
            {
                float y = -(_topSafetyMargin + _verticalOffset + i * _verticalSpacing);
                _directPositions[i] = new Vector3(x, y, 0f);
            }
        }

        private void CalculateCircleTopLayout()
        {
            float radius = Mathf.Min(_viewportWidth, _viewportHeight) * 0.35f;
            float centerX = (_viewportWidth / 2) + _horizontalOffset;
            float centerY = -(_topSafetyMargin + _verticalOffset + radius);
            float startAngle = 180f;
            float endAngle = 360f;
            for (int i = 0; i < _buttons.Count; i++)
            {
                float t = _buttons.Count <= 1 ? 0.5f : (float)i / (_buttons.Count - 1);
                float angle = Mathf.Lerp(startAngle, endAngle, t) * Deg2Rad;
                float xPos = centerX + Mathf.Cos(angle) * radius;
                float yPos = centerY + Mathf.Sin(angle) * radius;
                _directPositions[i] = new Vector3(xPos, yPos, 0f);
            }
        }

        private void CalculateCircleBottomLayout()
        {
            float radius = Mathf.Min(_viewportWidth, _viewportHeight) * 0.35f;
            float centerX = (_viewportWidth / 2) + _horizontalOffset;
            float centerY = -(_topSafetyMargin + _verticalOffset + radius);
            float startAngle = 0f;
            float endAngle = 180f;
            for (int i = 0; i < _buttons.Count; i++)
            {
                float t = _buttons.Count <= 1 ? 0.5f : (float)i / (_buttons.Count - 1);
                float angle = Mathf.Lerp(startAngle, endAngle, t) * Deg2Rad;
                float xPos = centerX + Mathf.Cos(angle) * radius;
                float yPos = centerY + Mathf.Sin(angle) * radius;
                _directPositions[i] = new Vector3(xPos, yPos, 0f);
            }
        }

        private void CalculateCircleFullLayout()
        {
            float radius = Mathf.Min(_viewportWidth, _viewportHeight) * 0.35f;
            float centerX = (_viewportWidth / 2) + _horizontalOffset;
            float centerY = -(_topSafetyMargin + _verticalOffset + radius);
            float angleStep = 360f / _buttons.Count;
            for (int i = 0; i < _buttons.Count; i++)
            {
                float angle = i * angleStep * Deg2Rad;
                float xPos = centerX + Mathf.Cos(angle) * radius;
                float yPos = centerY + Mathf.Sin(angle) * radius;
                _directPositions[i] = new Vector3(xPos, yPos, 0f);
            }
        }

        private void CalculateDiagonalRightLayout()
        {
            float diagonalLength = Mathf.Min(_viewportWidth - _leftSafetyMargin - _rightSafetyMargin, 
                                            _viewportHeight - _topSafetyMargin - _bottomSafetyMargin);
            for (int i = 0; i < _buttons.Count; i++)
            {
                float t = _buttons.Count <= 1 ? 0.5f : (float)i / (_buttons.Count - 1);
                float xPos = _leftSafetyMargin + _horizontalOffset + t * diagonalLength;
                float yPos = -(_topSafetyMargin + _verticalOffset + t * diagonalLength);
                _directPositions[i] = new Vector3(xPos, yPos, 0f);
            }
        }

        private void CalculateDiagonalLeftLayout()
        {
            float diagonalLength = Mathf.Min(_viewportWidth - _leftSafetyMargin - _rightSafetyMargin, 
                                            _viewportHeight - _topSafetyMargin - _bottomSafetyMargin);
            for (int i = 0; i < _buttons.Count; i++)
            {
                float t = _buttons.Count <= 1 ? 0.5f : (float)i / (_buttons.Count - 1);
                float xPos = (_viewportWidth - _rightSafetyMargin) + _horizontalOffset - t * diagonalLength;
                float yPos = -(_topSafetyMargin + _verticalOffset + t * diagonalLength);
                _directPositions[i] = new Vector3(xPos, yPos, 0f);
            }
        }

        private void CalculateSidesAndBottomLayout()
        {
            float buttonHeight = _useThinButtons ? _thinButtonHeight : _buttonHeight;
            if (_buttons.Count == 0) return;
            int leftCount = 0;
            int rightCount = 0;
            int bottomCount = 0;
            if (_buttons.Count == 1) bottomCount = 1;
            else if (_buttons.Count == 2)
            {
                leftCount = 1;
                rightCount = 1;
            }
            else
            {
                bottomCount = _buttons.Count % 2;
                int sideCount = (_buttons.Count - bottomCount) / 2;
                leftCount = sideCount;
                rightCount = sideCount;
            }
            float leftX = _leftSafetyMargin + _horizontalOffset;
            float rightX = (_viewportWidth - _rightSafetyMargin - _buttonWidth) + _horizontalOffset;
            float bottomY = -(_viewportHeight - _bottomSafetyMargin - buttonHeight - _verticalOffset);
            int index = 0;
            for (int i = 0; i < leftCount; i++)
            {
                float y = -(_topSafetyMargin + _verticalOffset + i * _verticalSpacing);
                _directPositions[index++] = new Vector3(leftX, y, 0f);
            }
            for (int i = 0; i < rightCount; i++)
            {
                float y = -(_topSafetyMargin + _verticalOffset + i * _verticalSpacing);
                _directPositions[index++] = new Vector3(rightX, y, 0f);
            }
            for (int i = 0; i < bottomCount; i++)
            {
                float x = ((_viewportWidth - _buttonWidth) / 2) + _horizontalOffset;
                _directPositions[index++] = new Vector3(x, bottomY, 0f);
            }
        }

        private void CalculateFivePanelLayout()
        {
            float buttonHeight = _useThinButtons ? _thinButtonHeight : _buttonHeight;
            if (_buttons.Count == 0) return;
            float centerX = ((_viewportWidth - _buttonWidth) / 2) + _horizontalOffset;
            float centerY = -((_viewportHeight - buttonHeight) / 2 + _verticalOffset);
            float horizOffset = _buttonWidth + _horizontalSpacing;
            float vertOffset = _verticalSpacing;
            Vector3[] positions = new Vector3[]
            {
                new Vector3(centerX, centerY, 0f),
                new Vector3(centerX - horizOffset, centerY - vertOffset, 0f),
                new Vector3(centerX + horizOffset, centerY - vertOffset, 0f),
                new Vector3(centerX - horizOffset, centerY + vertOffset, 0f),
                new Vector3(centerX + horizOffset, centerY + vertOffset, 0f),
            };
            for (int i = 0; i < Mathf.Min(_buttons.Count, positions.Length); i++)
            {
                _directPositions[i] = positions[i];
            }
            if (_buttons.Count > positions.Length)
            {
                int extraButtons = _buttons.Count - positions.Length;
                int columns = 3;
                float extraStartY = centerY - vertOffset * 2 - _horizontalSpacing;
                float extraStartX = centerX - (columns * (_buttonWidth + _horizontalSpacing)) / 2 + _buttonWidth / 2;
                for (int i = 0; i < extraButtons; i++)
                {
                    int row = i / columns;
                    int col = i % columns;
                    float x = extraStartX + col * (_buttonWidth + _horizontalSpacing);
                    float y = extraStartY - row * _verticalSpacing;
                    _directPositions[positions.Length + i] = new Vector3(x, y, 0f);
                }
            }
        }

        private void ApplyButtonPositions()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                if (_buttons[i]?.RectTransform == null) continue;
                if (_directPositions.TryGetValue(i, out Vector3 position))
                {
                    _buttons[i].RectTransform.localPosition = position;
                    EnsureButtonIgnoresLayout(_buttons[i]);
                }
            }
        }

        private void EnsureButtonIgnoresLayout(UiElement button)
        {
            if (button?.RectTransform == null) return;
            LayoutElement layoutElement = button.RectTransform.GetComponent<LayoutElement>();
            if (layoutElement == null) layoutElement = button.RectTransform.gameObject.AddComponent<LayoutElement>();
            layoutElement.ignoreLayout = true;
        }

        private void UpdateContentSize()
        {
            if (_buttons.Count == 0) return;
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;
            float buttonHeight = _useThinButtons ? _thinButtonHeight : _buttonHeight;
            foreach (var kvp in _directPositions)
            {
                Vector3 pos = kvp.Value;
                minX = Mathf.Min(minX, pos.x);
                maxX = Mathf.Max(maxX, pos.x + _buttonWidth);
                minY = Mathf.Min(minY, pos.y - buttonHeight);
                maxY = Mathf.Max(maxY, pos.y);
            }
            minX -= _leftSafetyMargin;
            maxX += _rightSafetyMargin;
            minY -= _bottomSafetyMargin;
            maxY += _topSafetyMargin;
            float width = maxX - minX;
            float height = Mathf.Abs(maxY - minY);
            width = Mathf.Max(width, _viewportWidth);
            height = Mathf.Max(height, _viewportHeight);
            RectTransform containerRect = _container as RectTransform;
            if (containerRect != null)
            {
                containerRect.sizeDelta = new Vector2(width, height);
                LayoutRebuilder.ForceRebuildLayoutImmediate(containerRect);
            }
        }

        private void UpdateExistingButtonsThinMode()
        {
            foreach (var button in _buttons)
            {
                if (button is KmMenuButton menuButton) menuButton.ThinMode = _useThinButtons;
                else if (button is KmMenuToggle toggle) toggle.ThinMode = _useThinButtons;
            }
        }
        
        public KmMenuButton AddButton(string text, string tooltip, System.Action onClick, Sprite sprite = null, string color = "#ffffff")
        {
            KmMenuButton button = new KmMenuButton(text, tooltip, onClick, _container, sprite, resizeTextNoSprite: true, color, thinMode: _useThinButtons);
            EnsureButtonIgnoresLayout(button);
            _buttons.Add(button);
            UpdateLayout();
            return button;
        }

        public KmMenuButton AddSpacer(Sprite sprite = null)
        {
            KmMenuButton spacer = AddButton(string.Empty, string.Empty, null, sprite);
            spacer.GameObject.name = "Button_Spacer";
            spacer.Background.gameObject.SetActive(false);
            return spacer;
        }

        public KmMenuToggle AddToggle(string text, string tooltip, System.Action<bool> onToggle, bool defaultValue = false, string color = "#ffffff")
        {
            return AddToggle(text, tooltip, onToggle, defaultValue, null, null, color);
        }

        public KmMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, string color = "#ffffff")
        {
            return AddToggle(text, tooltip, configValue, null, null, color);
        }

        public KmMenuToggle AddToggle(string text, string tooltip, System.Action<bool> onToggle, bool defaultValue, Sprite iconOn, Sprite iconOff, string color = "#ffffff")
        {
            KmMenuToggle toggle = new KmMenuToggle(text, tooltip, onToggle, _container, defaultValue, iconOn, iconOff, color, thinMode: _useThinButtons);
            EnsureButtonIgnoresLayout(toggle);
            _buttons.Add(toggle);
            UpdateLayout();
            return toggle;
        }

        public KmMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, Sprite iconOn, Sprite iconOff, string color = "#ffffff")
        {
            KmMenuToggle toggle = new KmMenuToggle(text, tooltip, configValue.SetValue, _container, configValue, iconOn, iconOff, color, thinMode: _useThinButtons);
            EnsureButtonIgnoresLayout(toggle);
            _buttons.Add(toggle);
            UpdateLayout();
            return toggle;
        }

        public KmMenuPage AddMenuPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
        {
            KmMenuPage page = GetMenuPage(text);
            if (page != null) return page;
            KmMenuPage newPage = new KmMenuPage(text, false, color);
            AddButton(text, string.IsNullOrEmpty(tooltip) ? $"Open the {text} menu" : tooltip, newPage.Open, sprite, color);
            return newPage;
        }

        public KmCategoryPage AddCategoryPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
        {
            KmCategoryPage page = GetCategoryPage(text);
            if (page != null) return page;
            KmCategoryPage newPage = new KmCategoryPage(text, false, color);
            AddButton(text, string.IsNullOrEmpty(tooltip) ? $"Open the {text} menu" : tooltip, newPage.Open, sprite, color);
            return newPage;
        }

        public void AddMenuPage(string text, string tooltip, System.Action<KmMenuPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
        {
            onPageBuilt(AddMenuPage(text, tooltip, sprite, color));
        }

        public void AddCategoryPage(string text, string tooltip, System.Action<KmCategoryPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
        {
            onPageBuilt(AddCategoryPage(text, tooltip, sprite, color));
        }

        public void AddTabbedPage(string text, string tooltip, System.Action<KmTabbedPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
        {
            onPageBuilt(AddTabbedPage(text, tooltip, sprite, color));
        }

        public KmTabbedPage AddTabbedPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
        {
            KmTabbedPage page = GetTabbedPage(text);
            if (page != null) return page;
            KmTabbedPage newPage = new KmTabbedPage(text, false, color);
            AddButton(text, string.IsNullOrEmpty(tooltip) ? $"Open the {text} menu" : tooltip, newPage.Open, sprite, color);
            return newPage;
        }

        public KmMenuPage GetMenuPage(string name)
        {
            Transform t = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
            return (t == null) ? null : new KmMenuPage(t);
        }

        public KmCategoryPage GetCategoryPage(string name)
        {
            Transform t = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
            return (t == null) ? null : new KmCategoryPage(t);
        }

        public KmTabbedPage GetTabbedPage(string name)
        {
            Transform t = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
            return (t == null) ? null : new KmTabbedPage(t);
        }

        public KmMenuPage ToMenuPage(string name, string tooltip = "", Sprite sprite = null)
        {
            KmMenuPage page = GetMenuPage(name);
            AddButton(name, string.IsNullOrEmpty(tooltip) ? $"Open the {name} menu" : tooltip, page.Open, sprite);
            return page;
        }

        public KmCategoryPage ToCategoryPage(string name, string tooltip = "", Sprite sprite = null)
        {
            KmCategoryPage page = GetCategoryPage(name);
            AddButton(name, string.IsNullOrEmpty(tooltip) ? $"Open the {name} menu" : tooltip, page.Open, sprite);
            return page;
        }

        public void SetButtonLayout(ButtonLayoutShape layoutShape) => LayoutShape = layoutShape;
        public void SetThinButtons(bool useThin) => UseThinButtons = useThin;

        public void SetSafetyMargins(float top, float right, float bottom, float left)
        {
            _topSafetyMargin = top;
            _rightSafetyMargin = right;
            _bottomSafetyMargin = bottom;
            _leftSafetyMargin = left;
            UpdateLayout();
        }

        public void SetHorizontalOffset(float offset)
        {
            _horizontalOffset = offset;
            UpdateLayout();
        }

        public void SetVerticalOffset(float offset)
        {
            _verticalOffset = offset;
            UpdateLayout();
        }

        public void SetVerticalSpacing(float spacing)
        {
            _verticalSpacing = spacing;
            UpdateLayout();
        }

        public void SetHorizontalSpacing(float spacing)
        {
            _horizontalSpacing = spacing;
            UpdateLayout();
        }

        public void SetButtonPosition(int buttonIndex, Vector3 position)
        {
            if (buttonIndex < 0 || buttonIndex >= _buttons.Count) return;
            _directPositions[buttonIndex] = position;
            if (_buttons[buttonIndex]?.RectTransform != null)
            {
                _buttons[buttonIndex].RectTransform.localPosition = position;
                EnsureButtonIgnoresLayout(_buttons[buttonIndex]);
            }
        }

        public void SetButtonPosition(UiElement button, Vector3 position)
        {
            int index = _buttons.IndexOf(button);
            if (index >= 0) SetButtonPosition(index, position);
            else if (button?.RectTransform != null)
            {
                button.RectTransform.localPosition = position;
                EnsureButtonIgnoresLayout(button);
            }
        }

        public void ClearButtons()
        {
            foreach (var button in _buttons)
            {
                if (button?.GameObject != null) UnityEngine.Object.Destroy(button.GameObject);
            }
            _buttons.Clear();
            _directPositions.Clear();
            RectTransform containerRect = _container as RectTransform;
            if (containerRect != null)
            {
                containerRect.sizeDelta = new Vector2(_viewportWidth, _viewportHeight);
            }
        }

        public void KmfreshLayout() => UpdateLayout();

        public UiElement GetButton(int index)
        {
            if (index >= 0 && index < _buttons.Count) return _buttons[index];
            return null;
        }

        public UiElement GetButtonByName(string name)
        {
            return _buttons.FirstOrDefault(button => 
                button?.GameObject?.name == name || 
                button?.GameObject?.name == "Button_" + name);
        }

        public int ButtonCount => _buttons.Count;

        public void SetButtonWidth(float width)
        {
            if (width <= 0) return;
            _buttonWidth = width;
            UpdateLayout();
        }

        public void SetButtonHeight(float normalHeight, float thinHeight)
        {
            if (normalHeight <= 0 || thinHeight <= 0) return;
            _buttonHeight = normalHeight;
            _thinButtonHeight = thinHeight;
            UpdateLayout();
        }

        public static KmMenuPage Create(string text, bool isRoot, string color = "#ffffff")
        {
            return new KmMenuPage(text, isRoot, color);
        }
    }
}