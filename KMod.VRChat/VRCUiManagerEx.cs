using System;
using System.Linq;
using System.Reflection;

namespace KMod.VRChat
{
    public static class VRCUiManagerEx
    {
        private static readonly Func<VRCUiManager> _getInstance;

        static VRCUiManagerEx()
        {
            var method = typeof(VRCUiManager)
                .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .First(m => m.ReturnType == typeof(VRCUiManager)
                            && m.GetParameters().Length == 0);

            _getInstance = (Func<VRCUiManager>)Delegate.CreateDelegate(
                typeof(Func<VRCUiManager>),
                method
            );
        }

        public static VRCUiManager Instance => _getInstance();

        public static bool IsOpen => Instance.field_Private_Boolean_0;
    }
}