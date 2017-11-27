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

            // Location
            Vector3 planet = body.transform.position;
            Vector3 building = city.transform.position; // From body to city
            Vector3 location = (building - planet).normalized;

            // Sigma Dimensions Settings
            double resize = body.Has("resize") ? body.Get<double>("resize") : 1;
            double landscape = body.Has("landscape") ? body.Get<double>("landscape") : 1;
            double resizeBuildings = body.Has("resizeBuildings") ? body.Get<double>("resizeBuildings") : 1;

            // Max distance
            double maxDistance = Math.Abs(2 * pqs.mapMaxHeight);
            maxDistance *= resize * landscape > 1 ? resize * landscape : 1;
            maxDistance += body.Radius;


            RaycastHit[] hits = Physics.RaycastAll(planet + location * (float)maxDistance, -location, (float)maxDistance, LayerMask.GetMask("Local Scenery"));

            for (int i = 0; i < hits?.Length; i++)
            {
                if (hits[i].collider?.GetComponent<PQ>())
                {
                    // Update only once
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, FixAltitude);
                    Debug.Log("PQSCityFixer", "> Planet: " + body.transform.name);
                    Debug.Log("PQSCityFixer", "    > PQSCity: " + city);

                    // PQSCity parameters
                    double groundLevel = (hits[i].point - planet).magnitude - body.Radius;
                    double error = pqs.GetSurfaceHeight(city.repositionRadial) - body.Radius - groundLevel;
                    double oceanDepth = body.ocean && groundLevel < 0 ? -groundLevel : 0d;
                    groundLevel = body.ocean && groundLevel < 0 ? 0d : groundLevel;

                    Debug.Log("PQSCityFixer", "        > Ground Level at Mod = " + groundLevel);
                    Debug.Log("PQSCityFixer", "        > Ocean Depth at Mod = " + groundLevel);
                    Debug.Log("PQSCityFixer", "        > Ground Level Error at Mod = " + groundLevel);

                    // Fix Altitude
                    if (city.repositionToSphere && !city.repositionToSphereSurface)
                    {
                        // Offset = Distance from the radius of the planet

                        Debug.Log("PQSCityFixer", "        > PQSCity Original Radius Offset = " + city.repositionRadiusOffset);

                        double builtInOffset = city.repositionRadiusOffset - groundLevel / (resize * landscape);

                        Debug.Log("PQSCityFixer", "        > Builtuin Offset = " + builtInOffset);

                        city.repositionRadiusOffset = groundLevel + error / (resize * landscape) - (groundLevel + error - city.repositionRadiusOffset) / resizeBuildings;

                        Debug.Log("PQSCityFixer", "        > PQSCity Fixed Radius Offset = " + city.repositionRadiusOffset);
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

                        Debug.Log("PQSCityFixer", "        > PQSCity Original Surface Offset = " + city.repositionRadiusOffset);

                        city.repositionRadiusOffset = oceanDepth + error / (resize * landscape) - (oceanDepth + error - city.repositionRadiusOffset) / resizeBuildings;

                        Debug.Log("PQSCityFixer", "        > PQSCity Fixed Surface Offset = " + city.repositionRadiusOffset);
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

            // Location
            Vector3 planet = body.transform.position;
            Vector3 building = city.transform.position; // From body to city
            Vector3 location = (building - planet).normalized;

            // Sigma Dimensions Settings
            double resize = body.Has("resize") ? body.Get<double>("resize") : 1;
            double landscape = body.Has("landscape") ? body.Get<double>("landscape") : 1;
            double resizeBuildings = body.Has("resizeBuildings") ? body.Get<double>("resizeBuildings") : 1;

            // Max distance
            double maxDistance = Math.Abs(2 * pqs.mapMaxHeight);
            maxDistance *= resize * landscape > 1 ? resize * landscape : 1;
            maxDistance += body.Radius;


            RaycastHit[] hits = Physics.RaycastAll(planet + location * (float)maxDistance, -location, (float)maxDistance, LayerMask.GetMask("Local Scenery"));

            for (int i = 0; i < hits?.Length; i++)
            {
                if (hits[i].collider?.GetComponent<PQ>())
                {
                    // Update only once
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, FixAltitude);
                    Debug.Log("PQSCity2Fixer", "> Planet: " + body.transform.name);
                    Debug.Log("PQSCity2Fixer", "    > PQSCity2: " + city);


                    // PQSCity2 parameters
                    double groundLevel = (hits[i].point - planet).magnitude - body.Radius;
                    double error = pqs.GetSurfaceHeight(city.PlanetRelativePosition) - body.Radius - groundLevel;
                    double oceanDepth = body.ocean && groundLevel < 0 ? -groundLevel : 0d;
                    groundLevel = body.ocean && groundLevel < 0 ? 0d : groundLevel;

                    Debug.Log("PQSCity2Fixer", "        > Ground Level at Mod = " + groundLevel);
                    Debug.Log("PQSCity2Fixer", "        > Ocean Depth at Mod = " + groundLevel);
                    Debug.Log("PQSCity2Fixer", "        > Ground Level Error at Mod = " + groundLevel);

                    // Fix Altitude
                    if (!city.snapToSurface)
                    {
                        // Offset = Distance from the radius of the planet

                        Debug.Log("PQSCity2Fixer", "        > PQSCity2 Original Radius Offset = " + city.alt);

                        double builtInOffset = city.alt - groundLevel / (resize * landscape);

                        Debug.Log("PQSCity2Fixer", "        > Builtuin Offset = " + builtInOffset);

                        city.alt = groundLevel + error / (resize * landscape) - (groundLevel + error - city.alt) / resizeBuildings;

                        Debug.Log("PQSCity2Fixer", "        > PQSCity2 Fixed Radius Offset = " + city.alt);
                    }
                    else
                    {
                        Debug.Log("PQSCity2Fixer", "        > PQSCity2 Original Surface Offset = " + city.snapHeightOffset);

                        city.snapHeightOffset = oceanDepth + error / (resize * landscape) - (oceanDepth + error - city.snapHeightOffset) / resizeBuildings;

                        Debug.Log("PQSCity2Fixer", "        > PQSCity2 Fixed Surface Offset = " + city.snapHeightOffset);
                    }

                    city.Orientate();
                    DestroyImmediate(this);
                }
            }
        }
    }
}
