using UnityEngine;
public enum DoorDirection { Up, Down, Left, Right }

public class DoorTeleportation : MonoBehaviour
{
    public int connectedRoomID; // i dungeongenerator1.cs
    private TeleportConnector connector;
    public DoorDirection direction;

    void Start()
    {
        connector = GetComponentInParent<TeleportConnector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform target = connector.GetConnectedDoor(connectedRoomID);
            if (target != null)
            {
                other.transform.position = target.position;
            }
        }
    }

    void OnDrawGizmos()
    {
        // sträck
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

        if (Application.isPlaying && connector != null)
        {
            Transform target = connector.GetConnectedDoor(connectedRoomID);
            if (target != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, target.position);
            }
        }
    }

}
