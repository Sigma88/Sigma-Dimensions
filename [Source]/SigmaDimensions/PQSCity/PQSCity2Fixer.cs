using System;
using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
    public class PQSCity2Fixer : MonoBehaviour
    {
        double time = 0;

        void Update()
        {
            if (time < 0.2)
            {
                time += Time.deltaTime;
            }
            else
            {
                time = 0;

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
                        Debug.Log("PQSCity2Fixer", "> Planet: " + body.transform.name);
                        Debug.Log("PQSCity2Fixer", "    > PQSCity2: " + city);

                        // PQSCity2 parameters
                        double oldGroundLevel = pqs.GetSurfaceHeight(city.PlanetRelativePosition) - body.Radius;
                        Debug.Log("PQSCity2Fixer", "        > Old Ground Level at Mod (GETSURFACE) = " + oldGroundLevel);
                        double oldOceanOffset = body.ocean && oldGroundLevel < 0 ? oldGroundLevel : 0d;
                        Debug.Log("PQSCity2Fixer", "        > Old Ocean Offset at Mod = " + oldOceanOffset);
                        oldGroundLevel = body.ocean && oldGroundLevel < 0 ? 0d : oldGroundLevel;
                        Debug.Log("PQSCity2Fixer", "        > Old Ground Level at Mod (WITH OCEAN) = " + oldGroundLevel);

                        double groundLevel = (hits[i].point - planet).magnitude - body.Radius;
                        Debug.Log("PQSCity2Fixer", "        > Ground Level at Mod (RAYCAST) = " + groundLevel);
                        double oceanOffset = body.ocean && groundLevel < 0 ? groundLevel : 0d;
                        Debug.Log("PQSCity2Fixer", "        > Ocean Offset at Mod = " + oceanOffset);
                        groundLevel = body.ocean && groundLevel < 0 ? 0d : groundLevel;
                        Debug.Log("PQSCity2Fixer", "        > Ground Level at Mod (NEW) = " + groundLevel);

                        // Because, SQUAD
                        city.PositioningPoint.localPosition /= (float)(body.Radius + city.alt);

                        // Fix Altitude
                        if (!city.snapToSurface)
                        {
                            // Alt = Distance from the radius of the planet

                            Debug.Log("PQSCity2Fixer", "        > PQSCity2 Original Radius Offset = " + city.alt);

                            double builtInOffset = (city.alt - oldGroundLevel) / resizeBuildings - (groundLevel - oldGroundLevel) / (resize * landscape);

                            Debug.Log("PQSCity2Fixer", "        > Builtin Offset = " + builtInOffset);

                            city.alt = groundLevel + builtInOffset * resizeBuildings;

                            Debug.Log("PQSCity2Fixer", "        > PQSCity2 Fixed Radius Offset = " + city.alt);
                        }
                        else
                        {
                            // Offset = Distance from the surface of the planet

                            Debug.Log("PQSCity2Fixer", "        > PQSCity2 Original Surface Offset = " + city.snapHeightOffset);

                            double builtInOffset = city.snapHeightOffset / resizeBuildings - (groundLevel + oceanOffset - oldGroundLevel - oldOceanOffset) / (resize * landscape);

                            Debug.Log("PQSCity2Fixer", "        > Builtin Offset = " + builtInOffset);

                            double newOffset = builtInOffset * resizeBuildings + groundLevel + oceanOffset - oldGroundLevel - oldOceanOffset;

                            Debug.Log("PQSCity2Fixer", "        > PQSCity2 Fixed Surface Offset = " + newOffset);

                            city.alt += newOffset - city.snapHeightOffset;
                            city.snapHeightOffset = newOffset;
                        }

                        // Because, SQUAD
                        city.PositioningPoint.localPosition *= (float)(body.Radius + city.alt);

                        // Apply Changes and Destroy
                        city.Orientate();
                        DestroyImmediate(this);
                    }
                }
            }
        }
    }
}
