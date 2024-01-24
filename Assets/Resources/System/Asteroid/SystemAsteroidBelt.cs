using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAsteroidBelt : MonoBehaviour
{
    public int Type;
    public float Distance;

    public GameObject RockAsteroidBelt;
    public GameObject IceAsteroidBelt;
    public GameObject GasAsteroidBelt;
    public GameObject AsteroidBeltPrefab;

    void Start()
    {

        switch (Type)
        {
            case 1:
                AsteroidBeltPrefab = IceAsteroidBelt; break;
            case 2:
                AsteroidBeltPrefab = RockAsteroidBelt; break;
            case 3:
                AsteroidBeltPrefab = GasAsteroidBelt; break;
            default:
                AsteroidBeltPrefab = RockAsteroidBelt; break;
        }

        AsteroidBeltPrefab = Instantiate(AsteroidBeltPrefab, transform.position, transform.rotation) as GameObject;

        ParticleSystem.ShapeModule Shape = AsteroidBeltPrefab.GetComponent<ParticleSystem>().shape;
        Shape.radius = Distance;

        AsteroidBeltPrefab.transform.parent = this.transform;

    }

    void Update()
    {
        
    }
}
