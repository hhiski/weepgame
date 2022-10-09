using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
   // public GameObject GalaxyCameraSystem;
   // public GameObject ClusterCameraSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        
        /*
        GameObject ClusterCameraSystem = GameObject.Find("Camera/ClusterCameraSystem");
        GameObject GalaxyCameraSystem = GameObject.Find("Camera/GalaxyCameraSystem");

        SetCameraSystemOn("GalaxyCameraSystem");

*/
    }

    public void SetCameraSystemOn(string cameraSystem)

    {/*
        GameObject GalaxyCameraSystem = this.transform.Find("GalaxyCameraSystem").gameObject;
        GameObject ClusterCameraSystem = this.transform.Find("ClusterCameraSystem").gameObject;



        if (cameraSystem == "GalaxyCameraSystem") {
            GalaxyCameraSystem.SetActive(true);
            ClusterCameraSystem.SetActive(false);


        };
        if (cameraSystem == "ClusterCameraSystem")
        {

            ClusterCameraSystem.SetActive(true);
            GalaxyCameraSystem.SetActive(false);

            CameraOrbit cameraOrbit = GameObject.Find("/Camera/ClusterCameraSystem/CameraTarget/").GetComponent<CameraOrbit>(); ;
            cameraOrbit.ResetCamera();
        };*/
        
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
