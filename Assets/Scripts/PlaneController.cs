using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneController : MonoBehaviour, IDestroyable
{
    [SerializeField] private float speed;
    [SerializeField] private float lenghtIncrease;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private TrailRenderer trailRenderer;

    private Vector3 _nextGridPoint;
    private bool _hasNext;
    private Vector3 _turn;
    private float _gridSize;

    private void Start()
    {
        _gridSize = gridManager.GetGridSize();
        Battery.onBatteryPickupEvent += IncreaseTrailLenght;
    }

    private void IncreaseTrailLenght(GameObject g)
    {
        if (g != gameObject) return;
        trailRenderer.time += lenghtIncrease;
        print("Picked up Battery and increasing the size");
    }

    private void Update()
    {
        AddForwardMovement();
        CheckForNextTurn();
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
        _nextGridPoint = gridManager.GetPointOnGrid(transform.position + transform.forward * _gridSize);
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
        print("GameOver!");
        gameObject.SetActive(false);
    }
}
