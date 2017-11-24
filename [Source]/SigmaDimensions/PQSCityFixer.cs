using System;
using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
    public class PQSCityFixer : MonoBehaviour
    {
        void Start()
        {
            TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, FixAltitude);
        }

        void FixAltitude()
        {
            CelestialBody body = FlightGlobals.currentMainBody;
            if (body == null) return;

            PQS pqs = body.pqsController;
            if (pqs == null) return;

            PQSCity city = GetComponent<PQSCity>();
            if (city == null) return;

            // Sigma Dimensions Settings
            double resize = body.Has("resize") ? body.Get<double>("resize") : 1;
            double landscape = body.Has("landscape") ? body.Get<double>("landscape") : 1;
            double resizeBuildings = body.Has("resizeBuildings") ? body.Get<double>("resizeBuildings") : 1;

            // Max distance
            double maxDistance = Math.Abs(2 * pqs.mapMaxHeight);
            maxDistance *= resize * landscape > 1 ? resize * landscape : 1;
            maxDistance += body.Radius;

            // Location
            Vector3 location = (city.transform.position - body.position).normalized; // From body to city


            RaycastHit[] hits = Physics.RaycastAll(body.position + location * (float)maxDistance, -location, (float)maxDistance, LayerMask.GetMask("Local Scenery"));

            for (int i = 0; i < hits?.Length; i++)
            {
                if (hits[i].collider?.GetComponent<PQ>())
                {
                    // Update only once
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, FixAltitude);
                    Debug.Log("PQSCityFixer", " > Planet: " + body.transform.name);
                    Debug.Log("PQSCityFixer", "     > PQSCity: " + city);


                    // PQSCity parameters
                    double groundLevel = maxDistance - body.Radius - hits[i].distance;
                    groundLevel = body.ocean && groundLevel < 0 ? 0d : groundLevel;

                    Debug.Log("PQSCityFixer", "         > Ground Level at Mod = " + groundLevel);

                    // Fix Altitude
                    if (city.repositionToSphere && !city.repositionToSphereSurface)
                    {
                        // Offset = Distance from the radius of the planet

                        double builtInOffset = city.repositionRadiusOffset - groundLevel / (resize * landscape);

                        city.repositionRadiusOffset = groundLevel + builtInOffset * resizeBuildings;

                        Debug.Log("PQSCityFixer", "         > PQSCity Fixed Radius Offset = " + city.repositionRadiusOffset);
                    }
                    else
                    {
                        // Offset = Distance from the surface of the planet
                        if (!city.repositionToSphereSurface)
                        {
                            city.repositionToSphereSurface = true;
                            city.repositionRadiusOffset = 0;
                        }
                        if (!city.repositionToSphereSurfaceAddHeight)
                        {
                            city.repositionToSphereSurfaceAddHeight = true;
                            city.repositionRadiusOffset = 0;
                        }

                        double error = pqs.GetSurfaceHeight(city.PlanetRelativePosition) - body.Radius - groundLevel;

                        double builtInOffset = city.repositionRadiusOffset + error / (resize * landscape);

                        city.repositionRadiusOffset = builtInOffset * resizeBuildings - error;

                        Debug.Log("PQSCityFixer", "         > PQSCity Fixed Surface Offset = " + city.repositionRadiusOffset);
                    }

                    city.Orientate();
                    DestroyImmediate(this);
                }
            }
        }
    }

    public class PQSCity2Fixer : MonoBehaviour
    {
        void Start()
        {
            TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, FixAltitude);
        }

        void FixAltitude()
        {
            CelestialBody body = FlightGlobals.currentMainBody;
            if (body == null) return;

            PQS pqs = body.pqsController;
            if (pqs == null) return;

            PQSCity2 city = GetComponent<PQSCity2>();
            if (city == null) return;

            // Sigma Dimensions Settings
            double resize = body.Has("resize") ? body.Get<double>("resize") : 1;
            double landscape = body.Has("landscape") ? body.Get<double>("landscape") : 1;
            double resizeBuildings = body.Has("resizeBuildings") ? body.Get<double>("resizeBuildings") : 1;

            // Max distance
            double maxDistance = Math.Abs(2 * pqs.mapMaxHeight);
            maxDistance *= resize * landscape > 1 ? resize * landscape : 1;
            maxDistance += body.Radius;

            // Location
            Vector3 location = (city.transform.position - body.position).normalized; // From body to city


            RaycastHit[] hits = Physics.RaycastAll(body.position + location * (float)maxDistance, -location, (float)maxDistance, LayerMask.GetMask("Local Scenery"));

            for (int i = 0; i < hits?.Length; i++)
            {
                if (hits[i].collider?.GetComponent<PQ>())
                {
                    // Update only once
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, FixAltitude);
                    Debug.Log("PQSCity2Fixer", " > Planet: " + body.transform.name);
                    Debug.Log("PQSCity2Fixer", "     > PQSCity2: " + city);


                    // PQSCity parameters
                    double groundLevel = maxDistance - body.Radius - hits[i].distance;
                    groundLevel = body.ocean && groundLevel < 0 ? 0d : groundLevel;

                    Debug.Log("PQSCity2Fixer", "         > Ground Level at Mod = " + groundLevel);

                    // Fix Altitude
                    if (!city.snapToSurface)
                    {
                        // Offset = Distance from the radius of the planet

                        double builtInOffset = city.alt - groundLevel / (resize * landscape);

                        city.alt = groundLevel + builtInOffset * resizeBuildings;

                        Debug.Log("PQSCity2Fixer", "         > PQSCity2 Fixed Alt = " + city.alt);
                    }
                    else
                    {
                        // Offset = Distance from the surface of the planet

                        double error = pqs.GetSurfaceHeight(city.PlanetRelativePosition) - body.Radius - groundLevel;

                        double builtInOffset = city.snapHeightOffset + error / (resize * landscape);

                        city.snapHeightOffset = builtInOffset * resizeBuildings - error;

                        Debug.Log("PQSCity2Fixer", "         > PQSCity2 Fixed Offset = " + city.snapHeightOffset);
                    }

                    city.Orientate();
                    DestroyImmediate(this);
                }
            }
        }
    }
}
