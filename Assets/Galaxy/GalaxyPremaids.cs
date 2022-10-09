using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CelestialBody;

public class Premaid
{
    StarFormation starFormation = new StarFormation();

    Names nameList = new Names();

    int TotalPlanetCount = 0;
    int TotalStarCount = 0;

     int setPlanetId()
    {
        int id = TotalPlanetCount;
        TotalPlanetCount++;
        return id;
    }
    int setStarId()
    {
        int id = TotalStarCount;
        TotalStarCount++;
        return id;
    }

    public Cluster GenericCluster(int id)
    {
        Cluster GenericCluster = new Cluster(id);

        GenericCluster.Name = nameList.getRandomZoneName();

        int numOfStars = GenericCluster.NumOfStars;

        for (int starIndex = 0; starIndex < numOfStars; starIndex++)
        {
                
            Star Star = new Star();

            Star.Id = setStarId();
            Star.Name = nameList.getRandomStarName();
            Star.Type = starFormation.GetRandomTypeStar();
            Star.NumOfPlanets = Random.Range(1, 15);
            Star.SolarTemperature = starFormation.getStarTypeRandomTemperature(Star.Type);

            Star.GenerateComets();

            int systemPlanetCount = Star.NumOfPlanets;

            int beltType = Random.Range(0, 3);
            int numOfBelts = Random.Range(0, 4);

            GenerateBelts(Star, numOfBelts, beltType);
            GeneratePlanets(Star, systemPlanetCount);

            GenericCluster.Stars.Add(Star);
        }

        return GenericCluster;
    }

    void GeneratePlanets(Star star, int numberOfPlanets)
    {
        float orbitDistance = 10f + SpaceMath.StandardDeviation(40f, 15f, star.Id);

        // orbitDistance = SpaceMath.MoveAwayFromBelts(beltsOrbitDistances, orbitDistance); ;

        for (int planetIndex = 0; planetIndex < numberOfPlanets; planetIndex++)
        {

            Planet planet = new Planet(orbitDistance, star.SolarTemperature);
            planet.Id = setPlanetId();
            planet.Name = nameList.getPlanetName(star.Name, planetIndex);
            orbitDistance += Mathf.Abs(SpaceMath.StandardDeviation(30f, 15f, star.Id * planet.Id * 500));
            star.Planets.Add(planet);

        }
    }

    public void GenerateBelts(Star star, int numberOfBelts, int type)
    {
        for (int beltIndex = 0; beltIndex < numberOfBelts; beltIndex++)
        {

            AsteroidBelt belt = new AsteroidBelt();
            float beltDistance = (beltIndex + 1) * Random.Range(45f, 185f);
            belt.Distance = beltDistance;
            belt.Type = type;
            star.AsteroidBelts.Add(belt);
        }
    }

    public Cluster LocalBubbleCluster(int id)
    {
           

        Cluster LocalBubbleCluster = new Cluster(id);
        LocalBubbleCluster.Name = "Local Bubble";

        float bubbleScale = 10;

        AtmosphereGasFormation gasTypes = new AtmosphereGasFormation();

        Vector3 solPosition = new Vector3(0, 30, 0);


        //Firstly creates Sol to the local bubble
        Star Sol = new Star();
        Sol.Id = setStarId();
        Sol.Name = "Sol";
        Sol.Pos = solPosition;
        Sol.Type = starFormation.GetSpecificTypeStar("G-type"); 
        Sol.SolarTemperature = 5778;
        Sol.NumOfPlanets = 8;


     /*   Comet Comet = new Comet();
        Comet CometB = new Comet();
        Comet CometC = new Comet();
        Sol.Comets.Add(Comet);
        Sol.Comets.Add(CometB);
        Sol.Comets.Add(CometC);*/

        AsteroidBelt Belt = new AsteroidBelt();
        Belt.Distance = 150;
        Belt.Type = 2;
        Sol.AsteroidBelts.Add(Belt);

        //Then creates planets of the solar system
        Planet Mercury = new Planet();
        Mercury.OrbitDistance = 42;
        Mercury.Name = "Mercury";
        Mercury.Id = setPlanetId(); 
        Mercury.Pos = SpaceMath.RandomPositionOnCircle(Mercury.OrbitDistance, new Vector3(0, 0, 0));
        Mercury.OrbitSpeed = 0;
        Mercury.RotationSpeed = 0.08f;
        Mercury.Mass = 0.5f;
        Mercury.Type = new PlanetType("Metallic");
        Mercury.Atm.PrimaryGas = gasTypes.NitrogenMethane;
        Mercury.HueShift = 0;
        Mercury.RingType = 0;
        Mercury.Atm.TemperatureB = SpaceMath.GetEffectiveTemperature(Mercury.OrbitDistance, Mercury.Type.Albedo, Sol.SolarTemperature);
        Mercury.Atm.Pressure = 0;
        Mercury.Atm.Radiation = 30.1f;
        Sol.Planets.Add(Mercury);

        Planet Venus = new Planet();
        Venus.OrbitDistance = 60;
        Venus.Name = "Venus";
        Venus.Id = setPlanetId();
        Venus.Pos = SpaceMath.RandomPositionOnCircle(Venus.OrbitDistance, new Vector3(0, 0, 0));
        Venus.OrbitSpeed = 0;
        Venus.RotationSpeed = 0.02f;
        Venus.Type = new PlanetType("Greenhouse"); 
        Venus.HueShift = 0;
        Venus.Mass = 0.9f;
        Venus.RingType = 0;
        Venus.Atm.TemperatureB = SpaceMath.GetEffectiveTemperature(Venus.OrbitDistance, 0.75f, Sol.SolarTemperature);
        Venus.Atm.TemperatureG = 421;
        Venus.Atm.Pressure = 93.2f;
        Venus.Atm.Radiation = 0.78f;
        Venus.Atm.PrimaryGas = gasTypes.CarbonDioxideNitrogen;
        Sol.Planets.Add(Venus);

        Planet Earth = new Planet();
        Earth.OrbitDistance = 80;
        Earth.Name = "Earth";
        Earth.Id = setPlanetId();
        Earth.Pos = SpaceMath.RandomPositionOnCircle(Earth.OrbitDistance, new Vector3(0, 0, 0));
        Earth.OrbitSpeed = 0;
        Earth.RotationSpeed = 4;
        Earth.Type = new PlanetType("Water");
        Earth.HueShift = 0;
        Earth.Mass = 1f;
        Earth.RingType = 0;
        Earth.PolarCoverage = 0.15f;
        Earth.Atm.PrimaryGas = gasTypes.NitrogenOxygen;
        Earth.Atm.Pressure = 1;
        Earth.Atm.TemperatureB = SpaceMath.GetEffectiveTemperature(Earth.OrbitDistance, 0.3f, Sol.SolarTemperature);
        Earth.Atm.TemperatureG = 33;
        Earth.Atm.Radiation = 0.0024f;

        Moon Luna = new Moon();
        Luna.Pos = SpaceMath.RandomPositionOnCircle(15, Earth.Pos);
        Luna.Type = new PlanetType("Metallic");
        Earth.Moons.Add(Luna);
        Sol.Planets.Add(Earth);

        Planet Mars = new Planet();
        Mars.OrbitDistance = 105;
        Mars.Name = "Mars";
        Mars.Id = setPlanetId();
        Mars.Pos = SpaceMath.RandomPositionOnCircle(Mars.OrbitDistance, new Vector3(0, 0, 0));
        Mars.OrbitSpeed = 0;
        Mars.RotationSpeed = 4;
        Mars.Type = new PlanetType("Dust");
        Mars.Mass = 0.7f;
        Mars.HueShift = -0.09f;
        Mars.PolarCoverage = 0.15f;
        Mars.RingType = 0;
        Mars.Atm.TemperatureB = SpaceMath.GetEffectiveTemperature(Mars.OrbitDistance, Mars.Type.Albedo, Sol.SolarTemperature);
        Mars.Atm.PrimaryGas = gasTypes.CarbonDioxideNitrogen;
        Mars.Atm.Pressure = 0.00658f;
        Mars.Atm.Radiation = 0.233f;
        Sol.Planets.Add(Mars);

        Planet Jupiter = new Planet();
        Jupiter.OrbitDistance = 180;
        Jupiter.Name = "Jupiter";
        Jupiter.Id = setPlanetId();
        Jupiter.Pos = SpaceMath.RandomPositionOnCircle(Jupiter.OrbitDistance, new Vector3(0, 0, 0));
        Jupiter.OrbitSpeed = 0;
        Jupiter.RotationSpeed = 2;
        Jupiter.Type = new PlanetType("Gas");
        Jupiter.SubType = 0;
        Jupiter.Mass = 1.7f;
        Jupiter.HueShift = 0f;
        Jupiter.RingType = 0;
        Jupiter.Seed = 1231;
        Jupiter.Atm.PrimaryGas = gasTypes.HydrogenHelium;
        Jupiter.Atm.TemperatureB = SpaceMath.GetEffectiveTemperature(Jupiter.OrbitDistance, Jupiter.Type.Albedo, Sol.SolarTemperature);
        Jupiter.Atm.Pressure = 5000;
        Jupiter.Atm.Radiation = 5000;
        Sol.Planets.Add(Jupiter);

        Planet Saturn = new Planet();
        Saturn.OrbitDistance = 230;
        Saturn.Id = setPlanetId();
        Saturn.Name = "Saturn";
        Saturn.Pos = SpaceMath.RandomPositionOnCircle(Saturn.OrbitDistance, new Vector3(0, 0, 0));
        Saturn.OrbitSpeed = 0;
        Saturn.RotationSpeed = 2;
        Saturn.Type = new PlanetType("Gas");
        Saturn.SubType = 1;
        Saturn.HueShift = -0.00f;
        Saturn.Mass = 1.7f;
        Saturn.RingType = 3;
        Saturn.Atm.PrimaryGas = gasTypes.HydrogenHelium;
        Saturn.Atm.TemperatureB = SpaceMath.GetEffectiveTemperature(Saturn.OrbitDistance, Saturn.Type.Albedo, Sol.SolarTemperature);
        Saturn.Atm.Pressure = 5000;
        Saturn.Atm.Radiation = 5000;
        Sol.Planets.Add(Saturn);

        Planet Uranus = new Planet();
        Uranus.OrbitDistance = 260;
        Uranus.Id = setPlanetId();
        Uranus.Name = "Uranus";
        Uranus.Pos = SpaceMath.RandomPositionOnCircle(Uranus.OrbitDistance, new Vector3(0, 0, 0));
        Uranus.OrbitSpeed = 0;
        Uranus.RotationSpeed = 2;
        Uranus.Type = new PlanetType("Gas");
        Uranus.SubType = 3;
        Uranus.Mass = 1.5f;
        Uranus.HueShift = -0.00f;
        Uranus.RingType = 0;
        Uranus.Atm.PrimaryGas = gasTypes.HydrogenHelium;
        Uranus.Atm.TemperatureB = SpaceMath.GetEffectiveTemperature(Uranus.OrbitDistance, Uranus.Type.Albedo, Sol.SolarTemperature);
        Uranus.Atm.Pressure = 1000;
        Uranus.Atm.Radiation = 3;
        Sol.Planets.Add(Uranus);

        Planet Neptune = new Planet();
        Neptune.OrbitDistance = 285;
        Neptune.Id = setPlanetId();
        Neptune.Name = "Neptune";
        Neptune.Pos = SpaceMath.RandomPositionOnCircle(Neptune.OrbitDistance, new Vector3(0, 0, 0));
        Neptune.OrbitSpeed = 0;
        Neptune.RotationSpeed = 2;
        Neptune.Type = new PlanetType("Gas");
        Neptune.SubType = 2;
        Neptune.Mass = 1.6f;
        Neptune.HueShift = -0.00f;
        Neptune.RingType = 0;
        Neptune.Atm.PrimaryGas = gasTypes.HydrogenHelium;
        Neptune.Atm.TemperatureB = SpaceMath.GetEffectiveTemperature(Neptune.OrbitDistance, Neptune.Type.Albedo, Sol.SolarTemperature);
        Neptune.Atm.Pressure = 1000;
        Neptune.Atm.Radiation = 3.43f;
        Sol.Planets.Add(Neptune);

        LocalBubbleCluster.Stars.Add(Sol);





        //Alpha Centauri
        Star alphaCentauri = new Star();
        alphaCentauri.Id = setStarId();
        alphaCentauri.Name = "Alpha Centauri";
        alphaCentauri.Pos = solPosition + bubbleScale * new Vector3(3.126f, -0.052f, -3.047f);
        alphaCentauri.Type = starFormation.GetSpecificTypeStar("G-type");
        alphaCentauri.SolarTemperature = 5790;
        alphaCentauri.NumOfPlanets = 2;
        GeneratePlanets(alphaCentauri, 2);

        AsteroidBelt alphaCentauriBeltOne = new AsteroidBelt();
        alphaCentauriBeltOne.Distance = 45;
        alphaCentauriBeltOne.Type = 1;
        alphaCentauri.AsteroidBelts.Add(alphaCentauriBeltOne);
        LocalBubbleCluster.Stars.Add(alphaCentauri);

        //Proxima Centauri
        Star proximaCentauri = new Star();
        proximaCentauri.Id = setStarId();
        proximaCentauri.Name = "Proxima Centauri";
        proximaCentauri.Pos = solPosition + bubbleScale * new Vector3(2.545f, -0.243f, -3.256f);
        proximaCentauri.Type = starFormation.GetSpecificTypeStar("M-type");
        proximaCentauri.SolarTemperature = 3420;
        proximaCentauri.NumOfPlanets = 0;
        LocalBubbleCluster.Stars.Add(proximaCentauri);

        Planet proximaCentauriF = new Planet();
        proximaCentauriF.OrbitDistance = 55;
        proximaCentauriF.Id = setPlanetId();
        proximaCentauriF.Name = "Proxima Centauri F";
        proximaCentauriF.Pos = SpaceMath.RandomPositionOnCircle(proximaCentauriF.OrbitDistance, new Vector3(0, 0, 0));
        proximaCentauriF.OrbitSpeed = 0;
        proximaCentauriF.RotationSpeed = 1;
        proximaCentauriF.Type = new PlanetType("Slime");
        proximaCentauriF.Mass = 1.1f;
        proximaCentauriF.HueShift = -0.00f;
        proximaCentauriF.RingType = 0;
        proximaCentauriF.Atm.PrimaryGas = gasTypes.HydrogenHelium;
        proximaCentauriF.Atm.TemperatureB = SpaceMath.GetEffectiveTemperature(proximaCentauriF.OrbitDistance, proximaCentauriF.Type.Albedo, proximaCentauri.SolarTemperature);
        proximaCentauriF.Atm.Pressure = 7;
        proximaCentauriF.Atm.Radiation = 0.13f;
        proximaCentauri.Planets.Add(proximaCentauriF);

        LocalBubbleCluster.Stars.Add(Sol);

        //Wolf 359
        Star Wolf359 = new Star();
        Wolf359.Id = setStarId();
        Wolf359.Name = "Wolf 359";
        Wolf359.Pos = solPosition + bubbleScale * new Vector3(-1.916f, 6.522f, -3.938f);
        Wolf359.Type = starFormation.GetSpecificTypeStar("M-type");
        Wolf359.SolarTemperature = 2749;
        Wolf359.NumOfPlanets = 3;
        GeneratePlanets(Wolf359, 3);
        LocalBubbleCluster.Stars.Add(Wolf359);

        //Lalande 21185
        Star Lalande21185 = new Star();
        Lalande21185.Id = setStarId();
        Lalande21185.Name = "Wolf 359";
        Lalande21185.Pos = solPosition + bubbleScale * new Vector3(-3.439f, 7.553f, -0.308f);
        Lalande21185.Type = starFormation.GetSpecificTypeStar("M-type");
        Lalande21185.SolarTemperature = 3546;
        Lalande21185.NumOfPlanets = 1;
        LocalBubbleCluster.Stars.Add(Lalande21185);

        //Sirius A
        Star SiriusA = new Star();
        SiriusA.Id = setStarId();
        SiriusA.Name = "Sirius A";
        SiriusA.Pos = solPosition + bubbleScale * new Vector3(-5.809f, -1.338f, -6.28f);
        SiriusA.Type = starFormation.GetSpecificTypeStar("A-type");
        SiriusA.SolarTemperature = 9950;
        SiriusA.NumOfPlanets = 2;
        LocalBubbleCluster.Stars.Add(SiriusA);

        //Epsilon Eridani
        Star EpsilonEridani = new Star();
        EpsilonEridani.Id = setStarId();
        EpsilonEridani.Name = "Epsilon Eridani";
        EpsilonEridani.Pos = solPosition + bubbleScale * new Vector3(-6.753f, -7.811f, -1.917f);
        EpsilonEridani.Type = starFormation.GetSpecificTypeStar("K-type");
        EpsilonEridani.SolarTemperature = 5230;
        EpsilonEridani.NumOfPlanets = 6;
        LocalBubbleCluster.Stars.Add(EpsilonEridani);

        //Procyon A
        Star ProcyonA = new Star();
        ProcyonA.Id = setStarId();
        ProcyonA.Name = "Epsilon Eridani";
        ProcyonA.Pos = solPosition + bubbleScale * new Vector3(-9.27f, 2.577f, -6.183f);
        ProcyonA.Type = starFormation.GetSpecificTypeStar("F-type");
        ProcyonA.SolarTemperature = 6530;
        ProcyonA.NumOfPlanets = 3;
        LocalBubbleCluster.Stars.Add(ProcyonA);

        return LocalBubbleCluster;
    }
}
