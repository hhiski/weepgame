using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSpace
{
    class ColorFunctions
    {

        public Color ThreeColorLerp(Color colorA, Color colorB, Color colorC, float lerpT)
        {
            Color colorMix;
            float t = lerpT;

            if (lerpT <= 0.5f)
            {
                colorMix = Color.Lerp(colorA,colorB, t * 2);
            }
            else
            {
                t = (lerpT - 0.5f) * 2f;
                colorMix = Color.Lerp(colorB, colorC, t);
            }

            return colorMix;
        }

        public Color HueShiftColor(Color color, float colorHueShift, float colorHueMultiply)
        {
            float hue;
            float saturation;
            float colorValue;

            Color.RGBToHSV(color, out hue, out saturation, out colorValue);
            colorHueShift %= 1;

            hue = hue += colorHueShift;
            hue = hue *= colorHueMultiply;

            if (hue > 1) { hue -= 1; };
            if (hue < 0) { hue += 1; };

            color = Color.HSVToRGB(hue, saturation, colorValue);

            return color;
        }

        public Color SaturationShiftColor(Color color, float colorSaturationShift, float colorSaturationMultiply)
        {
            float hue;
            float saturation;
            float colorValue;

            Color.RGBToHSV(color, out hue, out saturation, out colorValue);

            saturation += colorSaturationShift;
            saturation *= colorSaturationMultiply;

            if (saturation > 1) { saturation = 1; };
            if (saturation < 0) { saturation = 0; };

            color = Color.HSVToRGB(hue, saturation, colorValue);

            return color;
        }

        public Color ValueShiftColor(Color color, float colorValueShift, float colorValueMultiply)
        {
            float hue;
            float saturation;
            float colorValue;

            Color.RGBToHSV(color, out hue, out saturation, out colorValue);

            colorValue += colorValueShift;
            colorValue *= colorValueMultiply;

            if (colorValue > 1) { colorValue = 1; };
            if (colorValue < 0) { colorValue = 0; };

            color = Color.HSVToRGB(hue, saturation, colorValue);

            return color;
        }

        public Color LatitudeBasedColoring(Color inputColor, float noise, float latitude, float polarCoverage)
        {

            Color polarZone = SaturationShiftColor(inputColor, 0, 0.1f);
            polarZone = ValueShiftColor(polarZone, 55, 55);
            Color taigaZone = SaturationShiftColor(inputColor, 0, 0.6f);
            taigaZone = HueShiftColor(taigaZone, 0.06f, 1);
            Color tundraZone = SaturationShiftColor(inputColor, 0, 0.3f);
            tundraZone = ValueShiftColor(tundraZone, 0, 1.3f);


            Color rightColor = inputColor;

            float snowNoise = noise * latitude;
            //  float noise = (PerlinFilter(point, NoiseLayer, 3.0f, 1, 0.15f)); ;
            //float snowNoise = noise * point.y;

            float polarLevel = 1 - (polarCoverage); 
            float tundraLevel = polarLevel - 0.14f;
            float taigaLevel = tundraLevel - 0.21f;

            if (snowNoise >= polarLevel || snowNoise <= -polarLevel)
            {
                rightColor = polarZone;
            }
            else if (snowNoise >= tundraLevel || snowNoise <= -tundraLevel)
            {
                rightColor = tundraZone;
            }
            else if (snowNoise >= taigaLevel || snowNoise <= -taigaLevel)
            {
                rightColor = taigaZone;
            }
            else
            {
                rightColor = inputColor;
            };


            return rightColor;
        }

    }
}
