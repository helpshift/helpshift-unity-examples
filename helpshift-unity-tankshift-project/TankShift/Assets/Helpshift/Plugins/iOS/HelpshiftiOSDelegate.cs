#if UNITY_IOS
using System;
using UnityEngine;

namespace Helpshift
{
    public class HelpshiftiOSDelegate
    {
        private HelpshiftiOSDelegate() { }

        internal static void RegisterUnitySupportMessageCallback()
        {
            hsRegisterUnitySupportMessageCallback(UnitySupportMessageCallbackImpl);
        }

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void hsRegisterUnitySupportMessageCallback(UnitySupportMessageCallback callback);

        private delegate void UnitySupportMessageCallback(string messageHandler, string methodName, string parameter);

        [MonoPInvokeCallback(typeof(UnitySupportMessageCallback))]
        private static void UnitySupportMessageCallbackImpl(string messageHandler, string methodName, string parameter)
        {
            GameObject externalDelegate = GameObject.Find(messageHandler);

            if (externalDelegate != null)
            {
                if (parameter == null)
                {
                    externalDelegate.SendMessage(methodName);
                }
                else
                {
                    externalDelegate.SendMessage(methodName, parameter);
                }
            }
        }
    }
}
#endif
