using System.Collections.Generic;
using UnityEngine;
using needRoomData;
using System.Collections;
using System.Linq;

public class DungeonGenerator1 : MonoBehaviour
{
    public List<DungeonLayoutSO> dungeonLayouts;
    public List<GameObject> StartRooms, NormalRooms, ShopNPCRooms, BossRooms;

    private Dictionary<int, RoomData> rooms = new();
    private Dictionary<int, GameObject> spawnedRooms = new();

    void Start()
    {
        StartCoroutine(GenerateDungeonCoroutine());
    }

    IEnumerator GenerateDungeonCoroutine()
    {
        // Force first layout for testing (0-1, 1-2, 1-3)
        DungeonLayoutSO selectedLayout = dungeonLayouts[0];

        // Initialize rooms
        foreach (var room in selectedLayout.rooms)
        {
            rooms[room.id] = room;
            yield return null;
        }

        // Spawn all rooms first
        foreach (var room in rooms.Values)
        {
            SpawnRoom(room);
            yield return null;
        }

        // Connect rooms
        ConnectRooms();
        Debug.Log("Dungeon generation complete!");
    }

    void SpawnRoom(RoomData room)
    {
        if (spawnedRooms.ContainsKey(room.id)) return;

        // Simple random positioning with some spacing
        Vector3 position = new Vector3(
            Random.Range(-20f, 20f),
            Random.Range(-20f, 20f),
            0
        );

        GameObject prefab = GetPrefabForRoomType(room.roomType, room.connections.Count);
        if (prefab != null)
        {
            spawnedRooms[room.id] = Instantiate(prefab, position, Quaternion.identity);
        }
    }

    void ConnectRooms()
    {
        StartCoroutine(ConnectRoomsCoroutine());
    }

    IEnumerator ConnectRoomsCoroutine()
    {
        foreach (var room in rooms.Values)
        {
            GameObject roomObj = spawnedRooms[room.id];
            TeleportConnector connector = roomObj.GetComponent<TeleportConnector>();
            if (connector == null) continue;

            // For each connection, find a matching door
            foreach (int connectedId in room.connections)
            {
                if (!spawnedRooms.ContainsKey(connectedId)) continue;

                GameObject targetRoom = spawnedRooms[connectedId];
                TeleportConnector targetConnector = targetRoom.GetComponent<TeleportConnector>();
                if (targetConnector == null) continue;

                // Find first available door in source room
                Transform sourceDoor = connector.doorPoints[0];
                if (sourceDoor == null) continue;

                // Find first available door in target room
                Transform targetDoor = targetConnector.doorPoints[0];
                if (targetDoor == null) continue;

                // Link them
                connector.LinkDoor(connectedId, targetDoor);
                targetConnector.LinkDoor(room.id, sourceDoor);

                // Set the connected room IDs
                DoorTeleportation doorA = sourceDoor.GetComponent<DoorTeleportation>();
                if (doorA != null) doorA.connectedRoomID = connectedId;

                DoorTeleportation doorB = targetDoor.GetComponent<DoorTeleportation>();
                if (doorB != null) doorB.connectedRoomID = room.id;

                yield return null;
            }
        }
    }

    GameObject GetPrefabForRoomType(string type, int minDoors)
    {
        List<GameObject> pool = type switch
        {
            "Start" => StartRooms,
            "Normal" => NormalRooms,
            "ShopNPC" => ShopNPCRooms,
            "Boss" => BossRooms,
            _ => null
        };

        if (pool == null || pool.Count == 0) return null;

        // Filter by minimum door count
        List<GameObject> suitablePrefabs = pool.Where(p =>
            p.GetComponent<TeleportConnector>()?.doorPoints.Count >= minDoors
        ).ToList();

        return suitablePrefabs.Count > 0
            ? suitablePrefabs[Random.Range(0, suitablePrefabs.Count)]
            : pool[Random.Range(0, pool.Count)];
    }

    void OnDrawGizmos()
    {
        if (spawnedRooms == null || spawnedRooms.Count == 0) return;

        foreach (var room in rooms.Values)
        {
            if (!spawnedRooms.TryGetValue(room.id, out var roomObj)) continue;

            foreach (int connectedId in room.connections)
            {
                if (!spawnedRooms.TryGetValue(connectedId, out var connectedRoom)) continue;

                // Draw a line between room centers
                Gizmos.color = Color.green;
                Gizmos.DrawLine(roomObj.transform.position, connectedRoom.transform.position);
            }
        }
    }
}