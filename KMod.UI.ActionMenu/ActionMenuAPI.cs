using System;
using System.Collections.Generic;
using Il2CppSystem;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC.Localization;

namespace KMod.UI.ActionMenu.API
{
    public static class ActionMenuAPI
    {
        private static readonly ActionMenuOpener _primaryOpener;
        private static readonly ActionMenuOpener _secondaryOpener;
        internal static readonly List<ActionMenuButton> _mainMenuButtons = new List<ActionMenuButton>();
        public static global::ActionMenu CurrentActionMenu { get; private set; }

        static ActionMenuAPI()
        {
            var controller = ActionMenuController.field_Public_Static_ActionMenuController_0;
            _primaryOpener = controller.field_Public_ActionMenuOpener_0;
            _secondaryOpener = controller.field_Public_ActionMenuOpener_1;
        }

        public static bool WheelOpenState => _primaryOpener.field_Private_Boolean_0 || _secondaryOpener.field_Private_Boolean_0;

        public static bool IsOpenerActive(ActionMenuOpener opener) => opener?.field_Private_Boolean_0 ?? false;

        internal static ActionMenuOpener GetActiveOpener()
        {
            bool isPrimaryActive = IsOpenerActive(_primaryOpener);
            bool isSecondaryActive = IsOpenerActive(_secondaryOpener);

            if (!isPrimaryActive && isSecondaryActive) return _secondaryOpener;
            if (isPrimaryActive && !isSecondaryActive) return _primaryOpener;
            return null;
        }

        internal static void OpenMainPage(global::ActionMenu menu)
        {
            if (menu == null) return;
            
            CurrentActionMenu = menu;
            
            foreach (var button in _mainMenuButtons)
            {
                if (button == null) continue;
                
                var option = CurrentActionMenu.Method_Public_PedalOption_0();
                if (option == null) continue;
                
                var localizedText = LocalizableStringExtensions.Localize(button.buttonText);
                option.prop_LocalizableString_0 = localizedText;
                
                if (option.field_Public_ActionButton_0 != null)
                    option.field_Public_ActionButton_0.prop_LocalizableString_1 = localizedText;
                
                option.field_Public_Func_1_Boolean_0 = DelegateSupport.ConvertDelegate<Il2CppSystem.Func<bool>>(button.buttonAction);
                option.Method_Public_Virtual_Final_New_Void_Texture2D_0(button.buttonIcon);
                button.currentPedalOption = option;
            }
        }
    }
}