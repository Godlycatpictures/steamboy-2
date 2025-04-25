using System.Collections.Generic;
using UnityEngine;

public class TeleportConnector : MonoBehaviour
{
    public List<Transform> doorPoints; // var dörrarna är

    public Dictionary<int, Transform> connectedDoors = new();

    public void LinkDoor(int connectedRoomID, Transform targetDoor)
    {
        if (!connectedDoors.ContainsKey(connectedRoomID))
            connectedDoors.Add(connectedRoomID, targetDoor);
    }

    public Transform GetConnectedDoor(int roomID)
    {
        return connectedDoors.ContainsKey(roomID) ? connectedDoors[roomID] : null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        foreach (var pair in connectedDoors)
        {
            // Find which door in this room connects to the target
            foreach (Transform door in doorPoints)
            {
                DoorTeleportation doorScript = door.GetComponent<DoorTeleportation>();
                if (doorScript != null && doorScript.connectedRoomID == pair.Key)
                {
                    // Draw line from this door to connected door
                    Gizmos.DrawLine(door.position, pair.Value.position);

                    // Draw small sphere at connection points
                    Gizmos.DrawSphere(door.position, 0.1f);
                    Gizmos.DrawSphere(pair.Value.position, 0.1f);

                    // Draw arrow head in the middle
                    DrawArrow(door.position, pair.Value.position);
                }
            }
        }
    }

    void DrawArrow(Vector3 from, Vector3 to)
    {
        Vector3 direction = to - from;
        Vector3 midPoint = from + direction * 0.5f;
        float arrowSize = Mathf.Clamp(direction.magnitude * 0.2f, 0.1f, 0.5f);

        Gizmos.DrawRay(midPoint, Quaternion.LookRotation(direction) * Vector3.up * arrowSize);
        Gizmos.DrawRay(midPoint, Quaternion.LookRotation(direction) * Vector3.down * arrowSize);
        Gizmos.DrawRay(midPoint, Quaternion.LookRotation(direction) * Vector3.left * arrowSize);
        Gizmos.DrawRay(midPoint, Quaternion.LookRotation(direction) * Vector3.right * arrowSize);
    }
}
