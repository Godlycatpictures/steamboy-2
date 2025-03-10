using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xpChar : MonoBehaviour
{
    public LevelManager levelManager;

    void Start()
    {
        if (levelManager == null)
        {
            levelManager = GetComponent<LevelManager>();
        }

        levelManager.OnLevelUp += HandleLevelUp;
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

