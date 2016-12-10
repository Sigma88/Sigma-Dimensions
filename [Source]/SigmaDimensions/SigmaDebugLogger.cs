namespace SigmaDimensionsPlugin
{
    public static class Debug
    {
        public static bool debug = false;

        public static void Log(string s)
        {
            if (debug) UnityEngine.Debug.Log("SigmaLog: " + s);
        }
    }
}
