using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classUpgrades
{

    [CreateAssetMenu(fileName = "Class Upgrade Data", menuName = "ScriptableObjects/ClassUpgradeData")]

    public class ClassUpgradeData : ScriptableObject
    {
        public static ClassUpgradeData instance;

        public classSystems[] classSystems;

        

    }

    [System.Serializable]
    public class classSystems
    {
        public string systemType;
        public classSections[] classSections;

    }

    [System.Serializable]
    public class classSections
    {
        public string sectionName;
        public SectionUpgrades[] Upgrades;
    }

    [System.Serializable]
    public class SectionUpgrades
    {
        public string upgradeName;
        public string upgradeDescription;
        public bool upgradeUnlocked;
        public bool starterUpgrade;
        public int upgradeCost;
        public bool hasLevels;
        public int upgradeLevel;
        public upgradeLevels[] levels;
    }

    [System.Serializable]
    public class upgradeLevels
    {
        public int levelCost;
        public float levelIncrease;
    }
}
