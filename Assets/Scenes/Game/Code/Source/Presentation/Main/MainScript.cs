using ModestTree;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MainScript : MonoBehaviour
{

    public ChooseWorldHandler chooseWorldHandler;
    public ChooseLevelHandler chooseLevelHandler;
    public ChooseRewardHandler chooseRewardHandler;
    public BoardContainerGameObject boardContainerGO;
    public StageContainerGameObject stageContainerGO;
    public PlayerStatsContainerGameObject playerStatsContainerGameObject;

    [Inject]
    private readonly IRetrieveWordsFromDictionaryUsecase retrieveWordsFromDictionaryUsecase;

    [Inject]
    private readonly IPickTileUsecase pickTileUsecase;

    [Inject]
    private readonly IPopulateEnemiesUsecase populateEnemiesUsecase;

    [Inject]
    private readonly IProcessWordUsecase processWordUsecase;

    [Inject]
    private readonly IGenerateCharTilesUsecase generateCharTilesUsecase;

    [Inject]
    private readonly IGetNextTargetUsecase getNextTargetUsecase;

    [Inject]
    private readonly ICalculateTurnFromEnemiesUsecase calculateTurnFromEnemiesUsecase;

    [Inject]
    private readonly ICalculateEnemyMoveUsecase calculateEnemyMoveUsecase;

    [Inject]
    private readonly ICalculateFightEndStateUsecase calculateFightEndStateUsecase;

    [Inject]
    private readonly IPlayerManager playerManager;

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
    }

    private GameState GetGameState()
    {
        return gameState;
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
            SetUpLevelSelection();
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
        calculateEnemyMoveUsecase.Invoke(
            movesPair,
            enemies
            );

        return true;
    }

    private void ProcessLevelState()
    {
        FightEndStateEnum levelState = calculateFightEndStateUsecase.Invoke(enemies);
        switch (levelState)
        {
            case FightEndStateEnum.Win:
                chooseLevelHandler.NextLevelChoice();
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

    private void SetUpWorldSelection()
    {
        gameState = new GameState.ChooseWorldState();
        chooseWorldHandler.SetUpWorldSelection();
    }

    private void SetUpLevelSelection()
    {
        gameState = new GameState.ChooseLevelState();
        World world = chooseWorldHandler.GetWorld();
        chooseLevelHandler.SetUpLevelSelection(world);
    }

    private void SelectLevel()
    {
        Level level = chooseLevelHandler.GetLevel();
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
        chooseRewardHandler.SetUpRewardSelection();
    }

    private void SelectReward()
    {
        Reward reward = chooseRewardHandler.PickReward();
        playerManager.HandleReward(reward);
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
            chooseWorldHandler.TargetNewWorld(key);
        }
        else if (gameState is GameState.ChooseLevelState)
        {
            chooseLevelHandler.TargetNewLevel(key);
        }
        else if (gameState is GameState.ChooseRewardState)
        {
            chooseRewardHandler.TargetNewReward(key);
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
                    Debug.Log($"{playerManager.CurrentHealth}hp & Targeting: {attackIndex}\n{string.Join(" - ", enemies)}\n{string.Join("", allowedTiles)}\n{word}");
                    break;
            }
        }
        else if (gameState is GameState.ChooseWorldState)
        {
            chooseWorldHandler.UpdateUIState();
            playerManager.FullHeath();
        }
        else if (gameState is GameState.ChooseLevelState)
        {
            chooseLevelHandler.UpdateUIState();
        }
        else if (gameState is GameState.ChooseRewardState)
        {
            chooseRewardHandler.UpdateUIState();
        }

        if (gameState is not GameState.PlayingLevelState)
        {
            boardContainerGO.ClearEverything();
            stageContainerGO.ClearEverything();
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
        chooseWorldHandler.Init(GetGameState, UpdateUIState, SetUpLevelSelection);
        chooseLevelHandler.Init(GetGameState, UpdateUIState, SelectLevel);
        chooseRewardHandler.Init(GetGameState, UpdateUIState, SelectReward);
    }

    private void ResetAllStates()
    {
        attackIndex = 0;
        chooseWorldHandler.Reset();
        chooseLevelHandler.Reset();
        chooseRewardHandler.Reset();
        SetUpWorldSelection();
    }

    private void PopulateEnemies()
    {
        attackIndex = 0;
        Level.Fight fightLevel = (Level.Fight) chooseLevelHandler.GetLevel();
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
}
