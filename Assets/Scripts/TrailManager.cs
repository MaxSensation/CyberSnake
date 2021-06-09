using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;

public class TrailManager : NetworkBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;

    private NetworkVariableFloat _trailLenght = new NetworkVariableFloat(new NetworkVariableSettings{ WritePermission = NetworkVariablePermission.ServerOnly}, 1f);
    private void Start()
    {
        if (IsServer)
        {
            Battery.onBatteryPickupEvent += IncreaseLength;   
        }
    }

    private void Update()
    {
        trailRenderer.time = _trailLenght.Value;
    }

    private void IncreaseLength(ulong playerID)
    {
        if (playerID == NetworkObjectId)
        {
            _trailLenght.Value += 1;
        }
    }
}