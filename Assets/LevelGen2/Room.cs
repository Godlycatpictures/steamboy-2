// Room.cs
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public RoomTemplate template;
    private Tilemap tilemap;
    private CompositeCollider2D roomCollider;

    public void Init(RoomTemplate template, GameObject doorPrefab)
    {
        this.template = template;

        // Instantiate the tilemap prefab
        Tilemap instance = Instantiate(template.tilemapPrefab, transform);
        tilemap = instance;

        GenerateColliderFromTilemap();
        SpawnDoors(doorPrefab);
    }

    void GenerateColliderFromTilemap()
    {
        // Ensure we have the required components
        TilemapCollider2D tilemapCollider = tilemap.gameObject.GetComponent<TilemapCollider2D>();
        if (tilemapCollider == null)
        {
            tilemapCollider = tilemap.gameObject.AddComponent<TilemapCollider2D>();
        }

        roomCollider = tilemap.gameObject.GetComponent<CompositeCollider2D>();
        if (roomCollider == null)
        {
            roomCollider = tilemap.gameObject.AddComponent<CompositeCollider2D>();
        }

        Rigidbody2D rb = tilemap.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = tilemap.gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
        }

        tilemapCollider.usedByComposite = true;
    }

    void SpawnDoors(GameObject doorPrefab)
    {
        foreach (Vector2 doorLocalPos in template.doorPoints)
        {
            Vector2 worldPos = (Vector2)transform.position + doorLocalPos;
            if (IsDoorValid(worldPos))
            {
                Instantiate(doorPrefab, worldPos, Quaternion.identity, transform);
            }
        }
    }

    bool IsDoorValid(Vector2 doorPos)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(doorPos, 0.1f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Door") && hit.transform.parent != transform)
            {
                return true;
            }
        }
        return false;
    }
}