using System.Collections;
using UnityEngine;

namespace telescope
{
    internal abstract class ITelescopeEvent : ScriptableObject
    {

        internal abstract TelescopeEvent GetTelescopeEvent();
        public static implicit operator TelescopeEvent(ITelescopeEvent te) => te.GetTelescopeEvent();
    }
}