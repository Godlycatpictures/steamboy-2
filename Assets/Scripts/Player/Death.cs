using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
public SceneInfo sceneInfo;
public GameObject DeathScreen; // Reference to the death screen UI prefab

void Start()
{
    sceneInfo = FindObjectOfType<SceneInfo>();
}

    // Update is called once per frame
    void Update()
    {
        if (sceneInfo != null && sceneInfo.health <= 0) // Ensure health is public or has a getter
        {
            PlayerDeath();
            
        }
    }
    public void PlayerDeath()
    {
        Debug.Log("Player Destroyed!");     
    
        if (sceneInfo != null)
        {
            sceneInfo.ResetSceneInfo(); // Reset the scene info
            Instantiate(DeathScreen); // Instantiate the death screen UI
          
        }
    }
}
