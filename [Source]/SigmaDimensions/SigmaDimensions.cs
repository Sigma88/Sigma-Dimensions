using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Kopernicus;
using Kopernicus.Configuration;
using SigmaDimensionsPlugin;


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
            foreach (CelestialBody cb in FlightGlobals.Bodies)
            {
                body = cb;
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
            // Resize the Building

            pqs.transform.localScale *= (float)resizeBuildings;


            // Fix PQSCity Groups

            if (body.Has("PQSCityGroups"))
            {
                Dictionary<object, Vector3> PQSList = body.Get<Dictionary<object, Vector3>>("PQSCityGroups");
                if (PQSList.ContainsKey(pqs))
                {
                    GroupFixer(pqs, PQSList[pqs].normalized);
                }
            }


            // Fix Altitude

            double groundLevel = body.pqsController.GetSurfaceHeight(pqs.repositionRadial) - body.Radius;

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
                    GroupFixer(pqs, PQSList[pqs].normalized);
                }
            }


            // Fix Altitude

            if (!pqs.snapToSurface)
            {
                // Offset = Distance from the center of the planet

                double groundLevel = body.pqsController.GetSurfaceHeight(pqs.PlanetRelativePosition) - body.Radius;
                double fromRadius = pqs.alt - (body.Radius / resize);
                double builtInOffset = fromRadius - groundLevel / (resize * landscape);

                pqs.alt = body.Radius + groundLevel + builtInOffset * resizeBuildings;
            }
            else
            {
                // Offset = Distance from the surface of the planet

                pqs.snapHeightOffset *= resizeBuildings;
            }
        }

        void GroupFixer(PQSCity pqs, Vector3 REFvector)
        {
            if (body == FlightGlobals.GetHomeBody())
                LinkToKSC(pqs);

            Vector3 PQSvector = pqs.repositionRadial.normalized;
            Vector3 NEWvector = Vector3.LerpUnclamped(REFvector, PQSvector, (float)(resizeBuildings / resize));
            pqs.repositionRadial = NEWvector;
        }

        void GroupFixer(PQSCity2 pqs, Vector3 REFvector)
        {
            if (body == FlightGlobals.GetHomeBody())
                LinkToKSC(pqs);

            Vector3 PQSvector = pqs.PlanetRelativePosition.normalized;
            Vector3 NEWvector = Vector3.LerpUnclamped(REFvector, PQSvector, (float)(resizeBuildings / resize));
            double[] LLA = ECEFtoLLA(NEWvector);
            pqs.lat = LLA[0];
            pqs.lon = LLA[1];
        }

        void LinkToKSC(PQSCity pqs)
        {
            PQSCity KSC = body.GetComponentsInChildren<PQSCity>().First(m => m.name == "KSC");
            Vector3 movedKSC = KSC.repositionRadial.normalized;

            if (body.Get<Dictionary<object, Vector3>>("PQSCityGroups")[pqs].normalized == movedKSC)
            {
                // Fix Rotation
                float angle = KSC.reorientFinalAngle - (-15);
                pqs.reorientFinalAngle += angle;


                // Fix Latitude and Longitude
                Vector3 stockKSC = new Vector3(157000, -1000, -570000).normalized;
                double dLON = (Math.Atan2(movedKSC.z, movedKSC.x) - Math.Atan2(stockKSC.z, stockKSC.x)) * 180 / Math.PI;
                Quaternion rotation = Quaternion.AngleAxis((float)dLON + angle, movedKSC);

                pqs.repositionRadial = pqs.repositionRadial.normalized + movedKSC - stockKSC;
                pqs.repositionRadial = rotation * pqs.repositionRadial.normalized;


                // Fix Altitude
                if (!pqs.repositionToSphereSurface)
                {
                    pqs.repositionRadiusOffset += (body.pqsController.GetSurfaceHeight(movedKSC) - body.Radius) / (resize * landscape) - 64.7846885412;
                }
            }
        }

        void LinkToKSC(PQSCity2 pqs)
        {
            PQSCity KSC = body.GetComponentsInChildren<PQSCity>().First(m => m.name == "KSC");
            Vector3 movedKSC = KSC.repositionRadial.normalized;

            if (body.Get<Dictionary<object, Vector3>>("PQSCityGroups")[pqs].normalized == movedKSC)
            {
                // Fix Rotation
                float angle = KSC.reorientFinalAngle - (-15);
                pqs.rotation += angle;


                // Fix Latitude and Longitude
                Vector3 stockKSC = new Vector3(157000, -1000, -570000).normalized;
                double dLON = (Math.Atan2(movedKSC.z, movedKSC.x) - Math.Atan2(stockKSC.z, stockKSC.x)) * 180 / Math.PI;
                Quaternion rotation = Quaternion.AngleAxis((float)dLON + angle, movedKSC);
                Vector3 vector = pqs.PlanetRelativePosition;
                vector = vector.normalized + movedKSC - stockKSC;
                vector = rotation * vector.normalized;


                // Fix Altitude
                if (!pqs.snapToSurface)
                {
                    pqs.snapHeightOffset += (body.pqsController.GetSurfaceHeight(movedKSC) - body.Radius) / (resize * landscape) - 64.7846885412;
                }
            }
        }

        public double[] ECEFtoLLA(Vector3 vector)
        {
            double lat = 90 + Math.Atan2(-vector.z / Math.Sin(Math.Atan2(vector.z, vector.x)), vector.y) * 180 / Math.PI;
            double lon = Math.Atan2(vector.z, vector.x) * 180 / Math.PI;
            double alt = Math.Pow(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z, 0.5);
            return new double[] { lat, lon, alt };
        }
    }
}
