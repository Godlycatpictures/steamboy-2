// Door.cs
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector2Int targetRoom;
    private GridSystem grid;

    void Start()
    {
        grid = FindObjectOfType<GridSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }

    void TeleportPlayer(Transform player)
    {
        player.position = grid.GridToWorldPos(targetRoom);
        Camera.main.transform.position = new Vector3(
            player.position.x,
            player.position.y,
            Camera.main.transform.position.z
        );
    }
}