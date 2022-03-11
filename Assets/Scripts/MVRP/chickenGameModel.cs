
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;
using UniRx.Toolkit;
[Serializable]

public struct chickenGameModel
{
    [Serializable]
    public enum GameSteps
    {
        OnLogin,
        Onlogged,
        OnNoNpc,
        OnPlayMenu,
        OnLeaderBoard,
        OnCharacterSelection,
        OnCharacterSelected,
        OnSwipeCharacterSelection,
        OnClickStart,
        OnStartGame,
        OnGameRunning,
        OnGameEnded,
        OnShowResults,
        OnTryAgain,
        OnSessionLimitReach,
        OnResultsLeadboardClick,
        OnBackToMenu,
        OnBackToCharacterSelection,
        OnExit,
        onSceneLoaded,
    }
    public enum sceneLoadType
    {
        menu,
        mountainsAndForest,
        japan,
        city,
        egypt,
        moon
    }
    public class sceneLoadData
    {
        public sceneLoadType type;
        public string sceneName;
        public sceneLoadData(sceneLoadType typeInput , string name)
        {
            type = typeInput;
            sceneName = name;
        }
    }

    public static ReactiveProperty<bool> userIsLogged = new ReactiveProperty<bool>();
    public static ReactiveProperty<GameSteps> gameCurrentStep = new ReactiveProperty<GameSteps>();
    public static GameSteps lastSavedStep;
    public static bool charactersSetted;

    public static string currentNFTString;
    public static NFTInfo[] currentNFTArray;
    public static int mainSceneLoad=0;
    public static int singlePlayerSceneInt=1;
    public static int currentNFTSession = 0;

    public static sceneLoadData mainSceneLoadname = new sceneLoadData(sceneLoadType.menu, "Menu");
    public static sceneLoadData singlePlayerScene1 = new sceneLoadData(sceneLoadType.mountainsAndForest, "SinglePlayerScene");
    public static sceneLoadData singlePlayerScene2 = new sceneLoadData(sceneLoadType.japan, "SinglePlayerScene 2");
    public static sceneLoadData singlePlayerScene3 = new sceneLoadData(sceneLoadType.city, "SinglePlayerScene 3");
    public static sceneLoadData singlePlayerScene4 = new sceneLoadData(sceneLoadType.egypt, "SinglePlayerScene 4");
    public static sceneLoadData singlePlayerScene5 = new sceneLoadData(sceneLoadType.moon, "SinglePlayerScene 5");



}

