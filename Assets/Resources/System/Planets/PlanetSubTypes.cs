using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSubTypes : MonoBehaviour
{




    public void GetSubTypeValues(PlanetSurface planetSurface, string type, int subType)
    {

        //Gas Giant planetary subtypes
         if (type == "Gas") {
            //Saturn-like
             if (subType == 1) {
                planetSurface.colorHigh = new Color(0.86f, 0.83f, 0.74f, 1);
                planetSurface.colorMid = new Color(0.79f, 0.78f, 0.61f, 1);
                planetSurface.colorLow = new Color(0.66f, 0.66f, 0.46f, 1);
                planetSurface.Frequency = 1.44f;
                planetSurface.Amplitude = 0.12f;
                planetSurface.colorStripify = 0.01f;
            }

            //Neptune-like
            if (subType == 2)
            {
                planetSurface.colorHigh = new Color(0.26f, 0.45f, 0.98f, 1);
                planetSurface.colorMid = new Color(0.22f, 0.32f, 1f, 1);
                planetSurface.colorLow = new Color(0.19f, 0.24f, 0.65f, 1);
                planetSurface.Frequency = 1.27f;
                planetSurface.Amplitude = 0.02f;
                planetSurface.colorStripify = 0.01f;
                planetSurface.levels = 2;
            }
            //Uranus-like
            if (subType == 3)
            {
                planetSurface.colorHigh = new Color(0.52f, 0.89f, 0.98f, 1);
                planetSurface.colorMid = new Color(0.22f, 0.74f, 1f, 1);
                planetSurface.colorLow = new Color(0.19f, 0.48f, 0.65f, 1);
                planetSurface.Frequency = 0.7f;
                planetSurface.Amplitude = 0.01f;
                planetSurface.colorStripify = 0.01f;
                planetSurface.levels = 2;
            }

            //wierd sea
            if (subType == 4)
            {
                planetSurface.colorHigh = new Color(0.78f, 1f, 0.93f, 1);
                planetSurface.colorMid = new Color(0.21f, 0.81f, 0.78f, 1);
                planetSurface.colorLow = new Color(0.14f, 0.81f, 0.57f, 1);
                planetSurface.Frequency = 2.1f;
                planetSurface.Amplitude = 0.08f;
                planetSurface.colorStripify = 0.27f;
                planetSurface.levels = 4;
                planetSurface.ridges = true;
            }


            if (subType == 5)
            {
                planetSurface.colorHigh = new Color(1f, 0.4f, 0.0f, 1);
                planetSurface.colorMid = new Color(0.72f, 0.27f, 0.27f, 1);
                planetSurface.colorLow = new Color(0.42f, 0.24f, 0.16f, 1);
                planetSurface.Frequency = 2.3f;
                planetSurface.Amplitude = 0.04f;
                planetSurface.colorStripify = 0.35f;
                planetSurface.levels = 1;
                planetSurface.fluidic = true;
                planetSurface.negativePeakClip = 0.98f;
                planetSurface.positivePeakClip = 1.02f;
            }
        }
         
    }
}
