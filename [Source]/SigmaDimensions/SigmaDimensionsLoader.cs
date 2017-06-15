using Kopernicus;
using Kopernicus.Configuration;


namespace SigmaDimensionsPlugin
{
    [ParserTargetExternal("Body", "SigmaDimensions", "Kopernicus")]
    public class SigmaDimensionsLoader : BaseLoader, IParserEventSubscriber
    {
        [ParserTarget("Resize", optional = true)]
        public NumericParser<double> resize
        {
            set
            {
                generatedBody.celestialBody.Set("resize", value.value);
            }
        }

        [ParserTarget("landscape", optional = true)]
        public NumericParser<double> landscape
        {
            set
            {
                generatedBody.celestialBody.Set("landscape", value.value);
            }
        }

        [ParserTarget("resizeBuildings", optional = true)]
        public NumericParser<double> resizeBuildings
        {
            set
            {
                generatedBody.celestialBody.Set("resizeBuildings", value.value);
            }
        }

        [ParserTarget("atmoTopLayer", optional = true)]
        public NumericParser<double> atmoTopLayer
        {
            set
            {
                if (value.value != 1)
                    generatedBody.celestialBody.Set("atmoTopLayer", value.value);
            }
        }

        [ParserTarget("debug", optional = true)]
        public NumericParser<bool> debug
        {
            set
            {
                generatedBody.celestialBody.Set("debug", value.value);
            }
        }

        void IParserEventSubscriber.Apply(ConfigNode node)
        {
        }

        void IParserEventSubscriber.PostApply(ConfigNode node)
        {
        }
    }
}
