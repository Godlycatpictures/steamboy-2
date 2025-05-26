using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
public SceneInfo sceneInfo;
public GameObject DeathScreen; // Reference to the death screen UI prefab



    // Update is called once per frame
    void Update()
    {
        if (sceneInfo.health <= 0) // Ensure health is public or has a getter
        {
            PlayerDeath();

            Debug.Log("Player Destroyed!");
        }
    }
    public void PlayerDeath()
    {
        Debug.Log("Player Destroyed!2");     
    
        if (sceneInfo != null)
        {
            sceneInfo.ResetSceneInfo(); // Reset the scene info
            
          
        }
        SceneManager.LoadScene("Main Menu");
    }
}
