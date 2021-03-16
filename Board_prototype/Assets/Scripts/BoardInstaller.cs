using Zenject;

public class BoardInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<SwipeElementSignal>();
        Container.DeclareSignal<SetTimerSignal>();
        Container.DeclareSignal<TimerHandlerSignal>();
        Container.DeclareSignal<AnimationCompletedSignal>();
        Container.DeclareSignal<MoveManagerAddSignal>();
        Container.DeclareSignal<MoveManagerDropSignal>();


        Container.Bind<BoardLayout>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BoardTimeController>().AsSingle().NonLazy();
        Container.Bind<IElementGenerator>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
