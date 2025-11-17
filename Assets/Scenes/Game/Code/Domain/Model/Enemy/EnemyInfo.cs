using System.Collections.Generic;

[System.Serializable]
public class EnemyInfo
{
    public DetailInfo Note;
    public DetailInfo Notebook;

    [System.Serializable]
    public class DetailInfo
    {
        public string title;

        public string description;

        public RarityInfo health;

        public List<MoveInfo> moves;

        public RarityInfo delay;

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