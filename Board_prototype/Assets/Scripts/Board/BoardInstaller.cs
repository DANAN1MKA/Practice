using UnityEngine;
using Zenject;

public class BoardInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<BoardFather>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BoardTimeController>().AsSingle().NonLazy();
        Container.Bind<IBoardTimerEvents>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<IElementGenerator>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<ITimerProgressBar>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<IBoardUIEvents>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<IMoveElementsManager>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
