using System;
using MLAPI;
using UnityEngine;

public class Battery : MonoBehaviour, IPickable
{
    public static Action<GameObject> onBatteryPickupEvent;
    public void Pickup(GameObject g)
    {
        onBatteryPickupEvent?.Invoke(g);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var p = other.GetComponent<PlaneController>();
        if (p != null && NetworkManager.Singleton.IsServer)
        {
            Pickup(p.gameObject);
        }
    }
}