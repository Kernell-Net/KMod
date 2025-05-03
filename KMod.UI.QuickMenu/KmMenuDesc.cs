using System.Collections;
using MelonLoader;
using KMod.VRChat;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VRC.UI.Elements.Controls;

namespace KMod.UI.QuickMenu
{
    public class KmMenuDesc : UiElement
    {
        private TextMeshProUGUI _textComponent;
        private static GameObject _descPrefab;
        
        private static GameObject DescriptionPrefab
        {
            get
            {
                if (_descPrefab == null)
                {
                    string textPath = "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup/QMCell_InstanceDetails/Panel/Info/Text_DetailsHeader";
                    Transform textTransform = GameObject.Find(textPath)?.transform;
                    if (textTransform != null)
                    {
                        _descPrefab = textTransform.gameObject;
                        MelonLogger.Msg("Found description text prefab (Text_DetailsHeader) successfully");
                    }
                    else
                    {
                        // Try alternative path
                        string altPath = "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_H1/LeftItemContainer/Text_Title";
                        textTransform = GameObject.Find(altPath)?.transform;
                        
                        if (textTransform != null)
                        {
                            _descPrefab = textTransform.gameObject;
                            MelonLogger.Msg("Found alternative text prefab (Text_Title) successfully");
                        }
                        else
                        {
                            // Fallback to menu header prefab as last resort
                            _descPrefab = QMMenuPrefabs.MenuCategoryHeaderPrefab;
                            MelonLogger.Warning("Could not find text prefabs, using header prefab as fallback");
                        }
                    }
                }
                return _descPrefab;
            }
        }

        public string Text
        {
            get => _textComponent?.text ?? string.Empty;
            set
            {
                if (_textComponent != null)
                {
                    MelonCoroutines.Start(SetTextWhenActive(value));
                }
            }
        }

        public float FontSize
        {
            get => _textComponent?.fontSize ?? 12f;
            set
            {
                if (_textComponent != null)
                {
                    _textComponent.fontSize = value;
                }
            }
        }

        public TextAlignmentOptions TextAlignment
        {
            get => _textComponent?.alignment ?? TextAlignmentOptions.Center;
            set
            {
                if (_textComponent != null)
                {
                    _textComponent.alignment = value;
                }
            }
        }

        public KmMenuDesc(
            string text,
            Transform parent,
            string color = "#FFFFFF",
            float fontSize = 14f,
            TextAlignmentOptions alignment = TextAlignmentOptions.Left
        )
            : base(DescriptionPrefab, parent, "Desc_" + UiElement.GetCleanName(text))
        {
            Initialize(text, color, fontSize, alignment, false);
        }

        public KmMenuDesc(
            string text,
            KmMenuCategory category,
            string color = "#FFFFFF",
            float fontSize = 14f,
            TextAlignmentOptions alignment = TextAlignmentOptions.Left
        )
            : base(DescriptionPrefab, category.RectTransform, "Desc_" + UiElement.GetCleanName(text))
        {
            Initialize(text, color, fontSize, alignment, true);
        }

        private void Initialize(
            string text,
            string color,
            float fontSize,
            TextAlignmentOptions alignment,
            bool inCategory
        )
        {
            try
            {
                _textComponent = GameObject.GetComponent<TextMeshProUGUI>()
                                ?? GameObject.GetComponentInChildren<TextMeshProUGUI>(true);

                if (_textComponent == null)
                {
                    MelonLogger.Error($"Could not find TextMeshProUGUI component for: {text}");
                    GameObject textObj = new GameObject("DescText");
                    textObj.transform.SetParent(GameObject.transform, false);

                    RectTransform textRect = textObj.AddComponent<RectTransform>();
                    textRect.anchorMin = new Vector2(0, 0);
                    textRect.anchorMax = new Vector2(1, 1);
                    textRect.sizeDelta = Vector2.zero;
                    
                    _textComponent = textObj.AddComponent<TextMeshProUGUI>();
                }

                if (_textComponent != null)
                {
                    _textComponent.richText = true;
                    _textComponent.fontSize = fontSize;
                    _textComponent.alignment = alignment;
                    _textComponent.enableWordWrapping = true;
                    _textComponent.overflowMode = TextOverflowModes.Overflow;
                    MelonCoroutines.Start(SetTextWhenActive($"<color={color}>{text}</color>"));
                }

                // Adjust how the layout is handled so the text lines up properly in a vertical list.
                SetupLayout(inCategory);
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"Error initializing KmMenuDesc: {ex}");
            }
        }

        /// <summary>
        /// Kmmoves or modifies layout constraints so text can flow properly 
        /// under a VerticalLayoutGroup or other container.
        /// </summary>
        private void SetupLayout(bool inCategory)
        {
            // If you want the parent’s vertical layout group to position each KmMenuDesc 
            // in order (top to bottom), DO NOT ignore layout:
            LayoutElement layoutElement = GameObject.GetComponent<LayoutElement>();
            if (layoutElement == null)
                layoutElement = GameObject.AddComponent<LayoutElement>();

            // If you want the text to appear in the normal flow, set this to false:
            layoutElement.ignoreLayout = false;

            // You can still tweak minHeight or preferredHeight if you want 
            // a certain spacing:
            // layoutElement.minHeight = 20;

            RectTransform rectTransform = GameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Anchor to top-left to ensure text starts at the left 
                // and flows downward if the parent's layout group is top-left aligned
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(0.5f, 1f);

                // Let the parent's layout group manage the actual y-position
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }

            KmmoveInterferingComponents();
        }

        /// <summary>
        /// Kmmoves or modifies any layout group components that might conflict with 
        /// the parent's vertical layout group. 
        /// </summary>
        private void KmmoveInterferingComponents()
        {
            HorizontalLayoutGroup hlg = GameObject.GetComponent<HorizontalLayoutGroup>();
            if (hlg != null)
            {
                
                hlg.childControlWidth = true;
                hlg.childForceExpandWidth = true;
                hlg.childControlHeight = true;
                hlg.childForceExpandHeight = true;
            }
            // Kmmove or adjust any other layout groups if needed
        }

        private IEnumerator SetTextWhenActive(string text)
        {
            int attempts = 0;
            while (!GameObject.activeInHierarchy && attempts < 30)
            {
                attempts++;
                yield return new WaitForSeconds(0.1f);
            }
            
            if (_textComponent != null)
            {
                _textComponent.text = text;
            }
        }

        public void SetColoredText(string text, string color)
        {
            Text = $"<color={color}>{text}</color>";
        }

        public void SetItalic(bool italic)
        {
            if (_textComponent != null)
            {
                FontStyles newStyle = _textComponent.fontStyle;
                if (italic) newStyle |= FontStyles.Italic;
                else newStyle &= ~FontStyles.Italic;
                _textComponent.fontStyle = newStyle;
            }
        }

        public void SetBold(bool bold)
        {
            if (_textComponent != null)
            {
                FontStyles newStyle = _textComponent.fontStyle;
                if (bold) newStyle |= FontStyles.Bold;
                else newStyle &= ~FontStyles.Bold;
                _textComponent.fontStyle = newStyle;
            }
        }

        public void SetMargins(int left, int right, int top, int bottom)
        {
            LayoutGroup layoutGroup = GameObject.GetComponent<LayoutGroup>();
            if (layoutGroup != null)
            {
                layoutGroup.padding = new RectOffset(left, right, top, bottom);
            }
        }

        public static KmMenuDesc CreateHeader(Transform parent, string text, string color = "#FFAA00")
        {
            KmMenuDesc header = new KmMenuDesc(text, parent, color, 16f, TextAlignmentOptions.Center);
            header.SetBold(true);

            LayoutElement layoutElement = header.GameObject.GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                // Typically you might allow the parent's layout to position it,
                // but if you want the header to be "outside" the normal flow:
                // layoutElement.ignoreLayout = true;
            }
            return header;
        }

        public static KmMenuDesc CreateCategoryHeader(KmMenuCategory category, string text, string color = "#FFAA00")
        {
            KmMenuDesc header = new KmMenuDesc(text, category, color, 16f, TextAlignmentOptions.Center);
            header.SetBold(true);

            LayoutElement layoutElement = header.GameObject.GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                // Same note about ignoreLayout
                // layoutElement.ignoreLayout = true;
            }
            return header;
        }

        public static KmMenuDesc CreateDivider(Transform parent, string color = "#888888")
        {
            KmMenuDesc divider = new KmMenuDesc("───────────────────────", parent, color, 12f, TextAlignmentOptions.Center);

            LayoutElement layoutElement = divider.GameObject.GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                // layoutElement.ignoreLayout = true;
            }
            return divider;
        }

        public static KmMenuDesc CreateCategoryDivider(KmMenuCategory category, string color = "#888888")
        {
            KmMenuDesc divider = new KmMenuDesc("───────────────────────", category, color, 12f, TextAlignmentOptions.Center);

            LayoutElement layoutElement = divider.GameObject.GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                // layoutElement.ignoreLayout = true;
            }
            return divider;
        }
    }
}
