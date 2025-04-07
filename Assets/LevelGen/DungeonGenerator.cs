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
    private Dictionary<int, List<Transform>> availableDoors = new();

    void Start()
    {
        StartCoroutine(GenerateDungeonCoroutine());
    }

    IEnumerator GenerateDungeonCoroutine()
    {
        // Choose layout
        DungeonLayoutSO selectedLayout = dungeonLayouts[Random.Range(0, dungeonLayouts.Count)];

        foreach (var room in selectedLayout.rooms)
        {
            rooms[room.id] = room;
            yield return null;
        }

        // Spawn rooms - now with strict door count checking
        bool allRoomsValid = true;
        foreach (var room in rooms.Values)
        {
            if (!SpawnRoomWithExactDoors(room))
            {
                Debug.LogError($"No suitable room found for Room ID {room.id} with exactly {room.connections.Count} doors");
                allRoomsValid = false;
                break;
            }
            yield return null;
        }

        if (allRoomsValid)
        {
            // Connect rooms
            ConnectRooms();
            Debug.Log("Dungeon generation complete!");
        }
        else
        {
            Debug.LogError("Dungeon generation failed due to missing room prefabs with required door counts");
        }
    }

    bool SpawnRoomWithExactDoors(RoomData room)
    {
        if (spawnedRooms.ContainsKey(room.id)) return true;

        GameObject prefab = GetPrefabWithExactDoorCount(room.roomType, room.connections.Count);
        if (prefab == null) return false;

        // Stack rooms downward with spacing
        float spacing = 30f;
        Vector3 position = new Vector3(0, -room.id * spacing, 0);

        GameObject newRoom = Instantiate(prefab, position, Quaternion.identity);
        spawnedRooms[room.id] = newRoom;

        // Initialize available doors list
        TeleportConnector connector = newRoom.GetComponent<TeleportConnector>();
        if (connector != null)
        {
            availableDoors[room.id] = new List<Transform>(connector.doorPoints);

            // Verify the room has exactly the required number of doors
            if (connector.doorPoints.Count != room.connections.Count)
            {
                Debug.LogError($"Room {room.id} has {connector.doorPoints.Count} doors but needs exactly {room.connections.Count}");
                return false;
            }
        }

        return true;
    }

    void ConnectRooms()
    {
        StartCoroutine(ConnectRoomsCoroutine());
    }

    IEnumerator ConnectRoomsCoroutine()
    {
        // First create all possible connections between rooms
        foreach (var room in rooms.Values)
        {
            if (!spawnedRooms.TryGetValue(room.id, out GameObject roomObj)) continue;
            TeleportConnector connector = roomObj.GetComponent<TeleportConnector>();
            if (connector == null) continue;

            foreach (int connectedId in room.connections)
            {
                // Skip if already linked or if target room doesn't exist
                if (connector.GetConnectedDoor(connectedId)) continue;
                if (!spawnedRooms.TryGetValue(connectedId, out GameObject targetRoom)) continue;

                TeleportConnector targetConnector = targetRoom.GetComponent<TeleportConnector>();
                if (targetConnector == null) continue;

                // Find available doors in both rooms
                if (availableDoors[room.id].Count == 0 || availableDoors[connectedId].Count == 0)
                {
                    Debug.LogWarning($"No available doors for connection between Room {room.id} and Room {connectedId}");
                    continue;
                }

                // Get first available door from each room
                Transform sourceDoor = availableDoors[room.id][0];
                Transform targetDoor = availableDoors[connectedId][0];

                DoorTeleportation sourceTeleport = sourceDoor.GetComponent<DoorTeleportation>();
                DoorTeleportation targetTeleport = targetDoor.GetComponent<DoorTeleportation>();

                if (sourceTeleport == null || targetTeleport == null) continue;

                // Link the doors
                connector.LinkDoor(connectedId, targetDoor);
                targetConnector.LinkDoor(room.id, sourceDoor);

                sourceTeleport.connectedRoomID = connectedId;
                targetTeleport.connectedRoomID = room.id;

                // Remove used doors from available lists
                availableDoors[room.id].Remove(sourceDoor);
                availableDoors[connectedId].Remove(targetDoor);

                yield return null;
            }
        }

        Debug.Log("Room connections established.");
    }

    GameObject GetPrefabWithExactDoorCount(string type, int requiredDoors)
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

        // Find prefabs with EXACTLY the required number of doors
        List<GameObject> suitablePrefabs = pool.Where(p =>
        {
            var connector = p.GetComponent<TeleportConnector>();
            return connector != null && connector.doorPoints.Count == requiredDoors;
        }).ToList();

        return suitablePrefabs.Count > 0
            ? suitablePrefabs[Random.Range(0, suitablePrefabs.Count)]
            : null; // Return null if no suitable prefab found
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

                Gizmos.color = Color.green;
                Gizmos.DrawLine(roomObj.transform.position, connectedRoom.transform.position);
            }
        }
    }
}