using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
public SceneInfo sceneInfo;

void Start()
{
    sceneInfo = FindObjectOfType<SceneInfo>();
}

    // Update is called once per frame
    void Update()
    {
        if (sceneInfo != null && sceneInfo.health <= 0)
        {
            PlayerDeath();
            
        }
    }
    public void PlayerDeath()
    {
        Debug.Log("Player Destroyed!");
        
       
        if (sceneInfo != null)
        {
            sceneInfo.ResetSceneInfo(); // Add XP when enemy dies
            Destroy(gameObject);
            //Inztantiate(DeathScreen); Death screen
        }
    }
}
