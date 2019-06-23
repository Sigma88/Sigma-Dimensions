using Kopernicus.ConfigParser.Attributes;
using Kopernicus.ConfigParser.BuiltinTypeParsers;
using Kopernicus.ConfigParser.Enumerations;
using Kopernicus.ConfigParser.Interfaces;
using Kopernicus.Configuration.ModLoader;


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
        [ParserTarget("Resize", Optional = true)]
        private NumericParser<double> Resize
        {
            get { return Mod.Resize; }
            set { Mod.Resize = value; }
        }

        // Atmosphere
        [ParserTarget("Atmosphere", Optional = true)]
        private NumericParser<float> Atmosphere
        {
            get { return Mod.Atmosphere; }
            set { Mod.Atmosphere = value; }
        }

        // landscape
        [ParserTarget("landscape", Optional = true)]
        private NumericParser<double> landscape
        {
            get { return Mod.landscape; }
            set { Mod.landscape = value; }
        }

        // changeScatterSize
        [ParserTarget("changeScatterSize", Optional = true)]
        private NumericParser<float> changeScatterSize
        {
            get { return Mod.changeScatterSize; }
            set { Mod.changeScatterSize = value; }
        }

        // changeScatterDensity
        [ParserTarget("changeScatterDensity", Optional = true)]
        private NumericParser<float> changeScatterDensity
        {
            get { return Mod.changeScatterDensity; }
            set { Mod.changeScatterDensity = value; }
        }

        // Resize
        [ParserTarget("resizeBuildings", Optional = true)]
        private NumericParser<double> resizeBuildings
        {
            get { return Mod.resizeBuildings; }
            set { Mod.resizeBuildings = value; }
        }

        // groundTiling
        [ParserTarget("groundTiling", Optional = true)]
        private NumericParser<double> groundTiling
        {
            get { return Mod.groundTiling; }
            set { Mod.groundTiling = value; }
        }

        // atmoTopLayer
        [ParserTarget("atmoTopLayer", Optional = true)]
        private NumericParser<float> atmoTopLayer
        {
            get { return Mod.atmoTopLayer; }
            set { Mod.atmoTopLayer = value; }
        }

        void IParserEventSubscriber.Apply(ConfigNode node)
        {
        }

        void IParserEventSubscriber.PostApply(ConfigNode node)
        {
            // Always Load Last
            Mod.order = int.MaxValue;
        }
    }
}
