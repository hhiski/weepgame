using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorSpace;

public class PlanetClouds : MonoBehaviour
{


    Mesh cloudMesh;
    Mesh cloudSharedMesh;

    Vector3[] vertices;
    Vector3[] verticesBackup;
    Noise NoiseLayer;
    Noise SecondNoiseLayer;

    ColorFunctions ColorFunctions = new ColorFunctions();

    public Color cloudColor = new Color(1, 1, 1, 1);
    public Color shiftedCloudColor = new Color(1, 1, 1, 1);
    public float cloudPower = 1.00f;
    public float cloudSwirl = 0.4f;
    public float Amplitude = 1;
    public float Frequency = 1;
    public float stripify = 1f;
    public int seed = 667;

    void Start()
    {
        /*
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Clouds")
            {
                cloud = child.gameObject;
                // MeshFilter SeaGlow = child.gameObject.GetComponent<MeshFilter>().sharedMesh;
                cloudSharedMesh = cloud.GetComponent<MeshFilter>().sharedMesh;
                cloudMesh = Instantiate(cloudSharedMesh);
                child.GetComponent<MeshFilter>().sharedMesh = cloudMesh;
            }
        }*/
        // MeshFilter SeaGlow = child.gameObject.GetComponent<MeshFilter>().sharedMesh;

        /*
        if (cloud != null)
        {
            cloud = Instantiate(cloud, this.transform) as GameObject;
            cloudSharedMesh = cloud.GetComponent<MeshFilter>().sharedMesh;
            cloudMesh = Instantiate(cloudSharedMesh);
            cloud.GetComponent<MeshFilter>().sharedMesh = cloudMesh;
        }*/


        cloudSharedMesh = this.gameObject.GetComponent<MeshFilter>().sharedMesh;
        cloudMesh = Instantiate(cloudSharedMesh);
        this.GetComponent<MeshFilter>().sharedMesh = cloudMesh;

        NoiseLayer = new Noise(seed);
        SecondNoiseLayer = new Noise(seed+563);
        ShapePlanetClouds();
    }

    void Update()
    {
        this.gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, 4f * Time.deltaTime);
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B) == true)
        {
            ShapePlanetClouds();
        }

    }

    public void SetSeeds()
    {
        NoiseLayer = new Noise(seed);
        SecondNoiseLayer = new Noise(seed + 4512);
    }

    float PerlinFilter(Vector3 point, Noise noiseFilter, float frequency, float amplitude)
    {

        point.x = point.x * stripify;
        point.z = point.z * stripify;
        point = new Vector3(point.x * frequency, point.y * frequency, point.z * frequency);
        float noise = noiseFilter.Evaluate(point);

        noise = noiseFilter.Evaluate(point * (1 + cloudSwirl * noise));
  
        noise = (((noise)+1)/2)* amplitude;
        noise = Mathf.Pow(noise, cloudPower);
        return noise;
    }

    void UpdateCloudColor()
    {

        float colorHueShift = this.gameObject.transform.parent.GetComponent<PlanetSurface>().colorHueShift;


        Color newCloudColor = ColorFunctions.HueShiftColor(cloudColor, colorHueShift, 1);

        shiftedCloudColor = newCloudColor; 


    }

    void UpdateCloudLevel()
    {
        List<float> alphaList = new List<float>();
        vertices = cloudSharedMesh.vertices;
        Color[] colors = new Color[vertices.Length];
        float alphaNoise = 1;

        Vector3 point;
        for (int i = 0; i < vertices.Length; i++)
        {
            point = vertices[i];
            alphaNoise = PerlinFilter(point, NoiseLayer, Frequency, Amplitude);

            colors[i] = shiftedCloudColor;
            colors[i].a = alphaNoise;
        }

        cloudMesh.colors = colors;
    }


    public void CopyVertices()
    {
        Mesh sharedMesh = GetComponent<MeshFilter>().sharedMesh;
        cloudMesh = Instantiate(sharedMesh);
        GetComponent<MeshFilter>().sharedMesh = cloudMesh;


        vertices = cloudMesh.vertices;
        verticesBackup = cloudMesh.vertices;
    }

    public void ShapePlanetClouds()
    {
        SetSeeds();
        UpdateCloudColor();
        UpdateCloudLevel();

    }
}
