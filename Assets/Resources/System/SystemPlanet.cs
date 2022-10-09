using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CelestialBody;
public class SystemPlanet : MonoBehaviour
{

    public Planet Planet;
    public Vector3 Pos { get; set; }
    public float orbitSpeed = 5f;


    void Start()
    {
        CreateOrbitPoints(transform.position, transform.parent.position);

      //  this.GetComponent<SphereCollider>().radius = Planet.Mass*7;
    }


    void OnMouseDown()
    {
        // this.transform.parent.GetComponent<SystemPlanet>().SetUiPlanetData();
        UiCanvas UI = GameObject.Find("/Canvas").GetComponent<UiCanvas>();
        UI.PlanetDataView(Planet);
    }

    public void SetUiPlanetData()
    {

        UiCanvas UI = GameObject.Find("/Canvas").GetComponent<UiCanvas>();
        UI.PlanetDataView(Planet);
    }



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
