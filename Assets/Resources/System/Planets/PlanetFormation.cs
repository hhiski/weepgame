using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gas
{

    public string Name;
    public float Rarity;
    public bool Breathable;
    public bool Toxic;

    public Gas() {
        Name = "None";
        Rarity = 0;
        Breathable = false;
        Toxic = false;
    }
}

public class Atmosphere { 

    public float Radiation;
    public float Pressure;
    public float TemperatureB; //Black-body temperature
    public float TemperatureG; //Black-body temperature + Greenhouse-effect
    public float Temperature; // Simplified

    public Gas PrimaryGas;
};



#region PlanetTypes
public class PlanetType
{
    public string Name = "EmptyType";
    public float Albedo = 0;
    public float SurfaceVariation = 0.2f;

    public float[] TemperatureRange = new float[1]; //min and max values
    public float[] GreenhouseEffectRange = new float[1]; //not used?
    public float[] PressureRange = new float[1]; //min and max values
    public float[] EnvRadioactivityRange = new float[1]; //terrestrial radiation (not cosmic rays!), min and max values 

    public Gas[] GasList = new Gas[] {  }; // list of possible gas mixes

    public bool RandomAtmosphereAvailable = false;

    public Atmosphere RandomAtmosphere = new Atmosphere(); //for storing random the atmosphere within the type frames

    public bool PolarCaps = false;


    void SetTypeValues(PlanetType type)
    {
        Name = type.Name;
        GasList = type.GasList;
        Albedo = type.Albedo;
        TemperatureRange = type.TemperatureRange;
        GreenhouseEffectRange = type.GreenhouseEffectRange;
        PressureRange = type.PressureRange;
        EnvRadioactivityRange = type.EnvRadioactivityRange;
        PolarCaps = type.PolarCaps;
        SurfaceVariation = type.SurfaceVariation;
    }

    public PlanetType(){}

    public PlanetType(string typeName)
    {

        bool viable = false;
        PlanetFormation planetFormation = new PlanetFormation();
        List<PlanetType> EveryPlanetTypeList = new List<PlanetType>();
        EveryPlanetTypeList = planetFormation.PopulatePlanetList();

        foreach (PlanetType type in EveryPlanetTypeList)
        {
            if (type.Name == typeName)
            {
                Name = typeName;
                SetTypeValues(type);
                viable = true;
                break;
            }
        }

        if (viable == false)
        {
            Debug.Log("CANT FIND PLANET TYPE :" + typeName);

        }

    }

    public PlanetType(float orbitalDistance, float solarTemperature) {

        bool viable = false;
        PlanetFormation planetFormation = new PlanetFormation();
        PlanetAtmosphereConditions planetAtmosphereConditions = new PlanetAtmosphereConditions();

        List<PlanetType> EveryPlanetTypeList = new List<PlanetType>();

        EveryPlanetTypeList = planetFormation.PopulatePlanetList();

        while (!viable)
            {
                int random = Random.Range(0, EveryPlanetTypeList.Count);
                PlanetType randomType = EveryPlanetTypeList[random];
                float minTemp = randomType.TemperatureRange[0];
                float maxTemp = randomType.TemperatureRange[1];

                RandomAtmosphere = planetAtmosphereConditions.AtmosphereProbability(orbitalDistance, solarTemperature, randomType);
                float temp = RandomAtmosphere.Temperature;
                
                // Checks if the planets atmosphere temperature is okey for its type.
                if (temp < maxTemp)
                {
                    if (temp > minTemp)
                    {
                        SetTypeValues(randomType);
                        RandomAtmosphereAvailable = true;
                        viable = true;

                    }
                }
            }
    }
}
class AtmosphereGasFormation
{    // string[] randomThinGas = { "Nitrogen-Oxygen", "Nitrogen - Carbon Dioxide", "Nitrogen-Methane" };
     // string[] randomGreenhouseGas = { "Carbon Dioxide-Nitrogen", "Nitrogen - Carbon Dioxide", "Nitrogen-Methane"};


    /*

    

    Nitrogen-Oxygen
    Nitrogen-Methane
    Nitrogen-Chlorine
    Nitrogen-Sulfur Dioxide
    Helium-Hydrogen
    Hydrogen-Helium
    */

    public readonly Gas NitrogenCarbonDioxide = new Gas  { Name = "Nitrogen-Carbon Dioxide",  Breathable = false, Toxic = false, Rarity = 1 };
    public readonly Gas NitrogenMethane = new Gas { Name = "Nitrogen-Methane", Breathable = false, Toxic = false, Rarity = 1};
    public readonly Gas NitrogenOxygen = new Gas { Name = "Nitrogen-Oxygen", Breathable = true, Toxic = false, Rarity = 1};
    public readonly Gas NitrogenSulfurDioxide = new Gas { Name = "Nitrogen-Sulfur Dioxide", Breathable = false, Toxic = true, Rarity = 1 };
    public readonly Gas NitrogenChlorine = new Gas { Name = "Nitrogen-Chlorine", Breathable = false, Toxic = true, Rarity = 1 };
    public readonly Gas CarbonDioxideNitrogen = new Gas { Name = "Carbon Dioxide-Nitrogen", Breathable = false, Toxic = false, Rarity = 1 };
    public readonly Gas CarbonDioxideOxygen = new Gas { Name = "Carbon Dioxide-Oxygen", Breathable = true, Toxic = false, Rarity = 1 };
    public readonly Gas CarbonDioxideMethane = new Gas { Name = "Carbon Dioxide-Methane", Breathable = false, Toxic = false, Rarity = 1 };

    public readonly Gas HeliumHydrogen = new Gas { Name = "Helium-Hydrogen", Breathable = false, Toxic = false, Rarity = 1 };
    public readonly Gas HydrogenHelium = new Gas { Name = "Hydrogen-Helium", Breathable = false, Toxic = false, Rarity = 1 };
    // List<Gas> CommonGasses = new List<Gas>();

    internal Gas[] GetWaterGasList()
    {
        Gas[] gases = new Gas[] { NitrogenOxygen };
        return gases;
    }


    internal Gas[] GetGreenhouseGasList()
    {
        Gas[] gases = new Gas[] { CarbonDioxideNitrogen, CarbonDioxideOxygen, CarbonDioxideMethane };
        return gases;
    }

    internal Gas[] GetCommonGasList()
    {
        Gas[] gases = new Gas[] {NitrogenOxygen,NitrogenCarbonDioxide,NitrogenMethane,NitrogenSulfurDioxide,CarbonDioxideNitrogen};
        return gases;
    }

    internal Gas[] GetGasGiantGasList()
    {
        Gas[] gases = new Gas[] { HydrogenHelium, HeliumHydrogen };
        return gases;
    }

    /*
    public List<Gas> GreenhouseGasList()
    {
        List<Gas> gasOptions = new List<Gas>
        {
            CarbonDioxideNitrogen,
            CarbonDioxideOxygen,
            CarbonDioxideMethane,
        };
        return gasOptions;
    }

    public List<Gas> WaterGasList()
    {
        List<Gas> gasOptions = new List<Gas>
        {
            NitrogenOxygen,
            CarbonDioxideOxygen
        };
        return gasOptions;
    }

    public List<Gas> CommonGasList() {
        List<Gas> gasOptions = new List<Gas>
        {
            NitrogenOxygen,
            NitrogenCarbonDioxide,
            NitrogenMethane,
            NitrogenSulfurDioxide,
            CarbonDioxideNitrogen
        };
        return gasOptions;
    }

    public List<Gas> GasGiantGasList()
    {
        List<Gas> gasOptions = new List<Gas>
        {
            HydrogenHelium,
            HeliumHydrogen
        };
        return gasOptions;
    }
    */
}

class PlanetFormation
{
    AtmosphereGasFormation GasTypes = new AtmosphereGasFormation();
    public List<PlanetType> PopulatePlanetList()
    {
        Gas[] commonMix = GasTypes.GetCommonGasList();
        Gas[] giantMix = GasTypes.GetGasGiantGasList();
        Gas[] waterMix = GasTypes.GetWaterGasList();
        Gas[] greenMix = GasTypes.GetGreenhouseGasList();

        List<PlanetType> EveryPlanetTypeList = new List<PlanetType>();
        PlanetType Water = new PlanetType
        {
            Name = "Water",
            Albedo = 0.3f,
            GasList = waterMix,
            PressureRange = new float[] { 0.2f, 3f },
            TemperatureRange = new float[] { -5, 60 },
            GreenhouseEffectRange = new float[] { 25, 36 },
            EnvRadioactivityRange = new float[] { 0.0005f, 0.01f },
            PolarCaps = true
        };
        EveryPlanetTypeList.Add(Water);

        PlanetType Dust = new PlanetType
        {
            Name = "Dust",
            GasList = commonMix,
            Albedo = 0.3f,
            TemperatureRange = new float[] { -70, 70 },
            GreenhouseEffectRange = new float[] { 5, 30 },
            PressureRange = new float[] { 0.2f, 3f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.01f },
            PolarCaps = true
        };
        EveryPlanetTypeList.Add(Dust);

        PlanetType Rocky = new PlanetType
        {
            Name = "Rocky",
            GasList = commonMix,
            Albedo = 0.1f,
            TemperatureRange = new float[] { -1000, 1400 },
            GreenhouseEffectRange = new float[] { 0, 1 },
            PressureRange = new float[] { 0, 0.0001f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.001f },
            PolarCaps = true
        };
        EveryPlanetTypeList.Add(Rocky);

        PlanetType Ice = new PlanetType
        {
            Name = "Ice",
            Albedo = 0.8f,
            GasList = commonMix,
            TemperatureRange = new float[] { -1000, -5 },
            GreenhouseEffectRange = new float[] { 0, 3 },
            PressureRange = new float[] { 0f, 0.5f },
            EnvRadioactivityRange = new float[] { 0, 0.02f }
        };
        EveryPlanetTypeList.Add(Ice);

        PlanetType Snow = new PlanetType
        {
            Name = "Snow",
            Albedo = 0.8f,
            GasList = commonMix,
            TemperatureRange = new float[] { -150, 0 },
            GreenhouseEffectRange = new float[] { 0, 7 },
            PressureRange = new float[] { 0f, 0.3f },
            EnvRadioactivityRange = new float[] { 0.1f, 0.7f }
        };
        EveryPlanetTypeList.Add(Snow);

        PlanetType Mud = new PlanetType
        {
            Name = "Mud",
            Albedo = 0.2f,
            GasList = commonMix,
            TemperatureRange = new float[] { -10, 60 },
            GreenhouseEffectRange = new float[] { 25, 33 },
            PressureRange = new float[] { 0.4f, 5.7f },
            EnvRadioactivityRange = new float[] { 0.001f, 0.05f },
            PolarCaps = true
        };
        EveryPlanetTypeList.Add(Mud);

        PlanetType Greenhouse = new PlanetType
        {
            Name = "Greenhouse",
            GasList = greenMix,
            Albedo = 0.7f,
            TemperatureRange = new float[] { 100, 600 },
            GreenhouseEffectRange = new float[] { 100, 400 },
            PressureRange = new float[] { 30f, 100f },
            EnvRadioactivityRange = new float[] { 0.001f, 0.05f }
        };
        EveryPlanetTypeList.Add(Greenhouse);

        PlanetType Gas = new PlanetType
        {
            Name = "Gas",
            Albedo = 0.55f,
            GasList = giantMix,
            TemperatureRange = new float[] { -1000, 20000 },
            GreenhouseEffectRange = new float[] { 5, 40 },
            PressureRange = new float[] { 20f, 300f },
            EnvRadioactivityRange = new float[] { 0.001f, 5f }
        };
        EveryPlanetTypeList.Add(Gas);

        PlanetType Metallic = new PlanetType
        {
            Name = "Metallic",
            GasList = commonMix,
            Albedo = 0.1f,
            TemperatureRange = new float[] { -1000, 1300 },
            GreenhouseEffectRange = new float[] { 0, 2 },
            PressureRange = new float[] { 0f, 0.01f },
            EnvRadioactivityRange = new float[] { 0.005f, 0.1f }
        };
        EveryPlanetTypeList.Add(Metallic);

        PlanetType Nitrate = new PlanetType
        {
            Name = "Nitrate",
            GasList = commonMix,
            Albedo = 0.7f,
            TemperatureRange = new float[] { -1000, -5 },
            GreenhouseEffectRange = new float[] { 0, 4 },
            PressureRange = new float[] { 0f, 0.03f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.002f }
        };
        EveryPlanetTypeList.Add(Nitrate);

        PlanetType Lava = new PlanetType
        {
            Name = "Lava",
            Albedo = 0.05f,
            GasList = commonMix,
            TemperatureRange = new float[] { 1200, 999999 },
            GreenhouseEffectRange = new float[] { 0, 5 },
            PressureRange = new float[] { 0f, 0.001f },
            EnvRadioactivityRange = new float[] { 0.02f, 0.2f }
        };
        EveryPlanetTypeList.Add(Lava);


        PlanetType Acid = new PlanetType
        {
            Name = "Acid",
            Albedo = 0.2f,
            GasList = commonMix,
            TemperatureRange = new float[] { -100, 340 },
            GreenhouseEffectRange = new float[] { 5, 60 },
            PressureRange = new float[] { 0.01f, 6.5f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.1f }
        };
        EveryPlanetTypeList.Add(Acid);

        PlanetType Asphalt = new PlanetType
        {
            Name = "Asphalt",
            Albedo = 0.05f,
            GasList = commonMix,
            TemperatureRange = new float[] { -1000, 130 },
            GreenhouseEffectRange = new float[] { 0, 15 },
            PressureRange = new float[] { 0.0f, 0.1f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.002f }
        };
        EveryPlanetTypeList.Add(Asphalt);

        PlanetType Citric = new PlanetType
        {
            Name = "Citric",
            Albedo = 0.5f,
            GasList = commonMix,
            TemperatureRange = new float[] { -1000, -50 },
            GreenhouseEffectRange = new float[] { 2, 25 },
            PressureRange = new float[] { 0.0f, 0.5f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.002f }
        };
        EveryPlanetTypeList.Add(Citric);

        PlanetType Chromic = new PlanetType
        {
            Name = "Chromic",
            Albedo = 0.7f,
            GasList = commonMix,
            TemperatureRange = new float[] { -1000, 1900 },
            GreenhouseEffectRange = new float[] { 0, 1 },
            PressureRange = new float[] { 0.0f, 0.001f },
            EnvRadioactivityRange = new float[] { 0.0001f, 0.002f }
        };
        EveryPlanetTypeList.Add(Chromic);

        PlanetType Slime = new PlanetType
        {
            Name = "Slime",
            Albedo = 0.4f,
            GasList = commonMix,
            TemperatureRange = new float[] { -20, 100 },
            GreenhouseEffectRange = new float[] { 0, 80 },
            PressureRange = new float[] { 0.1f, 20.000f },
            EnvRadioactivityRange = new float[] { 0.00001f, 0.002f }
        };

        EveryPlanetTypeList.Add(Slime);

        return EveryPlanetTypeList;
    }
}

#endregion

#region PlanetAtmosphereConditions
class PlanetAtmosphereConditions
{
    public Atmosphere AtmosphereProbability(float orbitDistance, float solarTemperature, PlanetType type)
    {
        int randomGasIndex = Random.Range(0, type.GasList.Length);
        Gas randomGas = type.GasList[randomGasIndex];
        float randomPressure = Random.Range(type.PressureRange[0], type.PressureRange[1]);
        float randomRadiation = Random.Range(type.EnvRadioactivityRange[0], type.EnvRadioactivityRange[1]);
        float blackBodyTemperature = GetEffectiveTemperature(orbitDistance, type.Albedo, solarTemperature);
        float greenhouseTemperature = GetGreenhouseTemperature(blackBodyTemperature, randomPressure, randomGas);

        Atmosphere atm = new Atmosphere
        {
            TemperatureB = blackBodyTemperature,
            Pressure = randomPressure,
            Radiation = randomRadiation,
            TemperatureG = greenhouseTemperature,
            Temperature = blackBodyTemperature + greenhouseTemperature,
            PrimaryGas = randomGas
        };


        return atm;
    }



    float GetGreenhouseTemperature(float blackBodyTemperature, float pressure, Gas gas)
    {
        float greenhouseTemperatureEffect = Mathf.Pow(pressure, 0.5975f) * 34;

        float absorption = 1;
        if (blackBodyTemperature > 400) { absorption = 1.6f; }
        if (blackBodyTemperature <= 400 && blackBodyTemperature > 200) { absorption = 1.3f; }
        if (blackBodyTemperature <= 100 && blackBodyTemperature > -100) { absorption = 1f; }
        if (blackBodyTemperature <= -100 && blackBodyTemperature > -180) { absorption = 0.6f; }
        if (blackBodyTemperature <= -180) { absorption = 0.2f; }

        if (gas.Name == "Helium-Hydrogen" || gas.Name == "Hydrogen-Helium")
        {
            absorption /= 4.5f;
        }

        greenhouseTemperatureEffect *= absorption;

        return greenhouseTemperatureEffect;
    }

    float GetEffectiveTemperature(float distance, float albedo, float solarTemp)
    {
        float temperatureB = 0;
        distance = distance / 80.000f; // distance conversion to AUs

        if (distance <= 0.55f)
        {
            temperatureB = -560f * distance + 733f;
        }
        else if (distance > 0.55f && distance <= 0.819f)
        {
            temperatureB = -500f * distance + 700f;
        }
        else if (distance > 0.819f && distance <= 2.968f)
        {
            temperatureB = -107f * distance + 378f;
        }
        else
        {
            temperatureB = -13.7f * distance + 101;
        }


        if (temperatureB <= 3) { temperatureB = 3; }; // Planets cant get colder than space

        temperatureB = temperatureB * (0.000173f * solarTemp - 0.0008f); //Solar Heat Factor
        temperatureB = temperatureB - (temperatureB * (0.1667f * albedo)); //Bond albedo reflection
        return temperatureB - 273;
    }

}
#endregion



