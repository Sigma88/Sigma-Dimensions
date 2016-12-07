using System;
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
            double groundLevel = body.pqsController.GetSurfaceHeight(pqs.repositionRadial) - body.Radius;

            if (!pqs.repositionToSphere && !pqs.repositionToSphereSurface)
            {
                // Offset = Distance from the center of the planet

                double fromRadius = pqs.repositionRadiusOffset - (body.Radius / resize);
                double builtInOffset = fromRadius - groundLevel / (resize * landscape);

                pqs.repositionRadiusOffset = body.Radius + groundLevel + builtInOffset * resizeBuildings;
            }
            if (pqs.repositionToSphere && !pqs.repositionToSphereSurface)
            {
                // Offset = Distance from the radius of the planet

                double builtInOffset = pqs.repositionRadiusOffset - groundLevel / (resize * landscape);

                pqs.repositionRadiusOffset = groundLevel + builtInOffset * resizeBuildings;
            }
            if (pqs.repositionToSphereSurface && pqs.repositionToSphereSurfaceAddHeight && resizeBuildings != 1)
            {
                // Offset = Distance from the surface of the planet

                pqs.repositionRadiusOffset *= resizeBuildings;
            }
            if (resizeBuildings != 1)
            {
                // Resize the Building

                pqs.transform.localScale *= (float)resizeBuildings;
            }
            if (pqs.name == "KerbinSide/CoreAssets/ksidehangars1(Clone)")
            {
                Vector3 vector = pqs.repositionRadial;
                Debug.Log("SigmaDimensionsLog: vector = " + vector);
                Vector3 REFvector = Array.Find(body.GetComponentsInChildren<PQSCity>(), m => m.name == "KSC").repositionRadial;
                Debug.Log("SigmaDimensionsLog: REFvector = " + vector);
                Vector3 newVector = Vector3.LerpUnclamped((REFvector * 100).normalized, vector.normalized, (float)(resizeBuildings / resize));
                Debug.Log("SigmaDimensionsLog: newVector = " + vector);
                pqs.repositionRadial = newVector;
                Debug.Log("SigmaDimensionsLog: pqs.repositionRadial = " + pqs.repositionRadial);
            }
        }

        void City2Fixer(PQSCity2 pqs)
        {
            double groundLevel = body.pqsController.GetSurfaceHeight(pqs.PlanetRelativePosition) - body.Radius;

            if (!pqs.snapToSurface)
            {
                // Offset = Distance from the center of the planet

                double fromRadius = pqs.alt - (body.Radius / resize);
                double builtInOffset = fromRadius - groundLevel / (resize * landscape);

                pqs.alt = body.Radius + groundLevel + builtInOffset * resizeBuildings;
            }
            else
            {
                // Offset = Distance from the surface of the planet

                pqs.snapHeightOffset *= resizeBuildings;
            }
            if (resizeBuildings != 1)
            {
                // Resize the Building

                pqs.transform.localScale *= (float)resizeBuildings;
            }
        }
    }
}
