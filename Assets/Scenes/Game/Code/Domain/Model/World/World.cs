using System.Collections.Generic;

public class World
{
    public WorldEnum WorldEnum { get; }

    public string Title { get; }

    public string Description { get; }

    public List<LevelChoice> LevelChoices { get; }

    public World(
        WorldEnum worldEnum,
        string title,
        string description,
        List<LevelChoice> levelChoices
        )
    {
        WorldEnum = worldEnum;
        Title = title;
        Description = description;
        LevelChoices = levelChoices;
    }

    public class LevelChoice
    {
        public int Pick { get; }
        public List<Level> Choices { get; }

        public LevelChoice(
            int pick,
            List<Level> choices
            )
        {
            Pick = pick;
            Choices = choices;
        }
    }
}