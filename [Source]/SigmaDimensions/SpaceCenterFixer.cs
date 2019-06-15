using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    internal class LightsFixer : MonoBehaviour
    {
        void Start()
        {
            foreach (CelestialBody cb in FlightGlobals.Bodies)
            {
                if (cb?.pqsController != null)
                {
                    if (cb.Has("resizeBuildings"))
                    {
                        float resizeBuildings = (float)cb.Get<double>("resizeBuildings");

                        foreach (Light light in cb.pqsController.GetComponentsInChildren<Light>(true))
                        {
                            UnityEngine.Debug.Log("SigmaLog: LIGHT = " + light);
                            light.range *= resizeBuildings;
                        }
                    }
                }
            }
        }
    }

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    internal class SpaceCenterFixer : MonoBehaviour
    {
        void Start()
        {
            foreach (SpaceCenterCamera2 camera in Resources.FindObjectsOfTypeAll<SpaceCenterCamera2>())
            {
                float resizeBuildings = (float)FlightGlobals.GetHomeBody().Get<double>("resizeBuildings");

                camera.zoomInitial *= resizeBuildings;
                camera.zoomMax *= resizeBuildings;
                camera.zoomMin *= resizeBuildings;
                camera.zoomSpeed *= resizeBuildings;
            }
        }
    }
}
