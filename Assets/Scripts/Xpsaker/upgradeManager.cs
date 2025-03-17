using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeCanvas;
    public SceneInfo sceneInfo;
    public GameObject ghostCompanionPrefab;
    public GameObject activeGhost;

    public GameObject shieldUpgradeObject;
    public GameObject bulletsizeUpgradeObject;
    public GameObject FireRateUpgradePrefab;
    public GameObject bulletSizeIncreasePrefab;

    private List<string> unlockedUpgrades = new List<string>();

    private void Start()
    {
        ApplyUpgrades();
    }
    void Update()
{
    if (Input.GetKeyDown(KeyCode.F9)) // Test instantiation with F9
    {
        UnlockGhostCompanion();
    }
}

    public void ApplyUpgrades()
    {
        shieldUpgradeObject.SetActive(sceneInfo.hasShieldUpgrade);
        bulletsizeUpgradeObject.SetActive(sceneInfo.hasBulletsizeUpgrade);
        FireRateUpgradePrefab.SetActive(sceneInfo.hasFireRateUpgrade);

        if (sceneInfo.hasBulletsizeUpgrade)
        {
            ApplyBulletSizeUpgrade();
        }

        if (sceneInfo.hasFireRateUpgrade)
        {
            ApplyFireRateUpgrade();
        }
        
        // Aktivera Re-Roll om spelaren har den
        if (sceneInfo.hasReRoll)
        {
            Debug.Log("Re-Roll Upgrade is available!");
        }
    }

    public bool HasUpgrade(string upgradeName)
    {
        return unlockedUpgrades.Contains(upgradeName);
    }

    public void UnlockShieldUpgrade()
    {
        if (!sceneInfo.hasShieldUpgrade)
        {
            sceneInfo.hasShieldUpgrade = true;
            shieldUpgradeObject.SetActive(true);
            unlockedUpgrades.Add("ShieldUpgrade");
            Debug.Log("Shield upgrade unlocked.");
        }

        CloseUpgradeMenu();
    }

    public void UnlockBulletSizeUpgrade()
    {
        if (!sceneInfo.hasBulletsizeUpgrade)
        {
            sceneInfo.hasBulletsizeUpgrade = true;
            bulletsizeUpgradeObject.SetActive(true);
            ApplyBulletSizeUpgrade();
            unlockedUpgrades.Add("BulletsizeUpgrade");
            Debug.Log("Bullet size upgrade unlocked.");
        }

        CloseUpgradeMenu();
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

    public void UnlockFireRateUpgrade()
    {
        if (sceneInfo.fireRate > 0.1f)
        {
            sceneInfo.fireRate -= 0.1f;
            PlayerPrefs.SetFloat("fireRate", sceneInfo.fireRate);
            Debug.Log("Fire rate decreased to: " + sceneInfo.fireRate);
        }
        else
        {
            Debug.Log("Maximum fire rate upgrade reached.");
        }

        if (!sceneInfo.hasFireRateUpgrade)
        {
            sceneInfo.hasFireRateUpgrade = true;
            FireRateUpgradePrefab.SetActive(true);
            unlockedUpgrades.Add("FireRateUpgrade");
            ApplyFireRateUpgrade();
        }

        CloseUpgradeMenu();
    }

    private void ApplyFireRateUpgrade()
    {
        if (FireRateUpgradePrefab != null)
        {
            Instantiate(FireRateUpgradePrefab);
        }
        else
        {
            Debug.LogWarning("FireRateUpgrade prefab is not assigned!");
        }
    }

    public void UnlockFullAutoUpgrade()
    {
        if (!sceneInfo.hasAutoFireUpgrade)
        {
            sceneInfo.hasAutoFireUpgrade = true;
            unlockedUpgrades.Add("FullAutoUpgrade");
            sceneInfo.fireRate *= 2;

            Weapon weapon = FindObjectOfType<Weapon>();
            if (weapon != null)
            {
                weapon.isAutoFireEnabled = true;
            }
        }

        CloseUpgradeMenu();
    }

    // Ny metod för att låsa upp Re-Roll
public void UnlockReRollUpgrade()
{
    if (!sceneInfo.hasReRoll)
    {
        sceneInfo.hasReRoll = true;
        sceneInfo.hasUsedReRoll = false; // Se till att spelaren kan använda den
        Debug.Log("Re-Roll upgrade unlocked!");
    }
    CloseUpgradeMenu();
}



    // Återställ Re-Roll när spelaren går upp i level
    public void ResetReRollOnLevelUp()
    {
        sceneInfo.hasUsedReRoll = false;
        Debug.Log("Re-Roll Reset on Level Up!");
    }

    private void CloseUpgradeMenu()
    {
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
    public void UnlockGhostCompanion()
    {
        if (!sceneInfo.hasGhostCompanion)
        {
            sceneInfo.hasGhostCompanion = true;
            
            // Instantiate ghost using same pattern as shield/bullet upgrades
            if (ghostCompanionPrefab != null)
            {
                activeGhost = Instantiate(ghostCompanionPrefab);
                activeGhost.GetComponent<GhostCompanion>().player = 
                    GameObject.FindGameObjectWithTag("Player").transform;
                activeGhost.SetActive(true); // Explicit activation
            }
            
            unlockedUpgrades.Add("GhostCompanion");
            Debug.Log("Ghost companion unlocked!");
        }
        CloseUpgradeMenu();
    }
}
