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
        onBatteryPickupEvent?.Invoke(playerId);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsServer)
        {
            PickupServerRpc(other.GetComponent<NetworkObject>().NetworkObjectId);
        }
    }
}