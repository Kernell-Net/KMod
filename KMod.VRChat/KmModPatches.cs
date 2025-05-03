using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using HarmonyLib;
using MelonLoader;
using KMod.UI.ActionMenu;
using KMod.UI.ActionMenu.API;
using VRC.UI.Elements;

namespace KMod.VRChat
{
    public class KmModPatches
    {
        public delegate void QuickMenuTriggeredHandler(bool triggered);
        public delegate void QuickMenuFirstOpenHandler();

        private static readonly ConcurrentQueue<Action> _pendingEventActions = new ConcurrentQueue<Action>();
        private static readonly object _eventSynchronizationLock = new object();
        public static event QuickMenuTriggeredHandler QuickMenuStateChanged;
        public static event QuickMenuFirstOpenHandler QuickMenuFirstOpened;

        public static bool IsQuickMenuFirstTimeOpened { get; private set; }
        
        private static readonly Task _backgroundEventProcessorTask;
        private static volatile bool _continueProcessingEvents = true;

        static KmModPatches()
        {
            _backgroundEventProcessorTask = Task.Factory.StartNew(ProcessPendingEvents, 
                TaskCreationOptions.LongRunning);
        }

        private static void ProcessPendingEvents()
        {
            while (_continueProcessingEvents)
            {
                while (_pendingEventActions.TryDequeue(out Action eventAction))
                {
                    try
                    {
                        eventAction();
                    }
                    catch (Exception eventException)
                    {
                        MelonLogger.Error($"Event handler error: {eventException.Message}");
                    }
                }
                
                Task.Delay(5).Wait();
            }
        }

        internal static void OnActionMenu(global::ActionMenu actionMenuInstance)
        {
            if (actionMenuInstance == null) return;
            
            Task.Run(() => ActionMenuAPI.OpenMainPage(actionMenuInstance));
        }

        private static void QuickMenuEnabled()
        {
            _pendingEventActions.Enqueue(() => 
            {
                if (!IsQuickMenuFirstTimeOpened)
                {
                    IsQuickMenuFirstTimeOpened = true;
                    QuickMenuFirstOpened?.Invoke();
                }
                
                QuickMenuStateChanged?.Invoke(true);
            });
        }

        private static void QuickMenuDisabled()
        {
            _pendingEventActions.Enqueue(() => QuickMenuStateChanged?.Invoke(false));
        }
        
        public static void Patch()
        {
            try
            {
                var harmonyInstance = new HarmonyLib.Harmony($"KmMod.Patches.{Guid.NewGuid()}");
                
                var quickMenuType = typeof(QuickMenu);
                var actionMenuType = typeof(global::ActionMenu);
                var reModPatchesType = typeof(KmModPatches);
                
                var quickMenuEnableMethod = quickMenuType.GetMethod("OnEnable");
                var quickMenuDisableMethod = quickMenuType.GetMethod("OnDisable");
                var actionMenuPdm0Method = actionMenuType.GetMethod("Method_Public_Void_PDM_0");
                var actionMenuPdm8Method = actionMenuType.GetMethod("Method_Public_Void_PDM_8");
                
                var quickMenuEnabledHandler = reModPatchesType.GetMethod("QuickMenuEnabled", 
                    BindingFlags.Static | BindingFlags.NonPublic);
                var quickMenuDisabledHandler = reModPatchesType.GetMethod("QuickMenuDisabled", 
                    BindingFlags.Static | BindingFlags.NonPublic);
                var actionMenuHandler = reModPatchesType.GetMethod("OnActionMenu", 
                    BindingFlags.Static | BindingFlags.NonPublic);
                
                var parallelPatchTasks = new Task[]
                {
                    Task.Run(() => harmonyInstance.Patch(quickMenuEnableMethod, 
                        new HarmonyMethod(quickMenuEnabledHandler))),
                    
                    Task.Run(() => harmonyInstance.Patch(quickMenuDisableMethod, 
                        new HarmonyMethod(quickMenuDisabledHandler))),
                    
                    Task.Run(() => harmonyInstance.Patch(actionMenuPdm0Method, null, 
                        new HarmonyMethod(actionMenuHandler))),
                    
                    Task.Run(() => harmonyInstance.Patch(actionMenuPdm8Method, null, 
                        new HarmonyMethod(actionMenuHandler)))
                };
                
                Task.WaitAll(parallelPatchTasks);
                
                MelonLogger.Msg("Successfully patched all methods");
            }
            catch (Exception patchingException)
            {
                MelonLogger.Error($"Patching failed: {patchingException.Message}\nStack trace: {patchingException.StackTrace}");
            }
        }
        
        public static void Cleanup()
        {
            _continueProcessingEvents = false;
            Task.WaitAll(new[] { _backgroundEventProcessorTask }, 1000);
        }
    }
}