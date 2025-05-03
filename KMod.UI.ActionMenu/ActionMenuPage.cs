using System;
using System.Collections.Generic;
using Il2CppSystem;
using KMod.UI.ActionMenu.API;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC.Localization;

namespace KMod.UI.ActionMenu
{
    public class ActionMenuPage
    {
       internal readonly List<ActionMenuButton> buttons = new List<ActionMenuButton>();

       public ActionMenuPage previousPage { get; }

       public ActionMenuButton menuEntryButton { get; }

       public ActionMenuPage(string buttonText, Sprite buttonIcon = null)
       {
          menuEntryButton = new ActionMenuButton(buttonText, OpenMenu, buttonIcon);
       }

       public ActionMenuPage(ActionMenuPage basePage, string buttonText, Sprite buttonIcon = null)
       {
          previousPage = basePage;
          menuEntryButton = new ActionMenuButton(previousPage, buttonText, OpenMenu, buttonIcon);
       }

       internal void OpenMenu()
       {
          if (ActionMenuAPI.GetActiveOpener() == null)
          {
             return;
          }
          foreach (ActionMenuButton button in buttons)
          {
             ActionMenuAPI.GetActiveOpener().field_Public_ActionMenu_0.Method_Public_ObjectNPublicAcTeAcLoGaFu1BoUnique_Action_Action_Texture2D_LocalizableString_0((System.Action)delegate
             {
                PedalOption pedalOption = ActionMenuAPI.GetActiveOpener().field_Public_ActionMenu_0.Method_Public_PedalOption_0();
                pedalOption.prop_LocalizableString_0 = LocalizableStringExtensions.Localize(button.buttonText);
                pedalOption.field_Public_ActionButton_0.prop_LocalizableString_1 = LocalizableStringExtensions.Localize(button.buttonText);
                pedalOption.field_Public_Func_1_Boolean_0 = DelegateSupport.ConvertDelegate<Il2CppSystem.Func<bool>>(button.buttonAction);
                if (button.buttonIcon != null)
                {
                   pedalOption.Method_Public_Virtual_Final_New_Void_Texture2D_0(button.buttonIcon);
                }
                button.currentPedalOption = pedalOption;
             }, null, null, LocalizableStringExtensions.Localize(button.buttonText));
          }
       }
    }
}