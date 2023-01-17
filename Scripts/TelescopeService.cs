using Assets.TelescopeLabs.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace telescope
{

    internal class TelescopeService : MonoBehaviour
    {
        #region Singleton

        private static TelescopeService _instance;
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
            TelescopeNetwork.Initialize();
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

        internal static TelescopeService GetInstance()
        {
            if (_instance == null)
            {
                GameObject g = new GameObject("Telescope");
                _instance = g.AddComponent<TelescopeService>();
                _instance._gameObject = g;
                DontDestroyOnLoad(g);
            }
            return _instance;
        }
        #endregion


        #region GameLifeCycle
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

        private void OnApplicationFocus(bool focus)
        {
            Debug.Log("Focus State" + focus.ToString());
        }


        private void OnDestroy()
        {

            EndSession();
            Debug.Log("Telescope Controller Destroying");

        }

        #endregion

        private IEnumerator WaitAndFlush()
        {
            bool a = true;
            while (a)
            {
                a = false;
                yield return new WaitForSecondsRealtime(Config.FlushInterval);
                Telescope.Track(TelescopeEvent.GameRunningEvent());
                DoFlush();
            }
        }

        internal void DoFlush()
        {
            TelescopeNetwork.SendTrack(TelescopeBuffer.DequeueBatchTrackingData(1000));
        }


        // Send immidiately
        private static void StartSession()
        {
            Metadata.InitSession();
            TelescopeNetwork.SendTrack(new List<TelescopeEvent>() { TelescopeEvent.StartSessionEvent(), TelescopeEvent.ClientDeviceEvent() });
        }

        // Send immidiately
        private static void EndSession()
        {
            TelescopeNetwork.SendTrack(TelescopeEvent.EndSessionEvent());
        }

    }

}