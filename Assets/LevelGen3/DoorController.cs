using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;

    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;
        spriteRenderer.sprite = openSprite;
        doorCollider.enabled = false;
    }

    public void CloseDoor()
    {
        if (!isOpen) return;

        isOpen = false;
        spriteRenderer.sprite = closedSprite;
        doorCollider.enabled = true;
    }
}
