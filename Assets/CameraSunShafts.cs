using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraSunShafts : MonoBehaviour
{

    SunShafts sunShafts;
    // Start is called before the first frame update
    void Start()
    {
        sunShafts = transform.Find("System Camera").gameObject.GetComponent<SunShafts>();

    }
    public void ChangeCameraSunShafts(bool status)
    {
        if (sunShafts != null) {
            sunShafts.enabled = status;
        }

    }

    public void ChangeCameraSunShafts(bool status, float temperature, Color color)
    {

        Color sunColor = color;
        float falloutDistance = 0.5f;
        float blurSize = 2f;
        float intensity = 1f;

        float maxFalloutDistance = 0.5f;
        float maxBlurSize = 2.6f;
        float maxIntensity = 1.2f;

        float superHotMin = 4500;
        float superHotMax = 45000;
        float superHotness = 0;

        Color colorThreshold = Color.black;



        superHotness = Mathf.InverseLerp(superHotMin, superHotMax, temperature);

        //  colorThreshold = Color.Lerp(colorCold, color, superHotness);

        falloutDistance = Mathf.Lerp(falloutDistance, maxFalloutDistance, superHotness);
        blurSize = Mathf.Lerp(blurSize, maxBlurSize, superHotness);
        intensity = Mathf.Lerp(intensity, maxIntensity, superHotness);



        sunShafts.enabled = status;
        sunShafts.sunColor = color;
        sunShafts.sunThreshold = colorThreshold;
        sunShafts.maxRadius =  falloutDistance; 
        sunShafts.sunShaftBlurRadius = blurSize;
        sunShafts.sunShaftIntensity = intensity;
    }


}
