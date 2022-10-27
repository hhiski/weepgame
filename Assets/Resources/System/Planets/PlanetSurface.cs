using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using NoiseSpace;
using ColorSpace;
using static CelestialBody;

[System.Serializable]
public class PlanetSurface : MonoBehaviour
{

    Mesh planetMesh;
    Mesh seaMesh;

    Color[] colors;

    Vector3[] planetVerticesBackup;
    Vector3[] seaVerticesBackup;

    Noise NoiseLayer;
    Noise SecondNoiseLayer = new Noise(4512);
    Noise ThirdNoiseLayer = new Noise(777);

    NoiseFunctions NoiseFunctions = new NoiseFunctions();
    ColorFunctions ColorFunctions = new ColorFunctions();

    System.Random Random = new System.Random();

    [Range(0.0f, 1.0f)]
    public float Amplitude = 0.03f;
    [Range(0, 10)]
    public int levels = 1;
    [Range(0f, 10f)]
    public float Frequency = 1.8f;
    [Range(0f, 1f)]
    public float roughness = 0.5f;
    [Range(0f, 2f)]
    public float Persistence = 1;


    public AnimationCurve heightCurve = AnimationCurve.Linear(-1f, -1f, 1f, 1f);
    public AnimationCurve modifiedHeightCurve = AnimationCurve.Linear(-1f, -1f, 1f, 1f);

    public float surfaceVariation = 0f;

    public float negativePeakClip = 0f;
    public float positivePeakClip = 2f;
    public float seaLevel = 1f;
    public float craterAmplitude = 0;
    public float craterSize = 0;
    public int craterNum = 0;

    public bool invertTerrain = false;
    public bool ridges = false;

    public bool doubleRidges = false;
    public bool antiRidges = false;
    public float ridgePower = 1;
    public bool polkaDots = false;
    public bool gasSurface = false;
    public bool fluidic = false;
    public bool weirdA = false;
    public bool supercontinentsB = false;
    public bool supercontinents = false;

    public bool blotches = false;
    public bool wrinkle = false;
    public bool warp = false;
    public float warpForce = 3f;
    public bool cheese = false;

    public int crystalStructuresNum = 0;
    

    public Color colorHigh = new Color(0, 0.5f, 1, 1);
    public Color colorMid = new Color(0, 0.5f, 1, 1);
    public Color colorLow = new Color(0.3f, 0, 0.5f, 1);
    public Color colorHighShifted = new Color(0, 0.5f, 1, 1);
    public Color colorMidShifted = new Color(0, 0.5f, 1, 1);
    public Color colorLowShifted = new Color(0.3f, 0, 0.5f, 1);
    public Color colorSea = new Color(0.1f, 0.3f, 0.9f, 1);
    public Color colorSeaShifted = new Color(0.1f, 0.3f, 0.9f, 1);

    public float colorHeightHigh = 1.6f;
    public float colorHeightMid = 1.02f;
    public float colorHeightLow = 1.00f;
    public float colorHeightDeep = 0.95f;
    public float colorStripify = 1f;
    public float colorHueShift = 0f;

    public float atmosphereThickness = 0f;
    public int cloudType = 0;

    public float polarCoverage = 0f;
    public float mass = 1;
    public float rotationSpeed = 1;

    public Planet Planet;

    GameObject Sea;

    public int seed = 1;
    public int subType = 0;
    string type = "empty";
    float temperature = -273;

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B) == true)
        {
            ShapePlanetSurface();
        }

    }

    public void UpdateSea()
    {
        if (Sea != null)
        {
            seaMesh.vertices = seaVerticesBackup;
            Vector3[] seaVertices = seaMesh.vertices;

            Color[] colors = new Color[seaVertices.Length];

            for (int i = 0; i < seaVertices.Length; i++)
            {
                seaVertices[i] *= seaLevel;
                colors[i] = colorSeaShifted;

            }
            seaMesh.colors = colors;
            seaMesh.vertices = seaVertices;
        };
    }



    public void SetValues()
    {
        seed = Planet.Seed;
        type = Planet.Type.Name;
        temperature = Planet.Atm.Temperature;
        subType = Planet.SubType;
        colorHueShift = Planet.HueShift;
        rotationSpeed = Planet.RotationSpeed;
        polarCoverage = Planet.PolarCoverage;
        atmosphereThickness = Planet.Atm.Pressure;
        mass = Planet.Mass;
        bool polarCaps = Planet.Type.PolarCaps;
        polarCoverage = PolarCapsCoverage(polarCaps, polarCoverage);
        surfaceVariation = Planet.Type.SurfaceVariation;

        Random = new System.Random(seed);
    }

    float PolarCapsCoverage(bool polarCaps, float polarCoverage)
    {
        if (polarCoverage == 0) //polarCoverage not already set;
        {
            if (polarCaps) //planet type has caps
            {

                if (Planet.Type.Name == "Water")
                {
                    polarCoverage = (-temperature * 0.017f + 0.7f) * 1.2f;
                }
                else if (temperature < 30)
                {
                    polarCoverage = (float)Random.NextDouble() * 0.11f + 0.01f;
                }
            }
        }
        return polarCoverage;
    }

    public void CopyVertices()
    {
        Mesh planetSharedMesh = GetComponent<MeshFilter>().sharedMesh;
        planetMesh = Instantiate(planetSharedMesh);
        GetComponent<MeshFilter>().sharedMesh = planetMesh;
        Vector3[] planetVertices = planetMesh.vertices;
        planetVerticesBackup = planetMesh.vertices;
        colors = new Color[planetVertices.Length];

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "Sea" || child.gameObject.name == "Lava" || child.gameObject.name == "Mud")
            {
                Sea = child.gameObject;
                Mesh seaSharedMesh = Sea.GetComponent<MeshFilter>().sharedMesh;
                seaMesh = Instantiate(seaSharedMesh);
                Sea.GetComponent<MeshFilter>().sharedMesh = seaMesh;
                seaVerticesBackup = seaMesh.vertices;
            }
        }


    }


    public void ShapePlanetSurface()
    {
        planetMesh.vertices = planetVerticesBackup;


        this.transform.parent.GetComponent<PlanetSubTypes>().GetSubTypeValues(this, type, subType); //Overwrites some values from subType

        UpdateSurfaceVariability();
        UpdateSurface();
        UpdateSurfaceColors();
        UpdateFlatColors();

        UpdatePolkaDots();
        UpdateCrystalStructures();


        UpdateCraters();

        UpdateSea();

    }


    Vector3 SurfacePointPatterns(Vector3 point)
    {
        if (colorStripify != 1)
        {
            point.x = point.x * colorStripify;
            point.z = point.z * colorStripify;
        };
        return point;
    }

    float SurfaceNoisePatterns(float noise, Vector3 point, Noise noiseFilter, int level)
    {
        noise = modifiedHeightCurve.Evaluate(noise);


        if (supercontinents)
        {
            if (level == 1)
            {

                Vector3 pointB = new Vector3(point.x + 2512, point.y + 1512, point.z + 412);
                float continentNoise = NoiseFunctions.PerlinFilter(pointB, NoiseLayer, Frequency, 1, noise, 0);

                if (noise < continentNoise)
                {
                    noise = continentNoise;
                }


            }
        };

        if (supercontinentsB)
        {
            float continentalNoise = NoiseFunctions.PerlinFilter(point, NoiseLayer, Frequency, 1, noise, 6850);

            if (continentalNoise > 0.2f)
            {
                noise = noise * 1.20f;
            }
            else if (continentalNoise > 0.3f)

                noise = noise + 0.36f;

        }

        if (fluidic)
        {
            point = new Vector3(point.x * noise + seed, point.y * noise + (seed / 2), point.z * noise + (seed / 3));
            noise = noiseFilter.Evaluate(point);
        };



        if (wrinkle)
        {
            float wrinkleNoise = NoiseFunctions.PerlinFilter(point / level, NoiseLayer, 1.4f, 1, noise, 2883);

            if (wrinkleNoise > 0.2f)
            {
                noise =  Mathf.Abs(noise)- 0.5f;

            }
        };

        if (warp)
        {
            float xDistortion = noiseFilter.Evaluate(warpForce * (new Vector3(point.x + 2.3f, point.y + 2.9f, point.z + 2.2f)));
            float yDistortion = noiseFilter.Evaluate(warpForce * (new Vector3(point.x + 3.1f, point.y + 4.2f, point.z + 3.5f)));
            float zDistortion = noiseFilter.Evaluate(warpForce * (new Vector3(point.x + 5.3f, point.y + 5.9f, point.z + 4.2f)));
            noise = noise * noiseFilter.Evaluate((new Vector3(point.x + xDistortion, point.y + yDistortion, point.z + zDistortion)));
        };

        if (cheese)
        {
            
            float cheeseNoise = NoiseFunctions.PerlinFilter(point / level, NoiseLayer, Frequency*0.9f, 1, noise, 4883);

            if (cheeseNoise > 0.3f)
            {
                noise = noise - (1.333f * noise - 0.9333f);

            }



        };



        if (ridges)
        {
            float secondNoise = 0;
            float ridgeNoise = 1;
            if (antiRidges)
            {
                secondNoise = SecondNoiseLayer.Evaluate(point);
                secondNoise = Mathf.Abs(secondNoise);

            }
            ridgeNoise = Mathf.Abs(noise) - 0.5f;
            ridgeNoise = Mathf.Lerp(ridgeNoise, noise, secondNoise);

            noise = Mathf.Pow(ridgeNoise, ridgePower);
        };

        if (doubleRidges)
        {
            float antiRidgeNoise = SecondNoiseLayer.Evaluate(point);
            antiRidgeNoise = Mathf.Abs(antiRidgeNoise);
            noise += antiRidgeNoise;
        };


        if (invertTerrain) { noise = -noise; }



        return noise;
    }

    void UpdateSurfaceVariability()
    {
        Keyframe[] keys = heightCurve.keys;

        float heightValue;

        for (var keyIndex = 0; keyIndex < keys.Length; keyIndex++)
        {

            heightValue = keys[keyIndex].value;
            heightValue = heightValue + (((float)Random.NextDouble() * 2f - 1) * surfaceVariation);
            keys[keyIndex].value = heightValue;

        }

        modifiedHeightCurve.keys = keys;
    }

    public void UpdateSurface()
    {
        Vector3[] vertices = planetMesh.vertices;
        List<Vector3> terrain = new List<Vector3>();

        NoiseLayer = new Noise(seed);

        float subAmplitude = Amplitude * roughness;
        float subFrequency = Frequency;
        float noise = 0;
        float iceHeight = 1;
        Vector3 point;


            
      

        foreach (Vector3 vertex in vertices) // First harmonic
        {
            point = vertex;

            Vector3 modPoint = SurfacePointPatterns(point);
            noise = NoiseFunctions.PerlinFilter(modPoint, NoiseLayer, Frequency, 1, subAmplitude, 0);
            noise = SurfaceNoisePatterns(noise, point, NoiseLayer, 1);
            noise = 1 + (noise * subAmplitude);
            point = point *= noise;
            terrain.Add(point);
        }

        for (int currentLevel = 2; currentLevel < levels; currentLevel++) // Higher harmonics
        {
            subAmplitude = (subAmplitude - (subAmplitude * roughness)) + ((Mathf.Pow(1 - roughness, levels) * Amplitude) / levels);
            subFrequency = currentLevel * Frequency * Persistence;

            for (int i = 0; i < terrain.Count; i++)
            {
                point = terrain[i];
            
                Vector3 modPoint = SurfacePointPatterns(point);
                noise = NoiseFunctions.PerlinFilter(modPoint, NoiseLayer, subFrequency, currentLevel, subAmplitude, 0);
                noise = SurfaceNoisePatterns(noise, modPoint, NoiseLayer, currentLevel);
                noise = 1 + (noise * subAmplitude);
                point = point *= noise;

                if (Mathf.Abs(point.y) > (1 - polarCoverage) + 0.1f)
                {
                    iceHeight = 1;
                    if (point.magnitude < seaLevel) { iceHeight = (seaLevel / iceHeight) + 0.004f; };
                    point = new Vector3(point.x * iceHeight, point.y * iceHeight, point.z * iceHeight);
                }

                if (point.magnitude < negativePeakClip)
                {
                    point = new Vector3(vertices[i].x * negativePeakClip, vertices[i].y * negativePeakClip, vertices[i].z * negativePeakClip);
                }

                if (point.magnitude >= positivePeakClip)
                {
                    point = new Vector3(vertices[i].x * positivePeakClip, vertices[i].y * positivePeakClip, vertices[i].z * positivePeakClip);
                }

                terrain[i] = point;
            }
        }

        for (int i = 0; i < terrain.Count; i++)
        {
            vertices[i] = terrain[i];
        }

        planetMesh.vertices = vertices;
        planetMesh.RecalculateNormals();
    }

    void HueShiftColors()
    {
        float hueVariation = 1.2f * colorHueShift;
        float highColorHueVariation = colorHueShift + (float)Random.NextDouble() * hueVariation - 0.5f* hueVariation;
        float midColorHueVariation = highColorHueVariation + (float)Random.NextDouble() * hueVariation - 0.5f* hueVariation;
        float lowColorHueVariation = midColorHueVariation + (float)Random.NextDouble() * hueVariation - 0.5f* hueVariation;


        colorHighShifted = ColorFunctions.HueShiftColor(colorHigh, highColorHueVariation, 1);
        colorMidShifted = ColorFunctions.HueShiftColor(colorMid, midColorHueVariation, 1);
        colorLowShifted = ColorFunctions.HueShiftColor(colorLow, lowColorHueVariation, 1);
        colorSeaShifted = ColorFunctions.HueShiftColor(colorSea, colorHueShift, 1);
    }


    void UpdateFlatColors()
    {
        if (gasSurface == true)
        {
            planetMesh.vertices = planetVerticesBackup;
        }

    }

    void Update()
    {
        transform.RotateAround(gameObject.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void UpdateSurfaceColors()
    {
        Vector3[] vertices = planetMesh.vertices;

        Vector3 point;
        float colorNoise = 0;

        Noise noiseLayer = new Noise(seed + 4512);

        HueShiftColors();
        Color highGround = colorHighShifted;
        Color middleGround = colorMidShifted;
        Color lowGround = colorLowShifted;

        for (int i = 0; i < vertices.Length; i++)
        {
            point = vertices[i];

            colorNoise = (NoiseFunctions.PerlinFilter(point, noiseLayer, 2.1f, 1, 0.15f, 0));
            colorNoise = 1 + (colorNoise * 0.15f);

            float height = point.magnitude;
            float heighSpectrum = 0.5f;
            float latitude = point.y;

            //Latitude based coloring
            if (polarCoverage > 0.001f)
            {
                lowGround = ColorFunctions.LatitudeBasedColoring(colorLowShifted, colorNoise, latitude, polarCoverage);
                middleGround = ColorFunctions.LatitudeBasedColoring(colorMidShifted, colorNoise, latitude, polarCoverage);
                highGround = ColorFunctions.LatitudeBasedColoring(colorHighShifted, colorNoise, latitude, polarCoverage);
            }

            /*
            //polar caps?
            if (Mathf.Abs(latitude) > 0.88f)
            {
                if (Mathf.Abs(latitude) < 0.93)
                {
                    height += Mathf.Abs(latitude) + 0.01f;
                }
                else
                {
                    height += 0.025f;
                }
            }*/

            //Elevation based coloring
            if (height > colorHeightHigh)
            {
                colors[i] = highGround;
            }
            else if (height <= colorHeightHigh && height > colorHeightMid)
            {
                heighSpectrum = (height - colorHeightMid) / (colorHeightHigh - colorHeightMid);
                colors[i] = Color.Lerp(middleGround, highGround, heighSpectrum);
            }
            else if (height <= colorHeightMid && height > colorHeightLow)
            {
                colors[i] = middleGround;
            }
            else if (height <= colorHeightLow && height > colorHeightDeep)
            {
                heighSpectrum = (height - colorHeightDeep) / (colorHeightLow - colorHeightDeep);
                colors[i] = Color.Lerp(lowGround, middleGround, heighSpectrum);
            }
            else if (height <= colorHeightDeep)
            {
                colors[i] = lowGround;
            }




        }

        /*
         for (int i = 0; i < colors.Length - colors.Length % 3; i += 3)
         {
             colors[i + 1] = colors[i];
             colors[i + 2] = colors[i];
         }
         */

        planetMesh.colors = colors;

    }

    void SplitMesh()
    {

        Vector3[] vertices = planetMesh.vertices;
        Vector3[] normals = planetMesh.normals;
        int[] triangles = planetMesh.triangles;
        var newVertices = new Vector3[triangles.Length];
        var newNormals = new Vector3[triangles.Length];
        var newTriangles = new int[triangles.Length];

        for (var i = 0; i < triangles.Length; i++)
        {
            newVertices[i] = vertices[triangles[i]];
            newNormals[i] = normals[triangles[i]];
            newTriangles[i] = i;
        }

        planetMesh.vertices = newVertices;
        planetMesh.normals = newNormals;
        planetMesh.triangles = newTriangles;

    }
    void UpdateCrystalStructures()
    {
        if (crystalStructuresNum>0)
        {
            Random = new System.Random(seed + 3);
            Vector3[] vertices = planetMesh.vertices;
            Vector3[] peakVertices = new Vector3[crystalStructuresNum];
            // List<Vector3> terrain = new List<Vector3>();
            // terrain.Add(point);
            Color polkaColor = ColorFunctions.SaturationShiftColor(colorMidShifted, 0f, 1.2f);
            polkaColor = ColorFunctions.HueShiftColor(colorMidShifted, 0.0f, 1.1f);
            polkaColor = ColorFunctions.ValueShiftColor(colorMidShifted, 1.05f, 1.0f);
            float distance = 0;

            float structureSize = 0.2f;

            for (int i = 0; i < peakVertices.Length; i++)
            {
                int randomPoint = Random.Next(0, vertices.Length);
                peakVertices[i] = vertices[randomPoint];
            }


            for (int j = 0; j < peakVertices.Length; j++)
            {


                for (int i = 0; i < vertices.Length; i++)
                {

                    distance = Vector3.Distance(vertices[i], peakVertices[j]);

                    if (distance <= structureSize )
                    {
 
                        float arcDistance = Mathf.Asin(distance / 2f);

                        float ratio = 1f- (distance / structureSize);

                        //  float angle = Vector3.Angle(vertices[i], peakVertices[j]);
                        Vector3 newPoint = vertices[i] + Vector3.Normalize(peakVertices[j]) * 0.9f* ratio;
                        vertices[i] = newPoint;


                    }


                }

            }

            planetMesh.colors = colors;

            planetMesh.vertices = vertices;
        }
    }

    void UpdatePolkaDots()
    {
        if (polkaDots)
        {
            Random = new System.Random(seed + 3);
            Vector3[] vertices = planetMesh.vertices;
            Vector3[] polkaVertices = new Vector3[35];
            // List<Vector3> terrain = new List<Vector3>();
            // terrain.Add(point);
            Color polkaColor = ColorFunctions.SaturationShiftColor(colorMidShifted, 0f, 1.2f);
            polkaColor = ColorFunctions.HueShiftColor(colorMidShifted, 0.0f, 1.1f);
            polkaColor = ColorFunctions.ValueShiftColor(colorMidShifted, 1.05f, 1.0f);
            float distance = 0;
            float polkaSize = 0.55f;
            for (int i = 0; i < polkaVertices.Length; i++)
            {
                int randomPoint = Random.Next(0, vertices.Length);
                polkaVertices[i] = vertices[randomPoint];
            }


            for (int j = 0; j < polkaVertices.Length; j++)
            {

                polkaSize = SpaceMath.StandardDeviation(craterSize, 0.7f*craterSize, seed+j); ;

                for (int i = 0; i < vertices.Length; i++)
                {

                    distance = Vector3.Distance(vertices[i], polkaVertices[j]);

                    if (distance <= polkaSize && distance > polkaSize * 0.80f - 0.05f)
                    {
                        float absDistance = Mathf.Abs(distance - polkaSize * 0.80f - 0.05f);
                        float edgeHighlight = 1 + Mathf.InverseLerp(0, 0.1f, absDistance) * 0.65f;

                        colors[i] = ColorFunctions.ValueShiftColor(colors[i], 0, edgeHighlight);

                    }

                    else if (distance <= polkaSize * 0.80f - 0.05f)
                    {

                        colors[i] = ColorFunctions.SaturationShiftColor(colors[i], 0, 0.85f);

                    }

                }

            }

            planetMesh.colors = colors;
        }
    }

    void UpdateCraters()
    {
        Random = new System.Random(seed);
        Vector3[] vertices = planetMesh.vertices;

        for (int i = 0; i < craterNum; i++)
        {
            int craterId = Random.Next(0, vertices.Length);
            Vector3 craterPos = vertices[craterId];
            // Vector3 craterWorldPos = transform.TransformPoint(craterPos);
            Color craterColor = ColorFunctions.SaturationShiftColor(colorLowShifted, 0, 0.9f);
            craterColor = ColorFunctions.ValueShiftColor(craterColor, 0, 0.95f);

            List<Vector3> craterVertices = new List<Vector3>();

            // float randomCraterSize = (float)Random.NextDouble() * craterSize
            float randomCraterSize = SpaceMath.StandardDeviation(craterSize, 0.6f * craterSize); ;

            for (int j = 0; j < vertices.Length; j++)
            {
                // Vector3 verticeWorldPos = transform.TransformPoint(vertices[i]);

                //  float distance = Vector3.Distance(verticeWorldPos, craterWorldPos);
                float distance = Vector3.Distance(vertices[j], craterPos);

                if (distance <= randomCraterSize)
                {

                    float rDistance = distance / randomCraterSize;

                    float rDepth = 0;
                    colors[j] = 0.4f * colors[j] + 0.6f * craterColor;
                    rDepth = -0.5f * Mathf.Pow(-Mathf.Pow(rDistance, 2) + 1.4f, 0.5f) + 0.32f;
                    //  planetMesh.colors = colors;

                    vertices[j] = vertices[j] * (1 + (rDepth * craterAmplitude));
                }
            }


        }


        planetMesh.vertices = vertices;
        planetMesh.colors = colors;
        planetMesh.RecalculateNormals();
    }



}

