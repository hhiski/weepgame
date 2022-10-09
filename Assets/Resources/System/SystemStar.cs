using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CelestialBody;

public class SystemStar : MonoBehaviour
{
    public Star Star;
    public int StarType;
    public Color starColor;
    public Color starColorCold;

    void Start()
    {


        /*
        B 	10,000–30,000 K blue white deep blue white 	2.1–16 M☉ 	1.8–6.6 R☉ 	25–30,000 L☉ 	Medium 	0.13%
        A 	7,500–10,000 K white   blue white 	1.4–2.1 M☉ 	1.4–1.8 R☉ 	5–25 L☉ 	Strong 	0.6%
        F 	6,000–7,500 K yellow white white 	1.04–1.4 M☉ 	1.15–1.4 R☉ 	1.5–5 L☉ 	Medium 	3%
        G 	5,200–6,000 K yellow  yellowish white 	0.8–1.04 M☉ 	0.96–1.15 R☉ 	0.6–1.5 L☉ 	Weak 	7.6%
        K 	3,700–5,200 K light orange pale yellow orange 	0.45–0.8 M☉ 	0.7–0.96 R☉ 	0.08–0.6 L☉ 	Very weak 	12.1%
        M 	2,400–3,700 K orange red
        */

        

    }


    void OnMouseDown()
    {
        // this.transform.parent.GetComponent<SystemPlanet>().SetUiPlanetData();
        UiCanvas UI = GameObject.Find("/Canvas").GetComponent<UiCanvas>();
        UI.StarDataView(Star);
    }

    void Update()
    {
        transform.RotateAround(gameObject.transform.position, Vector3.up, 0.6f * Time.deltaTime);
    }
}
