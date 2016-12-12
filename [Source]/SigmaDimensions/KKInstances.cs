using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class KKInstancesLoader : MonoBehaviour
    {
        void Start()
        {
            foreach (ConfigNode Static in GameDatabase.Instance.GetConfigNodes("STATIC"))
            {
                foreach (ConfigNode Instance in Static.GetNodes("Instances"))
                {
                    ConfigNode[] Group = new ConfigNode[] { new ConfigNode() };
                    string name = Instance.GetValue("Group");
                    CelestialBody body = FlightGlobals.Bodies.First(b => b.transform.name == Instance.GetValue("CelestialBody"));
                    if (string.IsNullOrEmpty(name) || body == null) continue;

                    EnumParser<bool> GroupCenter = new EnumParser<bool>();
                    GroupCenter.SetFromString(Instance.GetValue("GroupCenter"));
                    Vector3Parser position = new Vector3Parser();
                    if (Instance.HasValue("RadialPosition"))
                        position.SetFromString(Instance.GetValue("RadialPosition"));
                    else if (Instance.HasValue("RefLatitude") && Instance.HasValue("RefLongitude"))
                    {
                        EnumParser<double> LAT = new EnumParser<double>();
                        EnumParser<double> LON = new EnumParser<double>();
                        LAT.SetFromString(Instance.GetValue("RefLatitude"));
                        LON.SetFromString(Instance.GetValue("RefLongitude"));
                        position = Utility.LLAtoECEF(LAT, LON, 1, 1);
                    }
                    else continue;

                    PQSCity[] PQSCity = body.GetComponentsInChildren<PQSCity>().Where(p => p.repositionRadial.normalized == position.value.normalized).ToArray();

                    if (GroupCenter) { }
                }
            }
        }
    }
}
