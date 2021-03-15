#if UNITY_ANDROID
using UnityEngine;
using System.Collections;

namespace Helpshift
{

    /**
	 * Internal Logger for android console logging.
	 */
    public class HelpshiftInternalLogger
    {

        private static string TAG = "HelpshiftUnityPlugin";
		private static AndroidJavaClass hsInternalLogger = new AndroidJavaClass("com.helpshift.util.HSLogger");

        /*
		 * Debug level logging
		 */ 
        public static void d(string message)
        {
			hsInternalLogger.CallStatic("d", new object[] {TAG, message});
        }

        /*
         * Error level logging
        */
		public static void e(string message)
        {
			hsInternalLogger.CallStatic("e", new object[] {TAG, message});
        }

        /*
		 * Warn level logging
		 */
		public static void w(string message)
        {
			hsInternalLogger.CallStatic("w", new object[] {TAG, message});
        }

        /*
		 * Fatal level logging
		 */
		public static void f(string message)
        {
			hsInternalLogger.CallStatic("f", new object[] {TAG, message});
        }
    }
}
#endif
