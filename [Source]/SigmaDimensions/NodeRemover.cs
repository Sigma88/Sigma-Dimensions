using System.IO;
using System.Linq;
using System.Reflection;
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
            string path = Assembly.GetExecutingAssembly().Location;


            if (path != folder + "Plugin/" + Path.GetFileName(path))
            {
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Incorrect plugin location => " + path);
            }

            if (!Directory.Exists(folder))
            {
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Missing folder => " + folder);
                return;
            }

            if (!File.Exists(folder + file + ".cfg"))
            {
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Missing file => " + folder + file + ".cfg");

                File.WriteAllLines(folder + file + ".cfg", new[] { node + " {}" });
                return;
            }

            if (ConfigNode.Load(folder + file + ".cfg")?.HasNode("SigmaDimensions") != true)
            {
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Missing node => " + folder + file + "/" + node);

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
