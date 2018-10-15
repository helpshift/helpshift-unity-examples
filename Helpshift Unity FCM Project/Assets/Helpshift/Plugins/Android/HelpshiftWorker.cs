
#if UNITY_ANDROID
using System;
using System.Linq;
using System.Collections;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;

namespace Helpshift
{
	public interface IWorkerMethodDispatcher
	{
		// Callback with api info to be called.
		void resolveAndCallApi (string methodIdentifier, string api, object[] args);

	}

	// Holds meta data of methods to be called in worker thread.
	public class APICallInfo
	{
		// Class instance on which api is to be called.
		public String instanceIdentifier;

		// Method type identifier.
		public String methodIdentifier;

		// Api to be called.
		public String apiName;

		// Arguments for the api call.
		public object[] args;

		// Reset event for synchronous api call.
		public ManualResetEvent resetEvent;

		public APICallInfo (String instanceIdentifier, String methodIdentifier, String apiName, object[] args)
		{
			this.instanceIdentifier = instanceIdentifier;
			this.methodIdentifier = methodIdentifier;
			this.apiName = apiName;
			this.args = args;

		}

		public APICallInfo (ManualResetEvent resetEvent)
		{
			this.resetEvent = resetEvent;
		}
	}

	public class HelpshiftWorker
	{
		private static HelpshiftWorker hsWorker;
		private Queue<APICallInfo> callerQueue;
		private ManualResetEvent waitHandle = new ManualResetEvent (false);
		private Dictionary<String, IWorkerMethodDispatcher> listeners = new Dictionary<String, IWorkerMethodDispatcher> ();
		private Thread workerThread;
		private bool shouldStop = false;

		private HelpshiftWorker ()
		{
			callerQueue = new Queue<APICallInfo> ();
			// Worker thread to read and execute from queue.
			workerThread = new Thread (() => {

				try {
					// Attach current thread to android JNI.
					AndroidJNI.AttachCurrentThread ();
					while (!shouldStop) {
						// Execute from queue.
						while (!shouldStop && callerQueue.Count > 0) {
							APICallInfo apiInfo = callerQueue.Dequeue ();
							try {
								resolveHsApiCall (apiInfo);
							} catch (Exception e) {
								// Catch all exceptions since we want the thread to be running.
								Debug.Log ("Error in : " + apiInfo.apiName + ". Exception : " + e.Message + e.StackTrace);
							}
						}

						if(!shouldStop) {
							waitHandle.WaitOne ();
							waitHandle.Reset ();
						}
					}
				} finally {
					AndroidJNI.DetachCurrentThread ();
				}
			});
			workerThread.Name = "HSAsyncThread";
			workerThread.Start ();
		}

		public static HelpshiftWorker getInstance ()
		{
			if (hsWorker == null) {
				hsWorker = new HelpshiftWorker ();
			}

			return hsWorker;
		}

		public void registerClient (String identifier, IWorkerMethodDispatcher instance)
		{
			if (listeners.ContainsKey (identifier)) {
				listeners [identifier] = instance;
			} else {
				listeners.Add (identifier, instance);
			}
		}

		public void enqueueApiCall (string instanceIdentifier, string methodIdentifier, string api, object[] args)
		{
			callerQueue.Enqueue (new APICallInfo (instanceIdentifier, methodIdentifier, api, args));
			// Signal worker thread to resume.
			waitHandle.Set ();
		}

		public void synchronousWaitForApiCallQueue ()
		{
			ManualResetEvent resetEvent = new ManualResetEvent (false);
			callerQueue.Enqueue (new APICallInfo (resetEvent));
			// Signal worker thread to resume.
			waitHandle.Set ();

			// Wait for Set() from worker thread.
			resetEvent.WaitOne ();
		}

		public void resolveHsApiCall (APICallInfo apiInfo)
		{

			if (apiInfo.resetEvent != null) {
				apiInfo.resetEvent.Set ();
				return;
			}
			listeners [apiInfo.instanceIdentifier].resolveAndCallApi (apiInfo.methodIdentifier, 
				                                                             apiInfo.apiName, apiInfo.args);

		}

		public void onApplicationQuit()
		{
			shouldStop = true;

			// Force it not to wait
			waitHandle.Set();

			// Force the main thread to clean up the worker thread before terminating
			workerThread.Join();
		}
	}
}
#endif
