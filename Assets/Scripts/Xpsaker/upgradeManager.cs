using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
      public GameObject upgradeCanvas;
    public SceneInfo sceneInfo;
    
    
    public GameObject shieldUpgradeObject;

    private void Start()
    {
        ApplyUpgrades();
    }

    public void ApplyUpgrades()
    {
 
        
        shieldUpgradeObject.SetActive(sceneInfo.hasShieldUpgrade);
    }

    public void UnlockUpgrade(string upgradeName)
    {
        sceneInfo.UnlockUpgrade(upgradeName);
        ApplyUpgrades(); // Uppdatera uppgraderingar direkt
    }
     public void UnlockShieldUpgrade()
    {
        sceneInfo.hasShieldUpgrade = true;
        ApplyUpgrades();
        upgradeCanvas.SetActive(false); // Dölj menyn
        Time.timeScale = 1f;
    }
}
