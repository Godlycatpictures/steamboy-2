using System.Collections.Generic;
using UnityEngine;
using needRoomData;

public class DungeonGenerator : MonoBehaviour
{
    public List<DungeonLayoutSO> dungeonLayouts; // List of predefined layouts
    public List<GameObject> StartRooms, NormalRooms, RewardRooms, BossRooms;
    public GameObject CorridorPrefab;

    private Dictionary<int, RoomData> rooms = new();
    private Dictionary<int, GameObject> spawnedRooms = new();

    void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        // 1. Pick a random predefined dungeon layout
        DungeonLayoutSO selectedLayout = dungeonLayouts[Random.Range(0, dungeonLayouts.Count)];

        // 2. Store room data
        foreach (var room in selectedLayout.rooms)
        {
            rooms[room.id] = room;
            Debug.Log($"Room {room.id}: {room.roomType}");
        }

        // 3. Spawn rooms
        SpawnRooms();  // Call this once, not inside the loop
        Debug.Log("All rooms spawned");

        // 4. Connect rooms with corridors
        ConnectRooms();
        Debug.Log("Rooms connected");
    }


    void SpawnRooms()
    {
        Queue<RoomData> queue = new Queue<RoomData>();
        HashSet<int> visited = new HashSet<int>();

        // Start placement from the first room (ID 0)
        if (rooms.ContainsKey(0))
        {
            rooms[0].Position = Vector2Int.zero;
            queue.Enqueue(rooms[0]);
            visited.Add(0);
        }

        // Process rooms using BFS to ensure proper layout
        while (queue.Count > 0)
        {
            RoomData current = queue.Dequeue();
            Vector2Int currentPosition = current.Position;

            foreach (int connection in current.connections)
            {
                if (!visited.Contains(connection) && rooms.ContainsKey(connection))
                {
                    visited.Add(connection);
                    queue.Enqueue(rooms[connection]);

                    // Assign a position based on the connection direction
                    Vector2Int offset = GetPlacementOffset(currentPosition, connection);
                    rooms[connection].Position = currentPosition + offset;
                }
            }
        }

        // Instantiate all rooms in their calculated positions (this part handles spawning)
        foreach (var room in rooms.Values)
        {
            Vector3 worldPosition = new Vector3(room.Position.x * 10, room.Position.y * 10, 0);
            GameObject prefab = GetPrefabForRoomType(room.roomType);
            if (prefab != null)
            {
                if (!spawnedRooms.ContainsKey(room.id))  // Ensure no duplicates are spawned
                {
                    spawnedRooms[room.id] = Instantiate(prefab, worldPosition, Quaternion.identity);
                }
            }
        }
    }


    // Helper function to determine where to place connected rooms
    Vector2Int GetPlacementOffset(Vector2Int currentPosition, int connectionID)
    {
        if (connectionID % 2 == 0) return new Vector2Int(1, 0); // Right
        else return new Vector2Int(0, -1); // Down
    }

    void ConnectRooms()
    {
        foreach (var room in rooms.Values)
        {
            foreach (int connection in room.connections)
            {
                if (!spawnedRooms.ContainsKey(connection)) continue;

                Vector3 from = spawnedRooms[room.id].transform.position;
                Vector3 to = spawnedRooms[connection].transform.position;
                Vector3 midpoint = (from + to) / 2;

                Instantiate(CorridorPrefab, midpoint, Quaternion.identity);
            }
        }
    }

    GameObject GetPrefabForRoomType(string type)
    {
        return type switch
        {
            "Start" => StartRooms[Random.Range(0, StartRooms.Count)],
            "Normal" => NormalRooms[Random.Range(0, NormalRooms.Count)],
            "Reward" => RewardRooms[Random.Range(0, RewardRooms.Count)],
            "Boss" => BossRooms[Random.Range(0, BossRooms.Count)],
            _ => null
        };
    }
}