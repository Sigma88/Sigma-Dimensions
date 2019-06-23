using System.Reflection;
using UnityEngine;
using Kopernicus;
using Kopernicus.Configuration;
using Debug = SigmaDimensionsPlugin.Debug;


namespace PQSMod_SigmaDimensions
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    internal class PQSModsFixer : MonoBehaviour
    {
        void Start()
        {
            Events.OnBodyPostApply.Add(FixPQS);
        }

        void FixPQS(Body body, ConfigNode node)
        {
            Debug.Log("PQSModsFixer.FixPQS", "Body = " + body.Name);

            // generatedBody
            PSystemBody generatedBody = body?.GeneratedBody;

            // PQSMod_SigmaDimensions
            PQSMod_SigmaDimensions mod = generatedBody?.pqsVersion?.gameObject?.GetComponentInChildren<PQS>(true)?.GetComponentInChildren<PQSMod_SigmaDimensions>(true);

            // PQS MODS LIST
            PQSMod[] modlist = generatedBody?.pqsVersion?.gameObject?.GetComponentInChildren<PQS>(true)?.GetComponentsInChildren<PQSMod>(true);

            if (generatedBody == null || mod == null || modlist == null) return;


            // PQS MATERIALS
            string[] textures = new[] { "_groundTexStart", "_groundTexEnd", "_steepTexStart", "_steepTexEnd" };
            string[] tilings = new[] { "_texTiling", "_steepNearTiling", "_steepTiling", "_lowNearTiling", "_lowBumpNearTiling", "_lowBumpFarTiling", "_midNearTiling", "_midBumpNearTiling", "_midBumpFarTiling", "_highNearTiling", "_highBumpNearTiling", "_highBumpFarTiling" };

            Material surfaceMaterial = generatedBody.pqsVersion.surfaceMaterial;
            EditProperties(surfaceMaterial, textures, mod.Resize * mod.landscape);
            EditProperties(surfaceMaterial, tilings, mod.groundTiling);

            Material fallbackMaterial = generatedBody.pqsVersion.surfaceMaterial;
            EditProperties(fallbackMaterial, textures, mod.Resize * mod.landscape);
            EditProperties(fallbackMaterial, tilings, mod.groundTiling);


            for (int i = 0; i < modlist?.Length; i++)
            {
                // Fix scaleDeformityByRadius
                ScaleByRadius(modlist[i], mod.Resize);

                // PQSLandControl
                if (modlist[i].GetType() == typeof(PQSLandControl))
                {
                    foreach (PQSLandControl.LandClassScatter scatter in ((PQSLandControl)modlist[i]).scatters)
                    {
                        scatter.maxScale *= mod.changeScatterSize;
                        scatter.minScale *= mod.changeScatterSize;
                        scatter.verticalOffset *= mod.changeScatterSize;
                        scatter.densityFactor *= mod.changeScatterDensity;
                    }
                }

                // AerialPerspectiveMaterial
                if (modlist[i].GetType() == typeof(PQSMod_AerialPerspectiveMaterial))
                {
                    ((PQSMod_AerialPerspectiveMaterial)modlist[i]).atmosphereDepth *= mod.Atmosphere * mod.atmoTopLayer;
                }

                // AltitudeAlpha
                if (modlist[i].GetType() == typeof(PQSMod_AltitudeAlpha))
                {
                    ((PQSMod_AltitudeAlpha)modlist[i]).atmosphereDepth *= mod.Resize;
                }

                // FlattenArea
                if (modlist[i].GetType() == typeof(PQSMod_FlattenArea))
                {
                    ((PQSMod_FlattenArea)modlist[i]).innerRadius *= mod.Resize;
                    ((PQSMod_FlattenArea)modlist[i]).outerRadius *= mod.Resize;
                }

                // PQSMod_MapDecalTangent
                if (modlist[i].GetType() == typeof(PQSMod_MapDecalTangent))
                {
                    double mult = mod.Resize;
                    if (generatedBody.celestialBody.isHomeWorld && modlist[i].name == "KSC")
                    {
                        mult /= mod.resizeBuildings;
                        mult = mult > 0.009 && mult < 0.75 ? 0.75 : mult;
                    }
                    ((PQSMod_MapDecalTangent)modlist[i]).radius *= mult * mod.resizeBuildings;
                }

                // PQSMod_MapDecal
                if (modlist[i].GetType() == typeof(PQSMod_MapDecal))
                {
                    ((PQSMod_MapDecal)modlist[i]).radius *= mod.Resize;
                }
            }
        }

        // Scale By Radius
        void ScaleByRadius(PQSMod mod, double Resize)
        {
            FieldInfo scaleByRadius = mod.GetType().GetField("scaleDeformityByRadius");

            if (scaleByRadius?.FieldType == typeof(bool) && (scaleByRadius?.GetValue(mod) as bool?) == true)
            {
                FieldInfo deformity = mod.GetType().GetField("heightMapDeformity");

                if (deformity == null)
                    deformity = mod.GetType().GetField("deformity");

                if (deformity?.FieldType == typeof(double))
                    deformity.SetValue(mod, (double)deformity.GetValue(mod) / Resize);
            }
        }

        // Material Edit
        void EditProperties(Material material, string[] properties, double mult)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                if (material.HasProperty(properties[i]))
                {
                    material.SetFloat(properties[i], material.GetFloat(properties[i]) * (float)mult);
                }
            }
        }
    }
}
