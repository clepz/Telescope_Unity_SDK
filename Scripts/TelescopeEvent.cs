
using Newtonsoft.Json;
using System.Collections.Generic;

namespace telescope
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TelescopeEvent
    {
        public string entityName;
        public string type = "insert";
        public string id {
            get => TelescopeBuffer.DistinctId;
            internal set { id = value; }
        }
        public Dictionary<string, object> value;

        public TelescopeEvent()
        {
        }

        public TelescopeEvent(string entityName, Dictionary<string, object> value)
        {
            this.entityName = entityName;
            this.value = value;
        }

        internal object this[string key]
        {
            get { return value[key]; }
            set { this.value[key] = value; }
        }

        #region StandartEvents

        internal static TelescopeEvent StartSessionEvent()
        {
            return new TelescopeEvent()
            {
                entityName = "session_start",
                value = Metadata.GetEventMetadata()
            };
        }

        internal static TelescopeEvent GameRunningEvent()
        {
            return new TelescopeEvent()
            {
                entityName = "game_running",
                value = Metadata.GetEventMetadata()
            };
        }

        internal static TelescopeEvent ClientDeviceEvent()
        {
            return new TelescopeEvent()
            {
                entityName = "client_device",
                value = Telescope.MergeValues(Metadata.GetEventMetadata(), Metadata.GetClientDeviceMetaData())
            };
        }

        internal static TelescopeEvent EndSessionEvent()
        {
            return new TelescopeEvent()
            {
                entityName = "session_end",
                value = Telescope.MergeValues(Metadata.GetEventMetadata(), Metadata.GetEndSessionMetadata())
            };
        }

        #endregion
    }

}
