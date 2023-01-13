
using System;
using System.Collections.Generic;

namespace telescope
{
    public class TelescopeEvent
    {
        public string entityName;
        public string type;
        public string id { get => TelescopeBuffer.DistinctId; set => id = value; }
        public Dictionary<string, object> value;

        public TelescopeEvent()
        {
        }

        public TelescopeEvent(string entityName, string type, Dictionary<string, object> value)
        {
            this.entityName = entityName;
            this.type = type;
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
                type = "insert",
                value = Metadata.GetEventMetadata()
            };
        }

        internal static TelescopeEvent GameRunningEvent()
        {
            return new TelescopeEvent()
            {
                entityName = "game_running",
                type = "insert",
                value = Metadata.GetEventMetadata()
            };
        }

        internal static TelescopeEvent ClientDeviceEvent()
        {
            return new TelescopeEvent()
            {
                entityName = "client_device",
                type = "insert",
                value = Telescope.MergeValues(Metadata.GetEventMetadata(), Metadata.GetClientDeviceMetaData())
            };
        }

        internal static TelescopeEvent EndSessionEvent()
        {
            return new TelescopeEvent()
            {
                entityName = "session_end",
                type = "insert",
                value = Telescope.MergeValues(Metadata.GetEventMetadata(), Metadata.GetEndSessionMetadata())
            };
        }

        #endregion
    }

}
