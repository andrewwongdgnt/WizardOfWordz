using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Repository
        Container.Bind<IDictionaryRepository>().To<DictionaryRepository>().AsSingle();
        Container.Bind<IEnemyInfoRepository>().To<EnemyInfoRepository>().AsSingle();
        Container.Bind<ILetterDistributionRepository>().To<LetterDistributionRepository>().AsSingle();
        Container.Bind<ILevelInfoRepository>().To<LevelInfoRepository>().AsSingle();
        Container.Bind<IPlayerInfoRepository>().To<PlayerInfoRepository>().AsSingle();
        Container.Bind<IRewardInfoRepository>().To<RewardInfoRepository>().AsSingle();
        Container.Bind<IWorldInfoRepository>().To<WorldInfoRepository>().AsSingle();

        // Manager
        Container.Bind<IPlayerManager>().To<PlayerManager>().AsSingle();
        Container.Bind<IRewardManager>().To<RewardManager>().AsSingle();

        // Usecase
        Container.Bind<ICalculateEnemyMoveUsecase>().To<CalculateEnemyMoveUsecase>().AsSingle();
        Container.Bind<ICalculateFightEndStateUsecase>().To<CalculateFightEndStateUsecase>().AsSingle();
        Container.Bind<ICalculateNextIndexUsecase>().To<CalculateNextIndexUsecase>().AsSingle();
        Container.Bind<ICalculateTurnFromEnemiesUsecase>().To<CalculateTurnFromEnemiesUsecase>().AsSingle();
        Container.Bind<IGenerateCharTilesUsecase>().To<GenerateCharTilesUsecase>().AsSingle();
        Container.Bind<IGenerateRandomNumberUsecase>().To<GenerateRandomNumberUsecase>().AsSingle();
        Container.Bind<IGetNextEnemyMoveUsecase>().To<GetNextEnemyMoveUsecase>().AsSingle();
        Container.Bind<IGetNextTargetUsecase>().To<GetNextTargetUsecase>().AsSingle();
        Container.Bind<IGetTileAdjustedScoreUsecase>().To<GetTileAdjustedScoreUsecase>().AsSingle();
        Container.Bind<IGetWorldUseCase>().To<GetWorldUsecase>().AsSingle();
        Container.Bind<IPickTileUsecase>().To<PickTileUsecase>().AsSingle();
        Container.Bind<IPopulateEnemiesUsecase>().To<PopulateEnemiesUsecase>().AsSingle();
        Container.Bind<IProcessWordUsecase>().To<ProcessWordUsecase>().AsSingle();
        Container.Bind<IRetrieveWordsFromDictionaryUsecase>().To<RetrieveWordsFromDictionaryUsecase>().AsSingle();
        Container.Bind<ISelectLevelChoicesUseCase>().To<SelectLevelChoicesUseCase>().AsSingle();

        // Component
        Container.Bind<MainScript>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RewardSelectorGameObject>().FromComponentInHierarchy().AsSingle();
    }
}
