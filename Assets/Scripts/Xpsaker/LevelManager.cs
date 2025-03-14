using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public SceneInfo sceneInfo; // Rätt namn med stor bokstav
  //public UpgradeManager upgradeManager; // Stor bokstav!
  public GameObject player;



    public event Action<int> OnLevelUp;

    public void GainXP(int amount)
    {
        if (sceneInfo == null)
        {
            Debug.LogError("SceneInfo är inte tilldelad i Inspector!");
            return;
        }

        sceneInfo.xp += amount;
        Debug.Log($"Gained {amount} XP. Total XP: {sceneInfo.xp}/{sceneInfo.xpToNextLevel}");

        // Kontrollera om spelaren har tillräckligt med XP för att levela upp
        while (sceneInfo.xp >= sceneInfo.xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        if (sceneInfo == null) return; // Skydda mot null error

        sceneInfo.LevelUp(); // Använd metoden i SceneInfo för att hantera level-up

        Debug.Log($"Level Up! New Level: {sceneInfo.level}. XP required for next level: {sceneInfo.xpToNextLevel}");

        OnLevelUp?.Invoke(sceneInfo.level); // Event som kan användas i UI eller effekter
        //upgradeManager.ApplyUpgrades(); // Uppdatera spelarens uppgraderingar
    }

    private void Start()
    {
        if (sceneInfo != null)
        {
            sceneInfo.ResetSceneInfo();  // Återställ all data när spelet startar
        }
    }
}

