using System.Collections.Generic;
using System.Linq;

//TODO: common parametreler icin bir yapi dusun.

namespace telescope
{
    public static partial class Telescope
    {
        internal const string TelescopeUnitySDKVersion = "1";

        public static void Track(TelescopeEvent te)
        {
            Controller.Track(te);
        }
        public static void Track(List<TelescopeEvent> tes)
        {
            Controller.Track(tes);
        }

        public static void Track(string entityName, Dictionary<string, object> value)
        {
            Controller.Track(new TelescopeEvent(entityName, "insert", value));
        }

        // You must call the function in awake to use your userId.
        public static void SetCustomUserID(string userId)
        {
            return;
        }

        #region TelescopeRelatedUtils
        internal static Dictionary<string, object> MergeValues(Dictionary<string, object> val1, Dictionary<string, object> val2)
        {
            return val1.Concat(val2).ToDictionary(e => e.Key, e => e.Value);
        }

        internal static Dictionary<string, object> MergeValues(Dictionary<string, object> val1, Dictionary<string, object> val2, Dictionary<string, object> val3)
        {
            return val1.Concat(val2).Concat(val3).ToDictionary(e => e.Key, e => e.Value);
        }

        internal static Dictionary<string, object> MergeValues(Dictionary<string, object> val1, Dictionary<string, object> val2, Dictionary<string, object> val3, Dictionary<string, object> val4)
        {
            return val1.Concat(val2).Concat(val3).Concat(val4).ToDictionary(e => e.Key, e => e.Value);
        }
        #endregion

    }

}