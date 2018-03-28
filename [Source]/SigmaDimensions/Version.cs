using UnityEngine;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class Version : MonoBehaviour
    {
        public static readonly System.Version number = new System.Version("0.9.8");

        void Awake()
        {
            UnityEngine.Debug.Log("[SigmaLog] Version Check:   Sigma Dimensions v" + number);
        }
    }
}
