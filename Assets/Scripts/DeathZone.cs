using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var destroyable = other.GetComponent<IDestroyable>();
        destroyable?.Kill();
    }
}
