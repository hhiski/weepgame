using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CelestialBody;

public class SystemController : GalaxyCatalog
{
    public GameObject PlanetSystemPrefab;
    public GameObject BeltPrefab;
    public GameObject CometPrefab;
    public GameObject RingPrefab;
    public GameObject SystemStarPrefab;

    Star selectedVisibleSystem;

    GameObject PlanetSystem;
    GameObject PlanetVisual;
    GameObject StarVisual;

    CameraSunShafts cameraSunShafts;

    void Start()
    {
        cameraSunShafts = GameObject.Find("/Camera/ClusterCameraSystem/CameraTarget/").GetComponent<CameraSunShafts>();
    }

    void OnDisable()
    {
        cameraSunShafts.ChangeCameraSunShafts(false);

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.tag == "System")
            {
                Destroy(child.gameObject);
            }
        }



    }

    

    public void VisualizeSystem(Star visibleSystem)
    {
        selectedVisibleSystem = visibleSystem;

        //Creates a main star for the system
        GameObject StarSystem = Instantiate(SystemStarPrefab, new Vector3(0f, 0f, 0f), transform.rotation) as GameObject;
        StarSystem.GetComponent<SystemStar>().Star = visibleSystem;
        string starPath = "System/Star/MainSequenceStar";
        GameObject StarVisualPrefab = Resources.Load<GameObject>(starPath) as GameObject;

        if (StarVisualPrefab == null)
        {
            Debug.Log("STAR:" + starPath + " NOT FOUND!");
            StarVisualPrefab = Resources.Load<GameObject>("System/Planets/MinorPlanet/MinorPlanet") as GameObject;
        }

        StarVisual = Instantiate(StarVisualPrefab, StarSystem.transform, false) as GameObject;
        StarVisual.GetComponent<StarSurface>().Star = visibleSystem;
        StarVisual.GetComponent<StarSurface>().InitStar();
        StarSystem.transform.parent = this.transform;

        //Set sun shaft colors for the camera
        CameraSunShafts cameraSunShafts = GameObject.Find("/Camera/ClusterCameraSystem/CameraTarget/").GetComponent<CameraSunShafts>();
        cameraSunShafts.ChangeCameraSunShafts(true, visibleSystem.SolarTemperature, visibleSystem.Type.StarLightColor);



        //  var starWindMain = starWind.main;

        // Color windColor = starWindMain.startColor;
        // starWindMain.startColor = visibleSystem.SolarWindStrenght;



        //    starWind.Emit(visibleSystem.DustAmount);

        // SolarWindStrenght
        //  starWindEmission = starWind.emission;
        //    starWindEmission.rateOverTime = visibleSystem.SolarWindAmount;
        //  starWindEmission.rateOverDistance = 0;
        // starWindMain.prewarm = true;
        // starWind.Emit(visibleSystem.DustAmount);



        //var starFlaresMain = StarSystem.transform.Find("SolarFlares").GetComponent<ParticleSystem>().main;
        //   starFlaresMain.startColor = visibleSystem.Type.StarLightColor;

        //Creates asteroid belts for the system
        foreach (AsteroidBelt asteroidBelt in visibleSystem.AsteroidBelts)
        {

            if (BeltPrefab != null)
            {
                GameObject BeltInstance = Instantiate(BeltPrefab, new Vector3(0f, 0f, 0f), transform.rotation) as GameObject;

                BeltInstance.GetComponent<SystemAsteroidBelt>().Type = asteroidBelt.Type;
                BeltInstance.GetComponent<SystemAsteroidBelt>().Distance = asteroidBelt.Distance;
                BeltInstance.transform.parent = this.transform;
            }
            else
            {
                Debug.Log("null BeltPrefab!");
            }
        }

        
         foreach (Comet comet in visibleSystem.Comets)
            {

            if (CometPrefab != null)
            {
                GameObject CometInstance = Instantiate(CometPrefab, new Vector3(0f, 0f, 0f), transform.rotation) as GameObject;

                CometInstance.GetComponent<SystemComet>().Type = comet.Type;
                CometInstance.GetComponent<SystemComet>().SemiMajorAxis_A = comet.SemiMajorAxis_A;
                CometInstance.GetComponent<SystemComet>().SemiMinorAxis_B = comet.SemiMinorAxis_B;
                CometInstance.GetComponent<SystemComet>().Inclination = comet.Inclination;
                CometInstance.GetComponent<SystemComet>().Direction = comet.Direction;
                CometInstance.transform.parent = this.transform;

            }
            else
            {
                Debug.Log("null comet!");
            }
        }

        //Creates planets for the system
        foreach (Planet planet in visibleSystem.Planets)
            {

            PlanetSystem = Instantiate(PlanetSystemPrefab, planet.Pos, transform.rotation) as GameObject;
            PlanetSystem.GetComponent<SystemPlanet>().Planet = planet;
            PlanetSystem.transform.localScale = new Vector3(planet.Mass, planet.Mass, planet.Mass);
            PlanetSystem.transform.parent = this.transform;

            string planetTypename = planet.Type.Name;
            string planetPath = "System/Planets/"+ planetTypename +"Planet/" + planetTypename + "Planet";
            GameObject PlanetVisualPrefab = Resources.Load<GameObject>(planetPath) as GameObject;

            if (PlanetVisualPrefab == null) {
                Debug.Log("PLANET:" + planetPath + " NOT FOUND!");
                PlanetVisualPrefab = Resources.Load<GameObject>("System/Planets/MinorPlanet/MinorPlanet") as GameObject;
            }

            PlanetVisual = Instantiate(PlanetVisualPrefab, PlanetSystem.transform, false) as GameObject;
            PlanetVisual.GetComponent<PlanetSurface>().Planet = planet;
            PlanetVisual.GetComponent<PlanetSurface>().SetValues();
            PlanetVisual.GetComponent<PlanetSurface>().CopyVertices();
            PlanetVisual.GetComponent<PlanetSurface>().ShapePlanetSurface();

            GameObject PlanetRing = Instantiate(RingPrefab, PlanetVisual.transform, false) as GameObject;
            PlanetRing.GetComponent<PlanetRing>().CreateRing(planet.RingType);

            foreach (Moon moon in planet.Moons) {
            GameObject MoonSystem = Instantiate(PlanetSystemPrefab, moon.Pos, transform.rotation) as GameObject;

            PlanetVisualPrefab = Resources.Load<GameObject>("System/Planets/MinorPlanet/MinorPlanet") as GameObject;
            MoonSystem.transform.parent = PlanetSystem.transform;
            PlanetVisual = Instantiate(PlanetVisualPrefab, MoonSystem.transform, false) as GameObject;
            PlanetVisual.GetComponent<PlanetSurface>().Planet = planet;
            PlanetVisual.GetComponent<PlanetSurface>().SetValues();
            PlanetVisual.GetComponent<PlanetSurface>().CopyVertices();
            PlanetVisual.GetComponent<PlanetSurface>().ShapePlanetSurface();
            PlanetVisual.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
                }
        }


        UI.StarDataView(visibleSystem);

    }



}
