using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ColorSpace;
using static CelestialBody;

public class ClusterController : GalaxyCatalog
{
    public GameObject StarPrefab;

    GameObject Deepfield;

    ColorFunctions ColorFunctions = new ColorFunctions();

    void Awake()
    {
        Deepfield = this.transform.Find("BackgroundSpace/BgDeepfield").gameObject;
      //  UI = GameObject.Find("/Canvas").GetComponent<UiCanvas>();

    }

    void Start()
    {
 
    }

    void OnDisable()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.tag == "Cluster")
            {
                Destroy(child.gameObject);
            }
            if (child.gameObject.tag == "System")
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void VisualizeCluster(Cluster activeCluster)
    {



        int starIndex;
        Vector3 starPosition;

        foreach (Star star in activeCluster.Stars)
        {
            starIndex = star.Id;
            starPosition = star.Pos;

            GameObject clusterStar = Instantiate(StarPrefab, starPosition, transform.rotation) as GameObject;
            clusterStar.GetComponent<ClusterStar>().StarColor = star.Type.StarColor;
            clusterStar.GetComponent<ClusterStar>().HomeClusterId = activeCluster.Id;
            clusterStar.GetComponent<ClusterStar>().StarId = starIndex;
            clusterStar.transform.parent = this.transform;

        }

        ColorizeCluster(activeCluster.Id);

        UI.ClusterDataView(activeCluster.Name);


    }

    void ColorizeCluster(int seed)
    {

        System.Random Random = new System.Random(seed);

        Color PrimaryColor = new Color(0.0f, 0.0f, 0f, 1f);
        Color SecondaryColor = new Color(0.0f, 0.0f, 0f, 1f);


        float primaryColorHue;
        float primaryColorSaturation;
        float primaryColorValue;

        float secondaryColorHue;
        float secondaryColorSaturation;
        float secondaryColorValue;

        primaryColorHue = (float)Random.NextDouble();
        primaryColorSaturation = (float)Random.NextDouble() * 0.3f + 0.7f;
        primaryColorValue = (float)Random.NextDouble() * 0.3f + 0.7f;

        secondaryColorHue = primaryColorHue + ((float)Random.NextDouble() * 0.5f - 0.25f);
        secondaryColorSaturation = (float)Random.NextDouble() * 0.5f + 0.5f;
        secondaryColorValue = (float)Random.NextDouble() * 0.5f + 0.5f;




        PrimaryColor = Color.HSVToRGB(primaryColorHue, primaryColorSaturation, primaryColorValue);
        SecondaryColor = Color.HSVToRGB(secondaryColorHue, secondaryColorSaturation, secondaryColorValue);

        Deepfield = this.transform.Find("BackgroundSpace/BgDeepfield").gameObject;

        if (Deepfield.GetComponent<ParticleSystem>() != null)
        {
            ParticleSystem.MainModule main = Deepfield.GetComponent<ParticleSystem>().main;

            main.startColor = new ParticleSystem.MinMaxGradient(PrimaryColor, SecondaryColor);

        }
    }

    void Update()
    {

    }
}
