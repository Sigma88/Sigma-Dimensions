using System.Linq;
using UnityEngine;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class ConfigNodeRemover : MonoBehaviour
    {
        void Start()
        {
            foreach (UrlDir.UrlConfig node in GameDatabase.Instance.GetConfigs("SigmaDimensions").Where(c => c.url != "Sigma/Dimensions/Settings/SigmaDimensions"))
            {
                node.parent.configs.Remove(node);
            }
        }
    }
}
