using Kopernicus;
using Kopernicus.Configuration;


namespace SigmaDimensionsPlugin
{
    [ExternalParserTarget("SigmaDimensions")]
    public class SigmaDimensionsLoader : ExternalParserTargetLoader, IParserEventSubscriber
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

        void IParserEventSubscriber.Apply(ConfigNode node)
        {
        }

        void IParserEventSubscriber.PostApply(ConfigNode node)
        {
        }
    }
}
