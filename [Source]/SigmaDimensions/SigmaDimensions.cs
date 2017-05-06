using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class SigmaDimensions : MonoBehaviour
    {
        public double resize = 1;
        public double landscape = 1;
        public double resizeBuildings = 1;
        public CelestialBody body = null;

        void Start()
        {
            Debug.debug = false; // Reset Debug
            Debug.debug = (PQSCityGroups.debug.Count > 0); Debug.Log(">>> Sigma Dimensions Log <<<");

            foreach (CelestialBody cb in FlightGlobals.Bodies)
            {
                body = cb; // DON'T CHANGE THIS (body is public and required by other methods)
                Debug.debug = (PQSCityGroups.debug.Count > 0);
                Debug.Log("> Planet: " + body.name);

                resize = body.Has("resize") ? body.Get<double>("resize") : 1;
                landscape = body.Has("landscape") ? body.Get<double>("landscape") : 1;
                resizeBuildings = body.Has("resizeBuildings") ? body.Get<double>("resizeBuildings") : 1;

                foreach (PQSCity mod in body.GetComponentsInChildren<PQSCity>(true))
                {
                    CityFixer(mod);
                }

                foreach (PQSCity2 mod in body.GetComponentsInChildren<PQSCity2>(true))
                {
                    City2Fixer(mod);
                }
            }
        }

        void CityFixer(PQSCity pqs)
        {
            Debug.debug = false;

            // Resize the Building
            pqs.transform.localScale *= (float)resizeBuildings;


            // Fix PQSCity Groups
            if (body.Has("PQSCityGroups"))
            {
                Dictionary<object, Vector3> PQSList = body.Get<Dictionary<object, Vector3>>("PQSCityGroups");
                if (PQSList.ContainsKey(pqs))
                {
                    Debug.debug = PQSCityGroups.debug.Contains(PQSList[pqs]);
                    Debug.Log("    > PQSCity: " + pqs.name);
                    GroupFixer(pqs, PQSList[pqs]);
                }
            }


            // Fix Altitude
            double groundLevel = body.pqsController.GetSurfaceHeight(pqs.repositionRadial) - body.Radius;
            Debug.Log("        > Ground Level at Mod = " + groundLevel);

            if (!pqs.repositionToSphere && !pqs.repositionToSphereSurface)
            {
                // Offset = Distance from the center of the planet

                double fromRadius = pqs.repositionRadiusOffset - (body.Radius / resize);
                double builtInOffset = fromRadius - groundLevel / (resize * landscape);

                pqs.repositionRadiusOffset = body.Radius + groundLevel + builtInOffset * resizeBuildings;
            }
            else if (pqs.repositionToSphere && !pqs.repositionToSphereSurface)
            {
                // Offset = Distance from the radius of the planet

                double builtInOffset = pqs.repositionRadiusOffset - groundLevel / (resize * landscape);
                pqs.repositionRadiusOffset = groundLevel + builtInOffset * resizeBuildings;
            }
            else if (pqs.repositionToSphereSurface && pqs.repositionToSphereSurfaceAddHeight)
            {
                // Offset = Distance from the surface of the planet

                pqs.repositionRadiusOffset *= resizeBuildings;
            }
            Debug.Log("        > PQSCity Offset = " + pqs.repositionRadiusOffset);
        }

        void City2Fixer(PQSCity2 pqs)
        {

            // Resize the Building
            pqs.transform.localScale *= (float)resizeBuildings;


            // Fix PQSCity Groups
            if (body.Has("PQSCityGroups"))
            {
                Dictionary<object, Vector3> PQSList = body.Get<Dictionary<object, Vector3>>("PQSCityGroups");
                if (PQSList.ContainsKey(pqs))
                {
                    Debug.debug = PQSCityGroups.debug.Contains(PQSList[pqs]);
                    Debug.Log("    > PQSCity2: " + pqs.name);
                    GroupFixer(pqs, PQSList[pqs]);
                }
            }


            // Fix Altitude
            if (!pqs.snapToSurface)
            {
                // Offset = Distance from the radius of the planet

                double groundLevel = body.pqsController.GetSurfaceHeight(pqs.PlanetRelativePosition) - body.Radius;
                Debug.Log("        > Ground Level at Mod = " + groundLevel);

                if (body.ocean && groundLevel < 0)
                    groundLevel = 0;

                double builtInOffset = pqs.alt - groundLevel / (resize * landscape);
                pqs.alt = groundLevel + builtInOffset * resizeBuildings;

                Debug.Log("        > PQSCity2 Alt = " + pqs.alt);
            }
            else
            {
                // Offset = Distance from the surface of the planet

                pqs.snapHeightOffset *= resizeBuildings;
                Debug.Log("        > PQSCity2 Offset = " + pqs.snapHeightOffset);
            }
        }

        void GroupFixer(object mod, Vector3 REFvector)
        {
            // Moves the group
            if (body == FlightGlobals.GetHomeBody() && REFvector == new Vector3(157000, -1000, -570000))
            {
                PQSCity KSC = body.GetComponentsInChildren<PQSCity>().First(m => m.name == "KSC");
                MoveGroup(mod, KSC.repositionRadial, KSC.reorientFinalAngle - (-15), 0, 64.7846885412);
                REFvector = KSC.repositionRadial; // Change the REFvector the the new position for Lerping
            }
            else if (body.Has("PQSCityGroupsMove"))
            {
                var MovesInfo = body.Get<Dictionary<string, KeyValuePair<Vector3[], NumericParser<double>[]>>> ("PQSCityGroupsMove");

                if (MovesInfo.ContainsKey(body.name) && MovesInfo[body.name].ContainsKey(name))
                {
                    var info = MovesInfo[body.name][name];
                    MoveGroup(mod, info.Key[1], (float)info.Value[0], info.Value[1], info.Value[2]);
                    REFvector = info.Key[1]; // Change the REFvector the the new position for Lerping
                }
            }


            // Spread or Shrinks the group to account for Resize
            Vector3 PQSvector = ((Vector3)GetPosition(mod)).normalized;
            Vector3 NEWvector = Vector3.LerpUnclamped(REFvector.normalized, PQSvector, (float)(resizeBuildings / resize));
            SetPosition(mod, NEWvector);
            Debug.Log("        > Mod lerped position = " + (Vector3)GetPosition(mod) + ", (LAT: " + new LatLon((Vector3)GetPosition(mod)).lat + ", LON: " + new LatLon((Vector3)GetPosition(mod)).lon + ")");
        }

        void MoveGroup(object mod, Vector3 moveTo, float angle = 0, double fixAltitude = 0, double originalAltitude = double.NegativeInfinity)
        {
            LatLon target = new LatLon(moveTo.normalized);

            // Fix Rotation
            Rotate(mod, angle);

            // ORIGINAL VECTORS (Center, North, East)
            LatLon origin = new LatLon(body.Get<Dictionary<object, Vector3>>("PQSCityGroups")[mod].normalized);
            Vector3 north = Vector3.ProjectOnPlane(Vector3.up, origin.vector);
            Vector3 east = QuaternionD.AngleAxis(90, origin.vector) * north;
            Debug.Log("        > Group center position = " + origin.vector + ", (LAT: " + origin.lat + ", LON: " + origin.lon + ")");
            Debug.Log("        > Final center position = " + target.vector + ", (LAT: " + target.lat + ", LON: " + target.lon + ")");
            Debug.Log("        > Mod original position = " + (Vector3)GetPosition(mod) + ", (LAT: " + new LatLon((Vector3)GetPosition(mod)).lat + ", LON: " + new LatLon((Vector3)GetPosition(mod)).lon + ")");

            // PQS Vectors (PQS, North, East)
            Vector3 oldPQS = Vector3.ProjectOnPlane(((Vector3)GetPosition(mod)).normalized, origin.vector);
            Vector3 pqsNorth = Vector3.Project(oldPQS, north);
            Vector3 pqsEast = Vector3.Project(oldPQS, east);

            // Distance from center (Northward, Eastward)
            float northward = pqsNorth.magnitude * (1 - (Vector3.Angle(north.normalized, pqsNorth.normalized) / 90));
            float eastward = pqsEast.magnitude * (1 - (Vector3.Angle(east.normalized, pqsEast.normalized) / 90));

            // New Position Vectors (North, East)
            Vector3 newNorth = Vector3.ProjectOnPlane(Vector3.up, target.vector).normalized;
            Vector3 newEast = (QuaternionD.AngleAxis(90, target.vector) * newNorth);

            // Account for PQSCity rotation:
            // PQSCity rotate when their Longitude changes
            angle -= (float)(origin.lon - target.lon);
            QuaternionD rotation = QuaternionD.AngleAxis(angle, target.vector);

            // Calculate final position by adding the north and east distances to the target position
            // then rotate the new vector by as many degrees as it is necessary to account for the PQS model rotation
            SetPosition(mod, rotation * (target.vector + newNorth * northward + newEast * eastward));
            Debug.Log("        > Mod final position = " + (Vector3)GetPosition(mod) + ", (LAT: " + new LatLon((Vector3)GetPosition(mod)).lat + ", LON: " + new LatLon((Vector3)GetPosition(mod)).lon + ")");

            // Fix Altitude
            if (originalAltitude == double.NegativeInfinity)
                originalAltitude = (body.pqsController.GetSurfaceHeight(origin.vector) - body.Radius) / (resize * landscape);
            Debug.Log("        > Mod original altitude = " + originalAltitude);

            FixAltitude(mod, (body.pqsController.GetSurfaceHeight(target.vector) - body.Radius) / (resize * landscape) - originalAltitude + fixAltitude);
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

        void SetPosition(object mod, Vector3 position)
        {
            string type = mod.GetType().ToString();
            if (type == "PQSCity")
                ((PQSCity)mod).repositionRadial = position;
            else if (type == "PQSCity2")
            {
                LatLon LLA = new LatLon(position);
                ((PQSCity2)mod).lat = LLA.lat;
                ((PQSCity2)mod).lon = LLA.lon;
            }
        }

        void FixAltitude(object mod, double fixAltitude)
        {
            string type = mod.GetType().ToString();
            if (type == "PQSCity")
                ((PQSCity)mod).repositionRadiusOffset += fixAltitude;
            else if (type == "PQSCity2")
                ((PQSCity2)mod).alt += fixAltitude;

            Debug.Log("        > Mod Fixed Altitude = " + fixAltitude);
        }

        void Rotate(object mod, float angle)
        {
            string type = mod.GetType().ToString();
            if (type == "PQSCity")
                ((PQSCity)mod).reorientFinalAngle += angle;
            else if (type == "PQSCity2")
                ((PQSCity2)mod).rotation += angle;
        }

        public class LatLon
        {
            double[] data = { 1, 1, 1 };
            Vector3 v = Vector3.one;

            public double lat
            {
                get { return data[0]; }
                set
                {
                    data[0] = value;
                    Update();
                }
            }
            public double lon
            {
                get { return data[1]; }
                set
                {
                    data[1] = value;
                    Update();
                }
            }
            public double alt
            {
                get { return data[2]; }
                set
                {
                    data[2] = value;
                    Update();
                }
            }
            public Vector3 vector
            {
                get { return v; }
                set
                {
                    v = value;
                    data[0] = 90 + Math.Atan2(-v.z / Math.Sin(Math.Atan2(v.z, v.x)), v.y) * 180 / Math.PI;
                    data[1] = Math.Atan2(v.z, v.x) * 180 / Math.PI;
                    data[2] = Math.Pow(v.x * v.x + v.y * v.y + v.z * v.z, 0.5);
                }
            }
            void Update()
            {
                v = Utility.LLAtoECEF(data[0], data[1], 0, data[2]);
            }

            public LatLon()
            {
            }

            public LatLon(Vector3 input)
            {
                vector = input;
            }
            public LatLon(LatLon input)
            {
                data[0] = input.lat;
                data[1] = input.lon;
                data[2] = input.alt;
                v = input.vector;
            }
        }
    }
}
