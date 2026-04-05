
using System.Collections.Generic;

[System.Serializable]
public class LevelInfo
{
    public DetailInfo.FightInfo F_1_1_a;
    public DetailInfo.FightInfo F_1_1_b;
    public DetailInfo.FightInfo F_1_1_c;
    public DetailInfo.FightInfo F_1_1_d;
    public DetailInfo.FightInfo F_1_1_e;
    public DetailInfo.FightInfo F_1_2_a;
    public DetailInfo.FightInfo F_1_2_b;
    public DetailInfo.FightInfo F_1_2_c;
    public DetailInfo.FightInfo F_1_2_d;
    public DetailInfo.FightInfo F_1_3_a;
    public DetailInfo.FightInfo F_1_3_b;
    public DetailInfo.FightInfo F_1_3_c;
    public DetailInfo.RestInfo R_1_4_a;

    [System.Serializable]
    public abstract class DetailInfo
    {
        public string type;
        public string title;
        public string description;

        [System.Serializable]
        public class FightInfo : DetailInfo
        {
            public List<EnemyArg> enemies;

            [System.Serializable]
            public class EnemyArg
            {
                public string enemy;
                public string rarity;
            }
        }

        [System.Serializable]
        public class RestInfo : DetailInfo
        {
            public List<string> choices;
            public List<string> letterPool;
        }
    }
}
