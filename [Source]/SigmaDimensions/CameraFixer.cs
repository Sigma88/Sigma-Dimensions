using UnityEngine;
using Kopernicus;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Reflection;
using System.Linq;
using KSP.UI.Screens;
using KSP.UI;
using Kopernicus.Components;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class CameraFixer : MonoBehaviour
    {
        void Awake()
        {
            foreach (SpaceCenterCamera2 camera in Resources.FindObjectsOfTypeAll<SpaceCenterCamera2>())
            {
                camera.altitudeInitial *= (float)FlightGlobals.GetHomeBody().Get<double>("resizeBuildings");
                camera.zoomInitial *= (float)FlightGlobals.GetHomeBody().Get<double>("resizeBuildings");
                camera.zoomMax *= (float)FlightGlobals.GetHomeBody().Get<double>("resizeBuildings");
                camera.zoomMin *= (float)FlightGlobals.GetHomeBody().Get<double>("resizeBuildings");
                camera.zoomSpeed *= (float)FlightGlobals.GetHomeBody().Get<double>("resizeBuildings");
            }
        }
    }
}
