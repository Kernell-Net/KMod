using System;
using KMod.VRChat;
using TMPro;
using UnityEngine;

namespace KMod.UI.QuickMenu
{
    public class KmMenuCategory : IButtonPage
    {
        public readonly KmMenuHeader Header;
        private readonly KmMenuButtonContainer _buttonContainer;

        public string Title
        {
            get { return Header.Title; }
            set { Header.Title = value; }
        }

        public bool Active
        {
            get { return _buttonContainer.GameObject.activeInHierarchy; }
            set
            {
                Header.Active = value;
                _buttonContainer.Active = value;
            }
        }

        public RectTransform RectTransform => _buttonContainer.RectTransform;

        /// <summary>
        /// Create a new KmMenuCategory.
        /// 
        /// The key addition here is 'bool skipLayoutGroup = false'.
        /// If skipLayoutGroup = true, the category's container and header
        /// will be placed in the parent's parent, skipping the VerticalLayoutGroup.
        /// </summary>
        public KmMenuCategory(
            string title,
            Transform parent = null,
            bool collapsible = true,
            string color = "#ffffff",
            bool skipLayoutGroup = false
        )
        {
            // Decide which transform to use as the parent
            // If skipLayoutGroup is true and the parent isn't null,
            // move one level up (i.e., parent.parent).
            Transform actualParent = parent;
            if (skipLayoutGroup && parent != null)
            {
                actualParent = parent.parent; 
            }

            // Create the header (collapsible or not) in the chosen parent
            if (collapsible)
            {
                var reMenuHeaderCollapsible = new KmMenuHeaderCollapsible("<color=" + color + ">" + title + "</color>", actualParent);
                reMenuHeaderCollapsible.OnToggle = (Action<bool>)Delegate.Combine(
                    reMenuHeaderCollapsible.OnToggle,
                    (Action<bool>)(b => _buttonContainer.GameObject.SetActive(b))
                );
                Header = reMenuHeaderCollapsible;
            }
            else
            {
                var header = new KmMenuHeader("<color=" + color + ">" + title + "</color>", actualParent);
                Header = header;
            }

            // Create the container in the chosen parent
            _buttonContainer = new KmMenuButtonContainer("<color=" + color + ">" + title + "</color>", actualParent);
        }

        public KmMenuCategory(KmMenuHeader headerElement, KmMenuButtonContainer container)
        {
            Header = headerElement;
            _buttonContainer = container;
        }

        public KmMenuButton AddButton(string text, string tooltip, Action onClick, Sprite sprite = null, string color = "#ffffff")
        {
            return new KmMenuButton(text, tooltip, onClick, _buttonContainer.RectTransform, sprite, resizeTextNoSprite: true, color);
        }

        public KmMenuButton AddSpacer(Sprite sprite = null)
        {
            var reMenuButton = AddButton(string.Empty, string.Empty, null, sprite);
            reMenuButton.GameObject.name = "Button_Spacer";
            ((Component)reMenuButton.Background).gameObject.SetActive(false);
            return reMenuButton;
        }

        public KmMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, string color = "#ffffff")
        {
            return AddToggle(text, tooltip, onToggle, defaultValue: false, color);
        }

        public KmMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue = false, string color = "#ffffff")
        {
            return AddToggle(text, tooltip, onToggle, defaultValue, null, null, color);
        }

     

        public KmMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle)
        {
            return AddToggle(text, tooltip, onToggle, defaultValue: false, null, null);
        }

        public KmMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, string color = "#ffffff")
        {
            return AddToggle(text, tooltip, configValue, null, null, color);
        }

        public KmMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue)
        {
            return AddToggle(text, tooltip, configValue, null, null);
        }

        public KmMenuToggle AddToggle(
            string text,
            string tooltip,
            Action<bool> onToggle,
            bool defaultValue,
            Sprite iconOn,
            Sprite iconOff,
            string color = "#ffffff"
        )
        {
            return new KmMenuToggle(text, tooltip, onToggle, _buttonContainer.RectTransform, defaultValue, iconOn, iconOff, color);
        }

        public KmMenuToggle AddToggle(
            string text,
            string tooltip,
            Action<bool> onToggle,
            bool defaultValue,
            Sprite iconOn,
            Sprite iconOff
        )
        {
            return new KmMenuToggle(text, tooltip, onToggle, _buttonContainer.RectTransform, defaultValue, iconOn, iconOff);
        }

        public KmMenuToggle AddToggle(
            string text,
            string tooltip,
            ConfigValue<bool> configValue,
            Sprite iconOn,
            Sprite iconOff,
            string color = "#ffffff"
        )
        {
            return new KmMenuToggle(text, tooltip, configValue.SetValue, _buttonContainer.RectTransform, configValue, iconOn, iconOff, color);
        }

        public KmMenuToggle AddToggle(
            string text,
            string tooltip,
            ConfigValue<bool> configValue,
            Sprite iconOn,
            Sprite iconOff
        )
        {
            return new KmMenuToggle(text, tooltip, configValue.SetValue, _buttonContainer.RectTransform, configValue, iconOn, iconOff);
        }

        /// <summary>
        /// Adds a description text element to the category.
        /// </summary>
        /// <param name="text">The description text to display.</param>
        /// <param name="color">HTML color code for the text.</param>
        /// <param name="fontSize">Font size for the text.</param>
        /// <param name="alignment">Text alignment.</param>
        /// <returns>The created KmMenuDesc instance.</returns>
        public KmMenuDesc AddDescription(
            string text,
            string color = "#FFFFFF",
            float fontSize = 14f,
            TextAlignmentOptions alignment = TextAlignmentOptions.Center
        )
        {
            return new KmMenuDesc(text, _buttonContainer.RectTransform, color, fontSize, alignment);
        }

        /// <summary>
        /// Adds a detailed description with additional customization options.
        /// </summary>
        public KmMenuDesc AddDetailedDescription(
            string text,
            string color = "#FFFFFF",
            float fontSize = 14f,
            TextAlignmentOptions alignment = TextAlignmentOptions.Center,
            bool isBold = false,
            bool isItalic = false,
            float width = 0f
        )
        {
            var desc = new KmMenuDesc(text, _buttonContainer.RectTransform, color, fontSize, alignment);
            desc.SetBold(isBold);
            desc.SetItalic(isItalic);
            return desc;
        }

        /// <summary>
        /// Adds a highlighted description with a different background color.
        /// </summary>
        public KmMenuDesc AddHighlightedDescription(
            string text,
            string textColor = "#FFFFFF",
            string backgroundColor = "#444444",
            float fontSize = 14f
        )
        {
            var desc = new KmMenuDesc(text, _buttonContainer.RectTransform, textColor, fontSize);
            string highlightedText = $"<mark={backgroundColor}>{text}</mark>";
            desc.SetColoredText(highlightedText, textColor);
            return desc;
        }

        public KmMenuPage AddMenuPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
        {
            KmMenuPage menuPage = GetMenuPage(text);
            if (menuPage != null) return menuPage;
            
            var reMenuPage = new KmMenuPage(text, isRoot: false, color);
            AddButton(
                text,
                string.IsNullOrEmpty(tooltip) ? ("Open the " + text + " menu") : tooltip,
                reMenuPage.Open,
                sprite,
                color
            );
            return reMenuPage;
        }

        public KmTabbedPage AddTabbedPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
        {
            KmTabbedPage tabbedPage = GetTabbedPage(text);
            if (tabbedPage != null) return tabbedPage;
            
            var reTabbedPage = new KmTabbedPage(text, isRoot: false, color);
            AddButton(
                text,
                string.IsNullOrEmpty(tooltip) ? ("Open the " + text + " menu") : tooltip,
                reTabbedPage.Open,
                sprite,
                color
            );
            return reTabbedPage;
        }

        public KmMenuPage ToMenuPage(string name, string tooltip = "", Sprite sprite = null)
        {
            var menuPage = GetMenuPage(name);
            AddButton(name, string.IsNullOrEmpty(tooltip) ? ("Open the " + name + " menu") : tooltip, menuPage.Open, sprite);
            return menuPage;
        }

        public KmCategoryPage ToCategoryPage(string name, string tooltip = "", Sprite sprite = null)
        {
            var categoryPage = GetCategoryPage(name);
            AddButton(name, string.IsNullOrEmpty(tooltip) ? ("Open the " + name + " menu") : tooltip, categoryPage.Open, sprite);
            return categoryPage;
        }

        public KmCategoryPage AddCategoryPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
        {
            KmCategoryPage categoryPage = GetCategoryPage(text);
            if (categoryPage != null) return categoryPage;
            
            var reCategoryPage = new KmCategoryPage(text, isRoot: false, color);
            AddButton(
                text,
                string.IsNullOrEmpty(tooltip) ? ("Open the " + text + " menu") : tooltip,
                reCategoryPage.Open,
                sprite,
                color
            );
            return reCategoryPage;
        }

        public void AddMenuPage(string text, string tooltip, Action<KmMenuPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
        {
            onPageBuilt(AddMenuPage(text, tooltip, sprite, color));
        }

        public void AddCategoryPage(string text, string tooltip, Action<KmCategoryPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
        {
            onPageBuilt(AddCategoryPage(text, tooltip, sprite, color));
        }

        public void AddTabbedPage(string text, string tooltip, Action<KmTabbedPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
        {
            onPageBuilt(AddTabbedPage(text, tooltip, sprite, color));
        }

        public KmMenuPage GetMenuPage(string name)
        {
            Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
            return (transform == null) ? null : new KmMenuPage(transform);
        }

        public KmCategoryPage GetCategoryPage(string name)
        {
            Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
            return (transform == null) ? null : new KmCategoryPage(transform);
        }

        public KmTabbedPage GetTabbedPage(string name)
        {
            Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
            return (transform == null) ? null : new KmTabbedPage(transform);
        }
        
        public void ClearSubCategories()
        {
            // This method clears all child UI elements (subcategories and buttons) 
            // from the container. Modify if you need to preserve certain static elements.
            for (int i = _buttonContainer.RectTransform.childCount - 1; i >= 0; i--)
            {
                Transform child = _buttonContainer.RectTransform.GetChild(i);
                if (child != null)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }


        /// <summary>
        /// Clears all the buttons (and any other UI elements) from this category.
        /// </summary>
        public void Clear()
        {
            for (int i = _buttonContainer.RectTransform.childCount - 1; i >= 0; i--)
            {
                Transform child = _buttonContainer.RectTransform.GetChild(i);
                if (child != null)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
    }
}
    