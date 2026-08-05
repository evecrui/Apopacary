using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SpawnPlants : MonoBehaviour
{
    public List<GameObject> SpawnedObjs;
    public float circleCircumf = 10f;
    public float chanceOfSpawning = 1f;


    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0f, 100f) < chanceOfSpawning)
        {
            GameObject plant = SpawnedObjs[Random.Range(0, SpawnedObjs.Count)];
            GameObject plampt = Instantiate(plant);
            plampt.name = plant.name;
            Vector2 pos = Random.insideUnitCircle * circleCircumf;
            plampt.transform.position = transform.position + new Vector3(pos.x, 0, pos.y);
        }
    }
}
