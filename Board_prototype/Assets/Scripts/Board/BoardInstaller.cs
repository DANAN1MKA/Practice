using UnityEngine;
using Zenject;

public class BoardInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<BoardFather>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<Timer>().AsSingle().NonLazy();
        Container.Bind<IPieceGenerator>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<IProgressBar>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}