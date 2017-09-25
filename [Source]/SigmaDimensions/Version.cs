using UnityEngine;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class Version : MonoBehaviour
    {
        public static readonly string number = "v0.9.3";
        void Awake()
        {
            UnityEngine.Debug.Log("[SigmaLog] Version Check:   Sigma Dimensions " + number);
        }
    }
}
