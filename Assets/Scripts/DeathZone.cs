using MLAPI;
using UnityEngine;

public class DeathZone : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            var destroyable = other.GetComponent<IDestroyable>();
            destroyable?.Kill();   
        }
    }
}
