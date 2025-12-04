using ModestTree;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LightTransport;
using Zenject;
using static UnityEngine.EventSystems.EventTrigger;

public class MainScript : MonoBehaviour
{

    public BoardContainerGameObject boardContainerGO;
    public StageContainerGameObject stageContainerGO;
    public PlayerStatsContainerGameObject playerStatsContainerGameObject;
    public WorldSelectorGameObject worldSelectorGameObject;
    public LevelSelectorGameObject levelSelectorGameObject;

    [Inject]
    private readonly RetrieveWordsFromDictionaryUsecase retrieveWordsFromDictionaryUsecase;

    [Inject]
    private readonly PickTileUsecase pickTileUsecase;

    [Inject]
    private readonly PopulateEnemiesUsecase populateEnemiesUsecase;

    [Inject]
    private readonly ProcessWordUsecase processWordUsecase;

    [Inject]
    private readonly GenerateCharTilesUsecase generateCharTilesUsecase;

    [Inject]
    private readonly GetNextTargetUsecase getNextTargetUsecase;

    [Inject]
    private readonly CalculateNextIndexUsecase calculateNextIndexUsecase;

    [Inject]
    private readonly CalculateTurnFromEnemiesUsecase calculateTurnFromEnemiesUsecase;

    [Inject]
    private readonly CalculatePlayerDamageUsecase calculatePlayerDamageUsecase;

    [Inject]
    private readonly GetWorldUseCase getWorldUseCase;

    [Inject]
    private readonly SelectLevelChoicesUseCase selectLevelChoicesUseCase;

    [Inject]
    private readonly CalculateLevelStateUsecase calculateLevelStateUsecase;

    [Inject]
    private readonly PlayerManager playerManager;

    [Inject]
    private readonly RewardManager rewardManager;

    private readonly ISet<Key> monitoredKeys = new HashSet<Key>()
    {
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J,
        Key.K, Key.L, Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T,
        Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z,
        Key.Enter, Key.Backspace,
    };

    private readonly ISet<Key> movementKeys = new HashSet<Key>()
    {
        Key.LeftArrow, Key.RightArrow
    };

    // Fight section
    private int attackIndex;
    private List<Tile> allowedTiles;
    private List<Enemy> enemies;
    private readonly List<Tile> currentWordList = new();
    private Dictionary<string, Word> dictionary;

    // World section
    private World world;
    private int worldIndex;
    private List<World> worlds;

    // Level selection section
    private int levelChoiceIndex;
    private int levelIndex;
    private List<Level> levelsToChooseFrom;

    // Reward selection section
    private int rewardIndex;
    private List<Reward> rewardsToChooseFrom;

    private GameState gameState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager.Init();
        dictionary = retrieveWordsFromDictionaryUsecase.Invoke();
        SetUp();
        ResetAllStates();
        UpdateUIState();
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;


        foreach (var key in monitoredKeys)
        {
            if (keyboard[key]?.wasPressedThisFrame == true)
            {
                HandleAlphabetKeyPress(key);
            }
        }
        foreach (var key in movementKeys)
        {
            if (keyboard[key]?.wasPressedThisFrame == true)
            {
                HandleArrowKeyPress(key);
            }
        }

        worldSelectorGameObject.Appear(gameState is GameState.ChooseWorldState);
        levelSelectorGameObject.Appear(gameState is GameState.ChooseLevelState);
    }

    private void HandleAlphabetKeyPress(Key key)
    {
        Tile tileThatChanged = null;
        if (gameState is GameState.PlayingLevelState)
        {
            tileThatChanged = AddToCurrentWord(key);
        }
        else if (gameState is GameState.ChooseWorldState)
        {
            SelectWorld();
        }
        else if (gameState is GameState.ChooseLevelState)
        {
            SelectLevel();
        }
        else if (gameState is GameState.ChooseRewardState)
        {
            SelectReward();
        }

        UpdateUIState(tileThatChanged: tileThatChanged);
    }

    private Tile AddToCurrentWord(Key key)
    {
        Tile tileThatChanged = null;
        switch (key)
        {
            case Key.Enter:
                bool valid = ProcessWord();
                if (!valid)
                {
                    return null;
                }

                ProcessLevelState();

                break;
            case Key.Backspace:
                if (currentWordList.Any())
                {
                    tileThatChanged = currentWordList[^1];
                    currentWordList.RemoveAt(currentWordList.Count - 1);
                    tileThatChanged.pickable = true;
                }
                break;
            default:
                tileThatChanged = pickTileUsecase.Invoke(key, allowedTiles);
                if (tileThatChanged != null)
                {
                    currentWordList.Add(tileThatChanged);
                }
                break;
        }

        return tileThatChanged;
    }

    private bool ProcessWord()
    {
        string word = GetCurrentWordListAsString();
        if (word.IsEmpty())
        {
            return false;
        }
        currentWordList.Clear();
        processWordUsecase.Invoke(
            word,
            dictionary,
            enemies,
            attackIndex
            );

        if (enemies[attackIndex].IsDead())
        {
            attackIndex = getNextTargetUsecase.Invoke(
                true,
                attackIndex,
                enemies
            );
        }

        List<(int enemyIndex, Enemy.Move move)> movesPair = calculateTurnFromEnemiesUsecase.Invoke(
            enemies
            );
        calculatePlayerDamageUsecase.Invoke(
            movesPair,
            enemies,
            playerManager
            );

        return true;
    }

    private void ProcessLevelState()
    {
        FightEndStateEnum levelState = calculateLevelStateUsecase.Invoke(enemies, playerManager);
        switch (levelState)
        {
            case FightEndStateEnum.Win:
                levelChoiceIndex++;
                UpdateUIState();
                boardContainerGO.ClearEverything();
                SetUpRewardSelection();
                break;
            case FightEndStateEnum.Lose:
                ResetAllStates();
                break;
            case FightEndStateEnum.Ongoing:
                RestartAllowedTiles();
                break;
        }
    }

    private void SelectWorld()
    {
        world = worlds[worldIndex];
        SetUpLevelSelection();
    }

    private void SetUpLevelSelection()
    {
        gameState = new GameState.ChooseLevelState();
        levelIndex = 0;
        levelsToChooseFrom = selectLevelChoicesUseCase.Invoke(levelChoiceIndex, world.LevelChoices);
        levelSelectorGameObject.SetUp(levelsToChooseFrom);
    }

    private void SelectLevel()
    {
        Level level = levelsToChooseFrom[levelIndex];
        if (level is Level.Fight)
        {
            gameState = new GameState.PlayingLevelState(LevelTypeEnum.Fight);
            PopulateEnemies();
            RestartAllowedTiles();
        }
    }

    private void SetUpRewardSelection()
    {
        gameState = new GameState.ChooseRewardState();
        rewardIndex = 0;
        rewardsToChooseFrom = rewardManager.Present();
    }

    private void SelectReward()
    {
        Reward reward = rewardsToChooseFrom[rewardIndex];
        rewardManager.Pick(reward);
        SetUpLevelSelection();
    }

    private void HandleArrowKeyPress(Key key)
    {
        if (gameState is GameState.PlayingLevelState)
        {
            TargetNewEnemy(key);
        }
        else if (gameState is GameState.ChooseWorldState)
        {
            TargetNewWorld(key);
        }
        else if (gameState is GameState.ChooseLevelState)
        {
            TargetNewLevel(key);
        }
        else if (gameState is GameState.ChooseRewardState)
        {
            TargetNewReward(key);
        }
        UpdateUIState();
    }

    private void TargetNewEnemy(Key key)
    {
        attackIndex = getNextTargetUsecase.Invoke(
            key == Key.RightArrow,
            attackIndex,
            enemies
        );
    }

    private void TargetNewWorld(Key key)
    {
        worldIndex = calculateNextIndexUsecase.Invoke(
            key == Key.RightArrow,
            worldIndex,
            worlds.Count
        );
    }

    private void TargetNewLevel(Key key)
    {
        levelIndex = calculateNextIndexUsecase.Invoke(
            key == Key.RightArrow,
            levelIndex,
            levelsToChooseFrom.Count
        );
    }

    private void TargetNewReward(Key key)
    {
        rewardIndex = calculateNextIndexUsecase.Invoke(
            key == Key.RightArrow,
            rewardIndex,
            rewardsToChooseFrom.Count
        );
    }

    private void UpdateUIState(Tile tileThatChanged = null)
    {
        if (gameState is GameState.PlayingLevelState levelGameState)
        {
            switch (levelGameState.LevelTypeEnum)
            {
                case LevelTypeEnum.Fight:
                    boardContainerGO.UpdateState(currentWordList, tileThatChanged);
                    stageContainerGO.UpdateState(enemies[attackIndex]);
                    playerStatsContainerGameObject.UpdateState();

                    string word = GetCurrentWordListAsString();
                    Debug.Log($"{playerManager.CurrentHealth}hp & Targeting: {attackIndex} & Level: {levelChoiceIndex + 1}\n{string.Join(" - ", enemies)}\n{string.Join("", allowedTiles)}\n{word}");
                    break;
            }
        }
        else if (gameState is GameState.ChooseWorldState)
        {
            Debug.Log($"Picking world: {worldIndex}\n{string.Join(",", worlds)}");
        }
        else if (gameState is GameState.ChooseLevelState)
        {
            levelSelectorGameObject.UpdateState(levelsToChooseFrom[levelIndex]);
            Debug.Log($"Picking level: {levelIndex}\n{string.Join(",", levelsToChooseFrom)}");
        }
        else if (gameState is GameState.ChooseRewardState)
        {
            //levelSelectorGameObject.UpdateState(levelsToChooseFrom[levelIndex]);
            Debug.Log($"Picking reward: {rewardIndex}\n{string.Join(",", rewardsToChooseFrom)}");
        }

        if (gameState is not GameState.PlayingLevelState)
        {
            boardContainerGO.ClearEverything();
            stageContainerGO.ClearEverything();
            playerManager.FullHeath();
        }

    }

    private string GetCurrentWordListAsString()
    {
        return new(currentWordList.Select(t => t.Value).ToArray());
    }

    private void SetUp()
    {
        boardContainerGO.tileAction = TileAction;
        boardContainerGO.tileInWordAction = TileInWordAction;
        stageContainerGO.enemySelectedAction = EnemySelectedAction;
        stageContainerGO.enemyHoverAction = EnemyHoverAction;
        playerStatsContainerGameObject.SetUp(playerManager);
        worldSelectorGameObject.SetUp(WorldAction);
        levelSelectorGameObject.levelSelectedAction = LevelSelectedAction;
        levelSelectorGameObject.levelHoverAction = LevelHoverAction;
    }

    private void ResetAllStates()
    {
        attackIndex = 0;
        worldIndex = 0;
        levelChoiceIndex = 0;
        levelIndex = 0;
        SetUpWorldSelection();
    }

    private void SetUpWorldSelection()
    {
        worldIndex = 0;
        gameState = new GameState.ChooseWorldState();
        worlds = Enum.GetValues(typeof(WorldEnum)).Cast<WorldEnum>().ToList()
           .Select(w => getWorldUseCase.Invoke(w)).ToList();
    }

    private void PopulateEnemies()
    {
        attackIndex = 0;
        Level.Fight fightLevel = (Level.Fight)levelsToChooseFrom[levelIndex];
        enemies = populateEnemiesUsecase.Invoke(fightLevel.Enemies);
        stageContainerGO.SetUp(enemies);
    }

    private void RestartAllowedTiles()
    {
        allowedTiles = generateCharTilesUsecase.Invoke();
        boardContainerGO.SetUpTiles(allowedTiles);
    }

    private void TileAction(Tile tile)
    {
        bool exists = pickTileUsecase.Invoke(tile, allowedTiles);
        if (exists)
        {
            currentWordList.Add(tile);
        }
        UpdateUIState(tileThatChanged: tile);
    }

    private void TileInWordAction(Tile tile)
    {
        currentWordList.Remove(tile);
        tile.pickable = true;
        UpdateUIState(tileThatChanged: tile);
    }

    private void EnemySelectedAction(Enemy enemy)
    {
        int originalIndex = attackIndex;
        TargetNewEnemy(enemy);
        bool valid = ProcessWord();
        if (!valid)
        {
            attackIndex = originalIndex;
            return;
        }
        ProcessLevelState();
        UpdateUIState();

    }

    private void EnemyHoverAction(Enemy enemy)
    {
        int originalIndex = attackIndex;
        TargetNewEnemy(enemy);
        if (originalIndex != attackIndex)
        {
            UpdateUIState();
        }
    }

    private void TargetNewEnemy(Enemy enemy)
    {
        int index = enemies.IndexOf(enemy);
        if (index < 0)
        {
            return;
        }
        attackIndex = index;
    }

    private void WorldAction(WorldEnum worldEnum)
    {
        worldIndex = worlds.FindIndex(w => w.WorldEnum == worldEnum);
        SelectWorld();
        UpdateUIState();
    }

    private void LevelSelectedAction(Level level)
    {
        TargetNewLevel(level);
        SelectLevel();
        UpdateUIState();
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
        int index = levelsToChooseFrom.IndexOf(level); ;
        if (index < 0)
        {
            return;
        }
        levelIndex = index;
    }
}
