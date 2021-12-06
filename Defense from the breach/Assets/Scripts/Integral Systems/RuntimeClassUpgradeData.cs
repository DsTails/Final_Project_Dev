using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeClassUpgradeData : MonoBehaviour
{
    public classUpgrades.ClassUpgradeData originalData;
    public classUpgrades.classSystems[] runtimeData;

    private void Awake()
    {
        runtimeData = originalData.classSystems;

        for (int i = 0; i < runtimeData.Length; i++)
        {
            for (int j = 0; j < runtimeData[i].classSections.Length; j++)
            {
                for (int k = 0; k < runtimeData[i].classSections[j].Upgrades.Length; k++)
                {
                    if (runtimeData[i].classSections[j].Upgrades[k].starterUpgrade)
                    {

                    }
                    else
                    {
                        runtimeData[i].classSections[j].Upgrades[k].upgradeUnlocked = false;
                    }

                    if (runtimeData[i].classSections[j].Upgrades[k].hasLevels)
                    {
                        runtimeData[i].classSections[j].Upgrades[k].upgradeLevel = 0;
                        runtimeData[i].classSections[j].Upgrades[k].upgradeCost = runtimeData[i].classSections[j].Upgrades[k].levels[0].levelCost;
                    }
                    
                }
            }

        }
        
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
