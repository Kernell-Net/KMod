using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Menus;

namespace KMod.VRChat
{
	public static class QMMenuPrefabs
	{
		private static GameObject _togglePrefab;

		private static GameObject _sliderPrefab;

		private static GameObject _wingButtonPrefab;

		private static GameObject _wingMenuPrefab;

		private static GameObject _tabButtonPrefab;

		private static GameObject _ContainerPrefab;

		private static GameObject _radioTogglePagePrefab;

		private static GameObject _radioTogglePrefab;

		private static GameObject _menuPagePrefab;

		private static GameObject _categoryPagePrefab;

		private static GameObject _tabbedPagePrefab;

		private static GameObject _tabPrefab;

		private static GameObject _tabcontentPrefab;

		private static GameObject _buttonPrefab;

		private static GameObject _labelPrefab;

		private static GameObject _henuCategoryHeaderPrefab;

		private static GameObject _menuCategoryHeaderCollapsiblePrefav;

		private static GameObject _menuCategoryContainerPrefab;

		private static GameObject _newContainerPrefab;

		private static GameObject _newBackgroundPrefab;

		private static GameObject _menuCategoryTogglePrefab;

		private static GameObject _sliderTogglePrefab;

		public static GameObject TogglePrefab
		{
			get
			{
				if (_togglePrefab == null)
				{
					_togglePrefab = MenuEx.QMDashboardMenu.transform.Find("ScrollRect").GetComponent<ScrollRect>().content.Find("Buttons_QuickActions/SitStandCalibrateButton/Button_SitStand").gameObject;
				}
				return _togglePrefab;
			}
		}

		public static GameObject SliderPrefab
		{
			get
			{
				if (_sliderPrefab == null)
				{
					_sliderPrefab = MenuEx.QMAudioSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("AudioVolume/QM_Settings_Panel/VerticalLayoutGroup/Master").gameObject;
				}
				return _sliderPrefab;
			}
		}

		public static GameObject WingButtonPrefab
		{
			get
			{
				if (_wingButtonPrefab == null)
				{
					_wingButtonPrefab = MenuEx.QMLeftWing.transform.Find("Container/InnerContainer/WingMenu/ScrollRect").GetComponent<ScrollRect>().content.Find("Button_Profile").gameObject;
				}
				return _wingButtonPrefab;
			}
		}

		public static GameObject WingMenuPrefab
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

		public static GameObject TabButtonPrefab
		{
			get
			{
				if (_tabButtonPrefab == null)
				{
					_tabButtonPrefab = MenuEx.QMInstance.transform.Find("CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings").gameObject;
				}
				return _tabButtonPrefab;
			}
		}

		public static GameObject ContainerPrefab
		{
			get
			{
				if (_ContainerPrefab == null)
				{
					_ContainerPrefab = MenuEx.QMAudioSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.gameObject;
				}
				return _ContainerPrefab;
			}
		}

		public static GameObject RadioTogglePagePrefab
		{
			get
			{
				if (_radioTogglePagePrefab == null)
				{
					_radioTogglePagePrefab = MenuEx.QMenuParent.transform.Find("Menu_ChangeAudioInputDevice").gameObject;
				}
				return _radioTogglePagePrefab;
			}
		}

		public static GameObject RadioTogglePrefab
		{
			get
			{
				if (_radioTogglePrefab == null)
				{
					GameObject gameObject = MenuEx.QMenuParent.transform.Find("Menu_ChangeAudioInputDevice").gameObject;
					MonoBehaviourPublicSt_cReGa_c_eBo_fObAcUnique component = gameObject.GetComponent<MonoBehaviourPublicSt_cReGa_c_eBo_fObAcUnique>();
					_radioTogglePrefab = component.gameObject;
				}
				return _radioTogglePrefab;
			}
		}

		public static GameObject MenuPagePrefab
		{
			get
			{
				if (_menuPagePrefab == null)
				{
					_menuPagePrefab = MenuEx.QMDevToolsMenu.gameObject;
				}
				return _menuPagePrefab;
			}
		}

		public static GameObject CategoryPagePrefab
		{
			get
			{
				if (_categoryPagePrefab == null)
				{
					_categoryPagePrefab = MenuEx.QMDashboardMenu.gameObject;
				}
				return _categoryPagePrefab;
			}
		}

		public static GameObject TabbedPagePrefab
		{
			get
			{
				if (_tabbedPagePrefab == null)
				{
					_tabbedPagePrefab = MenuEx.QMNotificationMenu.gameObject;
				}
				return _tabbedPagePrefab;
			}
		}

		public static GameObject TabPrefab
		{
			get
			{
				if (_tabPrefab == null)
				{
					_tabPrefab = MenuEx.QMNotificationMenu.transform.Find("Panel_Notification_Tabs/Tabs/InvitesTab").gameObject;
				}
				return _tabPrefab;
			}
		}

		public static GameObject TabContentPrefab
		{
			get
			{
				if (_tabcontentPrefab == null)
				{
					_tabcontentPrefab = MenuEx.QMNotificationMenu.transform.Find("Panel_Content/Invites").gameObject;
				}
				return _tabcontentPrefab;
			}
		}

		public static GameObject ButtonPrefab
		{
			get
			{
				if (_buttonPrefab == null)
				{
					_buttonPrefab = MenuEx.QMDashboardMenu.transform.Find("ScrollRect").GetComponent<ScrollRect>().content.Find("Buttons_QuickActions/Button_Respawn").gameObject;
				}
				return _buttonPrefab;
			}
		}

		public static GameObject LabelPrefab
		{
			get
			{
				if (_labelPrefab == null)
				{
					_labelPrefab = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("Debug/QM_Settings_Panel/VerticalLayoutGroup/Stats/LeftItemContainer/Cell_QM_SettingStat").gameObject;
				}
				return _labelPrefab;
			}
		}

		public static GameObject MenuCategoryHeaderPrefab
		{
			get
			{
				if (_henuCategoryHeaderPrefab == null)
				{
					_henuCategoryHeaderPrefab = MenuEx.QMDashboardMenu.transform.Find("ScrollRect").GetComponent<ScrollRect>().content.Find("Header_QuickActions").gameObject;
				}
				return _henuCategoryHeaderPrefab;
			}
		}

		public static GameObject MenuCategoryHeaderCollapsiblePrefav
		{
			get
			{
				if (_menuCategoryHeaderCollapsiblePrefav == null)
				{
					_menuCategoryHeaderCollapsiblePrefav = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("UIElements/QM_Foldout").gameObject;
				}
				return _menuCategoryHeaderCollapsiblePrefav;
			}
		}

		public static GameObject MenuCategoryContainerPrefab
		{
			get
			{
				if (_menuCategoryContainerPrefab == null)
				{
					_menuCategoryContainerPrefab = MenuEx.QMDashboardMenu.transform.Find("ScrollRect").GetComponent<ScrollRect>().content.Find("Buttons_QuickActions").gameObject;
				}
				return _menuCategoryContainerPrefab;
			}
		}

		public static GameObject NewContainerPrefab
		{
			get
			{
				if (_newContainerPrefab == null)
				{
					_newContainerPrefab = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("AdvancedOptions/QM_Settings_Panel/VerticalLayoutGroup/").gameObject;
				}
				return _newContainerPrefab;
			}
		}

		public static GameObject NewBackgroundPrefab
		{
			get
			{
				if (_newBackgroundPrefab == null)
				{
					_newBackgroundPrefab = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("AdvancedOptions/QM_Settings_Panel/VerticalLayoutGroup/Background_Info").gameObject;
				}
				return _newBackgroundPrefab;
			}
		}

		public static GameObject MenuCategoryTogglePrefab
		{
			get
			{
				if (_menuCategoryTogglePrefab == null)
				{
					_menuCategoryTogglePrefab = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("AdvancedOptions/QM_Settings_Panel/VerticalLayoutGroup/AllowHorizonAdjust").gameObject;
				}
				return _menuCategoryTogglePrefab;
			}
		}

		public static GameObject SliderTogglePrefab
		{
			get
			{
				if (_sliderTogglePrefab == null)
				{
					_sliderTogglePrefab = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("AvatarCulling/QM_Settings_Panel/VerticalLayoutGroup/HideBeyond").gameObject;
				}
				return _sliderTogglePrefab;
			}
		}
	}
}
