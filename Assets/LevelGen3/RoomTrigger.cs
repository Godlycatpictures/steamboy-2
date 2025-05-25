using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated && other.CompareTag("Player"))
        {
            Transform enemies = transform.parent.Find("Enemies");
            if (enemies != null)
            {
                foreach (Transform enemy in enemies)
                {
                    enemy.gameObject.SetActive(true);
                }
            }

            activated = true; // Optional: prevent multiple activations
        }
    }
}
