using System;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class Battery : NetworkBehaviour, IPickable
{
    public static Action<ulong> onBatteryPickupEvent;
    [SerializeField] private GameObject pickupEffectGameObject;
    
    [ServerRpc]
    public void PickupServerRpc(ulong playerId, ServerRpcParams serverRpcParams = default)
    {
        onBatteryPickupEvent?.Invoke(playerId);
        GameObject go = Instantiate(pickupEffectGameObject, transform.position, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsServer)
        {
            print("Collision");
            PickupServerRpc(other.GetComponent<NetworkObject>().NetworkObjectId);
        }
    }
}