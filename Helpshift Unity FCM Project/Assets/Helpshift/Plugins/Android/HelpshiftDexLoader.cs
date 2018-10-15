#if UNITY_ANDROID
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Helpshift
{
    // Listener for dex loader callbacks
    public interface IDexLoaderListener
    {
        // Callback for dex loaded
        void onDexLoaded();
    }

    public class HelpshiftDexLoader : IWorkerMethodDispatcher
    {
        private static HelpshiftDexLoader dexLoader;
        private static Boolean isDexLoaded;
        private HashSet<IDexLoaderListener> listeners = new HashSet<IDexLoaderListener>();
        private AndroidJavaObject application;
        private AndroidJavaClass helpshiftLoaderClass;
        private AndroidJavaClass unityApiDelegateClass;

        private HelpshiftDexLoader()
        {
        }

        public static HelpshiftDexLoader getInstance()
        {
            if (dexLoader == null)
            {
                dexLoader = new HelpshiftDexLoader();
            }
            return dexLoader;
        }

        public void loadDex(IDexLoaderListener listener, AndroidJavaObject application)
        {
            this.application = application;
            helpshiftLoaderClass = new AndroidJavaClass("com.helpshift.dex.HelpshiftDexLoader");
            unityApiDelegateClass = new AndroidJavaClass("com.helpshift.supportCampaigns.UnityAPIDelegate");
            registerListener(listener);
            HelpshiftWorker.getInstance().registerClient("dexLoader", this);
            HelpshiftWorker.getInstance().enqueueApiCall("dexLoader", "loadHelpshiftDex", null, new object[]{ helpshiftLoaderClass });
            return;
        }

        public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
        {
            if (methodIdentifier.Equals("loadHelpshiftDex"))
            {
                loadHelpshiftDex((AndroidJavaClass)args [0]);
            }
        }

        private void loadHelpshiftDex(AndroidJavaClass helpshiftLoaderClass)
        {
            unityApiDelegateClass.CallStatic("installDex", application);
            isDexLoaded = true;

            foreach (IDexLoaderListener listener in listeners)
            {
                listener.onDexLoaded();
            }
        }

        public void registerListener(IDexLoaderListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }

            if (isDexLoaded)
            {
                listener.onDexLoaded();
            }
        }

        public AndroidJavaClass getHSDexLoaderJavaClass()
        {
            return helpshiftLoaderClass;
        }
    }
}
#endif
