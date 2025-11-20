public abstract class GameState
{
    public class ChooseLevelState: GameState { }
    public class ChooseWorldState : GameState { }
    public class PlayingLevelState : GameState
    {
        public LevelTypeEnum LevelTypeEnum { get; }

        public PlayingLevelState(LevelTypeEnum levelTypeEnum)
        {
            LevelTypeEnum = levelTypeEnum;
        }

    }

}