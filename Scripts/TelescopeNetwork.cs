using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using telescope;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.TelescopeLabs.Scripts
{
    public class TelescopeNetwork : ScriptableObject
    {
        private static string Url;

        internal static void Initialize()
        {
            Url = "https://api.telescopelabs.io/api/eventrouter?apikey=" + Config.ApiKey;
        }

        // List<TelescopeEvent> | TelescopeEvent
        internal static void SendTrack<T>(T te)
        {
            string body = JsonConvert.SerializeObject(te);
            _ = PostRequestTask(body);
        }

        public static void Track(TelescopeEvent te)
        {
            Debug.Log(te.GetType());
            TelescopeBuffer.EnqueueTrackingData(te);
        }

        public static void Track(List<TelescopeEvent> tes)
        {
            TelescopeBuffer.EnqueueTrackingData(tes);
        }

        private static async Task PostRequestTask(string body)
        {
            if (!Config.Enabled) return;

            byte[] payload = new System.Text.UTF8Encoding().GetBytes(body);

            var req = new UnityWebRequest(Url, "POST");

            req.uploadHandler = new UploadHandlerRaw(payload);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            //Send the request then wait here until it returns
            req.SendWebRequest();
            while (!req.isDone)
            {
                await Task.Yield();
            }

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
                Telescope.Log("\nReceived: " + req.downloadHandler.text);
            }
            req.Dispose();
        }

    }
}