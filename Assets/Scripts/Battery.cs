using System;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class Battery : NetworkBehaviour, IPickable
{
    public static Action<ulong> onBatteryPickupEvent;
    
    [ServerRpc]
    public void PickupServerRpc(ulong playerId, ServerRpcParams serverRpcParams = default)
    {
        print("Pickup On Server");
        onBatteryPickupEvent?.Invoke(playerId);
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