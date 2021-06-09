using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private float smoothPosition;
    [SerializeField] private float smoothRotation;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] public Transform spectatorTarget;

    private Transform _target;
    private void Update()
    {
        if (_target == null) return;
        transform.position = Vector3.Lerp(transform.position, _target.position + new Vector3(0, positionOffset.y, 0).magnitude * _target.up - positionOffset.magnitude * _target.forward, smoothPosition * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, _target.rotation, smoothRotation * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }
    
    public void SetTarget()
    {
        _target = spectatorTarget;
    }
}
