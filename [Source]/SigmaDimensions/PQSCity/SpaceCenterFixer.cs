using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
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
