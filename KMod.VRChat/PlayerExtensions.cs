using System;
using System.Linq;
using System.Reflection;
using MelonLoader;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI.Elements;
using VRC.UI.Elements.Menus;

namespace KMod.VRChat
{
	public static class PlayerExtensions
	{
		private static MethodInfo LoadAvatarMethod
		{
			get
			{
				bool flag = PlayerExtensions._reloadAvatarMethod == null;
				if (flag)
				{
					PlayerExtensions._reloadAvatarMethod = typeof(VRCPlayer).GetMethods().First(delegate(MethodInfo mi)
					{
						if (mi.Name.StartsWith("Method_Private_Void_Boolean_") && mi.Name.Length < 31)
						{
							if (mi.GetParameters().Any((ParameterInfo pi) => pi.IsOptional))
							{
								return XrefUtils.CheckUsedBy(mi, "KmloadAvatarNetworkedRPC", null);
							}
						}
						return false;
					});
				}
				return PlayerExtensions._reloadAvatarMethod;
			}
		}
		
		private static MethodInfo KmloadAllAvatarsMethod
		{
			get
			{
				bool flag = PlayerExtensions._reloadAllAvatarsMethod == null;
				if (flag)
				{
					PlayerExtensions._reloadAllAvatarsMethod = typeof(VRCPlayer).GetMethods().First(delegate(MethodInfo mi)
					{
						if (mi.Name.StartsWith("Method_Public_Void_Boolean_") && mi.Name.Length < 30)
						{
							if (mi.GetParameters().All((ParameterInfo pi) => pi.IsOptional))
							{
								return XrefUtils.CheckUsedBy(mi, "Method_Public_Void_", typeof(FeaturePermissionManager));
							}
						}
						return false;
					});
				}
				return PlayerExtensions._reloadAllAvatarsMethod;
			}
		}
		
		public static Player GetPlayer(string UserID)
		{
			
			foreach (Player player in ObjectPublicIDisposableStObStLi1PlDiPl2InUnique.Method_Public_Static_List_1_Player_PDM_0()._items.ToArray().ToList<Player>())
			{
				bool flag = player.field_Private_APIUser_0.id == UserID;
				if (flag)
				{
					return player;
				}
			}
			return null;
		}

		public static SelectedUserMenuQM GetTarget()
		{
			QuickMenu quickMenu = Resources.FindObjectsOfTypeAll<QuickMenu>().FirstOrDefault<QuickMenu>();
			bool flag = quickMenu != null;
			SelectedUserMenuQM result;
			if (flag)
			{
				result = quickMenu.field_Private_UIPage_1.GetComponent<SelectedUserMenuQM>();
			}
			else
			{
				result = null;
			}
			return result;
		}
		
		public static Player GetPlayer(InterfacePublicAbstractBoSt1StLoCoStLi1ObUnique value)
		{
		
			return PlayerExtensions.GetPlayer(value.prop_String_0);
		}
		
		public static VRCPlayer GetVRCPlayer(InterfacePublicAbstractBoSt1StLoCoStLi1ObUnique value)
		{
			return PlayerExtensions.GetPlayer(value)._vrcplayer;
		}
		
		public static APIUser GetAPIUser(InterfacePublicAbstractBoSt1StLoCoStLi1ObUnique value)
		{
			return PlayerExtensions.GetPlayer(value).prop_APIUser_0;
		}
		
		public static ApiAvatar GetApiAvatar(InterfacePublicAbstractBoSt1StLoCoStLi1ObUnique value)
		{
			return PlayerExtensions.GetPlayer(value).prop_ApiAvatar_0;
		}
		
		public static InterfacePublicAbstractBoSt1StLoCoStLi1ObUnique SelectedIUser()
		{
			return PlayerExtensions.GetTarget().field_Private_InterfacePublicAbstractBoSt1StLoCoStLi1ObUnique_0;
		}
		
		public static VRCPlayer GetVRCPlayer()
		{
			return PlayerExtensions.GetPlayer(PlayerExtensions.GetTarget().field_Private_InterfacePublicAbstractBoSt1StLoCoStLi1ObUnique_0)._vrcplayer;
		}
		
		public static APIUser GetAPIUser()
		{
			return PlayerExtensions.GetPlayer(PlayerExtensions.GetTarget().field_Private_InterfacePublicAbstractBoSt1StLoCoStLi1ObUnique_0).field_Private_APIUser_0;
		}
		
		public static ApiAvatar GetApiAvatar()
		{
			return PlayerExtensions.GetPlayer(PlayerExtensions.GetTarget().field_Private_InterfacePublicAbstractBoSt1StLoCoStLi1ObUnique_0).prop_ApiAvatar_0;
		}
		
		public static Player[] GetPlayers(ObjectPublicIDisposableStObStLi1PlDiPl2InUnique playerManager)
		{
			return playerManager.field_Private_List_1_Player_0.ToArray();
		}
		

		
		public static GameObject GetAvatarObject(VRCPlayer vrcPlayer)
		{
			return vrcPlayer.field_Internal_GameObject_0;
		}
		
	
		
		public static bool IsStaff(APIUser user)
		{
			bool hasModerationPowers = user.hasModerationPowers;
			bool result;
			if (hasModerationPowers)
			{
				result = true;
			}
			else
			{
				bool flag = user.developerType > APIUser.DeveloperType.None;
				result = (flag || user.tags.Contains("admin_moderator") || user.tags.Contains("admin_scripting_access") || user.tags.Contains("admin_official_thumbnail"));
			}
			return result;
		}
		
		public static void KmloadAvatar(VRCPlayer instance)
		{
			PlayerExtensions.LoadAvatarMethod.Invoke(instance, new object[]
			{
				true
			});
		}
		
		public static void KmloadAllAvatars(VRCPlayer instance, bool ignoreSelf = false)
		{
			PlayerExtensions.KmloadAllAvatarsMethod.Invoke(instance, new object[]
			{
				ignoreSelf
			});
		}
		
		private static MethodInfo _reloadAvatarMethod;

		private static MethodInfo _reloadAllAvatarsMethod;
	}
}
