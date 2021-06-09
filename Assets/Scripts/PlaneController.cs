using System;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlaneController : NetworkBehaviour, IDestroyable
{
    [SerializeField] private float speed;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private MeshRenderer ship;
    [SerializeField] private GameObject explosionParticles;
    private NetworkVariableColor _color = new NetworkVariableColor(new NetworkVariableSettings{WritePermission = NetworkVariablePermission.ServerOnly}, new Color(0,0,1));
    private NetworkVariableBool _alive = new NetworkVariableBool(new NetworkVariableSettings{WritePermission = NetworkVariablePermission.ServerOnly}, true);
    private GridManager _gridManager;
    private Vector3 _nextGridPoint;
    private bool _hasNext;
    private bool _hasSetTarget;
    private Vector3 _turn;
    private float _gridSize;

    private void Start()
    {
        SetColor(FindObjectOfType<ColorManager>().GetColor());
    }

    private void Update()
    {
        if (IsLocalPlayer && _alive.Value)
        {
            if (!_hasSetTarget)
            {
                if (SceneManager.GetActiveScene().name == "Game")
                {
                    _gridManager = FindObjectOfType<GridManager>();
                    _gridSize = _gridManager.GetGridSize();
                    FindObjectOfType<CameraFollower>().SetTarget(transform);
                    GetComponent<PlayerInput>().enabled = true;
                    _hasSetTarget = true;
                }
            }
            else
            {
                AddForwardMovement();
                CheckForNextTurn();
            }
        }

        if (IsLocalPlayer && !_alive.Value)
        {
            FindObjectOfType<CameraFollower>().SetTarget();
        }
    }

    private void SetColor(Color color)
    {
        if (IsServer) _color.Value = color;
        trailRenderer.material.SetColor("Glow_color", _color.Value);
        ship.materials[8].SetColor("_EmissionColor", _color.Value*40);
        ship.materials[7].SetColor("_EmissionColor", _color.Value*40);
    }

    public void Control(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (context.ReadValue<Vector2>() == Vector2.left)
            _turn = new Vector3(0f, -90.0f, 0f);
        else if (context.ReadValue<Vector2>() == Vector2.right)
            _turn = new Vector3(0f, 90.0f, 0f);
        else if (context.ReadValue<Vector2>() == Vector2.up)
            _turn = new Vector3(-90f, 0f, 0f);
        else if (context.ReadValue<Vector2>() == Vector2.down) 
            _turn = new Vector3(90f, 0f, 0f);
        if (_hasNext) return;
        WaitForTurn();
    }
    
    public void RotateLeft(InputAction.CallbackContext context)
    {
        if (context.performed) transform.Rotate(0f, 0f, 90f);
    }
    
    public void RotateRight(InputAction.CallbackContext context)
    {
        if (context.performed) transform.Rotate(0f, 0f, -90f);
    }
    
    private void WaitForTurn()
    {
        _nextGridPoint = _gridManager.GetPointOnGrid(transform.position + transform.forward * _gridSize);
        _hasNext = true;
    }
    
    private void CheckForNextTurn()
    {
        if (!_hasNext || !(Vector3.Dot(_nextGridPoint - transform.position, transform.forward) <= 0)) return;
        transform.Rotate(_turn.x, _turn.y, _turn.z);
        _hasNext = false;
    }

    private void AddForwardMovement()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    public void Kill()
    {
        if (IsServer)
        {
            print("Dead");
            _alive.Value = false;
            GameObject go = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
        }
    }
}
