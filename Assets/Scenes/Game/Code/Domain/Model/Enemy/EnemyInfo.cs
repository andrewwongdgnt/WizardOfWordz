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

        [System.Serializable]
        public class MoveInfo
        {
            public string type;

            public string title;

            public string description;

            public RarityInfo wait;

            public RarityInfo weight;

            public RarityInfo value;
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