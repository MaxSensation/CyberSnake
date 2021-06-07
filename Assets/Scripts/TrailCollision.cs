//Andreas Berzelius

using System.Collections.Generic;
using UnityEngine;

public class TrailCollision : MonoBehaviour
{
    private TrailRenderer _trailRenderer;
    private List<Vector3> _trailPositions;
    private int _trailSegments;
    private float _timer;
    void Start()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _trailPositions = new List<Vector3>();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 0.1f)
        {
            AddTrailPositions();
            _timer = 0;
        }
    }

    private void AddTrailPositions()
    {
        _trailPositions.Clear();
        _trailSegments = _trailRenderer.positionCount;
        for (var i = 0; i < _trailSegments; i++)
        {
            _trailPositions.Add(_trailRenderer.GetPosition(i));
        }
        AddTrailCollision();
    }

    private void AddTrailCollision()
    {
        for (var i = 0; i < _trailSegments; i++)
        {
            var firstPos = _trailPositions[i];
            if (i + 1 < _trailSegments - 1)
            {
              var hit = Physics.Linecast(firstPos, _trailPositions[i + 1], out RaycastHit hitInfo);
              if (hit)
              {
                  var destroyable = hitInfo.collider.GetComponent<IDestroyable>();
                  destroyable?.Kill();
              }
            }
        }
    }
    
}
