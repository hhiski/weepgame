using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LineSpace;
using System.Linq;

public class UIFeatureTracker : MonoBehaviour
{



    Camera Camera;
    public GameObject FeaturePrefab;

    List<GameObject> FeatureLocations = new List<GameObject>();
    List<GameObject> Features = new List<GameObject>();
    GameObject TrackerLine;
    LineRenderer TrackerLineRenderer;

    public PlanetaryFeatures PlanetFeatures;

    public bool TrackingFeatures = false;
    bool FeatureFound = false;
    public Vector3 TargetPos; // Target planet's position


    class FeaturePointer
    {
        GameObject PointerLine;
        GameObject PointerFeature;
        GameObject PointerFeatureLocation;

        FeaturePointer(GameObject pointerFeature, GameObject pointerFeatureLocation)
        {
            PointerFeature = pointerFeature;
            PointerFeatureLocation = pointerFeatureLocation;

        }
    }

    void Start()
    {

        Camera = GameObject.Find("/Camera/ClusterCameraSystem/CameraTarget/System Camera").GetComponent<Camera>();

    }


    //Instantiate UI text holders (UIPlanetaryFeature) for each PlanetFeatureLocation created along with the planet
    public void TrackFeatureLocations(List<GameObject> featureLocations)
    {
        Features.Clear();

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        FeatureLocations = featureLocations;

        foreach (GameObject featureLocation in featureLocations)
        {
            GameObject feature = Instantiate(FeaturePrefab, transform); 
            Text primaryFeatureText = feature.GetComponent<UnityEngine.UI.Text>();
            primaryFeatureText.text = featureLocation.GetComponent<PlanetFeatureLocation>().Feature.Name.ToString();
            Features.Add(feature);
        }

        FeatureFound = true;
    }

    float FeatureFade(GameObject feature, float zeroAlphaValue, float fullAlphaValue, float pointValue)
    {
        float textAlpha = Mathf.InverseLerp(zeroAlphaValue, fullAlphaValue, pointValue);
        return textAlpha;
    }

    public void UpdateFeatureTracker()
    {
        Camera camera = Camera;
        Vector3 featureWorldPos;
        Vector3 featureExtrudedPos;
        Vector3 featureScreenPos;
        Vector3 featureCameraWorldPos;
        bool visible;
        bool nearEnough;

        float maxVisibleAngle = 90; // alpha < 1, the feature barely visible near max value
        float clearVisibleAngle = 70; // alpha = 1
        float maxVisibleDistance = 100; // alpha < 1 , the feature barely visible near max value
        float clearVisibleDistance = 30; // alpha = 1

        var FeaturesAndFeatureLocations = Features.Zip(FeatureLocations, (f, l) => new { Features = f, FeatureLocations = l });
        foreach (var fl in FeaturesAndFeatureLocations)
        {
            visible = false;
            nearEnough = false;

            if (fl.FeatureLocations == null || fl.Features == null) {
                break;
            }
       
            if (camera.transform.localPosition.z <= maxVisibleDistance)
            {
                nearEnough = true;
            }


         

            featureWorldPos = fl.FeatureLocations.transform.position;
            featureExtrudedPos = featureWorldPos + Vector3.Normalize(featureWorldPos) * 1f;

            //  visible = Physics.Linecast(featureExtrudedPos, Camera.transform.position); //LineCast visibility detection? 

            // feature surface normal and camera lens normal angle based detection
            Vector3 cameraNormal = Camera.transform.position - TargetPos;
            Vector3 featureNormal = featureWorldPos - TargetPos;
            float featureCameraAngle = Vector3.Angle(cameraNormal, featureNormal);



            if (featureCameraAngle <= maxVisibleAngle)
            {
                visible = true;
            }

          

            if (!visible || !nearEnough)
            {
                if (fl.Features.activeSelf)
                {
                    fl.Features.SetActive(false);
                }

            }

            else if (visible && nearEnough)
            {
                if (!fl.Features.activeSelf)
                {
                    fl.Features.SetActive(true);
                }


                //The total text fading amount is the sum of distance and angle based fading values 
                float textAlpha = FeatureFade(fl.Features, clearVisibleDistance, maxVisibleDistance , camera.transform.localPosition.z);
                textAlpha = FeatureFade(fl.Features, maxVisibleAngle, clearVisibleAngle, featureCameraAngle) - textAlpha;
                Color textColor = fl.Features.GetComponent<Text>().color;
                textColor = new Color(textColor.r, textColor.g, textColor.b, textAlpha);
                fl.Features.GetComponent<Text>().color = textColor;

                featureScreenPos = camera.WorldToScreenPoint(featureExtrudedPos);
                featureScreenPos = new Vector3(featureScreenPos.x, featureScreenPos.y, 5);
                featureCameraWorldPos = camera.ScreenToWorldPoint(featureScreenPos);

                fl.Features.transform.position = featureCameraWorldPos;

            }
        }
    }

   

    void Update()
    {

        if (TrackingFeatures && FeatureFound)
        {
            UpdateFeatureTracker();
        }
    }
}
