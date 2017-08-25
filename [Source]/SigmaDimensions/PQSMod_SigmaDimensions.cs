using System;
using UnityEngine;
using Kopernicus;
using Kopernicus.Configuration.ModLoader;
using Kopernicus.MaterialWrapper;

namespace PQSMod_SigmaDimensions
{
    public class PQSMod_SigmaDimensions : PQSMod
    {
        public double Resize = 1;
        public float Atmosphere = 1;
        public double landscape = 1;
        public float changeScatterSize = 1;
        public float changeScatterDensity = 1;
        public double resizeBuildings = 1;
        public double groundTiling = 1;
        public float atmoTopLayer = 1;

        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            data.vertHeight = sphere.radius + (data.vertHeight - sphere.radius) * Resize * landscape;
        }
    }

    [RequireConfigType(ConfigType.Node)]
    public class SigmaDimensions : ModLoader<PQSMod_SigmaDimensions>, IParserEventSubscriber
    {
        // Resize
        [ParserTarget("Resize", optional = true)]
        private NumericParser<double> Resize
        {
            get { return mod.Resize; }
            set { mod.Resize = value; }
        }

        // Atmosphere
        [ParserTarget("Atmosphere", optional = true)]
        private NumericParser<float> Atmosphere
        {
            get { return mod.Atmosphere; }
            set { mod.Atmosphere = value; }
        }

        // landscape
        [ParserTarget("landscape", optional = true)]
        private NumericParser<double> landscape
        {
            get { return mod.landscape; }
            set { mod.landscape = value; }
        }

        // changeScatterSize
        [ParserTarget("changeScatterSize", optional = true)]
        private NumericParser<float> changeScatterSize
        {
            get { return mod.changeScatterSize; }
            set { mod.changeScatterSize = value; }
        }

        // changeScatterDensity
        [ParserTarget("changeScatterDensity", optional = true)]
        private NumericParser<float> changeScatterDensity
        {
            get { return mod.changeScatterDensity; }
            set { mod.changeScatterDensity = value; }
        }

        // Resize
        [ParserTarget("resizeBuildings", optional = true)]
        private NumericParser<double> resizeBuildings
        {
            get { return mod.resizeBuildings; }
            set { mod.resizeBuildings = value; }
        }

        // groundTiling
        [ParserTarget("groundTiling", optional = true)]
        private NumericParser<double> groundTiling
        {
            get { return mod.groundTiling; }
            set { mod.groundTiling = value; }
        }

        // atmoTopLayer
        [ParserTarget("atmoTopLayer", optional = true)]
        private NumericParser<float> atmoTopLayer
        {
            get { return mod.atmoTopLayer; }
            set { mod.atmoTopLayer = value; }
        }

        void IParserEventSubscriber.Apply(ConfigNode node)
        {
        }

        void IParserEventSubscriber.PostApply(ConfigNode node)
        {
            // Always Load Last
            mod.order = int.MaxValue;

            // PQS MATERIALS
            string[] textures = new[] { "_groundTexStart", "_groundTexEnd", "_steepTexStart", "_steepTexEnd" };
            string[] tilings = new[] { "_texTiling", "_steepNearTiling", "_steepTiling", "_lowNearTiling", "_lowBumpNearTiling", "_lowBumpFarTiling", "_midNearTiling", "_midBumpNearTiling", "_midBumpFarTiling", "_highNearTiling", "_highBumpNearTiling", "_highBumpFarTiling" };

            Material surfaceMaterial = generatedBody.pqsVersion.surfaceMaterial;
            EditProperties(surfaceMaterial, textures, mod.Resize * mod.landscape);
            EditProperties(surfaceMaterial, tilings, mod.groundTiling);

            Material fallbackMaterial = generatedBody.pqsVersion.surfaceMaterial;
            EditProperties(fallbackMaterial, textures, mod.Resize * mod.landscape);
            EditProperties(fallbackMaterial, tilings, mod.groundTiling);

            // PQS MODS
            PQSMod[] modlist = generatedBody?.pqsVersion?.GetComponentsInChildren<PQSMod>();
            for (int i = 0; i < modlist.Length; i++)
            {
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

        // Material Edit
        public void EditProperties(Material material, string[] properties, double mult)
        {
            Debug.Log("SigmaLog: mult = " + mult);
            for (int i = 0; i < properties.Length; i++)
            {
                Debug.Log("SigmaLog: i = " + i);
                Debug.Log("SigmaLog: property = " + properties[i]);
                if (material.HasProperty(properties[i]))
                {
                    Debug.Log("SigmaLog: Float before = " + material.GetFloat(properties[i]));
                    material.SetFloat(properties[i], material.GetFloat(properties[i]) * (float)mult);
                    Debug.Log("SigmaLog: Float after = " + material.GetFloat(properties[i]));
                }
            }
        }
    }
}
