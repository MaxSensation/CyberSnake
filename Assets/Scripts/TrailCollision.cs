//Andreas Berzelius

using System.Collections.Generic;
using UnityEngine;

public class TrailCollision : MonoBehaviour
{
    private TrailRenderer _trailRenderer;
    private List<Vector3> _trailPositions;
    private int _trailSegments;
    private float _timer;
    private float _startUpTime = 5f;
    private float _currentTime;
    private bool _timerStarted;
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
        if (_timerStarted && Time.time - _currentTime >= _startUpTime)
        {
            _trailRenderer.emitting = true;
        }
    }

    public void StartTrail()
    {
        _currentTime = Time.time;
        _timerStarted = true;
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
                  if (_timerStarted)
                  {
                      var destroyable = hitInfo.collider.GetComponent<IDestroyable>();
                      destroyable?.Kill();   
                  }
              }
            }
        }
    }
    
}
