using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private float gridSize;

    public Vector3 GetPointOnGrid(Vector3 position)
    {
        position -= transform.position;
        return new Vector3(
            Mathf.RoundToInt(position.x / gridSize) * gridSize,
            Mathf.RoundToInt(position.y / gridSize) * gridSize,
            Mathf.RoundToInt(position.z / gridSize) * gridSize
        );
    }

    public float GetGridSize()
    {
        return gridSize;
    }
}
