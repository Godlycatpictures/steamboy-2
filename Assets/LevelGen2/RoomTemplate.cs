// RoomTemplate.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Dungeon/Room Template")]
public class RoomTemplate : ScriptableObject
{
    public Tilemap tilemapPrefab; // Prefab with Tilemap setup
    public List<Vector2> doorPoints; // Local positions (0-20 units)
    public int width = 20;
    public int height = 20;
}