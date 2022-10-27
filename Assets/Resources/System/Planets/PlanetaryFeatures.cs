using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class PlanetaryFeatureProfile
{
    public PlanetaryFeature HydrosphereFeature;
    public PlanetaryFeature PrimaryFeature;
    public PlanetaryFeature SecondaryFeature;
};*/

public class PlanetaryFeatures 
{

    public PlanetaryFeature HydrosphereFeature;
    public PlanetaryFeature PrimaryFeature;
    public PlanetaryFeature SecondaryFeature;

    public float Temperature; 
    public float Pressure; 
    public float Radioactivity;

    internal PlanetaryFeatures(Atmosphere atmosphere)
    {
        RandomFeature randomFeature = new RandomFeature();

        PrimaryFeature = randomFeature.GetRandomFeature();
        Debug.Log("random feature2" + PrimaryFeature.Name);
    }

}

public class PlanetaryFeature
{
    public string Name;
    public int Rarity;

    public float[] TemperatureRange = new float[1];
    public float[] PressureRange = new float[1];
    public float[] EnvRadioactivityRange = new float[1];

    public PlanetaryFeature()
    {
        Name = "None";
        Rarity = 0;

        TemperatureRange = new float[] { -274f, 99999f };
        PressureRange = new float[] { -1, 99999f };
        EnvRadioactivityRange = new float[] { -1, 99999f };
    }
}

class PlanetaryFeatureFormation
{

    public readonly PlanetaryFeature WierdRocks = new PlanetaryFeature { Name = "Wierd rocks", Rarity = 1 };
    public readonly PlanetaryFeature GraphiteDust = new PlanetaryFeature { Name = "Graphite dust", Rarity = 1 };
    public readonly PlanetaryFeature SaltDeserts = new PlanetaryFeature { Name = "Salt deserts", Rarity = 1 };
    public readonly PlanetaryFeature CaveNetworks = new PlanetaryFeature { Name = "Cave networks", Rarity = 1 };
    public readonly PlanetaryFeature CopperDeposits = new PlanetaryFeature { Name = "Copper deposits", Rarity = 1 };
    public readonly PlanetaryFeature SilicateDust = new PlanetaryFeature { Name = "Silicate Dust", Rarity = 1 };
    public readonly PlanetaryFeature CobaltDeposits = new PlanetaryFeature { Name = "Cobalt deposits", Rarity = 1 };
    public readonly PlanetaryFeature UnstableTectonics = new PlanetaryFeature { Name = "Unstable Tectonics", Rarity = 1 };
    public readonly PlanetaryFeature StrongMagneticField = new PlanetaryFeature { Name = "Strong Magnetic Field", Rarity = 1 };
    public readonly PlanetaryFeature AcidRains = new PlanetaryFeature { Name = "Acid Rains", Rarity = 1, TemperatureRange = new float[] { -30f, 100f }, PressureRange = new float[] { 0.1f, 999f } };
    public readonly PlanetaryFeature ExtremeStorms = new PlanetaryFeature { Name = "Extreme Storms", Rarity = 1, TemperatureRange = new float[] { -50f, 10000f }, PressureRange = new float[] { 0.2f, 999f } };
    public readonly PlanetaryFeature GeomagneticStorms = new PlanetaryFeature { Name = "Geomagnetic Storms", Rarity = 1 };
    public readonly PlanetaryFeature RareearthMinerals = new PlanetaryFeature { Name = "Rare-earth Minerals", Rarity = 1 };


    internal PlanetaryFeature[] GetCommonFeatureList()
    {
        PlanetaryFeature[] features = new PlanetaryFeature[] { WierdRocks, GraphiteDust, SaltDeserts, AcidRains, ExtremeStorms, RareearthMinerals };
        return features;
    }
}




public class RandomFeature
{
    PlanetaryFeatureFormation features = new PlanetaryFeatureFormation();
   

    public PlanetaryFeature GetRandomFeature() //gets random planetary feature weighted by rarity
    {
        PlanetaryFeature[] commonFeatures = features.GetCommonFeatureList();
        PlanetaryFeature randomFeature = new PlanetaryFeature();
        int weightSum = 0;
        int accumulatedWeightSum = 0;



        foreach (PlanetaryFeature feature in commonFeatures)
        {
            weightSum += feature.Rarity;
        }

        int random = Random.Range(0, weightSum-1);

        foreach (PlanetaryFeature feature in commonFeatures)
        {
            accumulatedWeightSum += feature.Rarity;
            if (accumulatedWeightSum > random)
            {
             
                randomFeature = feature;
                break;
            }
        }

        return randomFeature;
    }


    

   




}
/*
IEnumerator enumerator = collection.GetEnumerator();

while(enumerator.MoveNext())
{
    object obj = enumerator.Current;
    // work with the object
}*/