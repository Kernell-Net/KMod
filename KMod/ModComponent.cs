using System.Reflection;
using HarmonyLib;
using MelonLoader;
using KMod.Managers;
using VRC;
using VRC.Core;
using VRC.SDKBase;

namespace KMod
{
	public class ModComponent
	{
		public bool Enabled { get; set; } = true;

		public virtual void OnUiManagerInitEarly()
		{
		}

		public virtual void OnUiManagerInit(UiManager uiManager)
		{
		}

		public virtual void OnFixedUpdate()
		{
		}

		public virtual void OnUpdate()
		{
		}

		public virtual void OnLateUpdate()
		{
		}

		public virtual void OnGUI()
		{
		}

		public virtual void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
		}

		public virtual void OnSceneWasInitialized(int buildIndex, string sceneName)
		{
		}

		public virtual void OnApplicationQuit()
		{
		}

		public virtual void OnPreferencesSaved()
		{
		}

		public virtual void OnPreferencesLoaded()
		{
		}

		public virtual void OnPlayerJoined(VRCPlayer player)
		{
		}

		public virtual void OnPlayerLeft(VRCPlayer player)
		{
		}

		public virtual void OnAvatarIsReady(VRCPlayer player)
		{
		}

		public virtual void OnAvatarChanged(APIUser apiUser, ApiAvatar apiAvatar)
		{
		}

		public virtual void OnEnterWorld(ApiWorld world, ApiWorldInstance instance)
		{
		}

		public virtual void OnSelectUser(InterfacePublicAbstractBoSt1StLoCoStLi1ObUnique user, bool isRemote)
		{
		}

		public virtual void OnSetupUserInfo(APIUser apiUser)
		{
		}

		public virtual bool ExecuteEvent(VRCPlayer player, VRC_EventHandler.VrcEvent evt, VRC_EventHandler.VrcBroadcastType broadcastType, int instagatorId, float fastForward)
		{
			return false;
		}

		public virtual bool OnDownloadAvatar(ApiAvatar apiAvatar)
		{
			return false;
		}

		public virtual void OnRenderObject()
		{
		}

		public virtual void OnJoinedRoom()
		{
		}

		public virtual void OnLeftRoom()
		{
		}

		public virtual void OnModulesLoaded()
		{
		}

		protected HarmonyMethod GetLocalPatch(string methodName)
		{
			return MelonUtils.ToNewHarmonyMethod(GetType().GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic));
		}
	}
}
