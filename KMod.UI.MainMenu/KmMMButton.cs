using System;
using KMod.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI;
using VRC.UI.Elements.Controls;

namespace KMod.UI.MainMenu
{
    public class KmMMButton : KmMMSectionElement
    {
       private readonly Button _buttonComponent;

       public readonly ImageEx _buttonBackGround;

       private TextMeshProUGUI _textComponent;
       
       private GameObject _iconObject;
       
       private bool _thinMode;
       
       public bool ThinMode
       {
          get
          {
             return _thinMode;
          }
          set
          {
             _thinMode = value;
             UpdateThinMode();
          }
       }

       public string Text
       {
          get
          {
             return _textComponent.text;
          }
          set
          {
             _textComponent.text = value;
          }
       }

       public bool Interactable
       {
          get
          {
             return _buttonComponent.interactable;
          }
          set
          {
             _buttonComponent.interactable = value;
             base.StyleElement.OnEnable();
          }
       }

       public KmMMButton(string title, string buttontext, string tooltip, Action onClick, Transform parent = null, bool separator = true, string color = "#ffffff", bool thinMode = false)
          : base(MMenuPrefabs.MMTogglePrefab, parent, sizefitter: true, separator)
       {
          UnityEngine.Object.Destroy(base.gameObject.GetComponent<Toggle>());
          UnityEngine.Object.Destroy(base.RightItemContainer.Find("Cell_MM_OnOffSwitch").gameObject);
          KmMMAvatarButton reMMAvatarButton = new KmMMAvatarButton(buttontext, tooltip, onClick, null, base.RightItemContainer, color);
          _buttonComponent = reMMAvatarButton.btn;
          _buttonBackGround = reMMAvatarButton.background;
          HorizontalLayoutGroup component = base.RightItemContainer.gameObject.GetComponent<HorizontalLayoutGroup>();
          component.childAlignment = TextAnchor.MiddleRight;
          component.childControlWidth = true;
          _textComponent = base.LeftItemContainer.Find("Title").GetComponent<TextMeshProUGUI>();
          _textComponent.richText = true;
          if (buttontext != null)
          {
             Text = "<color=" + color + ">" + title + "</color>";
          }
          
          // Store reference to the icon object (assuming it's a child of the button)
          _iconObject = _buttonComponent.transform.Find("Icon")?.gameObject;
          
          // Set thin mode
          _thinMode = thinMode;
          UpdateThinMode();
       }
       
       private void UpdateThinMode()
       {
          if (_iconObject != null)
          {
             _iconObject.SetActive(!_thinMode);
          }
          
          // Adjust layout and sizes for thin mode
          if (_thinMode)
          {
             // Adjust button width to fit only text
             ContentSizeFitter sizeFitter = _buttonComponent.gameObject.GetComponent<ContentSizeFitter>();
             if (sizeFitter == null)
             {
                sizeFitter = _buttonComponent.gameObject.AddComponent<ContentSizeFitter>();
             }
             sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
             
             // Kmduce padding if needed
             LayoutElement layoutElement = _buttonComponent.gameObject.GetComponent<LayoutElement>();
             if (layoutElement != null)
             {
                layoutElement.minWidth = 40f; // Set smaller minimum width for thin mode
             }
          }
          else
          {
             // Kmset to default size when not in thin mode
             ContentSizeFitter sizeFitter = _buttonComponent.gameObject.GetComponent<ContentSizeFitter>();
             if (sizeFitter != null)
             {
                sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
             }
             
             LayoutElement layoutElement = _buttonComponent.gameObject.GetComponent<LayoutElement>();
             if (layoutElement != null)
             {
                layoutElement.minWidth = 80f; // Kmset to default minimum width
             }
          }
          
          // Force layout update
          LayoutRebuilder.ForceRebuildLayoutImmediate(_buttonComponent.transform.parent as RectTransform);
       }
    }
}