using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class PQSCityGroups : MonoBehaviour
    {
        Dictionary<string, ConfigNode> GroupsList = new Dictionary<string, ConfigNode>();
        public static Dictionary<string, Dictionary<string, List<object>>> ExternalGroups = new Dictionary<string, Dictionary<string, List<object>>>();
        
        void Start()
        {
            foreach (ConfigNode GroupsLoader in GameDatabase.Instance.GetConfigNodes("PQSCity_Groups"))
            {
                AddGroups(GroupsLoader.GetNodes("GROUP"));
            }
            
            SaveGroups();
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

        void SaveGroups()
        {
            // LOAD SD GROUPS
            foreach (ConfigNode Group in GroupsList.Values)
            {
                string name = Group.GetValue("name");
                CelestialBody body = FlightGlobals.Bodies.First(b => b.name == Group.GetValue("body"));
                if (string.IsNullOrEmpty(name) || body == null) continue;

                // Find Group Center
                Vector3Parser center = null;

                if (Group.HasNode("CENTER"))
                {
                    ConfigNode C = Group.GetNode("CENTER");

                    if (C.HasValue("CentralPQSCity"))
                    {
                        if (body == FlightGlobals.GetHomeBody() && C.GetValue("CentralPQSCity") == "KSC")
                            center = new Vector3(157000, -1000, -570000);
                    }

                    if (center == null)
                        center = GetCenter(C, body);
                }

                if (!body.Has("PQSCityGroups"))
                    body.Set("PQSCityGroups", new Dictionary<object, Vector3>());
                Dictionary<object, Vector3> PQSList = body.Get<Dictionary<object, Vector3>>("PQSCityGroups");

                if (Group.HasNode("MODS"))
                {
                    ConfigNode M = Group.GetNode("MODS");

                    foreach (string city in M.GetValues("PQSCity"))
                    {
                        PQSCity mod = body.GetComponentsInChildren<PQSCity>(true).First(m => m.name == city);

                        if (mod != null)
                        {
                            if (center == null)
                                center = mod.repositionRadial;
                            if (!PQSList.ContainsKey(mod))
                                PQSList.Add(mod, center);
                        }
                    }
                    foreach (string city2 in M.GetValues("PQSCity2"))
                    {
                        PQSCity2 mod = body.GetComponentsInChildren<PQSCity2>(true).First(m => m.name == city2);

                        if (mod != null)
                        {
                            if (center == null)
                                center = (Vector3)mod.PlanetRelativePosition;
                            if (!PQSList.ContainsKey(mod))
                                PQSList.Add(mod, center);
                        }
                    }
                }

                if (center == null) continue;


                // ADD EXTERNAL MODS TO THIS GROUP
                if (ExternalGroups.ContainsKey(body.name) && ExternalGroups[body.name].ContainsKey(name))
                {
                    foreach (object mod in ExternalGroups[body.name][name])
                    {
                        if (!PQSList.ContainsKey(mod))
                            PQSList.Add(mod, center);
                    }
                }

                body.Set("PQSCityGroups", PQSList);
                Debug.debug = true;
                foreach(object k in PQSList.Keys)
                {
                    Debug.Log(k + " >>> " + PQSList[k]);
                }

                // ADD THIS GROUP TO THE MOVE LIST

                if (Group.HasNode("MOVE"))
                {
                    ConfigNode C2 = Group.GetNode("MOVE");
                    Vector3? newCenter = GetCenter(C2, body);

                    if (newCenter == null) continue;
                    Vector3[] keys = new Vector3[] { center, (Vector3)newCenter };


                    NumericParser<double>[] values = new[] { 0, 0, new NumericParser<double>() };
                    values[2].SetFromString("-Infinity");

                    if (C2.HasValue("Rotate"))
                        values[0].SetFromString(C2.GetValue("Rotate"));
                    if (C2.HasValue("fixAltitude"))
                        values[1].SetFromString(C2.GetValue("fixAltitude"));
                    if (C2.HasValue("originalAltitude"))
                        values[2].SetFromString(C2.GetValue("originalAltitude"));
                    

                    if (!body.Has("PQSCityGroupsMove"))
                        body.Set("PQSCityGroupsMove", new Dictionary<Vector3[], NumericParser<double>[]>());
                    Dictionary<Vector3[], NumericParser<double>[]> MoveList = body.Get<Dictionary<Vector3[], NumericParser<double>[]>>("PQSCityGroupsMove");


                    if (!MoveList.ContainsKey(keys))
                        MoveList.Add(keys, values);

                    body.Set("PQSCityGroupsMove", MoveList);
                }
            }


            // LOAD REMAINING EXTERNAL GROUPS
            foreach (string planet in ExternalGroups.Keys)
            {
                foreach (string group in ExternalGroups[planet].Keys)
                {
                    CelestialBody body = FlightGlobals.Bodies.FirstOrDefault(b => b.name == planet);
                    if (body == null || ExternalGroups[planet][group].Count == 0) continue;

                    // Since these groups are new they don't have a center
                    // Define the center as the position of the first mod in the array
                    Vector3? center = null;
                    center = GetPosition(ExternalGroups[planet][group][0]);
                    if (center == null) continue;

                    if (!body.Has("PQSCityGroups"))
                        body.Set("PQSCityGroups", new Dictionary<object, Vector3>());
                    Dictionary<object, Vector3> PQSList = body.Get<Dictionary<object, Vector3>>("PQSCityGroups");

                    foreach (object mod in ExternalGroups[planet][group])
                    {
                        if (!PQSList.ContainsKey(mod))
                            PQSList.Add(mod, (Vector3)center);
                    }

                    body.Set("PQSCityGroups", PQSList);
                }
            }
        }

        Vector3? GetCenter(ConfigNode node, CelestialBody body)
        {
            if (node.HasValue("CentralPQSCity"))
            {
                return body.GetComponentsInChildren<PQSCity>(true).FirstOrDefault(p => p.name == node.GetValue("CentralPQSCity")).repositionRadial;
            }
            else if (node.HasValue("CentralPQSCity2"))
            {
                return body.GetComponentsInChildren<PQSCity2>(true).First(p => p.name == node.GetValue("CentralPQSCity2")).PlanetRelativePosition;
            }
            else if (node.HasValue("CentralPosition"))
            {
                Vector3Parser v = new Vector3Parser();
                v.SetFromString(node.GetValue("CentralPosition"));
                return v;
            }
            else if (node.HasValue("CentralLAT") && node.HasValue("CentralLON"))
            {
                NumericParser<double> LAT = new NumericParser<double>();
                NumericParser<double> LON = new NumericParser<double>();
                LAT.SetFromString(node.GetValue("CentralLAT"));
                LON.SetFromString(node.GetValue("CentralLON"));
                return Utility.LLAtoECEF(LAT, LON, 1, 1);
            }
            else return null;
        }

        Vector3? GetPosition(object mod)
        {
            string type = mod.GetType().ToString();
            if (type == "PQSCity")
                return ((PQSCity)mod).repositionRadial;
            else if (type == "PQSCity2")
                return ((PQSCity2)mod).PlanetRelativePosition;
            else return null;
        }
    }
}
