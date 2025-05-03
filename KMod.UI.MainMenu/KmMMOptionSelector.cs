using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using KMod.VRChat;
using TMPro;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Core.Styles;
using VRC.UI.Elements.Controls;

namespace KMod.UI.MainMenu
{
    public class KmMMOptionSelector : KmMMSectionElement
    {
        private readonly Button _prevButton;
        private readonly Button _nextButton;
        private readonly TextMeshProUGUI _optionText;
        private readonly Dictionary<string, Action> _options = new Dictionary<string, Action>();
        private readonly uint _defaultIndex;
        private bool _isRefreshing;

        public KmMMCategorySection Section { get; }
        public string CurrentOption { get; private set; }
        public int CurrentOptionIndex { get; private set; }

        public KmMMOptionSelector(string title, string tooltipForward = "Next", string tooltipBackward = "Back", 
                                 uint defaultOptionIndex = 0, Transform parent = null, bool separator = true, 
                                 string color = "#ffffff", KmMMCategorySection section = null)
            : base(MMenuPrefabs.MMSelectorPrefab, parent, true, separator)
        {
            _defaultIndex = defaultOptionIndex;
            Section = section;
            
            // Set title immediately without coroutine
            TextMeshProUGUI titleText = LeftItemContainer.Find("Title").GetComponent<TextMeshProUGUI>();
            titleText.richText = true;
            titleText.text = $"<color={color}>{title}</color>";
            
            StyleElement = RightItemContainer.GetComponent<StyleElement>();
            
            // Get UI components
            _prevButton = RightItemContainer.Find("ButtonLeft").GetComponent<Button>();
            _nextButton = RightItemContainer.Find("ButtonRight").GetComponent<Button>();
            _optionText = RightItemContainer.Find("OptionSelectionBox/Text_MM_H3").GetComponent<TextMeshProUGUI>();
            _optionText.text = string.Empty;
            
            // Set up buttons
            _prevButton.onClick.RemoveAllListeners();
            _nextButton.onClick.RemoveAllListeners();
            
            _prevButton.onClick.AddListener((UnityAction)OnPrevButtonClicked);
            _nextButton.onClick.AddListener((UnityAction)OnNextButtonClicked);
            
            // Configure tooltips
            SetTooltip(_prevButton, tooltipBackward);
            SetTooltip(_nextButton, tooltipForward);
            
            // Add category button listener
            if (section != null)
            {
                Button categoryButton = section.Category.ButtonObj.GetComponent<Button>();
                if (categoryButton != null)
                {
                    categoryButton.onClick.AddListener((UnityAction)KmfreshDelayed);
                }
            }
        }

        private void OnPrevButtonClicked()
        {
            if (_options.Count == 0) return;
            
            int newIndex = (CurrentOptionIndex == 0) ? _options.Count - 1 : CurrentOptionIndex - 1;
            Set((uint)newIndex, true);
        }

        private void OnNextButtonClicked()
        {
            if (_options.Count == 0) return;
            
            int newIndex = (CurrentOptionIndex + 1) % _options.Count;
            Set((uint)newIndex, true);
        }

        private void SetTooltip(Button button, string tooltipText)
        {
            if (button == null) return;
            
            LocalizableString tooltipString = LocalizableStringExtensions.Localize(tooltipText);
            
            ToolTip toolTip = null;
            Il2CppArrayBase<ToolTip> tooltips = button.GetComponents<ToolTip>();
            
            if (tooltips != null && tooltips.Length > 0)
            {
                toolTip = tooltips[0];
                
                for (int i = 1; i < tooltips.Length; i++)
                {
                    UnityEngine.Object.DestroyImmediate(tooltips[i]);
                }
                
                if (toolTip != null)
                {
                    toolTip._localizableString = tooltipString;
                    toolTip._alternateLocalizableString = tooltipString;
                }
            }
        }

        private void KmfreshDelayed()
        {
            if (!_isRefreshing)
            {
                _isRefreshing = true;
                MelonCoroutines.Start(KmfreshCoroutine());
            }
        }

        private IEnumerator KmfreshCoroutine()
        {
            // Use a single frame delay instead of time-based delay
            yield return null;
            
            Kmfresh();
            _isRefreshing = false;
        }

        public void Set(uint index, bool callBack = false)
        {
            if (_options.Count == 0) return;
            
            uint validIndex = (uint)(index % _options.Count);
            CurrentOptionIndex = (int)validIndex;
            
            KeyValuePair<string, Action> option = _options.ElementAt(CurrentOptionIndex);
            CurrentOption = option.Key;
            
            if (_optionText != null)
            {
                _optionText.text = CurrentOption;
            }
            
            if (callBack && option.Value != null)
            {
                option.Value();
            }
        }

        public void AddOption(string name, Action callback)
        {
            if (string.IsNullOrEmpty(name) || _options.ContainsKey(name)) return;
            
            _options[name] = callback;
            
            if (_options.Count == 1)
            {
                Set(_defaultIndex);
            }
        }

        public void RemoveOption(string name)
        {
            if (string.IsNullOrEmpty(name) || !_options.ContainsKey(name)) return;
            
            _options.Remove(name);
            
            if (_options.Count > 0 && CurrentOption == name)
            {
                Set(0);
            }
        }

        public void Kmfresh()
        {
            if (_options.Count > 0)
            {
                Set((uint)CurrentOptionIndex);
            }
        }
    }
}