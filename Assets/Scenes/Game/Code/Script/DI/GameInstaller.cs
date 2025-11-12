using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Repository
        Container.Bind<DictionaryRepository>().To<DictionaryRepositoryImpl>().AsSingle();
        Container.Bind<EnemyInfoRepository>().To<EnemyInfoRepositoryImpl>().AsSingle();
        Container.Bind<LetterDistributionRepository>().To<LetterDistributionRepositoryImpl>().AsSingle();
        Container.Bind<PlayerInfoRepository>().To<PlayerInfoRepositoryImpl>().AsSingle();

        // Manager
        Container.Bind<PlayerManager>().AsSingle();

        // Usecase
        Container.Bind<CalculateTurnFromEnemiesUsecase>().AsSingle();
        Container.Bind<GenerateCharTilesUsecase>().AsSingle();
        Container.Bind<GetNextTargetUsecase>().AsSingle();
        Container.Bind<PickTileUsecase>().AsSingle();
        Container.Bind<PopulateEnemiesUsecase>().AsSingle();
        Container.Bind<ProcessWordUsecase>().AsSingle();
        Container.Bind<RetrieveWordsFromDictionaryUsecase>().AsSingle();
        Container.Bind<ReturnTileUsecase>().AsSingle();

        // Component
        Container.Bind<MainScript>().FromComponentInHierarchy().AsSingle();
    }
}
