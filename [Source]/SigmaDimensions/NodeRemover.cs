using System.IO;
using System.Linq;
using UnityEngine;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class ConfigNodeRemover : MonoBehaviour
    {
        void Awake()
        {
            string folder = "GameData/Sigma/Dimensions/";
            string file = "Settings";
            string node = "SigmaDimensions";


            if (!Directory.Exists(folder))
            {
                UnityEngine.Debug.Log("[SigmaLog] WARNING: Missing folder => " + folder);
                return;
            }

            if (!File.Exists(folder + file + ".cfg"))
            {
                UnityEngine.Debug.Log("[SigmaLog] WARNING: Missing file => " + folder + file + ".cfg");

                File.WriteAllLines(folder + file + ".cfg", new[] { node + " {}" });
                return;
            }

            if (ConfigNode.Load(folder + file + ".cfg")?.HasNode("SigmaDimensions") != true)
            {
                UnityEngine.Debug.Log("[SigmaLog] WARNING: Missing node => " + folder + file + "/" + node);

                File.AppendAllText(folder + file + ".cfg", "\r\n" + node + " {}");
            }
        }

        void Start()
        {
            foreach (UrlDir.UrlConfig node in GameDatabase.Instance.GetConfigs("SigmaDimensions").Where(c => c.url != "Sigma/Dimensions/Settings/SigmaDimensions"))
            {
                node.parent.configs.Remove(node);
            }
        }
    }
}
