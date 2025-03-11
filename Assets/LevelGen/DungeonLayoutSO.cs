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
        public string roomType; // "Start", "Normal", "Boss", etc.
        public List<int> connections;
        public Vector2Int Position { get; set; }
    }
}

