using UnityEngine;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour
{
    public const int GRID_WIDTH = 9;
    public const int GRID_HEIGHT = 8;
    public const float ROOM_SIZE = 10f; // 10x10 units per room

    public Dictionary<Vector2Int, GameObject> rooms = new Dictionary<Vector2Int, GameObject>();

    // Convert grid position to world position (center of room)
    public Vector2 GridToWorldPos(Vector2Int gridPos)
    {
        return new Vector2(gridPos.x * ROOM_SIZE, gridPos.y * ROOM_SIZE);
    }
}