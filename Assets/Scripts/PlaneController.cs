using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlaneController : NetworkBehaviour, IDestroyable
{
    [SerializeField] private float speed;
    [SerializeField] private float lenghtIncrease;
    [SerializeField] private TrailRenderer trailRenderer;

    private GridManager _gridManager;
    private Vector3 _nextGridPoint;
    private bool _hasNext;
    private bool _hasSetTarget;
    private Vector3 _turn;
    private float _gridSize;

    public void Start()
    {
        if (IsServer)
        {
            Battery.onBatteryPickupEvent += playerId => IncreaseTrailLenghtServerRpc(playerId);   
        }
    }

    [ServerRpc]
    private void IncreaseTrailLenghtServerRpc(ulong playerID, ServerRpcParams serverRpcParams = default)
    {
        if (NetworkObjectId != playerID) return;
        trailRenderer.time += lenghtIncrease;
        print("Server: Battery Picked up");
        IncreaseTrailLenghtClientRpc(playerID, trailRenderer.time);
    }
    
    [ClientRpc]
    private void IncreaseTrailLenghtClientRpc(ulong playerID, float time, ClientRpcParams clientRpcParams = default)
    {
        if (NetworkObjectId != playerID) return;
        trailRenderer.time = time;
        print("Client: Battery Picked up");
    }

    private void Update()
    {
        if (IsLocalPlayer && !_hasSetTarget)
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
        // Add Explosion Effect
        // Add UI GameOver
        //print("GameOver!");
        //gameObject.SetActive(false);
    }
}
