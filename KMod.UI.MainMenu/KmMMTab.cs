using System;
using MelonLoader;
using KMod.VRChat;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VRC.Localization;
using VRC.UI.Elements.Controls;

namespace KMod.UI.MainMenu
{
    public class KmMMTab : UiElement
    {
        public KmMMTab(string name, string tooltip, string pageName, Sprite sprite)
            : base(MMenuPrefabs.MMTabButtonPrefab, MMenuPrefabs.MMTabButtonPrefab.transform.parent, "Page_" + name)
        {
            try
            {
                // Set up MenuTab
                MenuTab component = RectTransform.GetComponent<MenuTab>();
                component.name = UiElement.GetCleanName("MainMenuReMod" + pageName);
                component.field_Private_MenuStateController_0 = MenuEx.MMenuStateCtrl;

                // Set up Button
                Button component2 = GameObject.GetComponent<Button>();
                component2.onClick = new Button.ButtonClickedEvent();
                
                // Create delegate for IL2CPP compatibility
                component2.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(() =>
                {
                    component.field_Private_MenuStateController_0.ShowTabContent();
                })));

                // Set up Tooltip
                var localizableString = LocalizableStringExtensions.Localize(tooltip);
                var component3 = GameObject.GetComponent<ToolTip>();
                component3._alternateLocalizableString = localizableString;
                component3._localizableString = localizableString;

                // Set up Icon
                var component4 = RectTransform.Find("Icon").GetComponent<Image>();
                component4.sprite = sprite;
                component4.overrideSprite = sprite;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Error creating KmMMTab: {ex}");
            }
        }

        public static KmMMTab Create(string name, string tooltip, string pageName, Sprite sprite)
        {
            return new KmMMTab(name, tooltip, pageName, sprite);
        }
    }
}