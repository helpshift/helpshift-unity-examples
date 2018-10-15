#if UNITY_ANDROID
using UnityEngine;
using System.Collections;

namespace Helpshift
{

    /**
	 * Internal Logger for android console logging.
	 */
    public class HelpshiftInternalLogger : IWorkerMethodDispatcher, IDexLoaderListener
    {

        private static string TAG = "HelpshiftUnityPlugin";

        private static HelpshiftInternalLogger internalLoggerInstance;
        private AndroidJavaObject hsInternalLogger;

        /*
		 * Get instance for logger.
		 */
        public static HelpshiftInternalLogger getInstance()
        {
            if (internalLoggerInstance == null)
            {
                internalLoggerInstance = new HelpshiftInternalLogger();
            }
            return internalLoggerInstance;
        }

        private HelpshiftInternalLogger()
        {
            // Register for async worker.
            HelpshiftWorker.getInstance().registerClient("HelpshiftInternalLogger", this);

            // Register for dex load listener.
            HelpshiftDexLoader.getInstance().registerListener(this);
        }

        private void addApiCallToQueue(string apiName, object[] args)
        {
            // Add all logger apis to the worker queue.
            HelpshiftWorker.getInstance().enqueueApiCall("HelpshiftInternalLogger", "hsLoggerWithArgs", apiName, args);
        }

        public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
        {
            hsInternalLogger.CallStatic(api, args);
        }
		
        public void onDexLoaded()
        {
            // Load instance of Logger from java.
            hsInternalLogger = HelpshiftDexLoader.getInstance().getHSDexLoaderJavaClass().CallStatic<AndroidJavaObject>("getHSLoggerInstance");
        }

        /*
		 * Debug level logging
		 */ 
        public void d(string message)
        {
            addApiCallToQueue("d", new object[] {TAG, message});
        }

        /*
         * Error level logging
        */
        public void e(string message)
        {
            addApiCallToQueue("e", new object[] {TAG, message});
        }

        /*
		 * Warn level logging
		 */
        public void w(string message)
        {
            addApiCallToQueue("w", new object[] {TAG, message});
        }

        /*
		 * Fatal level logging
		 */
        public void f(string message)
        {
            addApiCallToQueue("f", new object[] {TAG, message});
        }
    }
}
#endif
