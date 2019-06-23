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
                            light.range *= resizeBuildings;
                        }
                    }
                }
            }
        }
    }
}
