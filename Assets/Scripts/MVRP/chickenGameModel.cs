
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


    public static ReactiveProperty<bool> userIsLogged = new ReactiveProperty<bool>();
    public static ReactiveProperty<GameSteps> gameCurrentStep = new ReactiveProperty<GameSteps>();
    public static GameSteps lastSavedStep;

    public static string currentNFTString;
    public static NFTInfo[] currentNFTArray;
    public static int mainSceneLoad=0;
    public static int singlePlayerSceneInt=1;
    public static string mainSceneLoadname = "Menu";
    public static string singlePlayerSceneName = "SinglePlayerScene";




}

