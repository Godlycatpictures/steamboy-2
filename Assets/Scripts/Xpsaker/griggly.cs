using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class griggly : MonoBehaviour
{
    public int health = 1; // Enemy health
    public int xp; // XP to grant when enemy dies
    private xpChar xpCharacter; // Reference to xpChar script
    
    //public GameObject deathEffect;

    void Start()
    {

        // Find the xpChar component in the scene
        xpCharacter = FindObjectOfType<xpChar>();

        // Ensure it's found
        if (xpCharacter == null)
        {
            Debug.LogError("xpChar script not found in the scene!");
        }

    }

void OnTriggerEnter2D(Collider2D collision)
{
    Debug.Log("Something entered the trigger: " + collision.gameObject.name);

    if (collision.CompareTag("Bullet")) // Check if it's a bullet
    { 
        Debug.Log("Enemy hit by bullet!");
        TakeDamage();
    }
     else if (collision.CompareTag("DroneProjectile")) // Check if it's a bullet
    { 
        Debug.Log("Enemy hit by bullet!");
        TakeDamage();
    }
    else{

    }
}
    void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Destroyed!");

        // Grant XP before destroying the enemy
        if (xpCharacter != null)
        {
            xpCharacter.AddXP(xp); // Add XP when enemy dies
        }
       // Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject); // Remove enemy from scene
    }
}