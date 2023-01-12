
using System.Collections.Generic;

namespace telescope
{
    public class TelescopeEvent
    {
        public string entityName;
        public string type;
        public string id;
        public Dictionary<string, object> value;

        public TelescopeEvent()
        {
        }

        public TelescopeEvent(string entityName, string type, string id, Dictionary<string, object> value)
        {
            this.entityName = entityName;
            this.type = type;
            this.id = id;
            this.value = value;
        }
    }

}
