using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LineSpace;

public class UIFeatureTracker : MonoBehaviour
{
    Camera Camera;
    public GameObject Feature;
    GameObject PrimaryFeature;
    GameObject TrackerLine;
    LineRenderer TrackerLineRenderer;

    public PlanetaryFeatures PlanetFeatures;

    public bool TrackingFeatures = false;
    public Vector3 TargetPos; // Target planet's position
    public Vector3 PrimaryFeaturePos;


    Renderer m_Renderer ;


    bool FeatureElementsCreated = false;

    void Start()
    {

        Camera = GameObject.Find("/Camera/ClusterCameraSystem/CameraTarget/System Camera").GetComponent<Camera>();
        PrimaryFeature = Instantiate(Feature, transform);
        /*
        Text UIPrimaryFeature = this.transform.Find("UIPlanetaryFeatures/UIPrimaryFeature").GetComponent<UnityEngine.UI.Text>();
        string primaryFeatureText = planet.Features.PrimaryFeature.Name.ToString();
        UIPrimaryFeature.text = primaryFeatureText;*/
    }

    public bool CreateFeatureUiElements()
    {
        LineFunctions LineFunctions = new LineFunctions();

        Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
        Vector3 startPos = TargetPos;
        Vector3[] lineSegments;

        Vector3 targetPos = startPos + Vector3.Normalize(new Vector3(7, 2, -8)) * 6.63f;
        PrimaryFeaturePos = targetPos;

        lineSegments = new[] { startPos, targetPos };
        TrackerLine = LineFunctions.CreateLineObject(this.transform, new Vector3(0, 0, 0), "Feature tracker line", lineSegments, lineMaterial, false, new Color(0,1,0,0.2f), 0.2f);
        TrackerLineRenderer = TrackerLine.GetComponent<LineRenderer>();

        Text primaryFeatureText = PrimaryFeature.GetComponent<UnityEngine.UI.Text>();
        primaryFeatureText.text = PlanetFeatures.PrimaryFeature.Name.ToString();

        m_Renderer = PrimaryFeature.GetComponent<Renderer>(); 

        return true;
    }

    /*
    void UpdatePlanetaryFeatures(Planet planet)
    {
        Text UIPrimaryFeature = this.transform.Find("UIPlanetaryFeatures/UIPrimaryFeature").GetComponent<UnityEngine.UI.Text>();
        string primaryFeatureText = planet.Features.PrimaryFeature.Name.ToString();
        UIPrimaryFeature.text = primaryFeatureText;
    }*/

    public void UpdateFeatureTracker()
    {
        Camera camera = Camera;
        Vector3 featureWorldPos;
        Vector3 featureVisibilityCheckPos;
        Vector3 featureScreenPos;
        Vector3 featureCameraWorldPos;


        featureWorldPos = TargetPos + Vector3.Normalize(new Vector3(7, 2, -8)) * 6.63f;
        featureVisibilityCheckPos = TargetPos + Vector3.Normalize(new Vector3(7, 2, -8)) * 6.23f;

        if (Physics.Linecast(featureVisibilityCheckPos, Camera.transform.position))
        {
            if (PrimaryFeature.activeSelf)
            {
                PrimaryFeature.SetActive(false);
            }
            PrimaryFeature.SetActive(false);
        }

        else if (!Physics.Linecast(featureVisibilityCheckPos, Camera.transform.position))
        {
            if (!PrimaryFeature.activeSelf)
            {
                PrimaryFeature.SetActive(true);
            }


            featureScreenPos = camera.WorldToScreenPoint(featureWorldPos);
            featureScreenPos = new Vector3(featureScreenPos.x, featureScreenPos.y, 5);
            featureCameraWorldPos = camera.ScreenToWorldPoint(featureScreenPos);

            PrimaryFeature.transform.position = featureCameraWorldPos;

            TrackerLineRenderer.SetPosition(0, TargetPos);
            TrackerLineRenderer.SetPosition(1, featureWorldPos);

            var FeatureDistanceToCamera = Vector3.Distance(Camera.transform.position, featureWorldPos);
            var TargetDistanceToCamera = Vector3.Distance(Camera.transform.position, TargetPos);

        }



        /*
        if (m_Renderer.isVisible)
        {
           
        }
        else {

        PrimaryFeature.SetActive(false);

        }*/

        /*
        if (FeatureDistanceToCamera > TargetDistanceToCamera && PrimaryFeature.activeSelf)
        {
            PrimaryFeature.SetActive(false);
        }

        if (FeatureDistanceToCamera < TargetDistanceToCamera && !PrimaryFeature.activeSelf)
        {
            PrimaryFeature.SetActive(true);
        }
        */


        //   PrimaryFeature.transform.position = PrimaryFeaturePos;

        /*
        float offset = targetSize * 7f;

        Camera camera = Camera;

        Vector3 topLeftPos = targetPos + camera.transform.right * -offset + camera.transform.up * offset;
        Vector3 topRightPos = targetPos + camera.transform.right * offset + camera.transform.up * offset;
        Vector3 buttomLeftPos = targetPos + camera.transform.right * offset + camera.transform.up * -offset;
        Vector3 buttomRightPos = targetPos + camera.transform.right * -offset + camera.transform.up * -offset;


        Vector3 targetPosViewport = camera.WorldToScreenPoint(topLeftPos);
        topLeftPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        topLeftPos = camera.ScreenToWorldPoint(topLeftPos);
        TopLeft.transform.position = topLeftPos;

        targetPosViewport = camera.WorldToScreenPoint(topRightPos);
        topRightPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        topRightPos = camera.ScreenToWorldPoint(topRightPos);
        TopRight.transform.position = topRightPos;

        targetPosViewport = camera.WorldToScreenPoint(buttomLeftPos);
        buttomLeftPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        buttomLeftPos = camera.ScreenToWorldPoint(buttomLeftPos);
        ButtomLeft.transform.position = buttomLeftPos;

        targetPosViewport = camera.WorldToScreenPoint(buttomRightPos);
        buttomRightPos = new Vector3(targetPosViewport.x, targetPosViewport.y, 5);
        buttomRightPos = camera.ScreenToWorldPoint(buttomRightPos);
        ButtomRight.transform.position = buttomRightPos;
        */


    }

    void Update()
    {

        if (TrackingFeatures)
        {
            if (!FeatureElementsCreated)
            {
                FeatureElementsCreated = CreateFeatureUiElements();
            } else
            {



                UpdateFeatureTracker();
            }

           
        }
    }
}
