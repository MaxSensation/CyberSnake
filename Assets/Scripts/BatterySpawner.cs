using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    [SerializeField] private int totalBatteriesOnMap;
    [SerializeField] private GameObject batteryPrefab;
    [SerializeField] private float worldSize;
    [SerializeField] private GridManager gridManager;
    
    private List<GameObject> _batteries = new List<GameObject>();

    private void Start()
    {
        if (!NetworkManager.Singleton.IsServer) return;
        SpawnAll();
        Battery.onBatteryPickupEvent += g => Spawn();
    }

    private void SpawnAll()
    {
        for (var i = 0; i < totalBatteriesOnMap; i++)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        _batteries.Add(Instantiate(batteryPrefab, gridManager.GetPointOnGrid(new Vector3(Random.Range(-worldSize / 2, worldSize / 2), Random.Range(-worldSize / 2, worldSize / 2), Random.Range(-worldSize / 2, worldSize / 2))), Quaternion.identity));
    }
}