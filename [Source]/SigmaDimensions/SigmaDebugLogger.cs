namespace SigmaDimensionsPlugin
{
    internal static class Debug
    {
        internal static bool debug = false;
        internal static string Tag = "[SigmaLog SD]";

        internal static void Log(string message)
        {
            if (debug)
            {
                UnityEngine.Debug.Log(Tag + ": " + message);
            }
        }

        internal static void Log(string Method, string message)
        {
            if (debug)
            {
                UnityEngine.Debug.Log(Tag + " " + Method + ": " + message);
            }
        }
    }
}
