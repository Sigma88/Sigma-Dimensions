namespace SigmaDimensionsPlugin
{
    internal static class Debug
    {
        internal static bool debug = false;

        internal static void Log(string s)
        {
            if (debug) UnityEngine.Debug.Log("SigmaLog: " + s);
        }
    }
}
