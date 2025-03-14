using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject
    
{
       
  
    public bool hasShieldUpgrade;
    

    public int health; // Mängden liv kvar
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

    // Håller reda på vilka rum som har blivit rensade
    public List<bool> roomsCleared = new List<bool>();
    


    

    // Metod för att tillämpa uppgraderingar på karaktären
    public void ResetSceneInfo()
    {
        level = 1;
        xp = 0;
        xpToNextLevel = 100;  // Start XP för nästa nivå

   
    hasShieldUpgrade = false;
      
    }
    public void UnlockUpgrade(string upgradeName)
    {
        switch (upgradeName)
        {
            
           
            case "ShieldUpgrade":
                hasShieldUpgrade = true;
                break;
            default:
                Debug.LogWarning($"Upgrade {upgradeName} not found!");
                break;
        }
    }
       public void LevelUp()
    {
        xp -= xpToNextLevel;  // Behåll överskotts-XP
        level++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpMultiplier); // Öka XP-kravet
    }
}
    // Metod för att hantera level-up
 

   




  

    




    
