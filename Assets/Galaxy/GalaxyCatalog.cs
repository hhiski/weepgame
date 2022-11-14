using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using static CelestialBody;
[ExecuteInEditMode]
public static class Globals
{
    public static string spaceScaleLevel = "system";
    public static int[] spaceAddress = new int[] { 0, 0, 0 };
}

public class GalaxyCatalog : MonoBehaviour
{

    public Premaid Premaid = new Premaid();
    public Universe Universe = new Universe(0);
    public UiCanvas UI = new UiCanvas();
    public SkyboxController SkyboxController = new SkyboxController();

    void Awake()
    {
        UI = GameObject.Find("/Canvas").GetComponent<UiCanvas>();
        SkyboxController = GetComponent<SkyboxController>();
    }

    void Start()
    {

      

        for (int id = 0; id < 50; id++)
        {
            if (id == 3) //Sol System
            {
                Cluster LocalBubbleCluster = Premaid.LocalBubbleCluster(id);
                Universe.Clusters.Add(LocalBubbleCluster);
            }
            else
            {
                Cluster GenericCluster = Premaid.GenericCluster(id);
                Universe.Clusters.Add(GenericCluster);
            }
        }
        
        CreateSystem(0, 0);
        SaveIntoJson();
    }


    public void LocateSol()
    {
        Debug.Log("Locating Sol...");
        foreach (Cluster cluster in Universe.Clusters)
        {
            Debug.Log("Cluster: " + cluster.Name);
            foreach (Star star in cluster.Stars)
            {
                Debug.Log("   Star: " + star.Name);
                if (star.Name == "Sol")
                {
                    int clusterID = cluster.Id;
                    int systemID = star.Id;
                    Debug.Log("clusterID:" + clusterID + " systemID:" + star.Id);
                    CreateSystem(clusterID, systemID);
                    break;
                }
            }
        }
    }

    public void SaveIntoJson()
    {
        var clusterList = Universe.Clusters;
        var outputString = JsonUtility.ToJson(clusterList);
        File.WriteAllText(Application.persistentDataPath  + "MyFile.json", outputString);
    }

    public void ShowPlanetNumbers()
    {
        List<string> planetTypesAll = new List<string>();
        List<string> planetTypesUniques = new List<string>(); //containing one of the each types

        int planetCount = 0;
        int typeCount = 0;

        foreach (Cluster cluster in Universe.Clusters)
        {
            foreach (Star star in cluster.Stars)
            {
                foreach (Planet planet in star.Planets)
                {
                    string planetType = planet.Type.Name;
                    planetTypesAll.Add(planetType);

                    if (!(planetTypesUniques.Contains(planetType)))
                    {
                        planetTypesUniques.Add(planetType);
                    }
                }
            }
        }

        planetCount = planetTypesAll.Count;
        Debug.Log("Total planet count: " + planetCount);

        foreach (string type in planetTypesUniques)
        {
            typeCount = planetTypesAll.Count(n => n == type);
            Debug.Log(type + "-type planet count:" + typeCount);
        }
    }

    public void CreateCluster(int clusterId)
    {
        Globals.spaceScaleLevel = "cluster";
        Globals.spaceAddress[0] = clusterId;


        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.tag == "Zone")
            { child.gameObject.SetActive(false); }
            if (child.gameObject.name == "SystemScope")
            { child.gameObject.SetActive(false); }
            if (child.gameObject.name == "ClusterScope")
            { child.gameObject.SetActive(true); }

        }
        
        GameObject ClusterScope = GameObject.Find("Galaxy/ClusterScope");
        ClusterController ClusterController = ClusterScope.GetComponent<ClusterController>();

        foreach (Cluster cluster in Universe.Clusters)
        {
            if (cluster.Id == clusterId)
            {
                ClusterController.VisualizeCluster(cluster);
                break;
            }
           
        }


    }

    public void CreateSystem(int clusterId, int systemId)
    {
        Globals.spaceScaleLevel = "system";
        Globals.spaceAddress[0] = clusterId;
        Globals.spaceAddress[1] = systemId;


        foreach (Transform child in gameObject.transform)
        {

            if (child.gameObject.tag == "Zone")
            { child.gameObject.SetActive(false); }
            if (child.gameObject.name == "ClusterScope")
            { child.gameObject.SetActive(false); }
            if (child.gameObject.name == "SystemScope")
            { child.gameObject.SetActive(false); }

        }

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "SystemScope")
            { child.gameObject.SetActive(true); }

        }
        GameObject SystemScope = GameObject.Find("Galaxy/SystemScope");
        SystemController SystemController = SystemScope.GetComponent<SystemController>();

        SkyboxController.SetSkybox("system");
        foreach (Cluster cluster in Universe.Clusters)
        {
            foreach (Star star in cluster.Stars)
            {
               if (star.Id == systemId)
                {
                    SystemController.VisualizeSystem(star);
                    break;
                }
            }
        }
    }

    public void CreateGalaxy()
    {

        Globals.spaceScaleLevel = "galaxy";
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "ClusterScope")
            {
                child.gameObject.SetActive(false);
            }
            if (child.gameObject.name == "SystemScope")
            {
                child.gameObject.SetActive(false);
            }
            if (child.gameObject.tag == "Zone")
            {
                child.gameObject.SetActive(true);
            }
        }

        UI.GalaxyDataView("Milky Way");
        SkyboxController.SetSkybox("galaxy");

    }



}
