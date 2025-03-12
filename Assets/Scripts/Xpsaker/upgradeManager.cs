using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public SceneInfo sceneInfo;

    private void Start()
    {
        if (sceneInfo == null)
        {
            Debug.LogError("SceneInfo är inte kopplad till UpgradeManager!");
            return;
        }

        ApplyUpgrades();
    }

    public void AddUpgrade(UpgradeType upgradeType, int value)
    {
        if (!sceneInfo.upgrades.ContainsKey(upgradeType))
        {
            sceneInfo.upgrades[upgradeType] = 0;
        }

        sceneInfo.upgrades[upgradeType] += value;
        Debug.Log($"Uppgradering: {upgradeType} ökad med {value}. Ny nivå: {sceneInfo.upgrades[upgradeType]}");

        ApplyUpgrades();
    }

    private int GetUpgradeValue(UpgradeType type)
    {
        return sceneInfo.upgrades.ContainsKey(type) ? sceneInfo.upgrades[type] : 0;
    }

    private void ApplyUpgrades()
    {
        sceneInfo.health += GetUpgradeValue(UpgradeType.Health);
        sceneInfo.fireRate -= GetUpgradeValue(UpgradeType.FireRate) * 0.1f;
        sceneInfo.damageModifier += GetUpgradeValue(UpgradeType.Damage);
       

        Debug.Log("Alla uppgraderingar har applicerats!");
    }
}


public enum UpgradeType
{
    Health,
    FireRate,
    Damage,
    Speed,
    AttachPrefab // Ny kategori för prefabs
}

