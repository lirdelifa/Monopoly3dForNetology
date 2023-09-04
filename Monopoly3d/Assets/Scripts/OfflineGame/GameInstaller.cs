using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller, IInitializable
{
    //private GameStart _gameStart;
    [SerializeField] private GUIHandler _GUIHandler;
    [SerializeField] private CellsData _cellsData;
    [SerializeField] private MouseClickHandler _mouseClickHandler;
    public override void InstallBindings()
    {
        
        Container.BindInterfacesTo<GameInstaller>().FromInstance(this).AsSingle();
        Container.Bind<UnitData>().AsSingle().NonLazy();
        Container.BindInstance(_GUIHandler).AsSingle();
        Container.BindInstance(_cellsData).AsSingle();
        Container.Bind<UnitFactory>().AsSingle();
        Container.BindInstance(_mouseClickHandler).AsSingle();
        Container.Bind<GameStart>().AsSingle().NonLazy();
    }

    public void Initialize()
    {
        var unitFactory = Container.Resolve<UnitFactory>();
        unitFactory.Load();
        var gameStart = Container.Resolve<GameStart>();
        gameStart.Start();
    }
}
