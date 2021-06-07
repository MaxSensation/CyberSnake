using UnityEngine;
using UnityEngine.InputSystem;

public class Plane : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GridManager gridManager;
    
    private Vector3 _nextGridPoint;
    private bool _hasNext;
    private Vector3 _turn;
    private float _gridSize;

    private void Start()
    {
        _gridSize = gridManager.GetGridSize();
    }

    private void Update()
    {
        Move();
        CheckForNextTurn();
    }

    public void Control(InputAction.CallbackContext context)
    {
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

    private void Move()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_nextGridPoint, Vector3.one);
    }
}
