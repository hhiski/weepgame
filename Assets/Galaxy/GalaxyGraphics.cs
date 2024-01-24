using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;




public class GalaxyGraphics : MonoBehaviour
{


    GameObject GalacticArm;

    public GameObject ArmEmitterPrefab;
    public GameObject ZonePrefab;
    public GameObject Galaxy;
    public Material DustMaterial;
    public Material RedDustMaterial;



    public static float Pow3Constrained(float x)
    {
        x = x - 0.5f;
        float value = Mathf.Pow(x, 3) * 4 + 0.5f;
        return Mathf.Max(Mathf.Min(1, value), 0);
    }


    public Vector3[] GenerateArm(string type, int numOfStars, float rotation, float armSpread, float spin, float starsAtCenterRatio)
    {

        Vector3[] result = new Vector3[numOfStars];

        GalacticArm = GameObject.Find("GalaxyGraphics/GalacticArm");
        //GameObject ArmEmitterP = GameObject.Find("GalaxyGraphics/GalacticArm/ArmEmitter");


        GameObject ArmEmitter = Instantiate(ArmEmitterPrefab, new Vector3(0, 0, 0), transform.rotation) as GameObject;

        ArmEmitter.transform.parent = GalacticArm.transform;
        ParticleSystem particleSystem = ArmEmitter.GetComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = ArmEmitter.GetComponent<ParticleSystemRenderer>();


        ParticleSystem.Particle[] particleList;

        // particleList = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        particleList = new ParticleSystem.Particle[numOfStars];

        var emitParams = new ParticleSystem.EmitParams();


        particleSystem.Emit(emitParams, numOfStars);
        int numberParticles = particleSystem.particleCount;

        int numParticlesAlive = particleSystem.GetParticles(particleList);


        var par_main = particleSystem.main;

        Vector3 StarPosition;
        for (int i = 0; i < numberParticles; i++)
        {

            StarPosition = Spiralize(i, numOfStars, rotation, armSpread, spin, starsAtCenterRatio, false);

            particleList[i].position = StarPosition;

            float distanceFromCenter = (float)i / (float)numberParticles;

            if (type == "dust")
            {
                ArmEmitter.name = "Dust Emitter";

                float ColR = -0.7f * distanceFromCenter + 1f;
                float ColG = -0.3f * distanceFromCenter + 0.5f;
                float ColB = 0.7f * distanceFromCenter + 0.3f;
                particleSystemRenderer.material = DustMaterial;
                particleList[i].startSize = Random.Range(4.4f, 10.00f);

                if (distanceFromCenter > 0.85f)
                {
                    ColR = ColR * (distanceFromCenter * (-6.66f) + 6.66f);
                    ColG = ColG * (distanceFromCenter * (-6.66f) + 6.66f);
                    ColB = ColB * (distanceFromCenter * (-6f) + 6.1f);
                };
                particleList[i].startColor = new Color(ColR, ColG, ColB, 1f);
            }

            if (type == "redDust")
            {
                ArmEmitter.name = "Red Dust Emitter";

                float ColR = 0.4f;
                float ColG = 0;
                float ColB = 0;
                particleSystemRenderer.material = RedDustMaterial;
                particleList[i].startSize = Random.Range(2.4f, 5.00f);
                particleList[i].rotation = Random.Range(0f, 360.00f);

                if (distanceFromCenter > 0.85f)
                {
                    ColR = ColR * (distanceFromCenter * (-6.66f) + 6.66f);

                };

                particleList[i].startColor = new Color(ColR, ColG, ColB, 1f);
            }

            if (type == "star")
            {
                ArmEmitter.name = "Star Emitter";
                particleList[i].startColor = new Color(Random.Range(0.8f, 1.00f), 1f, Random.Range(0.4f, 1.00f), 1f);
                particleList[i].startSize = Random.Range(0.4f, 1.00f);

            }




        }
        particleSystem.SetParticles(particleList, numberParticles);
        return result;
    }

    /***
    public void GenerateZones(int numOfZones, float rotation, float armSpread, float spin, float zonesAtCenterRatio)
    {

        Vector3 ZonePos, PrevZonePos, PrevPrevZonePos;
        ZonePos = PrevZonePos = PrevPrevZonePos = new Vector3(0, 0, 0);
        Vector3 ZoneA, ZoneB, ZoneC, ZoneD = new Vector3(0, 0, 0); //zone corners
        float OffsetX, OffsetZ, OffsetPrevX, OffsetPrevZ;
        OffsetX = OffsetZ = OffsetPrevX = OffsetPrevZ = 0;

        for (int i = 2; i < numOfZones; i++)
        {
            PrevPrevZonePos = PrevZonePos;
            PrevZonePos = ZonePos;

            ZonePos = Spiralize(i, numOfZones, rotation, armSpread, spin, zonesAtCenterRatio, false);

            OffsetX = ZonePos.x - PrevZonePos.x;
            OffsetZ = ZonePos.z - PrevZonePos.z;
            OffsetPrevX = PrevZonePos.x - PrevPrevZonePos.x;
            OffsetPrevZ = PrevZonePos.z - PrevPrevZonePos.z;

            ZoneA = 0.35f * (new Vector3(OffsetZ, 0, -OffsetX)) + PrevZonePos;
            ZoneD = 0.35f * (new Vector3(-OffsetZ, 0, OffsetX)) + PrevZonePos;
            ZoneB = 0.35f * (new Vector3(OffsetPrevZ, 0, -OffsetPrevX)) + PrevPrevZonePos;
            ZoneC = 0.35f * (new Vector3(-OffsetPrevZ, 0, OffsetPrevX)) + PrevPrevZonePos;

            //Creates a tiny gap between zones
            ZoneB = Vector3.MoveTowards(ZoneB, ZoneA, 5);
            ZoneC = Vector3.MoveTowards(ZoneC, ZoneD, 5);


            if (i >= 4)
            {
                //apuviivat
                /*    DrawLine(PrevZonePos, ZonePos, Color.green);
                    DrawLine(PrevZonePos, ZoneA, Color.red);
                    DrawLine(PrevZonePos, ZoneD, Color.blue);
                    DrawLine(ZoneD, ZoneC, Color.blue);
                    DrawLine(ZoneA, ZoneB, Color.red);*/

              //  GameObject ZoneInstance = Instantiate(ZonePrefab, new Vector3(0, 0, 0), transform.rotation) as GameObject;
              //  ZoneInstance.GetComponent<ZoneClick>().ZoneId = ZoneIdOrder;
              //  ZoneIdOrder++;
              //  ZoneInstance.transform.parent = this.transform;

                /*
                PolygonCollider2D Collider = ZoneInstance.GetComponent<PolygonCollider2D>();
                var points = Collider.points;
                points[0] = ZoneA;
                points[1] = ZoneB;
                points[2] = ZoneC;
                points[3] = ZoneD;
                Collider.points = points;*


                MeshFilter MeshFilter = ZoneInstance.GetComponent<MeshFilter>();
                Mesh mesh = new Mesh();
                MeshFilter.mesh = mesh;
                //We need two arrays one to hold the vertices and one to hold the triangles
                Vector3[] VerteicesArray = new Vector3[4];
                int[] trianglesArray = new int[6];

                //lets add 3 vertices in the 3d space
                VerteicesArray[0] = ZoneA;
                VerteicesArray[1] = ZoneB;
                VerteicesArray[2] = ZoneC;
                VerteicesArray[3] = ZoneD;

                //define the order in which the vertices in the VerteicesArray shoudl be used to draw the triangle
                trianglesArray[0] = 0;
                trianglesArray[1] = 1;
                trianglesArray[2] = 2;
                trianglesArray[3] = 2;
                trianglesArray[4] = 3;
                trianglesArray[5] = 0;


                //add these two triangles to the mesh
                mesh.vertices = VerteicesArray;
                mesh.triangles = trianglesArray;

                MeshCollider mc = ZoneInstance.AddComponent(typeof(MeshCollider)) as MeshCollider;

            }

        }
    }***/

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.parent = this.transform;
        myLine.name = "Line";
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.widthMultiplier = 0.5f;
        lr.positionCount = 2;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color, 0), new GradientColorKey(color, 1) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.4f, 0), new GradientAlphaKey(1, 1) }
        );
        lr.colorGradient = gradient;


        //    Debug.Log("aa" + );


        // GameObject.Destroy(myLine, duration);
    }


    Vector3 Spiralize(float star, int numberOfStars, float rotation, float armSpread, float spin, float starsAtCenterRatio, bool flat)
    {
    
        Vector3 StarPosition;
        float random_v = Random.Range(0.00f, 1.00f);
        float part = (float)star / (float)numberOfStars;
        part = Mathf.Pow(part, starsAtCenterRatio);

        float distanceFromCenter = (float)part;
        float position = (part * spin + rotation) * Mathf.PI * 2;

        float xFluctuation = (Pow3Constrained(Random.Range(0.00f, 1.00f)) - Pow3Constrained(Random.Range(0.00f, 1.00f))) * armSpread;
        float zFluctuation = (Pow3Constrained(Random.Range(0.00f, 1.00f)) - Pow3Constrained(Random.Range(0.00f, 1.00f))) * armSpread;

        float resultX = (float)Mathf.Cos(position) * distanceFromCenter / 2 + 0.5f + xFluctuation;
        float resultZ = (float)Mathf.Sin(position) * distanceFromCenter / 2 + 0.5f + zFluctuation;

        float resultY = 0f;

        resultX = (resultX - (0.5f)) * 300.0f;
        resultZ = (resultZ - (0.5f)) * 300.0f;

        if (!flat)
        {
            float distance = (Mathf.Pow(resultX * resultX + resultZ * resultZ, 0.5f));
            float GalaxyBulbHeightLine = -0.005f * distance * distance + 0.002f * distance + 11;
            float GalaxyEdgeHeightLine = -0.027f * distance + 4;
            if (GalaxyEdgeHeightLine < GalaxyBulbHeightLine) { resultY = GalaxyBulbHeightLine; } else { resultY = GalaxyEdgeHeightLine; };

            resultY = resultY * Random.Range(-1.00f, 1.00f);
        }
        else
        {
            resultY = 15f;
        }

        StarPosition = new Vector3(resultX, resultY, resultZ);
        return StarPosition;

    }



    // Use this for initialization
    void Start()
    {
       /// GenerateZones(22, 0f, 0.0f, 1.7f, 0.7f);
       // GenerateZones(22, 0.5f, 0.0f, 1.7f, 0.7f);
    }

    void OnEnable()
    {
        /*
        GenerateArm("star", 4000, 0f, 0.05f, 1.7f, 1.6f);
        GenerateArm("dust", 800, 0f, 0.05f, 1.7f, 1.1f);
        GenerateArm("redDust", 300, 0f, 0.05f, 1.7f, 1.0f);

        GenerateArm("star", 700, 0.25f, 0.03f, 1.7f, 1.2f);
        GenerateArm("star", 700, 0.75f, 0.03f, 1.7f, 1.2f);


        GenerateArm("star", 4000, 0.5f, 0.05f, 1.7f, 1.6f);
        GenerateArm("dust", 800, 0.5f, 0.05f, 1.7f, 1.6f);
        //  GenerateArm("star", 200000, 0.25f, 0.00f, 15f, 1.1f);
        //   GenerateArm("star", 200000, 0.75f, 0.00f, 15f, 1.1f);*/

    }



    // Update is called once per frame
    void Update()
    {

    }


}


