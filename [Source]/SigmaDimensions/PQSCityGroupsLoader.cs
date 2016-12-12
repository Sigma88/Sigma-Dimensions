using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    class PQSCityGroups
    {
        static Dictionary<string, ConfigNode> GroupsList = new Dictionary<string, ConfigNode>();
        public static Dictionary<string, ConfigNode> ExternalGroupsList = new Dictionary<string, ConfigNode>();

        void Start()
        {
            foreach (ConfigNode GroupsLoader in GameDatabase.Instance.GetConfigNodes("PQSCity_Groups"))
            {
                AddGroups(GroupsLoader.GetNodes("Group"));
            }
            SaveGroups(GroupsList);
            SaveGroups(ExternalGroupsList);
        }

        void AddGroups(ConfigNode[] Groups)
        {
            foreach (ConfigNode Group in Groups)
            {
                string name = Group.GetValue("name");
                if (string.IsNullOrEmpty(name)) continue;
                if (GroupsList.ContainsKey(name))
                    GroupsList[name].AddData(Group);
                else
                    GroupsList.Add(name, Group);
            }
        }

        void SaveGroups(Dictionary<string, ConfigNode> list)
        {
            foreach (ConfigNode Group in list.Values)
            {
                string name = Group.GetValue("name");
                CelestialBody body = FlightGlobals.Bodies.First(b => b.transform.name == Group.GetValue("body"));
                if (string.IsNullOrEmpty(name) || body == null) continue;

                Vector3Parser center = new Vector3Parser();
                if (Group.HasValue("CentralPQSCity"))
                {
                    center = body.GetComponentsInChildren<PQSCity>(true).FirstOrDefault(p => p.name == Group.GetValue("CentralPQSCity")).repositionRadial;
                }
                else if (Group.HasValue("CentralPQSCity2"))
                {
                    center = (Vector3)body.GetComponentsInChildren<PQSCity2>(true).First(p => p.name == Group.GetValue("CentralPQSCity2")).PlanetRelativePosition;
                }
                else if (Group.HasValue("CentralPosition"))
                {
                    center.SetFromString(Group.GetValue("CentralPosition"));
                }
                else if (Group.HasValue("CentralLAT") && Group.HasValue("CentralLON"))
                {
                    EnumParser<double> LAT = new EnumParser<double>();
                    EnumParser<double> LON = new EnumParser<double>();
                    LAT.SetFromString(Group.GetValue("CentralLAT"));
                    LON.SetFromString(Group.GetValue("CentralLON"));
                    center = Utility.LLAtoECEF(LAT, LON, 1, 1);
                }
                else if (Group.HasValue("PQSCity"))
                {
                    center = body.GetComponentsInChildren<PQSCity>(true).FirstOrDefault(p => p.name == Group.GetValue("PQSCity")).repositionRadial;
                }
                else if (Group.HasValue("PQSCity2"))
                {
                    center = (Vector3)body.GetComponentsInChildren<PQSCity2>(true).First(p => p.name == Group.GetValue("CentralPQSCity2")).PlanetRelativePosition;
                }
                else continue;

                if (!body.Has("PQSCityGroups"))
                    body.Set("PQSCityGroups", new Dictionary<object, Vector3>());
                Dictionary<object, Vector3> PQSList = body.Get<Dictionary<object, Vector3>>("PQSCityGroups");

                foreach (string city in Group.GetValues("PQSCity"))
                {
                    PQSCity mod = body.GetComponentsInChildren<PQSCity>(true).First(m => m.name == city);
                    if (mod != null && !PQSList.ContainsKey(mod))
                        PQSList.Add(mod, center);
                }
                foreach (string city2 in Group.GetValues("PQSCity2"))
                {
                    PQSCity2 mod = body.GetComponentsInChildren<PQSCity2>(true).First(m => m.name == city2);
                    if (mod != null && !PQSList.ContainsKey(mod))
                        PQSList.Add(mod, center);
                }
                body.Set("PQSCityGroups", PQSList);
            }
        }
    }
}
