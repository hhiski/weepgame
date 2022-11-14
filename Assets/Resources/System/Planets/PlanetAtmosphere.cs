using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAtmosphere : MonoBehaviour
{

    Color atmosphereColor = new Color(1f, 0.0f, 1, 0);
    float atmospherePressure = 0;
    float atmosphereThickness = 0;
    float atmosphereSize = 18;

    void Start()
    {
        Color seaColor = new Color(1f, 0.0f, 1, 1);
        seaColor = this.gameObject.transform.parent.GetComponent<PlanetSurface>().colorSeaShifted;
        atmosphereSize = this.gameObject.transform.parent.GetComponent<PlanetSurface>().Planet.Mass;

        atmospherePressure = this.gameObject.transform.parent.GetComponent<PlanetSurface>().Planet.Atm.Pressure;


        if (atmospherePressure <= 1) {
            atmosphereThickness = atmospherePressure * 0.5f;
        }
        else if (atmospherePressure > 1)
        {
            atmosphereThickness = atmospherePressure * 0.1f + 0.4f;
            atmosphereThickness = Mathf.Clamp(atmosphereThickness, 0, 1);
        }





        if (this.gameObject.GetComponent<ParticleSystem>() != null)
        {
            ParticleSystem.MainModule main = this.gameObject.GetComponent<ParticleSystem>().main;
            main.startColor = new Color(seaColor.r, seaColor.g, seaColor.b, atmosphereThickness);
            main.startSize = 18 * atmosphereSize;

        }

    }



}
