using System.Collections.Generic;

[System.Serializable]
public class RewardInfo
{
    public DetailInfo Consonant1Upgrade;
    public DetailInfo Consonant2Upgrade;
    public DetailInfo Consonant3Upgrade;
    public DetailInfo Consonant4Upgrade;
    public DetailInfo VowelUpgrade;
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