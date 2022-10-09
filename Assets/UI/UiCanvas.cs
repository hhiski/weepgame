using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CelestialBody;

public  class UiCanvas : MonoBehaviour
{
    GameObject SystemCamera;
    CameraOrbit CameraOrbit;
    GameObject Selector;


    public Material UIMainMaterial;
    public Material UICautionMaterial;
    public Material UIWarningMaterial;

    GameObject UIDescriptor;
    GameObject UIDownPanel;
    GameObject UILeftPanel;
    GameObject UISelector;
    GameObject UIZoomOutButton;
    GameObject UILocateSolButton;

    /*public Color MainColor = new Color(0,1,0);
    public Color CautionColor = new Color(1, 1, 0);
    public Color WarningColor = new Color(1, 0, 0,3f);*/
    void Awake()
    {

        SystemCamera = GameObject.Find("/Camera/ClusterCameraSystem/CameraTarget/System Camera");
        CameraOrbit = GameObject.Find("/Camera/ClusterCameraSystem/CameraTarget/").GetComponent<CameraOrbit>();
        UIDescriptor = this.transform.Find("Descriptor").gameObject;
        UIDownPanel = this.transform.Find("DownPanel").gameObject;
        UILeftPanel = this.transform.Find("LeftPanel").gameObject;
        UISelector = this.transform.Find("Selector").gameObject;
        UIZoomOutButton = this.transform.Find("ZoomOutButton").gameObject;
        UILocateSolButton = this.transform.Find("LocateSolButton").gameObject;
    }
    void Start()
    {



        GalaxyCatalog GalaxyCatalog = GameObject.Find("/Galaxy").GetComponent<GalaxyCatalog>();



       /* UIMainMaterial.SetColor("_Color", MainColor);
        UICautionMaterial.SetColor("_Color", CautionColor);
        UIWarningMaterial.SetColor("_Color", WarningColor);*/

       


    }



    public void HideAllElements()
    {
  
        foreach (Transform child in gameObject.transform)
        {

            child.gameObject.SetActive(false);
        }
    }

    void ShowUi(string uiScope)
    {
        HideAllElements();
        if (uiScope == "galaxy")
        {
            UIDescriptor.SetActive(true);
            UIDownPanel.SetActive(true);
            UILocateSolButton.SetActive(true);

        }
        else if (uiScope == "cluster")
        {
            UIDescriptor.SetActive(true);
            UIDownPanel.SetActive(true);
            UILocateSolButton.SetActive(true);
            UIZoomOutButton.SetActive(true);
        }
        else if(uiScope == "star")
        {
            UIDescriptor.SetActive(true);
            UIDownPanel.SetActive(true);
            UILocateSolButton.SetActive(true);
            UIZoomOutButton.SetActive(true);
        }
        else if(uiScope == "planet")
        {
            UIDescriptor.SetActive(true);
            UIDownPanel.SetActive(true);
            UILeftPanel.SetActive(true);
            UIDownPanel.SetActive(true);
            UISelector.SetActive(true);
            UILocateSolButton.SetActive(true);
            UIZoomOutButton.SetActive(true);
        }


    }

    public void ClusterDataView(string name)
    {
        ShowUi("cluster");
        UpdateDescriptor(name, "");
        Vector3 targetPos = new Vector3(0, 0, 0);
        CameraOrbit.CameraToPos(targetPos);

    }
    public void GalaxyDataView(string name)
    {
        ShowUi("galaxy");
        UpdateDescriptor(name, "");
        Vector3 targetPos = new Vector3(0, 0, 0);
        CameraOrbit.CameraToPos(targetPos);

    }
    public void StarDataView(Star star)
    {
        ShowUi("star");
        UpdateDescriptor(star.Name, star.Type.Name);
        float targetSize = 2;
        Vector3 targetPos = new Vector3(0, 0, 0);
        Tracker(true, targetPos, targetSize);
        CameraOrbit.CameraToPos(targetPos);
    }

    public void PlanetDataView(Planet planet)
    {

        ShowUi("planet");
        UpdateDescriptor(planet.Name, planet.Type.Name + " Planet");

        UILeftPanel.GetComponent<UILeftPanel>().UpdatePlanetConditionData(planet);
            
        float targetSize = planet.Mass;
        Vector3 targetPos = planet.Pos;


        Tracker(true, targetPos, targetSize);
        CameraOrbit.CameraToPos(targetPos);

    }

    public void UpdateDescriptor(string name, string type)
    {
        Text UIName = UIDescriptor.transform.Find("UITitle/UIName").GetComponent<UnityEngine.UI.Text>();
        Text UIType = UIDescriptor.transform.Find("UITitle/UIType").GetComponent<UnityEngine.UI.Text>();

        string nameText = name.ToString();
        string typeName = type.ToString();

        UIName.text = nameText;
        UIType.text = typeName;
    }





    void Tracker(bool status)
    {
        UISelector selector = UISelector.GetComponent<UISelector>();
        selector.Tracking = status;
    }

    void Tracker(bool status, Vector3 targetPos, float targetSize)
    {

        UISelector selector = UISelector.GetComponent<UISelector>();
        selector.Tracking = status;
        selector.TargetPos = targetPos;
        selector.TargetSize = targetSize;

    }


    void Update()
    {

        

        /*
        if (TargetTracking)
        {
            if (Camera == null) {
                Camera = SystemCamera.GetComponent<Camera>();
            }

            if (Selector != null && Camera != null)
            {

                Selector.GetComponent<UISelector>().UpdateSelectorCorners(SelectorPos, SelectorSize);
            }
           

    

        }*/
    }
 }

