using System.Collections.Generic;

public interface ISelectLevelChoicesUseCase
{
    public List<Level> Invoke(int levelChoiceIndex, List<World.LevelChoice> levelChoices);
}