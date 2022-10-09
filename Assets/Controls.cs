using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controls : GalaxyCatalog
{
    bool TimeControlReady = true;

    GalaxyCatalog GalaxyCatalog;
    // Start is called before the first frame update
    void Start()
    {

        GalaxyCatalog = GameObject.Find("/Galaxy").GetComponent<GalaxyCatalog>();

    }


        
    void ReadyTimeControl()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus) == false)
        {
            TimeControlReady = true;
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus) == false)
        {
            TimeControlReady = true;
        }
        else
        {
            Invoke("ReadyTimeControl", 0.11f);
        }

    }

    public void ZoomOut()
    {
        string scaleLevel = Globals.spaceScaleLevel;
        int clusterId = Globals.spaceAddress[0];
        int systemId = Globals.spaceAddress[1];


        if (scaleLevel == "system")
        {
            GalaxyCatalog.CreateCluster(clusterId);
        }
        else if (scaleLevel == "cluster")
        {
            GalaxyCatalog.CreateGalaxy();
        }
        else
        {
            GalaxyCatalog.CreateGalaxy();
        }
    }

    public void LocateSol()
    {

        GalaxyCatalog.CreateSystem(3, 0);

    }

    void Update()
    {

        if (TimeControlReady && Input.GetKeyDown(KeyCode.U) == true)
        {
            string scaleLevel = Globals.spaceScaleLevel;
            int clusterId = Globals.spaceAddress[0];
            int systemId = Globals.spaceAddress[1];
            Debug.Log("scaleLevel:" + scaleLevel + " clusterID:" + clusterId + " systemID:" + systemId);

            Invoke("ReadyTimeControl", 0.05f);

        }

        if (TimeControlReady && Input.GetKeyDown(KeyCode.Space) == true)
        {

            ZoomOut();
            Invoke("ReadyTimeControl", 0.11f);
        }


        if (TimeControlReady && Input.GetKeyDown(KeyCode.I) == true)
        {
            GalaxyCatalog.ShowPlanetNumbers();
        }
        
        if (TimeControlReady && Input.GetKeyDown(KeyCode.KeypadPlus) == true)
        {
            Time.timeScale = Time.timeScale * 2;
            Debug.Log("Time Scale:" + Time.timeScale);
            Invoke("ReadyTimeControl", 0.11f);
        }


        if (TimeControlReady && Input.GetKeyDown(KeyCode.KeypadMinus) == true)
        {
            Time.timeScale = Time.timeScale / 2;
            Debug.Log("Time Scale:" + Time.timeScale);
            Invoke("ReadyTimeControl", 0.11f);
        }
    }
}
