using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeCanvas;
    public SceneInfo sceneInfo;
    
    public GameObject shieldUpgradeObject;
    public GameObject bulletsizeUpgradeObject;

    // Prefab för BulletSizeIncrease
    public GameObject bulletSizeIncreasePrefab;

    private void Start()
    {
        ApplyUpgrades();
    }

    public void ApplyUpgrades()
    {
        // Uppdatera aktivering av objekt baserat på uppgraderingsstatus
        shieldUpgradeObject.SetActive(sceneInfo.hasShieldUpgrade);
        bulletsizeUpgradeObject.SetActive(sceneInfo.hasBulletsizeUpgrade); // Bulletsize-objektet ska vara synligt när uppgraderingen är upplåst

        // Kontrollera om BulletSizeUpgrade är upplåst och applicera den
        if (sceneInfo.hasBulletsizeUpgrade)
        {
            ApplyBulletSizeUpgrade();
        }
    }

    public void UnlockUpgrade(string upgradeName)
    {
        sceneInfo.UnlockUpgrade(upgradeName);
        ApplyUpgrades(); // Uppdatera uppgraderingar direkt
    }

    public void UnlockShieldUpgrade()
    {
        sceneInfo.hasShieldUpgrade = true;
        ApplyUpgrades(); // Uppdatera uppgraderingar direkt
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f; // Fortsätt spelet
    }

    public void UnlockBulletSizeUpgrade()
    {
        Debug.Log("Unlocking bullet size upgrade");
        sceneInfo.hasBulletsizeUpgrade = true;
        ApplyUpgrades(); // Uppdatera uppgraderingar direkt
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f; // Fortsätt spelet
    }

    // Till denna metod applicerar vi BulletSizeUpgrade
    private void ApplyBulletSizeUpgrade()
    {
        // Skapa en instans av BulletSizeIncrease prefabben i världen
        if (bulletSizeIncreasePrefab != null)
        {
            Instantiate(bulletSizeIncreasePrefab);  // Skapa en instans av prefabben
        }
        else
        {
            Debug.LogWarning("BulletSizeIncrease prefab is not assigned!");
        }
    }
}
