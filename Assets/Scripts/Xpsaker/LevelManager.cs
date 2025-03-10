using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public SceneInfo sceneInfo; // Rätt namn med stor bokstav

    public int xpToNextLevel = 100;  // Startvärde för första leveln
    public float xpMultiplier = 1.5f; // Ökning av XP-kravet per level

    public event Action<int> OnLevelUp;

    public void GainXP(int amount)
    {
        if (sceneInfo == null)
        {
            Debug.LogError("SceneInfo är inte tilldelad i Inspector!");
            return;
        }

        sceneInfo.xp += amount;
        Debug.Log($"Gained {amount} XP. Total XP: {sceneInfo.xp}/{xpToNextLevel}");

        while (sceneInfo.xp >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        if (sceneInfo == null) return; // Skydda mot null error

        sceneInfo.xp -= xpToNextLevel;  // Behåll överskotts-XP
        sceneInfo.level++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpMultiplier); // Öka XP-kravet

        Debug.Log($"Level Up! New Level: {sceneInfo.level}. XP required for next level: {xpToNextLevel}");

        OnLevelUp?.Invoke(sceneInfo.level); // Event som kan användas i UI eller effekter
    }
}
