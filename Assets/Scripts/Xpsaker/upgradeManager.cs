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

    private List<string> unlockedUpgrades = new List<string>();

    private void Start()
    {
        ApplyUpgrades();
    }

    public void ApplyUpgrades()
    {
        // Uppdatera aktivering av objekt baserat på uppgraderingsstatus
        shieldUpgradeObject.SetActive(sceneInfo.hasShieldUpgrade);
        bulletsizeUpgradeObject.SetActive(sceneInfo.hasBulletsizeUpgrade);

        // Kontrollera om BulletSizeUpgrade är upplåst och applicera den
        if (sceneInfo.hasBulletsizeUpgrade)
        {
            ApplyBulletSizeUpgrade();
        }
    }

    public void UnlockUpgrade(string upgradeName)
    {
        if (!unlockedUpgrades.Contains(upgradeName))
        {
            unlockedUpgrades.Add(upgradeName);
            sceneInfo.UnlockUpgrade(upgradeName);
            ApplyUpgrades();
        }
    }

    public bool HasUpgrade(string upgradeName)
    {
        return unlockedUpgrades.Contains(upgradeName);
    }

    public void UnlockShieldUpgrade()
    {
        sceneInfo.hasShieldUpgrade = true;
        ApplyUpgrades();
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UnlockBulletSizeUpgrade()
    {
        Debug.Log("Unlocking bullet size upgrade");
        sceneInfo.hasBulletsizeUpgrade = true;
        ApplyUpgrades();
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ApplyBulletSizeUpgrade()
    {
        if (bulletSizeIncreasePrefab != null)
        {
            Instantiate(bulletSizeIncreasePrefab);
        }
        else
        {
            Debug.LogWarning("BulletSizeIncrease prefab is not assigned!");
        }
    }

    // 🟢 Bullet Duplication Logik 🟢
    public void UnlockBulletDuplication()
    {
        if (!HasUpgrade("BulletDuplication"))
        {
            unlockedUpgrades.Add("BulletDuplication");
            Debug.Log("Bullet Duplication unlocked!");
        }

        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}
