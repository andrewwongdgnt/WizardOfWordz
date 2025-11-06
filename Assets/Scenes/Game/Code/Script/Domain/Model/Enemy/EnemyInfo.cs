using System.Collections.Generic;

[System.Serializable]
public class EnemyInfo
{
    public StatsInfo Note;
    public StatsInfo Notebook;

    [System.Serializable]
    public class StatsInfo
    {
        public string description;

        public RarityInfo health;

        public List<MoveInfo> moves;

        [System.Serializable]
        public class MoveInfo
        {
            public string type;

            public string title;

            public RarityInfo wait;

            public int weight;

            public RarityInfo damage;
        }

        [System.Serializable]
        public class RarityInfo
        {
            public int common;
            public int uncommon;
            public int rare;
            public int epic;
            public int legendary;
        }
    }
}