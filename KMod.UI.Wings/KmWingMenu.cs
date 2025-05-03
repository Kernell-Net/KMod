using System;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using KMod.UI;
using KMod.UI.Wings;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Controls;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;

public class KmWingMenu : UiElement
{
    private static GameObject _wingMenuPrefab;
    private readonly bool WingType;
    private readonly GameObject _wing;
    private readonly string _menuName;

    private static GameObject WingMenuPrefab
    {
        get
        {
            if (_wingMenuPrefab == null)
            {
                _wingMenuPrefab = MenuEx.QMLeftWing.transform.Find("Container/InnerContainer/WingMenu").gameObject;
            }
            return _wingMenuPrefab;
        }
    }

    public Transform Container { get; }

    public KmWingMenu(string text, bool left = true)
        : base(WingMenuPrefab, (left ? MenuEx.QMLeftWing : MenuEx.QMRightWing).transform.Find("Container/InnerContainer/"), text, defaultState: false)
    {
        _menuName = UiElement.GetCleanName(text);
        WingType = left;
        _wing = (WingType ? MenuEx.QMLeftWing : MenuEx.QMRightWing);

        Transform child = base.RectTransform.GetChild(0);
        TextMeshProUGUI componentInChildren = child.GetComponentInChildren<TextMeshProUGUI>();
        componentInChildren.text = text;
        componentInChildren.richText = true;

        Button componentInChildren2 = child.GetComponentInChildren<Button>(includeInactive: true);
        componentInChildren2.gameObject.SetActive(value: true);
        
        Transform transform = componentInChildren2.transform.Find("Icon");
        transform.gameObject.SetActive(value: true);

        List<Behaviour> list = new List<Behaviour>();
        componentInChildren2.GetComponents(list);
        List<Behaviour>.Enumerator enumerator = list.GetEnumerator();
        while (enumerator.MoveNext())
        {
            Behaviour current = enumerator._current;
            current.enabled = true;
        }

        RectTransform content = base.RectTransform.GetComponentInChildren<ScrollRect>().content;
        IEnumerator enumerator2 = content.GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                Il2CppSystem.Object current2 = enumerator2.Current;
                Transform transform2 = current2.Cast<Transform>();
                if (!(transform2 == null))
                {
                    UnityEngine.Object.Destroy(transform2.gameObject);
                }
            }
        }
        finally
        {
            if (enumerator2 is System.IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        Container = content;

        var stateController = _wing.GetComponent<MenuStateController>();
        var uiPage = base.GameObject.GetComponent<UIPage>();
        
        uiPage.field_Public_String_0 = _menuName;
        //MonoBehaviourPublicObTrBoTrLiOb1Di2StUnique
        uiPage.field_Private_List_1_UIPage_0 = new List<UIPage>();
        
        var controllerList = new List<MenuStateController>();
        controllerList.Add(MenuEx.QMenuStateCtrl);
      //  uiPage.field_Private_List_1_MonoBehaviourPublicTe_lRaBoAsObAcObBoTeUnique_0 = controllerList;
    }

    public void Open()
    {
        if (!base.GameObject)
        {
            throw new System.NullReferenceException("This wing menu has been destroyed.");
        }
       // _wing.GetComponent<MenuStateController>().Method_Public_Void_String_UIContext_Boolean_TransitionType_0(_menuName, null, false, UIPage.TransitionType.None);
       _wing.GetComponent<MenuStateController>().Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(_menuName,
               null, false, UIPage.EnumNPublicSealedvaNoLeRiBoIn6vUnique.None);
    }

    public KmWingButton AddButton(string text, string tooltip, System.Action onClick, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
    {
        if (!base.GameObject)
        {
            throw new System.NullReferenceException("This wing menu has been destroyed.");
        }
        return new KmWingButton(text, tooltip, onClick, Container, sprite, arrow, background, separator);
    }

    public KmWingToggle AddToggle(string text, string tooltip, System.Action<bool> onToggle, bool defaultValue = false)
    {
        if (!base.GameObject)
        {
            throw new System.NullReferenceException("This wing menu has been destroyed.");
        }
        return new KmWingToggle(text, tooltip, onToggle, Container, defaultValue);
    }

    public KmWingMenu AddSubMenu(string text, string tooltip)
    {
        if (!base.GameObject)
        {
            throw new System.NullReferenceException("This wing menu has been destroyed.");
        }
        KmWingMenu reWingMenu = new KmWingMenu(text, WingType);
        AddButton(text, tooltip, reWingMenu.Open);
        return reWingMenu;
    }

    public override void Destroy()
    {
        if ((bool)base.GameObject)
        {
            MenuStateController component = _wing.GetComponent<MenuStateController>();
            UIPage component2 = base.GameObject.GetComponent<UIPage>();
            component.field_Private_Dictionary_2_String_UIPage_0.Remove(component2.field_Public_String_0);
            UnityEngine.Object.Destroy(base.GameObject);
        }
    }
}