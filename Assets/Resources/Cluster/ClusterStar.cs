using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineSpace;
using ColorSpace;

public class ClusterStar : MonoBehaviour
{

    public int StarId;
    public int HomeClusterId;
    public Color StarColor;
    public Color StarColorCold;
    public Color HeightLineColor = new Color(1,1,1,0.5f);

    ColorFunctions ColorFunctions = new ColorFunctions();
    LineFunctions LineFunctions = new LineFunctions();

    void Start()
    {
        Vector3 position = this.transform.position;

        Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
        Vector3[] heightLineSegments = new[] { new Vector3(position.x, 0, position.z), position };


        GameObject line = LineFunctions.CreateLineObject(this.transform, new Vector3(0, 0, 0), "Height Line", heightLineSegments, lineMaterial, 0.6f);
        line.GetComponent<LineRenderer>().material.color = HeightLineColor;

        Color RimColor = ColorFunctions.HueShiftColor(StarColor, 0, 0.9f);
        RimColor = new Color(RimColor.r, RimColor.g, RimColor.b, 0.4f);

        if (this.GetComponent<Renderer>() != null)
        {

            this.GetComponent<Renderer>().material.SetColor("_Color", StarColor);
            this.GetComponent<Renderer>().material.SetColor("_RimColor", RimColor);
            
        }

    }


    


    void OnMouseDown()
    {
        transform.parent.parent.GetComponent<GalaxyCatalog>().CreateSystem(HomeClusterId, StarId);
    }

    // Update is called once per frame
    void Update()
    {

    }


}
