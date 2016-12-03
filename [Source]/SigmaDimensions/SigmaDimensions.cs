using Kopernicus;
using UnityEngine;


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
                    if (mod.name == "KSC")
                        KSCFixer(mod);
                    else
                        CityFixer(mod);
                }

                foreach (PQSCity2 mod in body.GetComponentsInChildren<PQSCity2>(true))
                {
                    City2Fixer(mod);
                }
            }
        }

        void KSCFixer(PQSCity pqs)
        {
            pqs.repositionToSphereSurface = true;
            pqs.repositionToSphereSurfaceAddHeight = true;
            pqs.repositionRadiusOffset = -22.0492050513854 * resizeBuildings;
            pqs.transform.localScale *= (float)resizeBuildings;
        }

        void CityFixer(PQSCity pqs)
        {
            if (!pqs.repositionToSphere && !pqs.repositionToSphereSurface)
            {
                // Offset = Distance from the center of the planet

                double fromRadius = pqs.repositionRadiusOffset - (body.Radius / resize);
                pqs.repositionRadiusOffset = fromRadius * resize * landscape + body.Radius;
            }
            if (pqs.repositionToSphere && !pqs.repositionToSphereSurface)
            {
                // Offset = Distance from the radius of the planet

                pqs.repositionRadiusOffset *= resize * landscape;
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
        }

        void City2Fixer(PQSCity2 pqs)
        {
            if (!pqs.snapToSurface)
            {
                // Offset = Distance from the center of the planet

                double fromRadius = pqs.alt - (body.Radius / resize);
                pqs.alt = fromRadius * resize * landscape + body.Radius;
            }
            else if (resizeBuildings != 1)
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
