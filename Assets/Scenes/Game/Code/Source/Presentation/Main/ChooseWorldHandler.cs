using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ChooseWorldHandler : MonoBehaviour
{

    public WorldSelectorGameObject worldSelectorGameObject;

    [Inject]
    private readonly IGetWorldUseCase getWorldUseCase;

    [Inject]
    private readonly ICalculateNextIndexUsecase calculateNextIndexUsecase;

    private Func<GameState> getGameStateFunc;
    private Action<Tile> mainUpdateUIState;
    private Action selectWorldCallBack;
    private int worldIndex;
    private List<World> worlds;

    void Update()
    {
        worldSelectorGameObject.Appear(getGameStateFunc() is GameState.ChooseWorldState);
    }

    public void Init(
        Func<GameState> getGameStateFunc,
        Action<Tile> mainUpdateUIState,
        Action selectWorldCallBack
       )
    {
        this.getGameStateFunc = getGameStateFunc;
        this.mainUpdateUIState = mainUpdateUIState;
        this.selectWorldCallBack = selectWorldCallBack;
        worldSelectorGameObject.SetUp(WorldAction);
    }

    public void SetUpWorldSelection()
    {
        worldIndex = 0;
        worlds = Enum.GetValues(typeof(WorldEnum)).Cast<WorldEnum>().ToList()
           .Select(w => getWorldUseCase.Invoke(w)).ToList();
    }

    public void TargetNewWorld(Key key)
    {
        worldIndex = calculateNextIndexUsecase.Invoke(
            key == Key.RightArrow,
            worldIndex,
            worlds.Count
        );
    }

    public World GetWorld()
    {
        return worlds[worldIndex];
    }
    public void UpdateUIState()
    {
        Debug.Log($"Picking world: {worldIndex}\n{string.Join(",", worlds)}");
    }

    public void Reset()
    {
        worldIndex = 0;
    }

    private void WorldAction(WorldEnum worldEnum)
    {
        worldIndex = worlds.FindIndex(w => w.WorldEnum == worldEnum);
        selectWorldCallBack();
        mainUpdateUIState(null);
    }
}
