using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Repository
        Container.Bind<DictionaryRepository>().To<DictionaryRepositoryImpl>().AsSingle();
        Container.Bind<LetterDistributionRepository>().To<LetterDistributionRepositoryImpl>().AsSingle();

        // Usecase
        Container.Bind<GenerateCharTilesUsecase>().AsSingle();
        Container.Bind<PickTileUsecase>().AsSingle();
        Container.Bind<ProcessWordUsecase>().AsSingle();
        Container.Bind<RetrieveWordsFromDictionaryUsecase>().AsSingle();
        Container.Bind<ReturnTileUsecase>().AsSingle();

        // Component
        Container.Bind<MainScript>().FromComponentInHierarchy().AsSingle();
    }
}
