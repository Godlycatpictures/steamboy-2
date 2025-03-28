using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject
    
{
       
  
    public bool hasShieldUpgrade;
    public bool hasBulletsizeUpgrade;
    public bool hasFireRateUpgrade;
    public bool hasAutoFireUpgrade = false;
    public bool hasKillToHeal; //måste göras
    public bool hasGhostCompanion;

    public bool hasReRoll;
     public bool hasUsedReRoll;

    public int health; // Mängden liv kvar
    public int EnemiesKilled;
    public int numOfHearts; // Max antal hjärtan

    public int currentCoins; // Antalet mynt som spelaren har

    public float fireForce; // Hastigheten av skottet
    public float fireRate; // Ska bli mindre för att den ska skjuta snabbare

    public int damageModifier = 1;

    public int randmap;

    public int xp = 0;
    public int level = 1;

    // XP-krav för nästa nivå och multiplikator
    public int xpToNextLevel = 100;
    public float xpMultiplier = 1.5f;

    public bool screenShake;

    // Håller reda på vilka rum som har blivit rensade
    public List<bool> roomsCleared = new List<bool>();
    

    

    // Metod för att tillämpa uppgraderingar på karaktären
    public void ResetSceneInfo()
    {
        level = 1;
        xp = 0;
        xpToNextLevel = 100;  // Start XP för nästa nivå


    hasShieldUpgrade = false;
    hasBulletsizeUpgrade = false;
    hasFireRateUpgrade = false;
    hasAutoFireUpgrade = false;
    hasUsedReRoll = false;
    hasReRoll = false;
    hasKillToHeal = false;
    hasGhostCompanion = false;
    fireRate = 0.5f;
    numOfHearts = 3;
    health = numOfHearts;

    
    }
    public void UnlockUpgrade(string upgradeName)
{
    switch (upgradeName)
    {
        case "ShieldUpgrade":
            hasShieldUpgrade = true;
            break;
        case "BulletsizeUpgrade":
            hasBulletsizeUpgrade = true;
            break;
        case "FireRateUpgrade":
            hasFireRateUpgrade = true;
            break;
        case "AutoFireUpgrade":
            hasAutoFireUpgrade = true;  // Lägg till denna rad för att hantera full-auto
            break;
        default:
            Debug.LogWarning($"Upgrade {upgradeName} not found!");
            break;
    }
}


 public void LevelUp()
{
    xp -= xpToNextLevel;
    level++;
     hasUsedReRoll = false;
    int baseXP = 100;  
    float growthFactor = 1.2f;  
    int linearFactor = 50;

    xpToNextLevel = Mathf.RoundToInt(baseXP + (Mathf.Pow(level, 2) * growthFactor) + (level * linearFactor));
}
}
    // Metod för att hantera level-up
 

   




  

    




    
