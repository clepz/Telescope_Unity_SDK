using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

namespace telescope
{

    internal class TelescopeNetwork : MonoBehaviour
    {
        #region Singleton

        private static TelescopeNetwork _instance;

        internal static void Initialize(GameObject go)
        {
            _instance = go.AddComponent<TelescopeNetwork>();
        }

        #endregion

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

        // List<TelescopeEvent> | TelescopeEvent
        private void GenericSend<T>(T obj)
        {
            string url = Config.Url + "/api/eventrouter?apikey=" + Config.ApiKey;

            string body = JsonConvert.SerializeObject(obj);

            StartCoroutine(PostRequestCoroutine(url, body));
        }

        public void Send(TelescopeEvent te)
        {
            GenericSend(te);
        }

        public void Send(List<TelescopeEvent> tes)
        {
           GenericSend(tes);
        }

        private IEnumerator PostRequestCoroutine(string url, string body)
        {
            if (!Config.Enabled) yield break;

            byte[] payload = new System.Text.UTF8Encoding().GetBytes(body);

            var req = new UnityWebRequest(url, "POST");

            req.uploadHandler = new UploadHandlerRaw(payload);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            //Send the request then wait here until it returns
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.ConnectionError)
            {
                Telescope.LogError("Error While Sending: " + req.error);
            }
            else
            {
                Telescope.Log("Received: " + req.downloadHandler.text);
            }

        }

        internal static TelescopeNetwork GetInstance()
        {
            return _instance;
        }
    }
}