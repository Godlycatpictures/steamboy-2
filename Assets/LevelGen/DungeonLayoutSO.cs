using System.Collections.Generic;
using UnityEngine;

namespace needRoomData
{
    [CreateAssetMenu(fileName = "NewDungeonLayout", menuName = "Dungeon/Dungeon Layout")]
    public class DungeonLayoutSO : ScriptableObject
    {
        public string dungeonName;
        public List<RoomData> rooms;
    }

    [System.Serializable]
    public class RoomData
    {
        public int id;
        public string roomType;
        public List<RoomConnection> connections;
        public Vector2 Position { get; set; }
    }

    [System.Serializable]
    public class RoomConnection
    {
        public int targetRoomID;
    }
}
