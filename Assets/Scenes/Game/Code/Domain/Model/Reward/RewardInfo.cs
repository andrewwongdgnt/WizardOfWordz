using System.Collections.Generic;

[System.Serializable]
public class RewardInfo
{
    public DetailInfo Reroll;
    public DetailInfo MaxHealth;
    public DetailInfo MaxTile;

    [System.Serializable]
    public class DetailInfo
    {
        public string title;

        public string description;

        public string rarity;

        public List<int> values;
    }
}