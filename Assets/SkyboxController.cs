using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Material GalaxySkybox;
    public Material ClusterSkybox;
    public Material SystemSkybox;

    public void SetSkybox(string skybox)
    {
        Debug.Log(skybox);
        if (skybox == "galaxy")
        {
            RenderSettings.skybox = GalaxySkybox;
        }

        if (skybox == "cluster")
        {

            RenderSettings.skybox = ClusterSkybox;
        }

        if (skybox == "system")
        {
            RenderSettings.skybox = SystemSkybox;
        }

    }
}
