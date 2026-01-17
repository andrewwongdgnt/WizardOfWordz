using System.Collections.Generic;

[System.Serializable]
public class RewardInfo
{
    public DetailInfo UpgradeLNRST;
    public DetailInfo UpgradeBCDGMP;
    public DetailInfo UpgradeFKHVWY;
    public DetailInfo UpgradeJXQZ;
    public DetailInfo UpgradeAEIOU;
    public DetailInfo MaxHealth;
    public DetailInfo MaxTile;

    [System.Serializable]
    public class DetailInfo
    {
        public string title;

        public string description;

        public List<ValueInfo> values;

        [System.Serializable]
        public class ValueInfo
        {
            public int value;

            public string rarity;
        }
    }
}