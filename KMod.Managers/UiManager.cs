using KMod.UI.MainMenu;
using KMod.UI.QuickMenu;
using KMod.VRChat;
using UnityEngine;
using VRC.UI.Elements;

namespace KMod.Managers
{
    public class UiManager
    {
       private static bool isMMPageCreated;
       private static KmMMPage mmpage;

       public IButtonPage QMMenu { get; }
       public IButtonPage TargetMenu { get; }
       public IButtonPage LaunchPad { get; }
       public IButtonPage LaunchPadTargetMenu { get; }
       public KmMMCategory MMenu { get; }

       // New properties for button layout settings
       public ButtonLayoutShape QMMenuLayout 
       { 
           get => QMMenu is KmMenuPage page ? page.LayoutShape : ButtonLayoutShape.Grid;
           set 
           { 
               if (QMMenu is KmMenuPage page) 
                   page.SetButtonLayout(value); 
           }
       }



       public VRCUiPage getcurrentopenpage()
       {
           
    

           return null;
       }

       public bool UseThinButtons
       {
           get => QMMenu is KmMenuPage page && page.UseThinButtons;
           set
           {
               if (QMMenu is KmMenuPage page)
                   page.SetThinButtons(value);
               
               if (TargetMenu is KmMenuPage targetPage)
                   targetPage.SetThinButtons(value);
               
               if (LaunchPad is KmMenuPage launchPage)
                   launchPage.SetThinButtons(value);
           }
       }

       public UiManager(
           string menuName, 
           Sprite menuSprite, 
           bool createQMTargets = true, 
           bool createLaunchPadMenu = true, 
           bool createMainMenu = false, 
           string color = "#ffffff",
           ButtonLayoutShape initialLayout = ButtonLayoutShape.Grid,
           bool useThinButtons = false)
       {
          // Create main QuickMenu page
          KmMenuPage qmMenuPage = new KmMenuPage(menuName, isRoot: true, color);
          QMMenu = qmMenuPage;
          
          // Apply initial layout settings
          qmMenuPage.SetButtonLayout(initialLayout);
          qmMenuPage.SetThinButtons(useThinButtons);
          
          // Create tab button
          KmTabButton.Create(
              menu: qmMenuPage, 
              name: menuName, 
              tooltip: "Open the " + menuName + " menu.", 
              pageName: menuName, 
              sprite: menuSprite);
          
          // Create target menu if requested
          if (createQMTargets)
          {
             KmCategoryPage reCategoryPage = new KmCategoryPage(MenuEx.QMSelectedUserLocal.transform);
             TargetMenu = reCategoryPage.AddCategory(menuName ?? "", color);
             
             // Apply thin button setting if needed
             if (TargetMenu is KmMenuPage targetPage && useThinButtons)
                 targetPage.SetThinButtons(true);
          }
          
          // Create launch pad menu if requested
          if (createLaunchPadMenu)
          {
             KmCategoryPage reCategoryPage2 = new KmCategoryPage(MenuEx.QMDashboardMenu.transform);
             LaunchPad = reCategoryPage2.AddCategory(menuName ?? "", color);
             
             // Apply thin button setting if needed
             if (LaunchPad is KmMenuPage launchPage && useThinButtons)
                 launchPage.SetThinButtons(true);
          }
          
          // Create main menu if requested
          if (createMainMenu)
          {
             if (!isMMPageCreated)
             {
                mmpage = new KmMMPage("KmMod", null, isRoot: true);
                KmMMTab.Create("KmMod", "Open the KmMod menu.", "KmMod", menuSprite);
                isMMPageCreated = true;
             }
             if (mmpage != null)
             {
                MMenu = mmpage.AddnGetMenuCategory(menuName, menuName, menuSprite, color);
             }
          }
       }
       
       /// <summary>
       /// Sets the layout of buttons in the QuickMenu page
       /// </summary>
       /// <param name="layoutShape">The desired button layout</param>
       public void SetQMButtonLayout(ButtonLayoutShape layoutShape)
       {
           QMMenuLayout = layoutShape;
       }
       
       /// <summary>
       /// Sets the layout of buttons in all menu pages
       /// </summary>
       /// <param name="layoutShape">The desired button layout</param>
       public void SetAllButtonLayouts(ButtonLayoutShape layoutShape)
       {
           if (QMMenu is KmMenuPage qmPage)
               qmPage.SetButtonLayout(layoutShape);
               
           if (TargetMenu is KmMenuPage targetPage)
               targetPage.SetButtonLayout(layoutShape);
               
           if (LaunchPad is KmMenuPage launchPage)
               launchPage.SetButtonLayout(layoutShape);
       }
       
       /// <summary>
       /// Toggles thin button mode across all menus
       /// </summary>
       /// <param name="useThin">Whether to use thin buttons</param>
       public void SetThinButtonsForAll(bool useThin)
       {
           UseThinButtons = useThin;
       }
       
       /// <summary>
       /// Adds a button to the QMMenu with proper layout and thin mode settings
       /// </summary>
       /// <param name="text">Button text</param>
       /// <param name="tooltip">Button tooltip</param>
       /// <param name="onClick">Action to perform when clicked</param>
       /// <param name="sprite">Button sprite (optional)</param>
       /// <param name="color">Button color (optional)</param>
       /// <returns>The created button</returns>
       public KmMenuButton AddButton(string text, string tooltip, System.Action onClick, Sprite sprite = null, string color = "#ffffff")
       {
           if (QMMenu is KmMenuPage page)
           {
               return page.AddButton(text, tooltip, onClick, sprite, color);
           }
           return null;
       }
       
       /// <summary>
       /// Adds a toggle to the QMMenu with proper layout and thin mode settings
       /// </summary>
       /// <param name="text">Toggle text</param>
       /// <param name="tooltip">Toggle tooltip</param>
       /// <param name="onToggle">Action to perform when toggled</param>
       /// <param name="defaultValue">Initial toggle state</param>
       /// <param name="color">Toggle color (optional)</param>
       /// <returns>The created toggle</returns>
       public KmMenuToggle AddToggle(string text, string tooltip, System.Action<bool> onToggle, bool defaultValue = false, string color = "#ffffff")
       {
           if (QMMenu is KmMenuPage page)
           {
               return page.AddToggle(text, tooltip, onToggle, defaultValue, color);
           }
           return null;
       }
    }
}