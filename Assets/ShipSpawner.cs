using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public GameObject ShipPrefab;
    public List<Transform> Spawners;
    
    IEnumerator Start()
    {
        while (true)
        {
            var randomWait = Random.Range(Globals.MinWaitTime, Globals.MaxWaitTime);
            var randomSpawnerIndex = Random.Range(0, Spawners.Count);
            var randomSpawner = Spawners[randomSpawnerIndex];
            Instantiate(ShipPrefab, randomSpawner.position, Quaternion.identity);
            yield return new WaitForSeconds(randomWait);
        }
    }
}
