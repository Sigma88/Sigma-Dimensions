using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Kopernicus;
using Kopernicus.Configuration;


namespace SigmaDimensionsPlugin
{
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class AtmosphereTopLayer : MonoBehaviour
    {
        Ktype curve = Ktype.Exponential;

        void Start()
        {
            foreach (CelestialBody body in FlightGlobals.Bodies.FindAll(b => b.atmosphere && b.Has("atmoTopLayer")))
            {
                // Debug
                debug = body.Has("debug") ? body.Get<bool>("debug") : false;
                if (debug) PrintCurve(body, "Original Curves");

                Normalize(body, body.atmosphereDepth);

                double topLayer = body.Get<double>("atmoTopLayer") * body.atmosphereDepth;
                FixPressure(body.atmospherePressureCurve, topLayer);
                QuickFix(body.atmosphereTemperatureCurve, topLayer);
                QuickFix(body.atmosphereTemperatureSunMultCurve, topLayer);
                FixMaxAltitude(body, topLayer);
                
                Normalize(body, 1 / body.atmosphereDepth);

                // Debug
                if (debug) PrintCurve(body, "Final Curves");
            }
        }

        void FixPressure(FloatCurve curve, double topLayer)
        {
            List<double[]> list = ReadCurve(curve); /* Avoid Bad Curves ==> */ if (list.Count < 2) { Debug.Log("SigmaDimensionsLog: This pressure curve has " + (list.Count == 0 ? "no keys" : "just one key") + ". I don't know what you expect me to do with that."); return; }

            double maxAltitude = list.Last()[0];

            bool smoothEnd = list.Last()[1] == 0 && list.Count > 2;
            double smoothRange = list.Last()[0] - list[list.Count - 2][0];
            if (smoothEnd) list.RemoveAt(list.Count - 1);

            if (topLayer > maxAltitude)
            {
                Extend(list, topLayer);
                maxAltitude = list.Last()[0];
            }

            if (topLayer < maxAltitude)
            {
                Trim(list, topLayer);
            }

            if (smoothEnd)
            {
                Smooth(list, smoothRange);
            }

            curve.Load(WriteCurve(list));
        }

        void QuickFix(FloatCurve curve, double topLayer)
        {
            NumericCollectionParser<double> key = new NumericCollectionParser<double>();
            double maxAltitude = curve.maxTime;

            if (topLayer > maxAltitude)
            {
                List<double[]> list = ReadCurve(curve); /* Avoid Bad Curves ==> */ if (list.Count == 0) { return; }
                list.Last()[3] = 0;
                list.Add(new double[] { list.Last()[0], list.Last()[1], 0, 0 });
                curve.Load(WriteCurve(list));
            }
        }

        void FixMaxAltitude(CelestialBody body, double topLayer)
        {
            body.atmosphereDepth = topLayer;
        }

        // Editors

        void Extend(List<double[]> list, double topLayer)
        {
            double newAltitude = list.Last()[0];
            double dX = list.Last()[0] - list[list.Count - 2][0];
            double[] K = getK(list);

            for (int i = 0; newAltitude < topLayer; i++)
            {
                newAltitude += dX;
                double newPressure = getY(newAltitude, list.Last(), K);
                double[] newKey = { newAltitude, newPressure };

                if (newKey[1] < 0)
                {
                    if (list.Last()[1] == 0)
                        break;
                    else
                        newKey[1] = 0;
                }

                list.Add(newKey);
            }

            // Debug
            if (debug) PrintCurve(list, "Extend");
        }

        void Trim(List<double[]> list, double topLayer)
        {
            FloatCurve curve = new FloatCurve();
            curve.Load(WriteCurve(list));

            double[] lastKey = { topLayer, curve.Evaluate((float)topLayer) };

            for (int i = list.Count; i > 0; i--)
            {
                if (list[i - 1][0] >= topLayer)
                    list.RemoveAt(i - 1);
            }

            list.Add(lastKey);

            // Debug
            if (debug) PrintCurve(list, "Trim");
        }

        void Smooth(List<double[]> list, double smoothRange)
        {
            FloatCurve curve = new FloatCurve();
            curve.Load(WriteCurve(list));
            double topLayer = curve.maxTime;
            double smoothStart = topLayer - smoothRange;

            double[] newKey = { smoothStart, curve.Evaluate((float)smoothStart) };
            double[] lastKey = { topLayer, 0, 0, 0 };

            for (int i = list.Count; i > 0; i--)
            {
                if (list[i - 1][0] >= smoothStart)
                    list.RemoveAt(i - 1);
            }

            list.Add(newKey);
            list.Add(lastKey);

            // Debug
            if (debug) PrintCurve(list, "Smooth");
        }

        // Normalizers

        void Normalize(CelestialBody body, double altitude)
        {
            if (body.atmospherePressureCurveIsNormalized)
                Multiply(body.atmospherePressureCurve, altitude);

            if (body.atmosphereTemperatureCurveIsNormalized)
                Multiply(body.atmosphereTemperatureCurve, altitude);
        }

        void Multiply(FloatCurve curve, double multiplier)
        {
            List<double[]> list = new List<double[]>();
            list = ReadCurve(curve);
            foreach (double[] key in list)
            {
                key[0] *= multiplier;
                key[2] /= multiplier;
                key[3] /= multiplier;
            }
            curve.Load(WriteCurve(list));
        }

        // Readers

        List<double[]> ReadCurve(FloatCurve curve)
        {
            ConfigNode config = new ConfigNode();
            List<double[]> list = new List<double[]>();
            NumericCollectionParser<double> value = new NumericCollectionParser<double>();

            curve.Save(config);

            foreach (string k in config.GetValues("key"))
            {
                value.SetFromString(k);
                list.Add(value.value.ToArray());
            }

            return list;
        }

        ConfigNode WriteCurve(List<double[]> list)
        {
            ConfigNode config = new ConfigNode();

            foreach (double[] values in list)
            {
                string key = "";
                foreach (double value in values)
                {
                    key += value + " ";
                }
                config.AddValue("key", key);
            }

            return config;
        }

        // Values

        double[] getK(List<double[]> list)
        {
            double[] K = { };
            if (list.Count == 2)
            {
                // Polynomial Curve:    dY = dX * ( K0 * (X0 + X1) + K1 )
                K = new double[] { 0, (list[1][1]-list[0][1]) / (list[1][0] - list[0][0]) };
                curve = Ktype.Polynomial;
                return K;
            }
            double[] dY = { list[list.Count - 2][1] - list[list.Count - 3][1], list[list.Count - 1][1] - list[list.Count - 2][1] };
            double[] dX = { list[list.Count - 2][0] - list[list.Count - 3][0], list[list.Count - 1][0] - list[list.Count - 2][0] };
            double curvature = (dY[1] / dX[1] - dY[0] / dX[0]) / ((dX[0] + dX[1]) / 2);

            if (curvature > 0)
            {
                // Exponential Curve:   Y1/Y0 = EXP(dX * K);
                K = new double[] { Math.Log(list.Last()[1] / list[list.Count - 2][1]) / dX[1] };
                curve = Ktype.Exponential;
            }
            else if (curvature < 0 && dY[1] >= 0)
            {
                // Logarithmic Curve:   dY = K * LN(X1/X0);
                K = new double[] { dY[1] / Math.Log(list.Last()[0] / list[list.Count - 2][0]) };
                curve = Ktype.Logarithmic;
            }
            else
            {
                // Polynomial Curve:    dY = dX * ( K0 * (X0 + X1) + K1 )
                K = new double[] { curvature / 2, dY[1] / dX[1] - (list.Last()[0] + list[list.Count - 2][0]) * curvature / 2 };
                curve = Ktype.Polynomial;
            }

            return K;
        }

        double getY(double X, double[] prevKey, double[] K)
        {
            double dX = X - prevKey[0];

            if (curve == Ktype.Exponential)
            {
                // Exponential Curve:   Y1/Y0 = EXP(dX * K);
                return prevKey[1] * Math.Exp(dX * K[0]);
            }
            else if (curve == Ktype.Logarithmic)
            {
                // Logarithmic Curve:   dY = K * LN(X1/X0);
                return K[0] * Math.Log(X / prevKey[0]) + prevKey[1];
            }
            else
            {
                // Polynomial Curve:    dY = dX * ( K0 * (X0 + X1) + K1 )
                return dX * (K[0] * (X + prevKey[0]) + K[1]) + prevKey[1];
            }
        }

        enum Ktype
        {
            Exponential,
            Logarithmic,
            Polynomial
        }

        // DEBUG

        bool debug = false;

        void PrintCurve(CelestialBody body, string name)
        {
            Debug.Log("SigmaDimensionsLog: " + name + " for body " + body.name);
            PrintCurve(body.atmospherePressureCurve, "pressureCurve");
            PrintCurve(body.atmosphereTemperatureCurve, "temperatureCurve");
            PrintCurve(body.atmosphereTemperatureSunMultCurve, "temperatureSunMultCurve");
        }

        void PrintCurve(List<double[]> list, string name)
        {
            PrintCurve(WriteCurve(list), name);
        }

        void PrintCurve(ConfigNode config, string name)
        {
            FloatCurve curve = new FloatCurve();
            curve.Load(config);
            PrintCurve(curve, name);
        }

        void PrintCurve(FloatCurve curve, string name)
        {
            ConfigNode config = new ConfigNode();
            curve.Save(config);
            Debug.Log("SigmaDimensionsLog: " + name);
            foreach (string key in config.GetValues("key"))
            {
                Debug.Log("SigmaDimensionsLog: key = " + key);
            }
        }
    }
}
