﻿using Zenject;

public class BoardInstaller : MonoInstaller
{
    public BoardProperties config;
    public EnemiesPool enemiesPool;
    public PlayerData playerData;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<SwipeElementSignal>();
        Container.DeclareSignal<SetTimerSignal>();
        Container.DeclareSignal<TimerHandlerSignal>();
        Container.DeclareSignal<AnimationCompletedSignal>();
        Container.DeclareSignal<MoveManagerSwipeSignal>();
        Container.DeclareSignal<MoveManagerDropSignal>();
        Container.DeclareSignal<RenderLineSignal>();
        Container.DeclareSignal<CheracterAttackSignal>();

        //TODO: UI
        Container.DeclareSignal<AddScoreSignal>();


        //TODO: characters
        Container.DeclareSignal<NewEnemySignal>();
        Container.DeclareSignal<SwipeDamageSignal>();
        Container.DeclareSignal<MoveEnemyCompliteSignal>();
        Container.DeclareSignal<KillingCompletedSignal>();

        //TODO: server
        Container.DeclareSignal<StartBoardStateSignal>();
        Container.DeclareSignal<NewGemsSignal>();
        Container.DeclareSignal<NewReplaySignal>();
        Container.DeclareSignal<ReplayCompliteSignal>();

        Container.BindInterfacesAndSelfTo<BoardTimeController>().AsSingle().NonLazy();
        Container.Bind<IElementGenerator>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<BoardProperties>().FromInstance(config);
        Container.Bind<EnemiesPool>().FromInstance(enemiesPool);
        Container.Bind<PlayerData>().FromInstance(playerData);


    }
}