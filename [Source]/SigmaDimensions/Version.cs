using UnityEngine;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class Version : MonoBehaviour
    {
        void Awake()
        {
            UnityEngine.Debug.Log("Sigma Version Check:   Sigma Dimensions v0.9.1");
        }
    }
}
