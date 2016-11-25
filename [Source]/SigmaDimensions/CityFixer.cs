using UnityEngine;
using Kopernicus;
using Kopernicus.Components;
using Kopernicus.Configuration;
using Kopernicus.OnDemand;
using System.Text;
using System;
using System.Reflection;
using System.Linq;
using KSP.UI.Screens;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class CityFixer : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("SigmaLog: 1");
            foreach (CelestialBody body in FlightGlobals.Bodies)
            {
               /* Debug.Log("SigmaLog: body = " + body.transform.name);
                // debug mode
                body.Set("resize", 0.1);
                Debug.Log("SigmaLog: 2");
                body.Set("landscape", 1.0);
                Debug.Log("SigmaLog: 3");
                body.Set("resizeBuildings", true);
                Debug.Log("SigmaLog: 4");*/


                // SD code
                double resize = body.Has("resize") ? body.Get<double>("resize") : 1;
                Debug.Log("SigmaLog: 5");
                double landscape = body.Has("landscape") ? body.Get<double>("landscape") : 1;
                Debug.Log("SigmaLog: 6");
                double resizeBuildings = body.Has("resizeBuildings") ? body.Get<double>("resizeBuildings") : 1;
                Debug.Log("SigmaLog: 7");

                foreach (PQSSurfaceObject obj in body.pqsSurfaceObjects.Where(o => o.GetComponent<PQSCity>() != null))
                {
                    Debug.Log("SigmaLog: 8");
                    Debug.Log("SigmaLog: obj = " + obj.name);
                    PQSCity pqs = obj.GetComponent<PQSCity>();
                    Debug.Log("SigmaLog: 9");
                    Debug.Log("SigmaLog: pqs = " + pqs.name);

                    if (!pqs.repositionToSphere && !pqs.repositionToSphereSurface)
                    {
                        // Offset = Distance from the center of the planet
                        Debug.Log("SigmaLog: 10");

                        double fromRadius = pqs.repositionRadiusOffset - (body.Radius / resize);
                        Debug.Log("SigmaLog: 11");
                        pqs.repositionRadiusOffset = fromRadius * resize * landscape + body.Radius;
                        Debug.Log("SigmaLog: 12");
                    }
                    Debug.Log("SigmaLog: 13");
                    if (!pqs.repositionToSphereSurface && !pqs.repositionToSphereSurfaceAddHeight)
                    {
                        Debug.Log("SigmaLog: 14");
                        // Offset = Distance from the radius of the planet

                        pqs.repositionRadiusOffset *= resize * landscape;
                        Debug.Log("SigmaLog: 15");
                    }
                    Debug.Log("SigmaLog: 16");
                    if (pqs.repositionToSphereSurface && pqs.repositionToSphereSurfaceAddHeight && resizeBuildings != 1)
                    {
                        // Offset = Distance from the surface of the planet
                        Debug.Log("SigmaLog: 17");

                        pqs.repositionRadiusOffset *= resizeBuildings;
                        Debug.Log("SigmaLog: 18");
                    }
                    Debug.Log("SigmaLog: 19");
                    if (resizeBuildings != 1)
                    {
                        Debug.Log("SigmaLog: 20");
                        pqs.transform.localScale *= (float)resizeBuildings;
                        Debug.Log("SigmaLog: 21");
                    }
                    Debug.Log("SigmaLog: 22");
                }
                Debug.Log("SigmaLog: 23");
            }
            Debug.Log("SigmaLog: 24");
        }
    }
}
