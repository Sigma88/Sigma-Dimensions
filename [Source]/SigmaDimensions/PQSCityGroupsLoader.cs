using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class PQSCityGroups : MonoBehaviour
    {
        // Public Dictionary for External Groups
        public static Dictionary<CelestialBody, Dictionary<string, List<object>>> ExternalGroups = new Dictionary<CelestialBody, Dictionary<string, List<object>>>();
        public static Dictionary<CelestialBody, Dictionary<string, List<object>>> ExternalExceptions = new Dictionary<CelestialBody, Dictionary<string, List<object>>>();

        Dictionary<string, ConfigNode> GroupsList = new Dictionary<string, ConfigNode>();
        internal static List<Vector3> debug = new List<Vector3>();
        internal static NumericParser<bool> debugAllGroups = false;

        void Start()
        {
            foreach (ConfigNode GroupsLoader in GameDatabase.Instance.GetConfigNodes("PQSCity_Groups"))
            {
                AddGroups(GroupsLoader.GetNodes("GROUP"));
                if (GroupsLoader.HasValue("debug") && !debugAllGroups.Value)
                    debugAllGroups.SetFromString(GroupsLoader.GetValue("debug"));
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
                string group = Group.GetValue("name");
                CelestialBody body = FlightGlobals.Bodies.FirstOrDefault(b => b.transform.name == Group.GetValue("body"));
                if (string.IsNullOrEmpty(group) || body == null) continue;
                Debug.Log("PQSCityGroups.SaveGroups", ">>> Loading PQSCityGroups from config files <<<");
                Debug.Log("PQSCityGroups.SaveGroups", "> Planet: " + body.name + (body.name != body.displayName.Replace("^N", "") ? (", (A.K.A.: " + body.displayName.Replace("^N", "") + ")") : "") + (body.name != body.transform.name ? (", (A.K.A.: " + body.transform.name + ")") : ""));
                Debug.Log("PQSCityGroups.SaveGroups", "    > Group: " + group);

                bool.TryParse(Group.GetValue("exclude"), out bool exclude);

                // FIND GROUP CENTER
                Vector3Parser center = null;

                // Get Center position from the CENTER node
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

                if (!body.Has("ExcludedPQSCityMods"))
                    body.Set("ExcludedPQSCityMods", new List<object>());
                List<object> ExcludeList = body.Get<List<object>>("ExcludedPQSCityMods");

                // If the Center position has not been found get it from the MODS node
                if (Group.HasNode("MODS"))
                {
                    ConfigNode M = Group.GetNode("MODS");

                    if (center == null)
                        center = GetCenter(M, body);
                }

                // If the Center position has not been found get it from the external groups
                if
                (
                    center == null &&
                    ExternalGroups?.ContainsKey(body) == true &&
                    ExternalGroups[body].ContainsKey(group)
                )
                {
                    center = GetPosition(ExternalGroups[body][group].FirstOrDefault());
                }

                // If the Center position has not been found stop here
                if (center == null) continue;
                Debug.Log("PQSCityGroups.SaveGroups", "        > Center position = " + center.Value + ", (LAT: " + new SigmaDimensions.LatLon(center).lat + ", LON: " + new SigmaDimensions.LatLon(center).lon + ")");


                // ADD PQS MODS TO THE GROUP
                if (Group.HasNode("MODS"))
                {
                    ConfigNode M = Group.GetNode("MODS");

                    foreach (string city in M.GetValues("PQSCity"))
                    {
                        PQSCity mod = body.GetComponentsInChildren<PQSCity>(true).FirstOrDefault(m => m.name == city);

                        if (mod != null)
                        {
                            // If the mod has already been added overwrite it
                            // This way custom groups will overwrite external ones
                            if (PQSList.ContainsKey(mod)) PQSList.Remove(mod);

                            if (!exclude)
                            {
                                PQSList.Add(mod, center);
                                Debug.Log("PQSCityGroups.SaveGroups", "            > PQSCity:  " + mod.name);
                            }
                            else
                            {
                                ExcludeList.Add(mod);
                                Debug.Log("PQSCityGroups.SaveGroups", "            > Excluded PQSCity:  " + mod.name);
                            }
                        }
                    }
                    foreach (string city2 in M.GetValues("PQSCity2"))
                    {
                        PQSCity2 mod = body.GetComponentsInChildren<PQSCity2>(true).FirstOrDefault(m => m.name == city2);

                        if (mod != null)
                        {
                            // If the mod has already been added overwrite it
                            // This way custom groups will overwrite external ones
                            if (PQSList.ContainsKey(mod)) PQSList.Remove(mod);

                            if (!exclude)
                            {
                                PQSList.Add(mod, center);
                                Debug.Log("PQSCityGroups.SaveGroups", "            > PQSCity2:  " + mod.name);
                            }
                            else
                            {
                                ExcludeList.Add(mod);
                                Debug.Log("PQSCityGroups.SaveGroups", "            > Excluded PQSCity2:  " + mod.name);
                            }
                        }
                    }
                }


                // ADD EXTERNAL MODS TO THIS GROUP
                if
                (
                    ExternalGroups?.ContainsKey(body) == true &&
                    ExternalGroups[body].ContainsKey(group) &&
                    ExternalGroups[body][group].Where(m => m != null)?.Count() > 0
                )
                {
                    foreach (object mod in ExternalGroups[body][group].Where(m => m != null))
                    {
                        // External groups should not overwrite custom ones
                        if (PQSList.ContainsKey(mod)) continue;

                        if (!exclude)
                        {
                            PQSList.Add(mod, center);
                            Debug.Log("PQSCityGroups.SaveGroups", "            > external:  " + mod);
                        }
                        else
                        {
                            ExcludeList.Add(mod);
                            Debug.Log("PQSCityGroups.SaveGroups", "            > Excluded external:  " + mod);
                        }
                    }
                    ExternalGroups[body].Remove(group);
                }


                // REMOVE KSC FROM THE LIST

                PQSCity ksc = FlightGlobals.GetHomeBody().GetComponentsInChildren<PQSCity>(true).FirstOrDefault(m => m.name == "KSC");
                if (PQSList.ContainsKey(ksc))
                    PQSList.Remove(ksc);
                if (ExcludeList.Contains(ksc))
                    ExcludeList.Remove(ksc);


                body.Set("PQSCityGroups", PQSList);
                body.Set("ExcludedPQSCityMods", ExcludeList);


                // ADD THIS GROUP TO THE MOVE LIST
                if (Group.HasNode("MOVE"))
                {
                    ConfigNode C2 = Group.GetNode("MOVE");
                    Vector3? newCenter = GetCenter(C2, body);

                    if (newCenter == null) newCenter = center;
                    Debug.Log("PQSCityGroups.SaveGroups", "Move Group to position = " + newCenter.Value + ", (LAT: " + new SigmaDimensions.LatLon(newCenter.Value).lat + ", LON: " + new SigmaDimensions.LatLon(newCenter.Value).lon + ")");


                    var info = new KeyValuePair<Vector3, NumericParser<double>[]>((Vector3)newCenter, new[] { 0, 0, new NumericParser<double>() });

                    if (C2.HasValue("Rotate"))
                        info.Value[0].SetFromString(C2.GetValue("Rotate")); Debug.Log("PQSCityGroups.SaveGroups", "Rotate group = " + info.Value[0].Value);
                    if (C2.HasValue("fixAltitude"))
                        info.Value[1].SetFromString(C2.GetValue("fixAltitude")); Debug.Log("PQSCityGroups.SaveGroups", "Fix group altitude = " + info.Value[1].Value);
                    if (C2.HasValue("originalAltitude"))
                        info.Value[2].SetFromString(C2.GetValue("originalAltitude"));
                    else
                        info.Value[2].SetFromString("-Infinity"); Debug.Log("PQSCityGroups.SaveGroups", "Original group altitude = " + (info.Value[2].Value == double.NegativeInfinity ? "[Not Specified]" : info.Value[2].Value.ToString()));


                    if (!body.Has("PQSCityGroupsMove"))
                        body.Set("PQSCityGroupsMove", new Dictionary<Vector3, KeyValuePair<Vector3, NumericParser<double>[]>>());
                    var MoveList = body.Get<Dictionary<Vector3, KeyValuePair<Vector3, NumericParser<double>[]>>>("PQSCityGroupsMove");

                    if (!MoveList.ContainsKey(center.Value))
                        MoveList.Add(center.Value, info);

                    body.Set("PQSCityGroupsMove", MoveList);
                }
            }


            // Make sure External Groups are valid
            if (ExternalGroups == null) ExternalGroups = new Dictionary<CelestialBody, Dictionary<string, List<object>>>();

            // LOAD REMAINING EXTERNAL GROUPS
            Debug.Log("PQSCityGroups.SaveGroups", ">>> Loading external PQSCityGroups <<<");
            foreach (CelestialBody planet in ExternalGroups.Keys.Where(p => p != null && ExternalGroups[p] != null))
            {
                Debug.Log("PQSCityGroups.SaveGroups", "> Planet: " + planet.name + (planet.name != planet.displayName.Replace("^N", "") ? (", (A.K.A.: " + planet.displayName.Replace("^N", "") + ")") : "") + (planet.name != planet.transform.name ? (", (A.K.A.: " + planet.transform.name + ")") : ""));
                foreach (string group in ExternalGroups[planet].Keys.Where(g => !string.IsNullOrEmpty(g) && ExternalGroups[planet][g] != null))
                {
                    if (ExternalGroups[planet][group].Count == 0) continue;
                    Debug.Log("PQSCityGroups.SaveGroups", "    > Group: " + group);

                    // Since these groups are new they don't have a center
                    // Define the center as the position of the first mod in the array
                    Vector3? center = null;
                    center = GetPosition(ExternalGroups[planet][group].FirstOrDefault());
                    if (center == null) continue;
                    Debug.Log("PQSCityGroups.SaveGroups", "        > Center position = " + center + ", (LAT: " + new SigmaDimensions.LatLon((Vector3)center).lat + ", LON: " + new SigmaDimensions.LatLon((Vector3)center).lon + ")");

                    if (!planet.Has("PQSCityGroups"))
                        planet.Set("PQSCityGroups", new Dictionary<object, Vector3>());
                    Dictionary<object, Vector3> PQSList = planet.Get<Dictionary<object, Vector3>>("PQSCityGroups");

                    foreach (object mod in ExternalGroups[planet][group])
                    {
                        if (!PQSList.ContainsKey(mod))
                        {
                            PQSList.Add(mod, (Vector3)center);
                            Debug.Log("PQSCityGroups.SaveGroups", "            > external: " + mod);
                        }
                    }


                    // REMOVE KSC FROM THE LIST

                    PQSCity ksc = FlightGlobals.GetHomeBody().GetComponentsInChildren<PQSCity>(true).FirstOrDefault(m => m.name == "KSC");
                    if (PQSList.ContainsKey(ksc))
                        PQSList.Remove(ksc);


                    planet.Set("PQSCityGroups", PQSList);
                }
            }

            // LOAD EXTERNAL EXCEPTIONS
            Debug.Log("PQSCityGroups.SaveGroups", ">>> Loading external PQSCityGroups exceptions <<<");
            foreach (CelestialBody planet in ExternalExceptions.Keys.Where(p => p != null && ExternalExceptions[p] != null))
            {
                Debug.Log("PQSCityGroups.SaveGroups", "> Planet: " + planet.name + (planet.name != planet.displayName.Replace("^N", "") ? (", (A.K.A.: " + planet.displayName.Replace("^N", "") + ")") : "") + (planet.name != planet.transform.name ? (", (A.K.A.: " + planet.transform.name + ")") : ""));
                foreach (string group in ExternalExceptions[planet].Keys.Where(g => !string.IsNullOrEmpty(g) && ExternalExceptions[planet][g] != null))
                {
                    if (ExternalExceptions[planet][group].Count == 0) continue;
                    Debug.Log("PQSCityGroups.SaveGroups", "    > Group: " + group);

                    // Since these groups are exceptions they don't need a center

                    if (!planet.Has("ExcludedPQSCityMods"))
                        planet.Set("ExcludedPQSCityMods", new Dictionary<object, Vector3>());
                    List<object> ExcludeList = planet.Get<List<object>>("PQSCityGroups");

                    foreach (object mod in ExternalExceptions[planet][group])
                    {
                        if (!ExcludeList.Contains(mod))
                        {
                            ExcludeList.Add(mod);
                            Debug.Log("PQSCityGroups.SaveGroups", "            > excluded external: " + mod);
                        }
                    }


                    // REMOVE KSC FROM THE LIST

                    PQSCity ksc = FlightGlobals.GetHomeBody().GetComponentsInChildren<PQSCity>(true).FirstOrDefault(m => m.name == "KSC");
                    if (ExcludeList.Contains(ksc))
                        ExcludeList.Remove(ksc);


                    planet.Set("ExcludedPQSCityMods", ExcludeList);
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
            string type = mod?.GetType()?.ToString();
            if (type == "PQSCity")
                return ((PQSCity)mod).repositionRadial;
            else if (type == "PQSCity2")
                return ((PQSCity2)mod).PlanetRelativePosition;
            else return null;
        }
    }
}
