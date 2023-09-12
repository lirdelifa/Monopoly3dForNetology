//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Zenject;

public class GameInstaller : MonoInstaller, IInitializable
{
    [SerializeField] private List<Cell> _cells;
    [SerializeField] private List<Dice> _dices;
    [SerializeField] private List<InfoPanel> _infoPanels;
    [SerializeField] private List<CellGroupParameters> _cellGroupParameters;

    [SerializeField] private MessagePanel _messagePanel;
    [SerializeField] private CardPanel _cardPanel;
    [SerializeField] private TopCharacterInformation _topCharacterInformation;
    [SerializeField] private MenuBar _menuBar;
    [SerializeField] private GameOverPanel _gameOverPanel;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameInstaller>().FromInstance(this).AsSingle();

        Container.BindInstance(_messagePanel).AsSingle();
        Container.BindInstance(_cardPanel).AsSingle();
        Container.BindInstance(_topCharacterInformation).AsSingle();
        Container.BindInstance(_gameOverPanel).AsSingle();
        Container.Bind<Raycaster>().AsSingle().NonLazy();
        Container.Bind<CellsData>().AsSingle().NonLazy();
        Container.Bind<BotIntellect>().AsSingle().NonLazy();
        Container.Bind<Player>().AsSingle().NonLazy();
        Container.Bind<CharacterFactory>().AsSingle().NonLazy();
        Container.Bind<CharactersData>().AsSingle().NonLazy();
        Container.Bind<ChangeButtonActionTypesSystem>().AsSingle().NonLazy();
        Container.Bind<TurnTransitionSystem>().AsSingle().NonLazy();
        Container.Bind<FreeTargetPointUpdateSystem>().AsSingle().NonLazy();
        Container.Bind<DicesFlipSystem>().AsSingle().NonLazy();
        Container.Bind<WorldSpaceSystem>().AsSingle().NonLazy();
        Container.Bind<MoneyCountingSystem>().AsSingle().NonLazy();
        Container.Bind<StoppingAtCellSystem>().AsSingle().NonLazy();
        Container.Bind<SaveSystem>().AsSingle().NonLazy();
    }

    public void Initialize()
    {
        List<IDispose> disposes = new List<IDispose>();

        disposes.Add((IDispose)Container.Resolve<Raycaster>());
        disposes.Add((IDispose)Container.Resolve<Player>());
        disposes.Add((IDispose)Container.Resolve<WorldSpaceSystem>());
        disposes.Add((IDispose)Container.Resolve<StoppingAtCellSystem>());
        disposes.Add((IDispose)Container.Resolve<CharactersData>());
        disposes.Add((IDispose)Container.Resolve<SaveSystem>());

        for (int i = 0; i < _infoPanels.Count; i++) _infoPanels[i].Init();

        CellsData.AddGroupParameters(_cellGroupParameters);
        CellsData.AddAllCells(_cells);

        FreeTargetPointUpdateSystem freeTargetPointUpdateSystem = Container.Resolve<FreeTargetPointUpdateSystem>();
        disposes.Add((IDispose)freeTargetPointUpdateSystem);
        List<TargetPoint> targetPoints = FindObjectsOfType<MonoBehaviour>(true).OfType<TargetPoint>().ToList();
        freeTargetPointUpdateSystem.AddAllTargetPoints(targetPoints);

        CharacterFactory factory = Container.Resolve<CharacterFactory>();
        BotIntellect botIntellect = Container.Resolve<BotIntellect>();
        disposes.Add((IDispose)botIntellect);
        factory.Load();

        AllSaveParameters allSaveParameters = SaveManager.GetCurrentAllSaveParameters();
        int currentCharacterId = 0;
        for (int i = 0; i < allSaveParameters.charactersSaveParameters.Count; i++) 
        {
            CharacterSaveParameters parameters = allSaveParameters.charactersSaveParameters[i];
            Vector3 spawnPosition = new Vector3();
            if (!allSaveParameters.isNewGame) spawnPosition = new Vector3(parameters.positionX, parameters.positionY, parameters.positionZ);
            else spawnPosition = CellsData.GetFreePointByCell(CellsData.GetStartCell);

            Character character = factory.Create(spawnPosition);
            character.SetSaveParameters(parameters);
            botIntellect.AddCharacter(character);
            if(!parameters.isBotControlled) Container.Resolve<Player>().AddCharacter(character);
            if (parameters.isMyTurn) currentCharacterId = i;

            freeTargetPointUpdateSystem.Update();
        }
        for (int i = 0; i < allSaveParameters.propertyCellSaveParameters.Count; i++)
        {
            PropertyCellSaveParameters parameters = allSaveParameters.propertyCellSaveParameters[i];
            if (parameters.haveOwner)
            {
                CellProperty cellProperty = (CellProperty)CellsData.GetCellById(parameters.id);
                cellProperty.SetOwner(CharactersData.GetCharacters[parameters.ownerId]);
            }
        }
        for (int i = 0; i < allSaveParameters.protectionWallSaveParameters.Count; i++)
        {
            ProtectionWallSaveParameters parameters = allSaveParameters.protectionWallSaveParameters[i];
            if (parameters.isShow)
            {
                CellProperty cellProperty = (CellProperty)CellsData.GetCellById(parameters.cellId);
                cellProperty.GetProtectionWalls[parameters.id].Show(true);
                cellProperty.GetProtectionWalls[parameters.id].SetHealth(parameters.health);
            }
        }


        MoneyCountingSystem moneyCountingSystem = Container.Resolve<MoneyCountingSystem>();
        disposes.Add((IDispose)moneyCountingSystem);
        moneyCountingSystem.CalculateAllMonetotyCondition();

        DicesFlipSystem dicesFlipSystem = Container.Resolve<DicesFlipSystem>();
        disposes.Add((IDispose)dicesFlipSystem);
        dicesFlipSystem.AddAllDices(_dices);

        ChangeButtonActionTypesSystem changeButtonActionTypesSystem = Container.Resolve<ChangeButtonActionTypesSystem>();
        disposes.Add((IDispose)changeButtonActionTypesSystem);
        List<IRaycasterListener> raycasterListeners = FindObjectsOfType<MonoBehaviour>(true).OfType<IRaycasterListener>().ToList();
        changeButtonActionTypesSystem.AddAllRaycasterListener(raycasterListeners);

        TurnTransitionSystem turnTransitionSystem = Container.Resolve<TurnTransitionSystem>();
        disposes.Add((IDispose)turnTransitionSystem);
        turnTransitionSystem.SetCurrentCharacterId(currentCharacterId);
        turnTransitionSystem.NextTurn(false);

        _menuBar.SetDisposes(disposes);
    }
}
