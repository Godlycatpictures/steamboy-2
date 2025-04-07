using UnityEngine;

public enum DoorDirection { Up, Down, Left, Right }

public class DoorTeleportation : MonoBehaviour
{
    public int connectedRoomID; // Set in DungeonGenerator1.cs
    private TeleportConnector connector;
    public DoorDirection direction;

    [Header("Teleport Settings")]
    public float teleportCooldown = 0.5f; // Prevents immediate re-triggering
    private float lastTeleportTime = -1f;
    private bool isTeleporting = false;

    void Start()
    {
        connector = GetComponentInParent<TeleportConnector>();
        if (connector == null)
        {
            Debug.LogError($"No TeleportConnector found in parent for door {gameObject.name}");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            Debug.Log($"Playe entered door {gameObject.name}");
            TryTeleportPlayer(other.gameObject);
        }
    }

    private void TryTeleportPlayer(GameObject player)
    {
        if (Time.time - lastTeleportTime < teleportCooldown) return;
        if (connector == null) return;

        Transform target = connector.GetConnectedDoor(connectedRoomID);
        if (target == null) return;

        isTeleporting = true;
        lastTeleportTime = Time.time;

        // Calculate offset to place player slightly in front of the target door
        Vector3 teleportOffset = GetTeleportOffset(target.GetComponent<DoorTeleportation>());
        player.transform.position = target.position + teleportOffset;

        // Reset teleport flag after a small delay
        Invoke(nameof(ResetTeleportFlag), 0.1f);
    }

    private Vector3 GetTeleportOffset(DoorTeleportation targetDoor)
    {
        if (targetDoor == null) return Vector3.forward * 5f;

        // Adjust this based on the target door's direction
        switch (targetDoor.direction)
        {
            case DoorDirection.Up:
                return Vector3.forward * -5f;
            case DoorDirection.Down:
                return Vector3.back * -5f;
            case DoorDirection.Left:
                return Vector3.left * -5f;
            case DoorDirection.Right:
                return Vector3.right * -5f;
            default:
                return Vector3.forward * -5f;
        }
    }

    private void ResetTeleportFlag()
    {
        isTeleporting = false;
    }

    void OnDrawGizmos()
    {
        // Draw direction line
        switch (direction)
        {
            case DoorDirection.Up:
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2);
                break;
            case DoorDirection.Down:
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 2);
                break;
            case DoorDirection.Left:
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + Vector3.left * 2);
                break;
            case DoorDirection.Right:
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 2);
                break;
        }

        // Draw trigger area
        Gizmos.color = new Color(0, 1, 1, 0.3f);
        if (TryGetComponent<Collider>(out var collider))
        {
            Gizmos.DrawCube(transform.position + collider.bounds.center, collider.bounds.size);
        }
    }
}