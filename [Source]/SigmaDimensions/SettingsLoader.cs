using Kopernicus;
using Kopernicus.Configuration;


namespace SigmaDimensionsPlugin
{
    [ParserTargetExternal("Body", "SigmaDimensions", "Kopernicus")]
    class SettingsLoader : BaseLoader, IParserEventSubscriber
    {
        [ParserTarget("Resize", Optional = true)]
        NumericParser<double> resize
        {
            set
            {
                generatedBody.celestialBody.Set("resize", value.Value);
            }
        }

        [ParserTarget("landscape", Optional = true)]
        NumericParser<double> landscape
        {
            set
            {
                generatedBody.celestialBody.Set("landscape", value.Value);
            }
        }

        [ParserTarget("resizeBuildings", Optional = true)]
        NumericParser<double> resizeBuildings
        {
            set
            {
                generatedBody.celestialBody.Set("resizeBuildings", value.Value);
            }
        }

        [ParserTarget("atmoTopLayer", Optional = true)]
        NumericParser<double> atmoTopLayer
        {
            set
            {
                if (value.Value != 1)
                    generatedBody.celestialBody.Set("atmoTopLayer", value.Value);
            }
        }

        [ParserTarget("debug", Optional = true)]
        NumericParser<bool> debug
        {
            set { Debug.debug = value?.Value == true ? true : Debug.debug; }
        }

        void IParserEventSubscriber.Apply(ConfigNode node)
        {
        }

        void IParserEventSubscriber.PostApply(ConfigNode node)
        {
        }
    }
}
