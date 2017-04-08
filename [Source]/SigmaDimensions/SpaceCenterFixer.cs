using System;
using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.FlightAndKSC, false)]
    public class SpaceCenterFixer : MonoBehaviour
    {
        void Start()
        {
            if (!HighLogic.LoadedSceneIsFlight)
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


        public bool fixLight = true;
        public Light light = null;

        void Update()
        {
            if (fixLight)
            {
                if (light == null)
                {
                    light = Array.Find((Light[])FindObjectsOfType(typeof(Light)), o => o.name == "Spotlight" && o.tag == "KSC_Pad_Water_Tower");
                }
                else
                {
                    if (FlightGlobals.GetHomeBody().Has("resizeBuildings"))
                        light.range *= (float)(FlightGlobals.GetHomeBody().Get<double>("resizeBuildings"));

                    fixLight = false;
                }
            }
        }
    }
}
