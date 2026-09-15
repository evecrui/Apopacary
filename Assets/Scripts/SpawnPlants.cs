using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SpawnPlants : MonoBehaviour
{
    public List<GameObject> SpawnedObjs;
    public float chanceOfSpawning = 1f;
    public float distBetween = 1.66f;
    public bool[,] plantsThere;

    private void Start()
    {
        plantsThere = new bool[6, 6];
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0f, 100f) < chanceOfSpawning)
        {
            Vector2Int pos = new Vector2Int(Random.Range(0, 6), Random.Range(0, 6)); // spawns in six by six area
            if (plantsThere[pos.x, pos.y])
                return;
            plantsThere[pos.x, pos.y] = true;
            GameObject plantPref = SpawnedObjs[pos.x];
            GameObject plampt = Instantiate(plantPref);
            plampt.name = plantPref.name;
            plampt.transform.position = transform.position + new Vector3(pos.x * distBetween, 0, pos.y * distBetween);
        }
    }
}
