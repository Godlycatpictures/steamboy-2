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
        // välj en layout
        DungeonLayoutSO selectedLayout = dungeonLayouts[Random.Range(0, dungeonLayouts.Count)];

        // vilka rum
        foreach (var room in selectedLayout.rooms)
        {
            rooms[room.id] = room;
            Debug.Log($"Room {room.id}: {room.roomType}");
        }

        // spawn
        foreach (var room in rooms.Values)
        {
            SpawnRooms();
            Debug.Log($"Room {room.id} spawned");
        }

        // korridorer
        ConnectRooms();
        Debug.Log("Rooms connected");
    }

    void SpawnRooms()
    {
        Queue<RoomData> queue = new Queue<RoomData>();
        HashSet<int> visited = new HashSet<int>();

        if (rooms.ContainsKey(0))
        {
            rooms[0].Position = Vector2.zero; // börja i början
            queue.Enqueue(rooms[0]);
            visited.Add(0);
        }

        while (queue.Count > 0)
        {
            RoomData current = queue.Dequeue();
            Vector2 currentPosition = current.Position;

            foreach (int connection in current.connections)
            {
                if (!visited.Contains(connection) && rooms.ContainsKey(connection))
                {
                    visited.Add(connection);
                    queue.Enqueue(rooms[connection]);

                    // Ensure rooms are spaced naturally without excessive gaps
                    Vector2 offset = GetSmarterOffset(currentPosition, connection);
                    rooms[connection].Position = currentPosition + offset;
                }
            }
        }

        ApplyRoomRepulsion(); // Adjust to avoid overlaps

        // Instantiate rooms at final positions
        foreach (var room in rooms.Values)
        {
            if (spawnedRooms.ContainsKey(room.id)) continue; // Prevent duplicate spawning

            Vector3 worldPosition = new Vector3(room.Position.x, room.Position.y, 0);
            GameObject prefab = GetPrefabForRoomType(room.roomType);
            if (prefab != null)
            {
                spawnedRooms[room.id] = Instantiate(prefab, worldPosition, Quaternion.identity);
            }
        }
    }

    // **Fix excessive distance issues**
    Vector2 GetSmarterOffset(Vector2 currentPosition, int connectionID)
    {
        float angle = Random.Range(0f, Mathf.PI * 2); // Random direction
        float minDistance = 6f; // Minimum spacing
        float maxDistance = 10f; // Maximum spacing
        float distance = Random.Range(minDistance, maxDistance);

        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
    }

    // **Prevent rooms from overlapping while maintaining structure**
    void ApplyRoomRepulsion()
    {
        float repulsionStrength = 1f; // Lowered strength for less drastic changes
        bool changed = true;

        while (changed)
        {
            changed = false;
            foreach (var roomA in rooms.Values)
            {
                foreach (var roomB in rooms.Values)
                {
                    if (roomA == roomB) continue;

                    Vector2 delta = roomA.Position - roomB.Position;
                    float distance = delta.magnitude;

                    if (distance < 5f) // Adjusted threshold
                    {
                        Vector2 push = delta.normalized * repulsionStrength;
                        roomA.Position += push / 2;
                        roomB.Position -= push / 2;
                        changed = true;
                    }
                }
            }
        }
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
