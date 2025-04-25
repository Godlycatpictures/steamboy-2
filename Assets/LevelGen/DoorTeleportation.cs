using UnityEngine;

public enum Direction { Up, Down, Left, Right }

public class DoorTeleportation : MonoBehaviour
{
    public int connectedRoomID = -1; // görs i dugeongenerator1.cs
    private TeleportConnector connector;
    public Direction direction;


    [Header("Teleport Settings")]
    public float teleportCooldown = 1f;
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
            Debug.Log($"Player entered door {gameObject.name}");
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
        Invoke(nameof(ResetTeleportFlag), 0.5f);
    }

    private Vector3 GetTeleportOffset(Direction dir)
    {
        float offset = 2f;
        return dir switch
        {
            Direction.Up => Vector3.up * offset,
            Direction.Down => Vector3.down * offset,
            Direction.Left => Vector3.left * offset,
            Direction.Right => Vector3.right * offset,
            _ => Vector3.forward * offset
        };
    }

    private Vector3 GetTeleportOffset(DoorTeleportation targetDoor)
    {
        return GetTeleportOffset(targetDoor.direction);
    }


    private void ResetTeleportFlag()
    {
        isTeleporting = false;
    }

    void OnDrawGizmos()
    {
        // sträck
        switch (direction)
        {
            case Direction.Up:
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2);
                break;
            case Direction.Down:
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 2);
                break;
            case Direction.Left:
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + Vector3.left * 2);
                break;
            case Direction.Right:
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