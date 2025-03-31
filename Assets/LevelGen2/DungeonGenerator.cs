using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public static class Directions
{
    public static List<Vector2Int> Cardinal = new List<Vector2Int> {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
}

public class DungeonGenerator : MonoBehaviour
{
    // In DungeonGenerator.cs
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private RuleTile wallRuleTile; // For smart walls
    [SerializeField] private List<RoomTemplate> roomTemplates;
    [SerializeField] private RoomTemplate bossRoomTemplate;
    [SerializeField] private RoomTemplate secretRoomTemplate;
    [SerializeField] private int maxRooms = 10;
    [SerializeField] private GameObject doorPrefab; // Add this in Inspector

    private Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();
    private List<Vector2Int> endRooms = new List<Vector2Int>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Vector2Int startPos = Vector2Int.zero;
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startPos);
        rooms.Add(startPos, CreateRoom(startPos, GetRandomTemplate()));

        while (queue.Count > 0 && rooms.Count < maxRooms)
        {
            Vector2Int current = queue.Dequeue();
            bool addedAny = false;

            foreach (var dir in Directions.Cardinal)
            {
                Vector2Int neighbor = current + dir;
                if (rooms.ContainsKey(neighbor)) continue;

                // 50% chance + no loops (Isaac's rule)
                if (Random.value > 0.5f || CountNeighbors(neighbor) > 1) continue;

                RoomTemplate template = GetRandomTemplate();
                rooms.Add(neighbor, CreateRoom(neighbor, template));
                queue.Enqueue(neighbor);
                addedAny = true;
            }

            if (!addedAny) endRooms.Add(current);
        }
        PlaceSpecialRooms();
    }

    int CountNeighbors(Vector2Int pos)
    {
        int count = 0;
        foreach (var dir in Directions.Cardinal)
        {
            if (rooms.ContainsKey(pos + dir)) count++;
        }
        return count;
    }

    RoomTemplate GetRandomTemplate()
    {
        return roomTemplates[Random.Range(0, roomTemplates.Count)];
    }

    Room CreateRoom(Vector2Int gridPos, RoomTemplate template)
    {
        GameObject roomObj = new GameObject($"Room_{gridPos.x}_{gridPos.y}");
        roomObj.transform.position = new Vector3(gridPos.x * 20, gridPos.y * 20, 0);
        Room room = roomObj.AddComponent<Room>();
        room.Init(template, doorPrefab);
        return room;
    }

    void PlaceSpecialRooms()
    {
        // Boss room at furthest dead end
        if (endRooms.Count > 0)
        {
            Vector2Int bossPos = endRooms[endRooms.Count - 1];
            rooms[bossPos].Init(bossRoomTemplate, doorPrefab);
        }

        // Secret room (simplified placement)
        foreach (var pos in rooms.Keys)
        {
            if (CountNeighbors(pos) >= 3 && !rooms.ContainsKey(pos + Vector2Int.up))
            {
                rooms.Add(pos + Vector2Int.up, CreateRoom(pos + Vector2Int.up, secretRoomTemplate));
                break;
            }
        }
    }
}