using System.Collections.Generic;
using UnityEngine;

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
        foreach (var room in rooms.Values)
        {
            SpawnRoom(room);
            Debug.Log($"Room {room.id} spawned");
        }

        // 4. Connect rooms with corridors
        ConnectRooms();
        Debug.Log("Rooms connected");
    }

    void SpawnRoom(RoomData room)
    {
        GameObject prefab = GetPrefabForRoomType(room.roomType);
        if (prefab == null) return;

        Vector3 worldPosition = new Vector3(room.id * 10, 0, 0); // Simple layout for now
        GameObject newRoom = Instantiate(prefab, worldPosition, Quaternion.identity);
        spawnedRooms[room.id] = newRoom;
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
