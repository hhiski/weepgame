using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NoiseSpace;
using ColorSpace;

public class StarSurface : MonoBehaviour
{
    public CelestialBody.Star Star;

    NoiseFunctions NoiseFunctions = new NoiseFunctions();
    ColorFunctions ColorFunctions = new ColorFunctions();

    Mesh mesh;
    Mesh sharedMesh;
    Vector3[] vertices;

    Noise NoiseLayerA, NoiseLayerB;


    Color[] colors;
    public Color starColor;
    public Color starColorCold;

    public bool surfaceWaves = false;
    public float amplitude = 0;
    public float frequency = 0;
    public float powerLevel = 1;


    public float phaseSpeed = 0.15f;
    public float timeD = 0;


    public bool ridges = false;
    public bool cheese = false;
    public bool invert = false;
    public bool power = false;
    // Start is called before the first frame update
    void Start()
    {



    }

    public void InitStar()
    {

        NoiseLayerA = new Noise(5);
        NoiseLayerB = new Noise(75 + 3);

        sharedMesh = GetComponent<MeshFilter>().sharedMesh;
        mesh = Instantiate(sharedMesh);
        GetComponent<MeshFilter>().sharedMesh = mesh;

        vertices = sharedMesh.vertices;

        colors = new Color[vertices.Length];

        starColor = Star.Type.StarColor;
        starColorCold = Star.Type.StarColorCold;

        if (this.GetComponent<Renderer>() != null)
        {

            this.GetComponent<Renderer>().material.SetColor("_Color", starColor);
            this.GetComponent<Renderer>().material.SetColor("_RimColor", starColorCold);

        }

    }



    void LateUpdate()
    {
        if (surfaceWaves)
        {
            if (Time.frameCount % 2 == 0)
            {

                UpdateNoise();

            }
            if (Time.frameCount % 1 == 0)
            {

                UpdateColor(vertices);
            }
        }

    }

    /*
    float EvaluateNoise(Vector3 point, Noise noiseFilter, float period, float amplitude, int flowDir)
    {
        float timeDimension =  (flowDir * timeD);
        float noise = noiseFilter.Evaluate(new Vector3(point.x * period + timeDimension, point.y * period + timeDimension, point.z * period + timeDimension));

        if (ridges)
        {
            noise = Mathf.Abs(noise) - 0.5f;
        };

        if (power)
        {
            noise = Mathf.Pow(noise, 3);
        };


        if (cheese)
        {


            if (noise >= 0.7f) { noise = -noise; }
            if (noise <= 0.23f && noise >= -0.23f) { noise = -0.23f; }
            if (noise <= 0.1f) { noise = -0.1f; }

            if (noise < 0f) { noise = 1.1f * noise; }


        };
        if (invert)
        {
            noise = -noise;
        };
        
        noise = 1 + (noise * amplitude);
        return noise;
    }*/


    void UpdateNoise()
    {
        vertices = sharedMesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 point = vertices[i];
            /*
            Vector3 point = vertices[i];


            float timeDimension =  phaseSpeed * Time.time;

            float noiseA = NoiseFunctions.PerlinFilter(point, NoiseLayerA, period,1, amplitude, timeDimension);
            float noiseB = NoiseFunctions.PerlinFilter(point, NoiseLayerA, period, 1, amplitude, -timeDimension-100);

            if (ridges)
            {
                noiseA = Mathf.Abs(noiseA) - 0.5f;
            };

            if (power)
            {
                noiseB = Mathf.Pow(noiseB, powerLevel);
                noiseA = Mathf.Pow(noiseA, powerLevel);
            };


            if (cheese)
            {


                if (noiseA >= 0.7f) { noiseA = -noiseA; }
                if (noiseA <= 0.23f && noiseA >= -0.23f) { noiseA = -0.23f; }
                if (noiseA <= 0.1f) { noiseA = -0.1f; }

                if (noiseA < 0f) { noiseA = 1.1f * noiseA; }


            };
            if (invert)
            {
                noiseA = -noiseA;
            };

            float noise = 1 + (0.5f * amplitude* noiseA + 0.5f * amplitude * noiseB);

            point = point *= noise;
            */
            float timeDimension = Time.time * phaseSpeed;
            float perlinNoise = NoiseFunctions.SimplePerlinFilter( point, frequency, timeDimension);
            float noise = 1 + amplitude * (perlinNoise );

            point = point * noise;
            vertices[i] = point;
        }

        mesh.vertices = vertices;
    }

    void UpdateColor(Vector3[] vertices)
    {
        colors = new Color[vertices.Length];
        Vector3 point;

        for (int i = 0; i < vertices.Length; i++)
        {
            point = vertices[i];
            float colorSpectrum = ((1 - point.magnitude) / amplitude) * 1.25f;

            //Add colour
            // float dist = Vector3.Distance(point, transform.position);
            //  float colorSpectrum = ((dist - (radius- amplitude)) / ((radius + amplitude) - (radius - amplitude)));
        colors[i] = Color.Lerp(starColor, starColorCold, colorSpectrum);

        }
        mesh.colors = colors;
    }
}
