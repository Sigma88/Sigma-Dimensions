using System.Linq;
using UnityEngine;
using Kopernicus;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class Compatibility : MonoBehaviour
    {
        private void Awake()
        {
            if (AssemblyLoader.loadedAssemblies.FirstOrDefault(a => a.name == "scatterer") == null)
                DestroyImmediate(this);
            else
                DontDestroyOnLoad(this);
        }

        public void ModuleManagerPostLoad()
        {
            NumericCollectionParser<double> flareSettings = new NumericCollectionParser<double>();
            NumericCollectionParser<double> spikesSettings = new NumericCollectionParser<double>();

            double Rescale = 1;
            double customRescale = 1;

            if (!double.TryParse(GameDatabase.Instance.GetConfigNode("Sigma/Dimensions/Settings/SigmaDimensions")?.GetValue("Rescale"), out Rescale))
            {
                Rescale = 1;
            }

            foreach (ConfigNode node in GameDatabase.Instance.GetConfigNodes("Scatterer_sunflare"))
            {
                foreach (ConfigNode star in node.GetNodes())
                {
                    // Load customRescale
                    if (!double.TryParse(star.GetValue("customRescale"), out customRescale))
                    {
                        customRescale = 1;
                    }

                    // Load Scatterer Settings
                    double sunGlareFadeDistance = 0;
                    double.TryParse(star.GetValue("sunGlareFadeDistance"), out sunGlareFadeDistance);

                    if (star.HasValue("flareSettings"))
                        flareSettings.SetFromString(star.GetValue("flareSettings").Replace(",", " "));
                    if (star.HasValue("spikesSettings"))
                        spikesSettings.SetFromString(star.GetValue("spikesSettings").Replace(",", " "));

                    // Rescale Scatterer Settings
                    if (customRescale != 1)
                    {
                        sunGlareFadeDistance *= customRescale;
                        if (flareSettings?.value?.Count > 2)
                            flareSettings.value[2] = flareSettings.value[2] / customRescale;
                        if (spikesSettings?.value?.Count > 2)
                            spikesSettings.value[2] = spikesSettings.value[2] / customRescale;
                    }
                    else
                    {
                        sunGlareFadeDistance *= Rescale;
                        if (flareSettings?.value?.Count > 2)
                            flareSettings.value[2] = flareSettings.value[2] / Rescale;
                        if (spikesSettings?.value?.Count > 2)
                            spikesSettings.value[2] = spikesSettings.value[2] / Rescale;
                    }

                    // Save Rescaled Scatterer Settings
                    if (flareSettings?.value?.Count > 2)
                    {
                        star.RemoveValues("flareSettings");
                        star.AddValue("flareSettings", string.Join(",", flareSettings.value.Select(p => p.ToString()).ToArray()));
                    }

                    if (spikesSettings?.value?.Count > 2)
                    {
                        star.RemoveValues("spikesSettings");
                        star.AddValue("spikesSettings", string.Join(",", spikesSettings.value.Select(p => p.ToString()).ToArray()));
                    }

                    if (sunGlareFadeDistance != 0)
                    {
                        star.RemoveValues("sunGlareFadeDistance");
                        star.AddValue("sunGlareFadeDistance", sunGlareFadeDistance);
                    }
                }
            }
            DestroyImmediate(this);
        }
    }
}
