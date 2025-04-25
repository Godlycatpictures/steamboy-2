using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using needRoomData;

public class DungeonGenerator1 : MonoBehaviour
{
    public List<DungeonLayoutSO> dungeonLayouts;
    public List<GameObject> StartRooms, NormalRooms, ShopNPCRooms, BossRooms;

    private Dictionary<int, RoomData> rooms = new();
    private Dictionary<int, GameObject> spawnedRooms = new();
    private Dictionary<int, List<Transform>> availableDoors = new();

    private Dictionary<Direction, Direction> opposite = new()
    {
        { Direction.Up, Direction.Down },
        { Direction.Down, Direction.Up },
        { Direction.Left, Direction.Right },
        { Direction.Right, Direction.Left }
    };

    void Start()
    {
        StartCoroutine(GenerateDungeonCoroutine());
    }

    IEnumerator GenerateDungeonCoroutine()
    {
        // Kontrollera om dungeonLayouts är tomt
        if (dungeonLayouts == null || dungeonLayouts.Count == 0)
        {
            Debug.LogError("No dungeon layouts available.");
            yield break;
        }

        // Välj en slumpmässig dungeonlayout
        DungeonLayoutSO layout = dungeonLayouts[Random.Range(0, dungeonLayouts.Count)];

        if (layout == null)
        {
            Debug.LogError("Selected dungeon layout is null.");
            yield break;
        }

        Debug.Log($"Generating dungeon: {layout.dungeonName}");
        Debug.Log($"Number of rooms in layout: {layout.rooms.Count}");

        // Lägg till alla rum i vår dictionary
        foreach (var room in layout.rooms)
        {
            rooms[room.id] = room;
        }

        // Starta från första rummet
        if (layout.rooms.Count > 0)
        {
            RoomData startRoom = layout.rooms[0];
            yield return SpawnRoom(startRoom, Vector3.zero);
        }
        else
        {
            Debug.LogError("No rooms found in selected dungeon layout.");
            yield break;
        }

        Queue<int> toProcess = new Queue<int>();
        HashSet<int> visited = new HashSet<int> { layout.rooms[0].id };
        toProcess.Enqueue(layout.rooms[0].id);

        // Bearbeta varje rum i kö för att koppla ihop dem
        while (toProcess.Count > 0)
        {
            int currentRoomId = toProcess.Dequeue();
            RoomData currentRoom = rooms[currentRoomId];

            if (!spawnedRooms.TryGetValue(currentRoomId, out GameObject currentRoomObj)) continue;
            if (!availableDoors.TryGetValue(currentRoomId, out var currentDoors)) continue;

            foreach (var connection in currentRoom.connections)
            {
                Debug.Log($"Processing room {currentRoom.id}, connections count: {currentRoom.connections.Count}");
                int targetRoomId = connection.targetRoomID;
                RoomData targetRoom = rooms[targetRoomId];

                // Här beräknar vi riktningen baserat på start rummets riktning
                Direction exitDir = GetDirectionBasedOnPosition(currentRoom, targetRoom);

                if (visited.Contains(targetRoomId)) continue;

                // Se till att det finns en dörr i den riktningen för nästa rum
                if (!GetDoorInDirection(currentRoomObj, exitDir, out Transform sourceDoor)) continue;

                Vector3 newPos = GetNewRoomPosition(currentRoomObj.transform.position, exitDir);
                yield return SpawnRoom(targetRoom, newPos);

                if (!spawnedRooms.TryGetValue(targetRoomId, out GameObject targetRoomObj)) continue;

                Direction entranceDir = opposite[exitDir];
                if (!GetDoorInDirection(targetRoomObj, entranceDir, out Transform targetDoor)) continue;

                LinkDoors(currentRoomId, targetRoomId, sourceDoor, targetDoor);

                toProcess.Enqueue(targetRoomId);
                visited.Add(targetRoomId);

                yield return null;
            }
        }

        Debug.Log("Dungeon generation complete!");
    }

    // Metod för att dynamiskt beräkna riktningen mellan två rum baserat på deras positioner
    Direction GetDirectionBasedOnPosition(RoomData roomA, RoomData roomB)
    {
        // Hämta positionerna för de två rummen
        Vector3 positionA = GetRoomPosition(roomA);
        Vector3 positionB = GetRoomPosition(roomB);

        // Jämför positionerna för att bestämma riktningen
        if (positionB.y > positionA.y) return Direction.Up;
        if (positionB.y < positionA.y) return Direction.Down;
        if (positionB.x > positionA.x) return Direction.Right;
        if (positionB.x < positionA.x) return Direction.Left;

        return Direction.Up; // Standardriktning om positionerna är exakt lika (kan anpassas om det behövs)
    }

    // Hämta positionen för ett rum (kan anpassas beroende på hur positionen sparas i layouten)
    Vector3 GetRoomPosition(RoomData room)
    {
        return room.Position; // Här används room.Position, men du kan justera denna kod baserat på din layout
    }

    // Metod för att spawna ett rum på en viss position
    IEnumerator SpawnRoom(RoomData room, Vector3 position)
    {
        if (spawnedRooms.ContainsKey(room.id)) yield break;

        Debug.Log($"Spawning room {room.id} at position {position}");

        // Hämta prefab för rummet baserat på dess typ och dörrantal
        GameObject prefab = GetPrefabWithExactDoorCount(room.roomType, room.connections.Count);
        if (prefab == null)
        {
            Debug.LogError($"No prefab found for Room ID {room.id} with {room.connections.Count} doors");
            yield break;
        }

        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        spawnedRooms[room.id] = instance;

        // Hämta dörrpunkter från TeleportConnector för det nya rummet
        var connector = instance.GetComponent<TeleportConnector>();
        if (connector != null)
        {
            availableDoors[room.id] = new List<Transform>(connector.doorPoints);
        }

        yield return null;
    }

    // Metod för att koppla samman dörrarna mellan två rum
    void LinkDoors(int roomAId, int roomBId, Transform doorA, Transform doorB)
    {
        TeleportConnector connectorA = spawnedRooms[roomAId].GetComponent<TeleportConnector>();
        TeleportConnector connectorB = spawnedRooms[roomBId].GetComponent<TeleportConnector>();

        DoorTeleportation teleportA = doorA.GetComponent<DoorTeleportation>();
        DoorTeleportation teleportB = doorB.GetComponent<DoorTeleportation>();

        connectorA.LinkDoor(roomBId, doorB);
        connectorB.LinkDoor(roomAId, doorA);

        teleportA.connectedRoomID = roomBId;
        teleportB.connectedRoomID = roomAId;

        // Logga när dörrarna länkas
        Debug.Log($"Linking door {doorA.name} in room {roomAId} to door {doorB.name} in room {roomBId}");

        availableDoors[roomAId].Remove(doorA);
        availableDoors[roomBId].Remove(doorB);
    }


    // Metod för att hitta dörren i en viss riktning för ett rum
    bool GetDoorInDirection(GameObject roomObj, Direction direction, out Transform door)
    {
        var connector = roomObj.GetComponent<TeleportConnector>();
        door = connector.doorPoints.FirstOrDefault(d =>
        {
            var tele = d.GetComponent<DoorTeleportation>();
            return tele != null && tele.direction == direction;
        });

        if (door != null)
        {
            Debug.Log($"Found door in direction {direction} for room {roomObj.name}");
        }
        else
        {
            Debug.Log($"No door found in direction {direction} for room {roomObj.name}");
        }

        return door != null;
    }


    // Metod för att beräkna den nya positionen för ett rum baserat på den aktuella positionen och riktningen
    Vector3 GetNewRoomPosition(Vector3 currentPos, Direction direction)
    {
        Vector3 newPos = direction switch
        {
            Direction.Up => currentPos + new Vector3(0, 30f, 0),
            Direction.Down => currentPos + new Vector3(0, -30f, 0),
            Direction.Left => currentPos + new Vector3(-30f, 0, 0),
            Direction.Right => currentPos + new Vector3(30f, 0, 0),
            _ => currentPos
        };

        // Logga den beräknade nya positionen
        Debug.Log($"Calculated new room position: {newPos} for direction: {direction}");

        return newPos;
    }


    // Metod för att få en prefab för ett rum baserat på typ och dörrantal
    GameObject GetPrefabWithExactDoorCount(string type, int doorCount)
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

        // Hitta rum med rätt dörrantal för den nuvarande layouten
        var candidates = pool.Where(p =>
        {
            var con = p.GetComponent<TeleportConnector>();
            return con != null && con.doorPoints.Count == doorCount;
        }).ToList();

        return candidates.Count > 0 ? candidates[Random.Range(0, candidates.Count)] : null;
    }

    void OnDrawGizmos()
    {
        if (spawnedRooms == null) return;

        // Rita ut rummen och deras kopplingar i Gizmos
        foreach (var room in rooms.Values)
        {
            if (!spawnedRooms.TryGetValue(room.id, out var roomObj)) continue;

            foreach (var conn in room.connections)
            {
                if (!spawnedRooms.TryGetValue(conn.targetRoomID, out var targetObj)) continue;
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(roomObj.transform.position, targetObj.transform.position);
            }
        }
    }
}
