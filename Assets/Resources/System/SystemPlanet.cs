using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CelestialBody;
public class SystemPlanet : MonoBehaviour
{

    public Planet Planet;

    public GameObject PlanetaryFeatureLocation;

    List<GameObject> FeatureLocations = new List<GameObject>();
    public Vector3 Pos;
    public GameObject RingPrefab;




    void Start()
    {
        CreateOrbitPoints(transform.position, transform.parent.position);

    }

    List<GameObject> SetFeatureLocations(GameObject surfaceObject)
    {
        PlanetSurface surface = surfaceObject.GetComponent<PlanetSurface>();
        List<GameObject> featureLocations = new List<GameObject>();

        foreach (var feature in Planet.Features.PlanetaryFeatureList)
        {
            GameObject planetFeatureLocation = Instantiate(PlanetaryFeatureLocation, surfaceObject.transform) as GameObject;
            planetFeatureLocation.GetComponent<PlanetFeatureLocation>().Feature = feature;
            planetFeatureLocation.transform.localPosition = surface.SuitableFeaturePosition(feature.LocationType);

            featureLocations.Add(planetFeatureLocation);
        }

        return featureLocations;

    }
    

    public void Visualize()
    {
        GameObject PlanetSurfaceObject;

        PlanetSurfaceObject = CreatePlanetSurface();
        CreatePlanetRing(PlanetSurfaceObject);
        FeatureLocations = SetFeatureLocations(PlanetSurfaceObject);
    }

    GameObject CreatePlanetSurface()
    {
        string planetTypename = Planet.Type.Name;
        string planetPath = "System/Planets/" + planetTypename + "Planet/" + planetTypename + "Planet";
        GameObject PlanetVisualPrefab = Resources.Load<GameObject>(planetPath) as GameObject;

        if (PlanetVisualPrefab == null)
        {
            Debug.Log("PLANET:" + planetPath + " NOT FOUND!");
            PlanetVisualPrefab = Resources.Load<GameObject>("System/Planets/MinorPlanet/MinorPlanet") as GameObject;
        }

        GameObject PlanetSurfaceObject = Instantiate(PlanetVisualPrefab, this.transform, false) as GameObject;
        PlanetSurfaceObject.GetComponent<PlanetSurface>().Planet = Planet;
        PlanetSurfaceObject.GetComponent<PlanetSurface>().SetValues();
        PlanetSurfaceObject.GetComponent<PlanetSurface>().CopyVertices();
        PlanetSurfaceObject.GetComponent<PlanetSurface>().ShapePlanetSurface();

        return PlanetSurfaceObject;
    }

    void CreatePlanetRing(GameObject surface)
    {
        GameObject PlanetRing = Instantiate(RingPrefab, surface.transform, false) as GameObject;
        PlanetRing.GetComponent<PlanetRing>().CreateRing(Planet.RingType);
    }



    void OnMouseDown()
    {
        // this.transform.parent.GetComponent<SystemPlanet>().SetUiPlanetData();
        UiCanvas UI = GameObject.Find("/Canvas").GetComponent<UiCanvas>();
        UI.PlanetDataView(Planet);

        UIFeatureTracker UIFeatureTracker = GameObject.Find("/Canvas/FeatureTracker").GetComponent<UIFeatureTracker>();

        UIFeatureTracker.TrackFeatureLocations(FeatureLocations);
    }


    /*
    public void CreatePlanetaryFeatureLocations()
    {
        GameObject hydrosphereLocation = Instantiate(PlanetaryFeatureLocation, transform);
        hydrosphereLocation.transform.position = PlanetSurface.RandomVertex();
    }*/


    float GetPlanetOrbitalDistance(Vector3 pos, Vector3 parentPos)
    {
        float distance;

        distance = Vector3.Distance(pos, parentPos);
        return distance;
    }

    void CreateOrbitPoints(Vector3 pos, Vector3 parentPos)
    {
        int segments = 120;

        LineRenderer orbitCircle;
        orbitCircle = this.GetComponent<LineRenderer>();
        if (orbitCircle == null) { Debug.Log("lineRenderer null"); }

        orbitCircle.positionCount = segments;

        float orbitalDistance = GetPlanetOrbitalDistance(pos, parentPos);

        float segmentX;
        float segmentZ;
        float angle = 42f;
        orbitCircle.positionCount = segments + 1;
        orbitCircle.widthMultiplier = 0.5f;
        orbitCircle.startColor = new Color(0.8f, 0.83f, 0.9f, 0.2f);
        orbitCircle.endColor = new Color(0.8f, 0.83f, 0.9f, 0.2f);


        for (int i = 0; i <= segments; i++)
        {
            segmentX = parentPos.x +  Mathf.Sin(Mathf.Deg2Rad * angle) * orbitalDistance;
            segmentZ = parentPos.z + Mathf.Cos(Mathf.Deg2Rad * angle) * orbitalDistance;

            orbitCircle.SetPosition(i, new Vector3(segmentX, 0, segmentZ));
            angle += (360f / segments);
        }

    }

    void Update()
    {

        //transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, orbitSpeed * Time.deltaTime);

    }




}
