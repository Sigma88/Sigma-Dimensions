using System;
using UnityEngine;
using Kopernicus;
using Kopernicus.Configuration.ModLoader;


namespace PQSMod_SigmaDimensions
{
    public class PQSMod_SigmaDimensions : PQSMod
    {
        public double Resize = 1;
        public double Rescale = 1;
        public float Atmosphere = 1;
        public double dayLengthMultiplier = 1;
        public double landscape = 1;
        public double geeASLmultiplier = 1;
        public float changeScatterSize = 1;
        public float changeScatterDensity = 1;
        public double resizeBuildings = 1;
        public double groundTiling = 1;
        public double CustomSoISize = 1;
        public double CustomRingSize = 1;
        public double atmoASL = 1;
        public double tempASL = 1;
        public float atmoTopLayer = 1;
        public double atmoVisualEffect = 1;
        public double scanAltitude = 1;

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
    }
}
