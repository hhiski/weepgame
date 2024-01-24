﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathSpace;


static public class CelestialBody {



    public class Universe
    {
        int Id;
        int NumberOfClusters;

        MathFunctions MathFunctions = new MathFunctions();



        public List<Cluster> Clusters = new List<Cluster>();



        public Universe(int id)
        {
            Id = id;
           

        }




    }

    public class Moon : Planet
    {
        public Moon(float parentsOrbitalDistance, float solarTemperature)
        {

            Seed = Random.Range(1, 9999); ;

            Mass = Random.Range(0.10f, 0.25f); ;
            OrbitDistance = 5;
            Pos = MathFunctions.RandomPositionOnCircle(5, new Vector3(0, 0, 0));
            RotationSpeed = Random.Range(0.5f, 8);
            OrbitSpeed = Random.Range(3, 6);
            OrbitPhase = Random.Range(0, 100) / 100f;
            OrbitOmega = Random.Range(0, 180);
            Type = new PlanetType(parentsOrbitalDistance, solarTemperature);
            Name = "Moon";
            RingType = 0;
            PolarCoverage = 0;
            Atm = Type.RandomAtmosphere;
            Hydrosphere = new Hydrosphere(Type, Atm);
            Biosphere = new Biosphere(Type.LifeProbability);
            Lithosphere = new Lithosphere(Type.SurfaceType);
            Features = new PlanetaryFeatures(Atm, Type, Biosphere, Hydrosphere);
            Society = new Society();
        }

        public Moon()
        {

            Seed = Random.Range(1, 9999); ;
            Mass = Random.Range(0.10f, 0.25f); ;
            OrbitDistance = 5;
            Pos = MathFunctions.RandomPositionOnCircle(5, new Vector3(0, 0, 0));
            RotationSpeed = Random.Range(0.5f, 8);
            OrbitSpeed = Random.Range(3, 6);
            OrbitPhase = Random.Range(0, 100) / 100f;
            OrbitOmega = Random.Range(0, 180);
            Type = new PlanetType("Minor");
            Name = "Moon";
            RingType = 0;
            PolarCoverage = 0;
            Atm = Type.RandomAtmosphere;
            Atm.Radiation = 0;
            Atm.Pressure = 0;
            Atm.TemperatureB = 0;
            Atm.TemperatureG = 0;
            Atm.PrimaryGas = new Gas();
            if (Type.RandomAtmosphereAvailable)
            {
                Atm = Type.RandomAtmosphere;
            }

            Hydrosphere = new Hydrosphere(Type, Atm);
            Biosphere = new Biosphere(Type.LifeProbability);
            Lithosphere = new Lithosphere(Type.SurfaceType);
            Features = new PlanetaryFeatures(Atm, Type, Biosphere, Hydrosphere);
            Society = new Society();
        }
    }

    public class Planet
    {
        public int Id; //Within the solar system
        public string Name;
        public float OrbitSpeed;
        public float OrbitPhase; 
        public float RotationSpeed;
        public float OrbitDistance;
        public float OrbitOmega; //Longitude of the ascending node
        public float OrbitInclination;
        public PlanetType Type;
        public int Seed;
        public int RingType;
        public Hydrosphere Hydrosphere;
        public Biosphere Biosphere;
        public Lithosphere Lithosphere;
        public float Mass;
        public float PolarCoverage;
        public Atmosphere Atm;
        public Vector3 Pos;
        public PlanetaryFeatures Features;
        public Society Society;

        public List<Moon> Moons = new List<Moon>();


        public Planet(float orbitalDistance, float solarTemperature) // Autoselects the suitable type for the planet based its suns distance and temperature.
        {
            Type = new PlanetType(orbitalDistance, solarTemperature);
            Name = "Unnamed";
            RingType = RingTypeProbability();
            Seed = Random.Range(1, 9999);
            Mass = MassProbability(Type.Name);
            OrbitDistance = orbitalDistance;
            OrbitPhase = Random.Range(0, 100)/100f;
            Pos = MathFunctions.PositionOnCircle(orbitalDistance, OrbitPhase, new Vector3(0, 0, 0));
            OrbitSpeed = MathFunctions.GetOrbitSpeed(orbitalDistance);
            OrbitOmega = Random.Range(0, 180);
            OrbitInclination = MathFunctions.GetRandomOrbitInclination(Seed);
            RotationSpeed = Random.Range(0.5f, 8);
            PolarCoverage = 0;
            Atm = Type.RandomAtmosphere;
            Hydrosphere = new Hydrosphere(Type, Atm);
            Biosphere = new Biosphere(Type.LifeProbability);
            Lithosphere = new Lithosphere(Type.SurfaceType);
            Features = new PlanetaryFeatures(Atm, Type, Biosphere, Hydrosphere);
            Society = new Society();
        }

        public Planet(string type) //Forced planet type. eg. ice planets near the sun
        {
            Type = new PlanetType(type);
            Name = "Unnamed";
            RingType = RingTypeProbability();
            OrbitPhase = Random.Range(0, 100) / 100f;
            Seed = Random.Range(1, 9999);
            Mass = MassProbability(Type.Name);
            RotationSpeed = Random.Range(0.5f, 8);
            PolarCoverage = 0;
            Atm.Radiation = 0;
            Atm.Pressure = 0;
            Atm.TemperatureB = 0;
            Atm.TemperatureG = 0;
            Atm.PrimaryGas = new Gas();
            if (Type.RandomAtmosphereAvailable)
            {
                Atm = Type.RandomAtmosphere;
            }
            Biosphere = new Biosphere(Type.LifeProbability);
            Lithosphere = new Lithosphere(Type.SurfaceType);
            Hydrosphere = new Hydrosphere(Type, Atm);
            Features = new PlanetaryFeatures(Atm, Type,Biosphere, Hydrosphere);
            Society = new Society();
        }


        public Planet()
        {
            Type = new PlanetType();
            Name = "Unnamed";
            RingType = RingTypeProbability();
            Seed = Random.Range(1, 9999);
            OrbitPhase = Random.Range(0, 100) / 100f;
            Mass = MassProbability(Type.Name);
            PolarCoverage = 0;
            Atm = Type.RandomAtmosphere;
            Hydrosphere = new Hydrosphere(Type, Atm);
            Biosphere = new Biosphere(Type.LifeProbability);
            Lithosphere = new Lithosphere(Type.SurfaceType);
            Features = new PlanetaryFeatures(Atm, Type, Biosphere, Hydrosphere);
            Society = new Society();
        }



        float MassProbability(string type)
        {
            float mass;

            if (type == "Gas")
            {
                mass = Random.Range(1.2f, 1.1f);
            }
            else if(type == "Minor")
            {
                mass = Random.Range(0.25f, 0.35f);
            }
          
            else 
            {
                mass = Random.Range(0.5f, 1f);
            }
            return mass;
        }


        int RingTypeProbability()
        {
            //70% planets have no ring
            if (Random.Range(1, 11) <= 0)
            {
                return 0;
            }
            else
                return Random.Range(1, 6);
        }
    }

    public class Star
    {
        public Vector3 Pos;
        public StarType Type;
        public int Seed;
        public string Name;
        public float SolarWindStrenght;
        public float SolarTemperature;
        public int NumOfPlanets;

        public int Id;

        public List<bool> OrbitSlots = new List<bool>();
        public List<Planet> Planets = new List<Planet>();
        public List<AsteroidBelt> AsteroidBelts = new List<AsteroidBelt>();


        public Star()
        {
            Name = "Unnamed Star";
            Seed = Random.Range(1, 9999);
            Vector3 middlePos = new Vector3(0, 0, 0);
            float randomHeight = Random.Range(5f, 50f); ;
            Pos = MathFunctions.RandomPositionOnCircle(0, 200, middlePos) + new Vector3(0, randomHeight,0 );
            StarFormation starFormation = new StarFormation();
            StarType starType = starFormation.GetRandomTypeStar();
            Type = starType;
            SolarWindStrenght = Mathf.Clamp((Random.Range(0,3f)-2),0f, 0.8f);
            SolarTemperature = Random.Range(starType.TemperatureRange[0], starType.TemperatureRange[1]); 
            NumOfPlanets = 0;

        }
    }

    public class AsteroidBelt
        {
        //Belt types:
        //0 - non-existing
        //1 - ice rocks
        //2 - smooth stones
        //3 - ??

        public int Type;
        public float Distance;

        public AsteroidBelt(int type, float distance) : base()
        {
            Type = type;
            Distance = distance;
        }

        public AsteroidBelt()
        {
            Type = Random.Range(0, 4);
            Distance = Random.Range(40, 400);
        }
        }
    
    public class Cluster 
    {

        public Cluster(int id, string name, int numOfStars, int color)
        {
            Id = id;
            Name = name;
            NumOfStars = numOfStars;
            Color = color;
        }

        public Cluster(int id)
        {
            Id = id;
            Name = "Unnamed Cluster";
            NumOfStars = Random.Range(5, 5);
            Color = Random.Range(1, 4);
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public int NumOfStars { get; set; }
        public int Color { get; set; }

        public List<Star> Stars = new List<Star>();

    }


}