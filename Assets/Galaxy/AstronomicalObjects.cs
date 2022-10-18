using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static public class CelestialBody {

    public class Universe
    {
        int Id;
        int NumberOfClusters;

        public List<Cluster> Clusters = new List<Cluster>();

        public Universe(int id)
        {
            Id = id;

        }




    }

    public class Moon : Planet
    {


    }

    public class Planet
    {
        public int Id; //Within the solar system
        public string Name;
        public float HueShift;
        public float OrbitSpeed;
        public float RotationSpeed;
        public float OrbitDistance;
        public PlanetType Type;
        public int SubType;
        public int Seed;
        public int RingType;
        public float Mass;
        public float PolarCoverage;
        public Atmosphere Atm;
        public Vector3 Pos;

        public List<Moon> Moons = new List<Moon>();


        public Planet(float orbitalDistance, float solarTemperature) // Autoselects the suitable type for the planet based its suns distance and temperature.
        {
            Type = new PlanetType(orbitalDistance, solarTemperature);
            Name = "Unnamed";
            SubType = Random.Range(1, 8);
            RingType = RingTypeProbability();
            Seed = Random.Range(1, 9999);
            HueShift = Random.Range(-0.2f, 0.2f);
            Mass = MassProbability(Type.Name);
            OrbitDistance = orbitalDistance;
            Pos = SpaceMath.RandomPositionOnCircle(orbitalDistance, new Vector3(0, 0, 0));
            OrbitSpeed = SpaceMath.GetOrbitSpeed(orbitalDistance);
            RotationSpeed = Random.Range(0.5f, 8);
            PolarCoverage = 0;
            Atm = Type.RandomAtmosphere;

     
        }

        public Planet(string type) //Forced planet type. eg. ice planets near the sun
        {
            Type = new PlanetType(type);
            Name = "Unnamed";
            SubType = Random.Range(1, 8);
            RingType = RingTypeProbability();
            Seed = Random.Range(1, 9999);
            HueShift = Random.Range(-0.2f, 0.2f);
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
        }


        public Planet()
        {
            Type = new PlanetType();
            Name = "Unnamed";
            SubType = Random.Range(1, 8);
            RingType = RingTypeProbability();
            Seed = Random.Range(1, 9999);
            HueShift = Random.Range(-0.15f, 0.15f);
            Mass = MassProbability(Type.Name);
            PolarCoverage = 0;
            Atm = Type.RandomAtmosphere;
        }

    public void GenerateMoons(Vector3 ParentPos)
            {
            float orbitDistance = 12f;
            if (RingType == 1) { orbitDistance += 1f; }
            else if (RingType == 2) { orbitDistance += 4f; }
            else if (RingType == 3) { orbitDistance += 5f; }
            else if (RingType == 4) { orbitDistance += 2.3f; }

            int numberOfMoons = MoonNumberProbability(RingType);
            for (int i = 0; i < numberOfMoons; i++)
            {
                Moon Moon = new Moon();
                Moon.Type = MoonTypeProbability();
                Moon.Pos = SpaceMath.RandomPositionOnCircle(orbitDistance, Pos);
                orbitDistance += 4;
                Moons.Add(Moon);
                }
            }

        float MassProbability(string type)
        {
            float mass;
            //gas planets are big
            if (type == "Gas")
            {
                mass = Random.Range(1.2f, 1.4f);
            }
            else 
            {
                mass = Random.Range(0.5f, 1f);
            }
            return mass;
        }

        PlanetType MoonTypeProbability()
        {
                
            //60% moons are minor planet-type
            if (Random.Range(1, 11) <= 6)
                {
                    PlanetType MoonType = new PlanetType("Lava"); ;
                    return MoonType;
                }
            else
            {
                PlanetType MoonType = new PlanetType();
            return MoonType;
            }
                    
        }

        int MoonNumberProbability(int ringType)
        {
            int moonNumber = 0;
            //80% planets have no moon
            if (Random.Range(1, 11) <= 6)
            {
                moonNumber = 0;
            }
            else
            {
                if (ringType == 1) { moonNumber = Random.Range(1, 3); }
                else if (ringType == 2) { moonNumber = 1; }
                else if (ringType == 3) { moonNumber = Random.Range(1, 2); }
                else { moonNumber = Random.Range(1, 4); }
            }
            return moonNumber;
        }

        int RingTypeProbability()
        {
            //70% planets have no ring
            if (Random.Range(1, 11) <= 7)
            {
                return 0;
            }
            else
                return Random.Range(1, 5);
        }
    }

    public class Star
    {
        public Vector3 Pos;
        public StarType Type;
        public string Name;
        public float SolarWindStrenght;
        public float SolarTemperature;
        public int NumOfPlanets;
        public int Id;


        public List<Planet> Planets = new List<Planet>();
        public List<AsteroidBelt> AsteroidBelts = new List<AsteroidBelt>();
        public List<Comet> Comets = new List<Comet>(40);

        public void GenerateComets()
        {
            int numberOfComets = Random.Range(0, 1);
            for (int i = 0; i < numberOfComets; i++)
            {
                Comet Comet = new Comet();
                Comets.Add(Comet);
            }
        }


       /*
        public void GenerateBelts(int numberOfPlanets)
        {
            float orbitDistance = 10f + SpaceMath.StandardDeviation(40f, 15f, Id);

            for (int planetIndex = 0; planetIndex < NumOfPlanets; planetIndex++)
            {

                Planet Planet = new Planet(orbitDistance, SolarTemperature);
                Planet.Id = planetIndex;
                Planet.Name = nameList.getPlanetName(Name, planetIndex);
                orbitDistance += Mathf.Abs(SpaceMath.StandardDeviation(30f, 15f, Star.Id * Planet.Id * 500));
                // Planet.GenerateMoons(Planet.Pos);
                Star.Planets.Add(Planet);

            }

        }*/

        /*
        public List<Planet> GeneratePlanetList()
        {
            for (int j = 0; j < NumOfPlanets; j++)
            {

                Planet Planet = new Planet(orbitDistance, Star.SolarTemperature);
                Planet.Id = j;
                Planet.Name = nameList.getPlanetName(Star.Name, j);
                orbitDistance += Mathf.Abs(SpaceMath.StandardDeviation(30f, 15f, Star.Id * Planet.Id * 500));
                // Planet.GenerateMoons(Planet.Pos);
                Star.Planets.Add(Planet);

            }
        }*/





        public Star()
        {
            Name = "Unnamed Star";
            Vector3 middlePos = new Vector3(0, 0, 0);
            float randomHeight = Random.Range(5f, 50f); ;
            Pos = SpaceMath.RandomPositionOnCircle(0, 200, middlePos) + new Vector3(0, randomHeight,0 );
            StarFormation starFormation = new StarFormation();
            StarType starType = starFormation.GetRandomTypeStar();
            Type = starType;
            SolarWindStrenght = Mathf.Clamp((Random.Range(0,1f)),0f, 0.8f);
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

    public class Comet
    {
        //Comet types:
        //0 - non-existing
        //1 - trail
        // more ?

        public int Type;
        public float SemiMajorAxis_A;
        public float SemiMinorAxis_B;
        public float Inclination;
        public float Direction;

        public Comet()
        {
            Type = 1;
            SemiMajorAxis_A = Random.Range(120, 320);
            SemiMinorAxis_B = Random.Range(100, 200); 
            Inclination = Random.Range(-10f, 10f);
            Direction = Random.Range(0, 360);
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
            NumOfStars = Random.Range(10, 20);
            Color = Random.Range(1, 4);
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public int NumOfStars { get; set; }
        public int Color { get; set; }

        public List<Star> Stars = new List<Star>();

    }

    public class SpaceMath : MonoBehaviour
    {

        
        public static float GetOrbitSpeed(float orbitDistance)
        {
            float orbitSpeed = 0;
            orbitSpeed =  (600f / ((1f / 27f) * orbitDistance * Mathf.PI * orbitDistance)) * Random.Range(2, 7);
            return orbitSpeed;
        }


        public static float GetEffectiveTemperature(float distance, float albedo, float solarTemp)
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

        public static Vector3 RandomPositionOnCircle(float minDistance,float maxDistance, Vector3 middlePos)
        {
            float RandomDistanceFromCenter = Random.Range(minDistance, maxDistance);
            float RandomRotation = Random.Range(0f, 6.28318531f);
            float tempX = middlePos.x + Mathf.Cos(RandomRotation) * RandomDistanceFromCenter;
            float tempZ = middlePos.z + Mathf.Sin(RandomRotation) * RandomDistanceFromCenter;

            Vector3 PlanetPos = new Vector3(tempX, 0f, tempZ);
            return PlanetPos;
        }

        public static Vector3 RandomPositionOnCircle(float distance, Vector3 middlePos)
        {
            float RandomDistanceFromCenter = Random.Range(0f, 6.28318531f);
            float tempX = middlePos.x + Mathf.Cos(RandomDistanceFromCenter) * distance;
            float tempZ = middlePos.z + Mathf.Sin(RandomDistanceFromCenter) * distance;

            Vector3 PlanetPos = new Vector3(tempX, 0f, tempZ);
            return PlanetPos;
        }

        public static float GetOrbitalDistance(Vector3 pos)
        {
            float distance = Vector3.Distance(pos, new Vector3(0f, 0f, 0f));
            return distance;
        }

        public static float StandardDeviation(float mean, float standardDeviation)
        {
            return StandardDeviation(mean, standardDeviation, 0);
        }


        public static float StandardDeviation(float mean, float standardDeviation, int seed)
        {
        
            System.Random Random = new System.Random(seed);

            // Box-Muller transform
            float u1 = 1.0f - (float)Random.NextDouble();
            float u2 = 1.0f - (float)Random.NextDouble();
            float normalDistribution = Mathf.Sqrt(-2.0f * Mathf.Log10(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)

            float gauss = mean + standardDeviation * normalDistribution;

            return gauss;
        }
    }

}