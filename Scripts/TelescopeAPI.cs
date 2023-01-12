using System.Collections.Generic;

//TODO: common parametreler icin bir yapi dusun.

namespace telescope
{
    public static partial class Telescope
    {
        internal const string TelescopeUnitySDKVersion = "1";
        private static readonly TelescopeNetwork telescopeNetwork = TelescopeNetwork.GetInstance();

        public static void Send(TelescopeEvent te)
        {
            telescopeNetwork.Send(te);


            //m_CommonParams.ClientVersion = Application.version;
            //m_CommonParams.ProjectID = Application.cloudProjectId;
            //m_CommonParams.GameBundleID = Application.identifier;
            //m_CommonParams.Platform = Runtime.Name();
            //m_CommonParams.BuildGuuid = Application.buildGUID;
            //m_CommonParams.Idfv = deviceIdentifiersInternal.Idfv;
            //SessionID = Guid.NewGuid().ToString();
        }
        public static void Send(List<TelescopeEvent> tes)
        {
            telescopeNetwork.Send(tes);
        }

        public static void Send(string entityName, string userId, Dictionary<string, object> value)
        {
            telescopeNetwork.Send(new TelescopeEvent(entityName, "insert", userId, value));
        }

        // You must call the function in awake to use your userId.
        public static void SetCustomUserID(string userId)
        {
            return;
        }
    }

}