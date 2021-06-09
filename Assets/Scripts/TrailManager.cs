using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;

public class TrailManager : NetworkBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;

    private NetworkVariableFloat _trailLenght = new NetworkVariableFloat(new NetworkVariableSettings{ WritePermission = NetworkVariablePermission.ServerOnly}, 1f);
    private void Start()
    {
        InGameMenu.onRestartEvent += Restart;
        if (IsServer)
        {
            Battery.onBatteryPickupEvent += IncreaseLength;   
        }
    }

    private void Restart()
    {
        if (IsServer)
        {
            _trailLenght.Value = 1;
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
            if (IsServer)
            {
                _trailLenght.Value += 1;   
            }
        }
    }
}