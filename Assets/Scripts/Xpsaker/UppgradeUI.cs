using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public UpgradeManager upgradeManager;

   
    public Button shieldUpgradeButton;

    private void Start()
    {
        
        shieldUpgradeButton.onClick.AddListener(() => upgradeManager.UnlockUpgrade("ShieldUpgrade"));
    }
}
