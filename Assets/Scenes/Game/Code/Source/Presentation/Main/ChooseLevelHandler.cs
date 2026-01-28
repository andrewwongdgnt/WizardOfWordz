using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LightTransport;
using Zenject;

public class ChooseLevelHandler : MonoBehaviour
{

    public LevelSelectorGameObject levelSelectorGameObject;

    [Inject]
    private readonly ISelectLevelChoicesUseCase selectLevelChoicesUseCase;

    [Inject]
    private readonly ICalculateNextIndexUsecase calculateNextIndexUsecase;

    private Func<GameState> getGameStateFunc;
    private Action<Tile> mainUpdateUIState;
    private Action selectLevelCallBack;
    private int levelChoiceIndex;
    private int levelIndex;
    private List<Level> levelsToChooseFrom;

    void Update()
    {
        levelSelectorGameObject.Appear(getGameStateFunc() is GameState.ChooseLevelState);
    }

    public void Init(
        Func<GameState> getGameStateFunc,
        Action<Tile> mainUpdateUIState,
        Action selectLevelCallBack
        )
    {
        this.getGameStateFunc = getGameStateFunc;
        this.mainUpdateUIState = mainUpdateUIState;
        this.selectLevelCallBack = selectLevelCallBack;
        levelSelectorGameObject.levelSelectedAction = LevelSelectedAction;
        levelSelectorGameObject.levelHoverAction = LevelHoverAction;
    }

    public void SetUpLevelSelection(World world)
    {
        levelIndex = 0;
        levelsToChooseFrom = selectLevelChoicesUseCase.Invoke(levelChoiceIndex, world.LevelChoices);
        levelSelectorGameObject.SetUp(levelsToChooseFrom);
    }

    public void TargetNewLevel(Key key)
    {
        levelIndex = calculateNextIndexUsecase.Invoke(
            key == Key.RightArrow,
            levelIndex,
            levelsToChooseFrom.Count
        );
    }

    public Level GetLevel()
    {
        return levelsToChooseFrom[levelIndex];
    }

    public void NextLevelChoice()
    {
        levelChoiceIndex++;
    }

    public void UpdateUIState()
    {
        levelSelectorGameObject.UpdateState(levelsToChooseFrom[levelIndex]);
        Debug.Log($"Picking level: {levelIndex}\n{string.Join(",", levelsToChooseFrom)}");
    }

    public void Reset()
    {
        levelChoiceIndex = 0;
        levelIndex = 0;
    }

    private void LevelSelectedAction(Level level)
    {
        TargetNewLevel(level);
        selectLevelCallBack();
        mainUpdateUIState(null);
    }

    private void LevelHoverAction(Level level)
    {
        int originalIndex = levelIndex;
        TargetNewLevel(level);
        if (originalIndex != levelIndex)
        {
            UpdateUIState();
        }
    }

    private void TargetNewLevel(Level level)
    {
        int index = levelsToChooseFrom.IndexOf(level);
        if (index < 0)
        {
            return;
        }
        levelIndex = index;
    }
}
