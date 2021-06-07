using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothPosition;
    [SerializeField] private float smoothRotation;
    [SerializeField] private Vector3 positionOffset;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, positionOffset.y, 0).magnitude * target.up - positionOffset.magnitude * target.forward, smoothPosition * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, smoothRotation * Time.deltaTime);
    }
}
