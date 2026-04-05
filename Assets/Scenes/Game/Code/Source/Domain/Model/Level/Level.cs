using System.Collections.Generic;

public abstract class Level
{
    public LevelEnum LevelEnum { get; protected set; }

    public string Title { get; protected set; }

    public string Description { get; protected set; }

    public class Fight : Level,  IRarityHolder
    {
        public List<EnemySummary> Enemies { get; }

        public RarityEnum RarityEnum { get; }

        public Fight(
            LevelEnum levelEnum,
            string title,
            string description,
            List<EnemySummary> enemies,
            RarityEnum topRarity
        )
        {
            LevelEnum = levelEnum;
            Title = title;
            Description = description;
            Enemies = enemies;
            RarityEnum = topRarity;
        }

        public class EnemySummary
        {
            public EnemyEnum EnemyEnum { get; }
            public RarityEnum RarityEnum { get; }

            public EnemySummary(
                EnemyEnum enemyEnum,
                RarityEnum rarityEnum
                )
            {
                EnemyEnum = enemyEnum;
                RarityEnum = rarityEnum;
            }
        }
    }

    public class Rest : Level
    {
        public List<RestChoiceEnum> Choices { get; }
        public List<char[]> LetterPool { get; }

        public Rest(
           LevelEnum levelEnum,
           string title,
           string description,
           List<RestChoiceEnum> choices,
           List<char[]> letterPool
       )
        {
            LevelEnum = levelEnum;
            Title = title;
            Description = description;
            Choices = choices;
            LetterPool = letterPool;
        }
    }

    public override string ToString()
    {
        return Title;
    }

}