using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xpChar : MonoBehaviour
{
    public LevelManager levelManager;

    void FixedUpdate()
    {
          if (levelManager == null)
        {
            levelManager = GetComponent<LevelManager>();
        }

    }

void Start()
{
    if (levelManager == null)
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    if (levelManager != null)
    {
        levelManager.OnLevelUp += HandleLevelUp;
    }
    else
    {
        Debug.LogError("LevelManager hittades inte i scenen!");
    }
}


    public void AddXP(int amount)
    {
        levelManager.GainXP(amount);
    }

    private void HandleLevelUp(int newLevel)
    {
        Debug.Log($"Character reached level {newLevel}!");
    }
}

