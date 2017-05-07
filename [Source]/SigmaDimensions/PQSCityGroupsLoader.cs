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
        public static List<Vector3> debug = new List<Vector3>();

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
                Debug.debug = false;
                if (Group.HasValue("debug"))
                {
                    NumericParser<bool> log = new NumericParser<bool>();
                    log.SetFromString(Group.GetValue("debug"));
                    if (log.value)
                        Debug.debug = true;
                }

                string name = Group.GetValue("name");
                CelestialBody body = FlightGlobals.Bodies.First(b => b.name == Group.GetValue("body"));
                if (string.IsNullOrEmpty(name) || body == null) continue;
                Debug.Log("> Planet: " + body.name + (body.name != body.transform.name ? (", (A.K.A.: " + body.transform.name + ")") : ""));
                Debug.Log("    > Group: " + name);


                // FIND GROUP CENTER
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

                    if (center == null)
                        center = GetCenter(M, body);
                }

                foreach (string planet in ExternalGroups.Keys)
                {
                    UnityEngine.Debug.Log("SigmaLog: EXTERNAL PLANET - " + planet);
                    foreach (string group in ExternalGroups[planet].Keys)
                    {
                        UnityEngine.Debug.Log("SigmaLog: EXTERNAL GROUP     - " + group);
                        UnityEngine.Debug.Log("SigmaLog: EXTERNAL MODS COUNT     - " + ExternalGroups[planet][group].Count);
                    }
                }
                if (center == null && ExternalGroups.ContainsKey(body.name) && ExternalGroups[body.name].ContainsKey(name))
                    center = GetPosition(ExternalGroups[body.name][name][0]);
                if (center == null) continue;
                if (Debug.debug && !debug.Contains(center)) debug.Add(center);
                Debug.Log("         > Center position = " + center.value + ", (LAT: " + new SigmaDimensions.LatLon(center).lat + ", LON: " + new SigmaDimensions.LatLon(center).lon + ")");


                // ADD PQS MODS TO THE GROUP
                if (Group.HasNode("MODS"))
                {
                    ConfigNode M = Group.GetNode("MODS");

                    foreach (string city in M.GetValues("PQSCity"))
                    {
                        PQSCity mod = body.GetComponentsInChildren<PQSCity>(true).FirstOrDefault(m => m.name == city);

                        if (mod != null)
                        {
                            if (PQSList.ContainsKey(mod)) continue;

                            PQSList.Add(mod, center);
                            Debug.Log("              > PQSCity:  " + mod.name);
                        }
                    }
                    foreach (string city2 in M.GetValues("PQSCity2"))
                    {
                        PQSCity2 mod = body.GetComponentsInChildren<PQSCity2>(true).FirstOrDefault(m => m.name == city2);

                        if (mod != null)
                        {
                            if (PQSList.ContainsKey(mod)) continue;

                            PQSList.Add(mod, center);
                            Debug.Log("              > PQSCity2: " + mod.name);
                        }
                    }
                }


                // ADD EXTERNAL MODS TO THIS GROUP
                if (ExternalGroups.ContainsKey(body.name) && ExternalGroups[body.name].ContainsKey(name))
                {
                    foreach (object mod in ExternalGroups[body.name][name])
                    {
                        if (PQSList.ContainsKey(mod)) continue;

                        PQSList.Add(mod, center);
                        Debug.Log("              > external: " + mod);
                    }
                }


                // REMOVE KSC FROM THE LIST

                PQSCity ksc = FlightGlobals.GetHomeBody().GetComponentsInChildren<PQSCity>(true).FirstOrDefault(m => m.name == "KSC");
                if (PQSList.ContainsKey(ksc))
                    PQSList.Remove(ksc);


                body.Set("PQSCityGroups", PQSList);


                // ADD THIS GROUP TO THE MOVE LIST
                if (Group.HasNode("MOVE"))
                {
                    ConfigNode C2 = Group.GetNode("MOVE");
                    Vector3? newCenter = GetCenter(C2, body);

                    if (newCenter == null) continue;
                    Debug.Log("Move Group to position = " + newCenter.Value + ", (LAT: " + new SigmaDimensions.LatLon(newCenter.Value).lat + ", LON: " + new SigmaDimensions.LatLon(newCenter.Value).lon + ")");


                    var info = new KeyValuePair<Vector3, NumericParser<double>[]>((Vector3)newCenter, new[] { 0, 0, new NumericParser<double>() });

                    if (C2.HasValue("Rotate"))
                        info.Value[0].SetFromString(C2.GetValue("Rotate")); Debug.Log("Rotate group = " + info.Value[0].value);
                    if (C2.HasValue("fixAltitude"))
                        info.Value[1].SetFromString(C2.GetValue("fixAltitude")); Debug.Log("Fix group altitude = " + info.Value[1].value);
                    if (C2.HasValue("originalAltitude"))
                        info.Value[2].SetFromString(C2.GetValue("originalAltitude"));
                    else
                        info.Value[2].SetFromString("-Infinity"); Debug.Log("Original group altitude = " + (info.Value[2].value == double.NegativeInfinity ? "[Not Specified]" : info.Value[2].value.ToString()));


                    if (!body.Has("PQSCityGroupsMove"))
                        body.Set("PQSCityGroupsMove", new Dictionary<Vector3, KeyValuePair<Vector3, NumericParser<double>[]>>());
                    var MoveList = body.Get<Dictionary<Vector3, KeyValuePair<Vector3, NumericParser<double>[]>>>("PQSCityGroupsMove");

                    if (!MoveList.ContainsKey(center.value))
                        MoveList.Add(center.value, info);

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


                    // REMOVE KSC FROM THE LIST

                    PQSCity ksc = FlightGlobals.GetHomeBody().GetComponentsInChildren<PQSCity>(true).FirstOrDefault(m => m.name == "KSC");
                    if (PQSList.ContainsKey(ksc))
                        PQSList.Remove(ksc);


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
            else if (node.HasValue("PQSCity"))
            {
                return body.GetComponentsInChildren<PQSCity>(true).FirstOrDefault(p => p.name == node.GetValue("PQSCity")).repositionRadial;
            }
            else if (node.HasValue("PQSCity2"))
            {
                return body.GetComponentsInChildren<PQSCity2>(true).First(p => p.name == node.GetValue("PQSCity2")).PlanetRelativePosition;
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
