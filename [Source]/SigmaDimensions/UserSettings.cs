using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    internal class UserSettings : MonoBehaviour
    {
        internal static ConfigNode ConfigNode
        {
            get
            {
                return GameDatabase.Instance?.GetConfigs(nodeName)?.FirstOrDefault(n => n.url == (folder.Substring(9) + file + "/" + nodeName))?.config;
            }
        }

        static string folder = "GameData/Sigma/Dimensions/";
        static string file = "Settings";
        internal static string nodeName = "SigmaDimensions";

        void Awake()
        {
            string path = Assembly.GetExecutingAssembly().Location.Replace('\\', '/');
            while (path.Substring(1).Contains("GameData/"))
                path = path.Substring(1 + path.Substring(1).IndexOf("GameData/"));
            if (path != folder + "Plugins/" + Path.GetFileName(path))
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Incorrect plugin location => " + path);

            if (!Directory.Exists(folder))
            {
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Missing folder => " + folder);
                return;
            }

            if (!File.Exists(folder + file + ".cfg"))
            {
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Missing file => " + folder + file + ".cfg");

                File.WriteAllLines(folder + file + ".cfg", new[] { nodeName + " {}" });
                return;
            }

            if (ConfigNode.Load(folder + file + ".cfg")?.HasNode(nodeName) != true)
            {
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Missing node => " + folder + file + "/" + nodeName);

                File.AppendAllText(folder + file + ".cfg", "\r\n" + nodeName + " {}");
            }
        }

        void Start()
        {
            var configs = GameDatabase.Instance.GetConfigs(nodeName);

            for (int i = 0; i < configs?.Length; i++)
            {
                if (configs[i].url != (folder.Substring(9) + file + "/" + nodeName))
                    configs[i].parent.configs.Remove(configs[i]);
            }
        }
    }
}
