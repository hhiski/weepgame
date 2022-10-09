using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemComet : MonoBehaviour
{

    public int Type = 1;
    [Range(1.0f, 400.0f)]
    public float SemiMajorAxis_A = 220;
    [Range(1.0f, 400.0f)]
    public float SemiMinorAxis_B = 80;
    public float Inclination = 0f; //deg
    public float Direction = 0f; //deg

    float Radius = 0;
    float Eccentricity = 0;

    List<Vector3> EllipticOrbit = new List<Vector3>();

    public float Speed = 0.04f;
    public int CometIndexPos = 75;

    GameObject Nucleus;


    void Start()
    {

        this.transform.Rotate(0, Direction , Inclination , Space.World);

        CalculateEllipticOrbit();
        DrawEllipticOrbit(EllipticOrbit.Count);

        Nucleus = this.transform.Find("Nucleus").gameObject;
        Nucleus.transform.position = EllipticOrbit[CometIndexPos];

        Nucleus.transform.position = transform.TransformPoint(EllipticOrbit[CometIndexPos]);
    }

    void CalculateEllipticOrbit()
    {
        EllipticOrbit.Clear();
        Eccentricity = Mathf.Pow((Mathf.Abs((Mathf.Pow(SemiMinorAxis_B, 2) / Mathf.Pow(SemiMajorAxis_A, 2)) - 1)), 0.5f);

        float ORadian = 0; 

        while (ORadian < 6.28f)
        {

            Radius = (SemiMajorAxis_A * (1 - Mathf.Pow(Eccentricity, 2))) / (1 + Eccentricity * Mathf.Cos(ORadian));

            float x = Radius * Mathf.Cos(ORadian);
            float z = Radius * Mathf.Sin(ORadian);
            //float y = Mathf.Tan(Mathf.Deg2Rad * Inclination) * x;
            float y = 0;

            Vector3 point = new Vector3(x, y, z);

            EllipticOrbit.Add(point);

            ORadian = ORadian + 5 * (0.1f + 0.9f * (1 - (Radius / (2 * SemiMajorAxis_A)))) * Mathf.Deg2Rad;


        }


        EllipticOrbit.Add(new Vector3(Radius,0, 0));


    }


    void Update()
    {

        float step = Speed;
        Vector3 targetPos = transform.TransformPoint(EllipticOrbit[CometIndexPos + 1]);
        Vector3 currentPos = Nucleus.transform.position;

        Nucleus.transform.position = Vector3.MoveTowards(currentPos, targetPos, step);
        float distanceToPoint = Vector3.Distance(currentPos, targetPos);

        if (distanceToPoint<= step) {
            CometIndexPos++;
            if (CometIndexPos+1 >= EllipticOrbit.Count)
            {
                CometIndexPos = 0;
            }

        }
      


        if (Input.GetKeyDown(KeyCode.V) == true)
        {

            CalculateEllipticOrbit();
            DrawEllipticOrbit(EllipticOrbit.Count);

        }
    }


    void DrawEllipticOrbit(int segmentCount)
    {
        int Segments = segmentCount;

        LineRenderer OrbitCircle;
        OrbitCircle = this.GetComponent<LineRenderer>();
        if (OrbitCircle == null) { Debug.Log("lineRenderer null"); }

  
  

        float segmentX;
        float segmentY;
        float segmentZ;
        OrbitCircle.positionCount = Segments;
        OrbitCircle.widthMultiplier = 0.3f;
        OrbitCircle.startColor = new Color(0.8f, 0.83f, 0.9f, 0.2f);
        OrbitCircle.endColor = new Color(0.8f, 0.83f, 0.9f, 0.2f);


        for (int i = 0; i < Segments; i++)
        {

            segmentX = EllipticOrbit[i].x;
            segmentY = EllipticOrbit[i].y;
            segmentZ = EllipticOrbit[i].z;

            OrbitCircle.SetPosition(i, new Vector3(segmentX, segmentY, segmentZ));

        }

    }





}
