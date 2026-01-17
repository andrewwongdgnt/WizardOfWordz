
using System.Collections.Generic;

[System.Serializable]
public class LevelInfo
{
    public DetailInfo F_1_1_a;
    public DetailInfo F_1_1_b;
    public DetailInfo F_1_1_c;
    public DetailInfo F_1_1_d;
    public DetailInfo F_1_1_e;
    public DetailInfo F_1_2_a;
    public DetailInfo F_1_2_b;
    public DetailInfo F_1_2_c;
    public DetailInfo F_1_2_d;
    public DetailInfo F_1_3_a;
    public DetailInfo F_1_3_b;
    public DetailInfo F_1_3_c;

    [System.Serializable]
    public class DetailInfo
    {
        public string type;
        public string title;

        public string description;

        public List<EnemyArg> enemies;

        [System.Serializable]
        public class EnemyArg
        {
            public string enemy;
            public string rarity;
        }
    }
}
