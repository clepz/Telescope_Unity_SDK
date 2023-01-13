using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace telescope
{

    internal class Controller : MonoBehaviour
    {
        #region Singleton

        private static Controller _instance;
        protected GameObject _gameObject;
        private static string Url;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeBeforeSceneLoad()
        {
            TelescopeSettings.LoadSettings();
            //if (Config.ManualInitialization) return;
            Initialize();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeAfterSceneLoad()
        {
            //if (Config.ManualInitialization) return;
            //GetEngageDefaultProperties();
            //GetEventsDefaultProperties();
        }

        internal static void Initialize()
        {
            // Copy over any runtime changes that happened before initialization from settings instance to the config.
            TelescopeSettings.Instance.ApplyToConfig();
            Url = Config.Url + "/api/eventrouter?apikey=" + Config.ApiKey;
            GetInstance();
        }

        internal static bool IsInitialized()
        {
            return _instance != null;
        }

        internal static void Disable()
        {
            if (_instance != null)
            {
                Destroy(_instance);
            }
        }

        internal static Controller GetInstance()
        {
            if (_instance == null)
            {
                GameObject g = new GameObject("Telescope");
                _instance = g.AddComponent<Controller>();
                _instance._gameObject = g;
                DontDestroyOnLoad(g);
            }
            return _instance;
        }
        #endregion

        private void Start()
        {
            Telescope.Log("Telescope event tracking started");
            StartSession();
            StartCoroutine(WaitAndFlush());
        }

        private void OnApplicationPause(bool pause)
        {
            Debug.Log("Pause State" + pause.ToString());
        }

        private IEnumerator WaitAndFlush()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(Config.FlushInterval);
                Telescope.Track(TelescopeEvent.GameRunningEvent());
                DoFlush();
            }
        }

        internal void DoFlush()
        {
            //StartCoroutine(Telescope.sendBatchData());
        }

        private void OnDestroy()
        {
            
            EndSession();
            Debug.Log("Telescope Controller Destroying");

        }

        #region Buffer

        // List<TelescopeEvent> | TelescopeEvent
        private static void GenericTrack<T>(T obj)
        {
            string body = JsonConvert.SerializeObject(obj);
            // TelescopeBuffer.BufferPush(id, body)

            //StartCoroutine(PostRequestCoroutine(url, body));
        }

        // List<TelescopeEvent> | TelescopeEvent
        private void SendTrack<T>(T te)
        {
            string body = JsonConvert.SerializeObject(te);
            StartCoroutine(PostRequestCoroutine(body));
        }

        public static void Track(TelescopeEvent te)
        {
            GenericTrack(te);
        }

        public static void Track(List<TelescopeEvent> tes)
        {
            GenericTrack(tes);
        }

        #endregion Buffer


        #region Network

        private IEnumerator PostRequestCoroutine(string body)
        {
            if (!Config.Enabled) yield break;

            byte[] payload = new System.Text.UTF8Encoding().GetBytes(body);

            var req = new UnityWebRequest(Url, "POST");

            req.uploadHandler = new UploadHandlerRaw(payload);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            //Send the request then wait here until it returns
            yield return req.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (req.result != UnityWebRequest.Result.Success)
#else
            if (request.isHttpError || request.isNetworkError)
#endif
            {
                Telescope.LogError("Error While Sending: " + req.error);
            }
            else
            {
                Telescope.Log("Received: " + req.downloadHandler.text);
            }
        }

        #endregion Network



        // Send immidiately
        private static void StartSession()
        {
            Metadata.InitSession();
            GetInstance().SendTrack(new List<TelescopeEvent>() { TelescopeEvent.StartSessionEvent(), TelescopeEvent.ClientDeviceEvent() });
        }

        // Send immidiately
        private static void EndSession()
        {
            GetInstance().SendTrack(TelescopeEvent.EndSessionEvent());
        }


        //    internal static Value GetEngageDefaultProperties()
        //    {
        //        if (_autoEngageProperties == null)
        //        {
        //            Value properties = new Value();
        //#if UNITY_IOS
        //                        properties["$ios_lib_version"] = Mixpanel.MixpanelUnityVersion;
        //                        properties["$ios_version"] = Device.systemVersion;
        //                        properties["$ios_app_release"] = Application.version;
        //                        properties["$ios_device_model"] = SystemInfo.deviceModel;
        //#elif UNITY_ANDROID
        //                        properties["$android_lib_version"] = Mixpanel.MixpanelUnityVersion;
        //                        properties["$android_os"] = "Android";
        //                        properties["$android_os_version"] = SystemInfo.operatingSystem;
        //                        properties["$android_model"] = SystemInfo.deviceModel;
        //                        properties["$android_app_version"] = Application.version;
        //#else
        //            properties["$lib_version"] = Mixpanel.MixpanelUnityVersion;
        //#endif
        //            _autoEngageProperties = properties;
        //        }
        //        return _autoEngageProperties;
        //    }

        //    private static Value GetEventsDefaultProperties()
        //    {
        //        if (_autoTrackProperties == null)
        //        {
        //            Value properties = new Value
        //                {
        //                    {"mp_lib", "unity"},
        //                    {"$lib_version", Mixpanel.MixpanelUnityVersion},
        //                    {"$os", SystemInfo.operatingSystemFamily.ToString()},
        //                    {"$os_version", SystemInfo.operatingSystem},
        //                    {"$model", SystemInfo.deviceModel},
        //                    {"$app_version_string", Application.version},
        //                    {"$wifi", Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork},
        //                    {"$radio", Util.GetRadio()},
        //                    {"$device", Application.platform.ToString()},
        //                    {"$screen_dpi", Screen.dpi},
        //                };
        //#if UNITY_IOS
        //                    properties["$os"] = "Apple";
        //                    properties["$os_version"] = Device.systemVersion;
        //                    properties["$manufacturer"] = "Apple";
        //#endif
        //#if UNITY_ANDROID
        //                    properties["$os"] = "Android";
        //#endif
        //            _autoTrackProperties = properties;
        //        }
        //        return _autoTrackProperties;
        //    }

    }


}