using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class PQSCityGroupsLoader : MonoBehaviour
    {
        public static Dictionary<string, ConfigNode> GroupList = new Dictionary<string, ConfigNode>();

        void Start()
        {
            foreach (ConfigNode GroupLoader in GameDatabase.Instance.GetConfigNodes("PQSCity_Groups"))
            {
                PQSCityGroups.AddGroups(GroupLoader.GetNodes("Group"));
            }
            PQSCityGroups.SaveGroups();
        }
    }

    public static class PQSCityGroups
    {
        public static void AddGroups(ConfigNode[] Groups)
        {
            foreach (ConfigNode Group in Groups)
            {
                string name = Group.GetValue("name");
                if (string.IsNullOrEmpty(name)) continue;
                if (PQSCityGroupsLoader.GroupList.ContainsKey(name))
                    PQSCityGroupsLoader.GroupList[name].AddData(Group);
                else
                    PQSCityGroupsLoader.GroupList.Add(name, Group);
            }
        }
        
        public static void SaveGroups()
        {
            foreach (ConfigNode Group in PQSCityGroupsLoader.GroupList.Values)
            {
                string name = Group.GetValue("name");
                CelestialBody body = FlightGlobals.Bodies.First(b => b.transform.name == Group.GetValue("body"));
                if (string.IsNullOrEmpty(name) || body == null) continue;

                Vector3Parser center = new Vector3Parser();
                if (Group.HasValue("CentralPQSCity"))
                    center = body.GetComponentsInChildren<PQSCity>(true).FirstOrDefault(p => p.name == Group.GetValue("CentralPQSCity")).repositionRadial;
                else if (Group.HasValue("CentralPQSCity2"))
                {
                    PQSCity2 city2 = body.GetComponentsInChildren<PQSCity2>(true).First(p => p.name == Group.GetValue("CentralPQSCity2"));
                    center = Utility.LLAtoECEF(city2.lat, city2.lon, 1, 1);
                }
                else if (Group.HasValue("CentralPosition"))
                    center.SetFromString(Group.GetValue("CentralPosition"));
                else if (Group.HasValue("CentralLAT") && Group.HasValue("CentralLON"))
                {
                    EnumParser<double> LAT = new EnumParser<double>();
                    EnumParser<double> LON = new EnumParser<double>();
                    LAT.SetFromString(Group.GetValue("CentralLAT"));
                    LON.SetFromString(Group.GetValue("CentralLON"));
                    center = Utility.LLAtoECEF(LAT, LON, 1, 1);
                }
                else continue;

                if (!body.Has("PQSCityGroups"))
                    body.Set("PQSCityGroups", new Dictionary<object, Vector3>());
                Dictionary<object, Vector3> PQSList = body.Get<Dictionary<object, Vector3>>("PQSCityGroups");

                foreach (string pqs in Group.GetValues("PQSCity"))
                {
                    PQSCity mod = body.GetComponentsInChildren<PQSCity>(true).First(m => m.name == pqs);
                    if (mod != null && !PQSList.ContainsKey(mod))
                        PQSList.Add(mod, center);
                }
                foreach (string pqs in Group.GetValues("PQSCity2"))
                {
                    PQSCity2 mod = body.GetComponentsInChildren<PQSCity2>(true).First(m => m.name == pqs);
                    if (mod != null && !PQSList.ContainsKey(mod))
                        PQSList.Add(mod, center);
                }
                body.Set("PQSCityGroups", PQSList);
            }
        }
    }
}
