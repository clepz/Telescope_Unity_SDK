using UnityEngine;

namespace telescope
{

    internal class Controller : MonoBehaviour
    {
        #region Singleton

        private static Controller _instance;
        protected GameObject _gameObject;

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
            GetInstance();
            TelescopeNetwork.Initialize(_instance.gameObject);
            TelescopeService.Initialize(_instance.gameObject);

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
            TelescopeNetwork.Disable();
            TelescopeService.Disable();
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