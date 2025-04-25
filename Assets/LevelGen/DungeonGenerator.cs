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
        // Kontrollera om dungeonLayouts �r tomt
        if (dungeonLayouts == null || dungeonLayouts.Count == 0)
        {
            Debug.LogError("No dungeon layouts available.");
            yield break;
        }

        // V�lj en slumpm�ssig dungeonlayout
        DungeonLayoutSO layout = dungeonLayouts[Random.Range(0, dungeonLayouts.Count)];

        if (layout == null)
        {
            Debug.LogError("Selected dungeon layout is null.");
            yield break;
        }

        Debug.Log($"Generating dungeon: {layout.dungeonName}");
        Debug.Log($"Number of rooms in layout: {layout.rooms.Count}");

        // L�gg till alla rum i v�r dictionary
        foreach (var room in layout.rooms)
        {
            rooms[room.id] = room;
        }

        // Starta fr�n f�rsta rummet
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

        // Bearbeta varje rum i k� f�r att koppla ihop dem
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

                // H�r ber�knar vi riktningen baserat p� start rummets riktning
                Direction exitDir = GetDirectionBasedOnPosition(currentRoom, targetRoom);

                if (visited.Contains(targetRoomId)) continue;

                // Se till att det finns en d�rr i den riktningen f�r n�sta rum
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

    // Metod f�r att dynamiskt ber�kna riktningen mellan tv� rum baserat p� deras positioner
    Direction GetDirectionBasedOnPosition(RoomData roomA, RoomData roomB)
    {
        // H�mta positionerna f�r de tv� rummen
        Vector3 positionA = GetRoomPosition(roomA);
        Vector3 positionB = GetRoomPosition(roomB);

        // J�mf�r positionerna f�r att best�mma riktningen
        if (positionB.y > positionA.y) return Direction.Up;
        if (positionB.y < positionA.y) return Direction.Down;
        if (positionB.x > positionA.x) return Direction.Right;
        if (positionB.x < positionA.x) return Direction.Left;

        return Direction.Up; // Standardriktning om positionerna �r exakt lika (kan anpassas om det beh�vs)
    }

    // H�mta positionen f�r ett rum (kan anpassas beroende p� hur positionen sparas i layouten)
    Vector3 GetRoomPosition(RoomData room)
    {
        return room.Position; // H�r anv�nds room.Position, men du kan justera denna kod baserat p� din layout
    }

    // Metod f�r att spawna ett rum p� en viss position
    IEnumerator SpawnRoom(RoomData room, Vector3 position)
    {
        if (spawnedRooms.ContainsKey(room.id)) yield break;

        Debug.Log($"Spawning room {room.id} at position {position}");

        // H�mta prefab f�r rummet baserat p� dess typ och d�rrantal
        GameObject prefab = GetPrefabWithExactDoorCount(room.roomType, room.connections.Count);
        if (prefab == null)
        {
            Debug.LogError($"No prefab found for Room ID {room.id} with {room.connections.Count} doors");
            yield break;
        }

        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        spawnedRooms[room.id] = instance;

        // H�mta d�rrpunkter fr�n TeleportConnector f�r det nya rummet
        var connector = instance.GetComponent<TeleportConnector>();
        if (connector != null)
        {
            availableDoors[room.id] = new List<Transform>(connector.doorPoints);
        }

        yield return null;
    }

    // Metod f�r att koppla samman d�rrarna mellan tv� rum
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

        // Logga n�r d�rrarna l�nkas
        Debug.Log($"Linking door {doorA.name} in room {roomAId} to door {doorB.name} in room {roomBId}");

        availableDoors[roomAId].Remove(doorA);
        availableDoors[roomBId].Remove(doorB);
    }


    // Metod f�r att hitta d�rren i en viss riktning f�r ett rum
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


    // Metod f�r att ber�kna den nya positionen f�r ett rum baserat p� den aktuella positionen och riktningen
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

        // Logga den ber�knade nya positionen
        Debug.Log($"Calculated new room position: {newPos} for direction: {direction}");

        return newPos;
    }


    // Metod f�r att f� en prefab f�r ett rum baserat p� typ och d�rrantal
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

        // Hitta rum med r�tt d�rrantal f�r den nuvarande layouten
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
