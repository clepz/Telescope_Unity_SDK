namespace telescope
{
    static class DebugDevice
    {
        internal static bool IsDebugDevice()
        {
#if UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }
}
