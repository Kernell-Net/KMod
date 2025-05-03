    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events; // Added for UnityAction
    using VRC.UI;
    using MelonLoader;
    using KMod.VRChat;
    using VRC.Localization;
    using VRC.UI.Core.Styles;
    using VRC.UI.Elements.Controls;
    using TMPro; // Ensure you have this using directive for TextMeshPro

    namespace KMod.UI.QuickMenu
    {
        public class KmMenuButton : UiElement
        {
            private readonly TextMeshProUGUI _text;
            private ToolTip _tooltip;
            private StyleElement _styleElement;
            private Button _button;
            private readonly ImageEx _iconImage;
            private GameObject _iconsGameObject;

            public ImageEx Background { get; }

            // Store original width if you need to reset later
            private float _originalWidth;
            private float _originalTextPosY;
            private float _originalFontSize;
            private bool _thinMode;

            #region Public Properties

            /// <summary>
            /// Whether the button should be displayed in thin mode (text only, smaller width).
            /// </summary>
            public bool ThinMode
            {
                get => _thinMode;
                set
                {
                    if (_thinMode != value)
                    {
                        _thinMode = value;
                        UpdateThinMode();
                    }
                }
            }

            /// <summary>
            /// The text shown on the button.
            /// </summary>
            public string Text
            {
                get => _text?.text ?? string.Empty;
                set
                {
                    if (_text != null)
                        _text.SetText(value);
                }
            }

            public float _btnwidth => _button.transform.lossyScale.x;

            /// <summary>
            /// The tooltip that appears on hover.
            /// </summary>
            public string Tooltip
            {
                get => _tooltip != null ? _tooltip._localizableString.Key : string.Empty;
                set
                {
                    if (_tooltip != null)
                    {
                        var localizableString = LocalizableStringExtensions.Localize(value);
                        _tooltip._localizableString = localizableString;
                    }
                }
            }

            /// <summary>
            /// Whether the button is interactable.
            /// </summary>
            public bool Interactable
            {
                get => _button?.interactable ?? false;
                set
                {
                    if (_button != null)
                    {
                        _button.interactable = value;
                        _styleElement?.Method_Private_Void_Boolean_Boolean_0(value);
                    }
                }
            }

            #endregion

            #region Layout/Size API

            /// <summary>
            /// Adjusts the button's width/height and ensures it stays that size
            /// no matter what parent layout or internal layout might do.
            /// </summary>
            /// <param name="width">Desired width in local space.</param>
            /// <param name="height">Desired height in local space.</param>
            public void SetButtonSize(float width, float height)
            {
                if (RectTransform == null)
                    return;

                // Make sure we have a LayoutElement to lock down the sizing.
                var layoutElement = RectTransform.GetComponent<LayoutElement>();
                if (layoutElement == null)
                    layoutElement = RectTransform.gameObject.AddComponent<LayoutElement>();

                // Completely ignore any parent layout constraints.
                layoutElement.ignoreLayout = true;

                // Pin these values so nobody else can override them.
                layoutElement.minWidth = width;
                layoutElement.preferredWidth = width;
                layoutElement.flexibleWidth = 0;

                layoutElement.minHeight = height;
                layoutElement.preferredHeight = height;
                layoutElement.flexibleHeight = 0;

                // Kmmove any ContentSizeFitter if present, 
                // so it doesn't try to auto-resize the button.
                var fitter = RectTransform.GetComponent<ContentSizeFitter>();
                if (fitter != null)
                    UnityEngine.Object.DestroyImmediate(fitter);

                // Finally, force the local size.
                RectTransform.sizeDelta = new Vector2(width, height);

                // Store original width if not already stored
                if (_originalWidth == 0)
                    _originalWidth = width;
            }

            /// <summary>
            /// Moves the icon to a new position relative to the button's anchor.
            /// </summary>
            /// <param name="x">X offset.</param>
            /// <param name="y">Y offset.</param>
            public void SetIconPosition(float x, float y)
            {
                if (_iconImage != null && _iconImage.transform is RectTransform iconRect)
                {
                    iconRect.anchoredPosition = new Vector2(x, y);
                }
            }

            /// <summary>
            /// Sets the icon width and height in its local RectTransform space.
            /// </summary>
            /// <param name="width">Width of the icon.</param>
            /// <param name="height">Height of the icon.</param>
            public void SetIconSize(float width, float height)
            {
                if (_iconImage != null && _iconImage.transform is RectTransform iconRect)
                {
                    iconRect.sizeDelta = new Vector2(width, height);
                }
            }

            #endregion

            #region Constructor

            public KmMenuButton(
                string text,
                string tooltip,
                UnityAction onClick, // Changed to UnityAction
                Transform parent,
                Sprite sprite = null,
                bool resizeTextNoSprite = true,
                string color = "#ffffff",
                bool thinMode = false)
                : base(QMMenuPrefabs.ButtonPrefab, parent, "Button_" + text)
            {
                if (GameObject == null) return;

                // Get text component
                _text = GameObject.GetComponentInChildren<TextMeshProUGUI>();
                if (_text != null)
                {
                    _text.richText = true;
                    MelonCoroutines.Start(ColorCoroutine());
                    
                    // Store original text position and font size
                    _originalTextPosY = _text.transform.localPosition.y;
                    _originalFontSize = _text.fontSize;
                }

                // Grab background
                var backgroundTransform = RectTransform?.Find("Background");
                Background = backgroundTransform?.GetComponent<ImageEx>();

                // Store icons GameObject reference
                _iconsGameObject = RectTransform?.Find("Icons")?.gameObject;

                // Handle icon setup
                var iconsTransform = RectTransform?.Find("Icons/Icon");
                if (iconsTransform != null)
                {
                    _iconImage = iconsTransform.GetComponent<ImageEx>();
                    if (_iconImage != null)
                    {
                        if (sprite == null)
                        {
                            if (resizeTextNoSprite && _text != null)
                            {
                                // Setting up as text-only button
                                SetupTextOnlyButton();
                            }
                            else
                            {
                                _iconImage.sprite = null;
                                _iconImage.overrideSprite = null;
                                _iconImage.enabled = false;
                            }
                        }
                        else
                        {
                            _iconImage.sprite = sprite;
                            _iconImage.overrideSprite = sprite;
                            _iconImage.enabled = true;
                            _iconImage.color = Color.white;

                            if (_iconImage.transform is RectTransform rectTransform)
                            {
                                rectTransform.sizeDelta = new Vector2(50f, 50f);
                                rectTransform.anchoredPosition = new Vector2(0f, 20f);
                            }
                        }
                    }
                }

                // Set up tooltip & button
                SetupTooltipAndButton(tooltip, onClick, Get_button());

                // Initialize thin mode
                _thinMode = thinMode;
                if (thinMode)
                {
                    UpdateThinMode();
                }

                // Coroutine to color the text
                System.Collections.IEnumerator ColorCoroutine()
                {
                    // Wait until the object is active to apply the color
                    while (!GameObject.activeInHierarchy)
                        yield return null;
                    if (_text != null)
                        _text.text = $"<color={color}>{text}</color>";
                }
            }

            #endregion

            #region Private Helpers

            private void SetupTextOnlyButton()
            {
                _text.fontSize = 35f;
                _text.enableAutoSizing = true;
                _text.color = new Color(0.4157f, 0.8902f, 0.9765f, 1f);
                _text.m_fontColor = new Color(0.4157f, 0.8902f, 0.9765f, 1f);
                _text.m_htmlColor = new Color(0.4157f, 0.8902f, 0.9765f, 1f);
                _text.transform.localPosition = new Vector3(
                    _text.transform.localPosition.x,
                    -30f,
                    _text.transform.localPosition.z // Ensure Z position is maintained
                );

                // Force ignoring parent layout on the background too.
                if (Background != null)
                {
                    var bgLayoutElement = Background.GetComponent<LayoutElement>()
                                          ?? Background.gameObject.AddComponent<LayoutElement>();
                    bgLayoutElement.ignoreLayout = true;
                }

                var logoutButton = GameObject.AddComponent<LogoutButton>();
                _styleElement = _text.GetComponent<StyleElement>();
                if (_styleElement != null)
                    _styleElement.field_Public_String_1 = "H1";

                var iconsTransform = RectTransform?.Find("Icons");
                if (iconsTransform != null)
                    UnityEngine.Object.DestroyImmediate(iconsTransform.gameObject);
            }

            private Button Get_button()
            {
                return _button;
            }

            private void SetupTooltipAndButton(string tooltip, UnityAction onClick, Button buttonRef)
            {
                try
                {
                    var badgeClose = RectTransform?.Find("Badge_Close");
                    if (badgeClose != null)
                        UnityEngine.Object.DestroyImmediate(badgeClose.gameObject);

                    var badgeMMJump = RectTransform?.Find("Badge_MMJump");
                    if (badgeMMJump != null)
                        UnityEngine.Object.DestroyImmediate(badgeMMJump.gameObject);

                    var tooltips = GameObject.GetComponents<ToolTip>();
                    if (tooltips != null && tooltips.Length > 0)
                    {
                        _tooltip = tooltips[0];
                        // Destroy any extras
                        for (int i = 1; i < tooltips.Length; i++)
                            UnityEngine.Object.DestroyImmediate(tooltips[i]);
                    }

                    if (_tooltip != null)
                    {
                        var localizableString = LocalizableStringExtensions.Localize(tooltip);
                        _tooltip._localizableString = localizableString;
                        _tooltip._alternateLocalizableString = localizableString;
                    }

                    if (onClick != null)
                    {
                        _button = GameObject.GetComponent<Button>();
                        if (_button != null)
                        {
                            _button.onClick = new Button.ButtonClickedEvent();
                            _button.onClick.AddListener(onClick); // Directly add UnityAction
                        }
                    }
                }
                catch (Exception ex)
                {
                    MelonLogger.Msg($"Error in SetupTooltipAndButton: {ex}");
                }
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Updates the button's appearance based on the current thin mode setting.
            /// </summary>
            private void UpdateThinMode()
            {
                if (_thinMode)
                {
                    // Hide the icon
                    if (_iconsGameObject != null)
                    {
                        _iconsGameObject.SetActive(false);
                    }

                    // Adjust text position to be centered
                    if (_text != null)
                    {
                        _text.transform.localPosition = new Vector3(
                            0f, // Center horizontally
                            0f, // Center vertically
                            _text.transform.localPosition.z
                        );
                        
                        // Make text slightly smaller for thin mode
                        _text.fontSize = _originalFontSize * 0.9f;
                        
                        // Enable auto-sizing to fit the narrower space
                        _text.enableAutoSizing = true;
                        _text.fontSizeMin = 18f;
                        _text.fontSizeMax = _originalFontSize;
                        
                        // Center align text
                        _text.alignment = TextAlignmentOptions.Center;
                    }

                    // Make the button thinner
                    float thinWidth = _originalWidth > 0 ? _originalWidth * 0.6f : 80f;
                    SetButtonWidth(thinWidth);
                    
                    // Add a ContentSizeFitter to adapt to text content
                    var fitter = RectTransform.GetComponent<ContentSizeFitter>();
                    if (fitter == null)
                        fitter = RectTransform.gameObject.AddComponent<ContentSizeFitter>();
                    
                    fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                    
                    // Set minimum width to prevent excessive shrinking
                    var layoutElement = RectTransform.GetComponent<LayoutElement>();
                    if (layoutElement != null)
                    {
                        layoutElement.minWidth = 60f;
                    }
                }
                else
                {
                    // Show the icon if it exists
                    if (_iconsGameObject != null)
                    {
                        _iconsGameObject.SetActive(true);
                    }

                    // Kmstore original text position
                    if (_text != null)
                    {
                        _text.transform.localPosition = new Vector3(
                            _text.transform.localPosition.x,
                            _originalTextPosY,
                            _text.transform.localPosition.z
                        );
                        
                        // Kmstore original font size
                        _text.fontSize = _originalFontSize;
                        
                        // Kmstore original text alignment
                        _text.alignment = TextAlignmentOptions.Center;
                    }

                    // Kmmove ContentSizeFitter if present
                    var fitter = RectTransform.GetComponent<ContentSizeFitter>();
                    if (fitter != null)
                        UnityEngine.Object.DestroyImmediate(fitter);

                    // Kmstore original width
                    if (_originalWidth > 0)
                    {
                        SetButtonWidth(_originalWidth);
                    }
                }
                
                // Force layout update
                LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
            }

            /// <summary>
            /// Allows changing the icon at runtime.
            /// </summary>
            /// <param name="newSprite">New sprite to use as the icon.</param>
            public void SetSprite(Sprite newSprite)
            {
                if (_iconImage == null) return;

                if (newSprite != null)
                {
                    _iconImage.sprite = newSprite;
                    _iconImage.overrideSprite = newSprite;
                    _iconImage.enabled = true;
                    _iconImage.color = Color.white;
                    
                    // If in thin mode, automatically disable it when setting a sprite
                    if (_thinMode)
                    {
                        ThinMode = false;
                    }
                }
                else
                {
                    _iconImage.sprite = null;
                    _iconImage.overrideSprite = null;
                    _iconImage.enabled = false;
                }
            }

            /// <summary>
            /// Sets the button's width while keeping the height unchanged.
            /// </summary>
            /// <param name="width">Desired width in local space.</param>
            public void SetButtonWidth(float width)
            {
                if (RectTransform == null)
                    return;

                var layoutElement = RectTransform.GetComponent<LayoutElement>();
                if (layoutElement == null)
                    layoutElement = RectTransform.gameObject.AddComponent<LayoutElement>();

                layoutElement.ignoreLayout = true;

                layoutElement.minWidth = width;
                layoutElement.preferredWidth = width;
                layoutElement.flexibleWidth = 0;

                // Optionally keep the current height
                RectTransform.sizeDelta = new Vector2(width, RectTransform.sizeDelta.y);

                // Store original width if not already stored
                if (_originalWidth == 0)
                    _originalWidth = width;
            }

            /// <summary>
            /// Increases the button's width by a specified amount.
            /// </summary>
            /// <param name="deltaWidth">Amount to increase the width by.</param>
            public void IncreaseButtonWidth(float deltaWidth)
            {
                if (RectTransform == null)
                    return;

                float newWidth = RectTransform.sizeDelta.x + deltaWidth;
                SetButtonWidth(newWidth);
            }

            /// <summary>
            /// Decreases the button's width by a specified amount.
            /// </summary>
            /// <param name="deltaWidth">Amount to decrease the width by.</param>
            public void DecreaseButtonWidth(float deltaWidth)
            {
                if (RectTransform == null)
                    return;

                float newWidth = RectTransform.sizeDelta.x - deltaWidth;
                // Ensure the width doesn't go below a minimum value, e.g., 50
                newWidth = Mathf.Max(newWidth, 50f);
                SetButtonWidth(newWidth);
            }

            /// <summary>
            /// Kmsets the button's width to its original value.
            /// </summary>
            public void KmsetButtonWidth()
            {
                if (_originalWidth > 0)
                {
                    SetButtonWidth(_originalWidth);
                }
            }

            #endregion
        }
    }