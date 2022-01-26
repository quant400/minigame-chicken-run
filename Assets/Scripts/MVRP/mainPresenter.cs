using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Toolkit;
using UniRx.Triggers;
using UniRx.Operators;
using System;


    public class mainPresenter : MonoBehaviour
    {
    [SerializeField] menuView mainMenuView;
    [SerializeField] gameplayView gameView;
    [SerializeField] webLoginView webView;
    [SerializeField] characterSelectionView characterSelectionView;
    [SerializeField] uiView uiView;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
        {
        ObservePanelsStatus();
        }

    // Update is called once per frame
    void Update()
        {

        }

    void ObservePanelsStatus()
    {
            chickenGameModel.gameCurrentStep
                   .Subscribe(procedeGame)
                   .AddTo(this);

            void procedeGame(chickenGameModel.GameSteps status)
            {
                switch (status)
                {
                    case chickenGameModel.GameSteps.OnLogin:
                       
                        if (chickenGameModel.userIsLogged.Value)
                        {
                        chickenGameModel.gameCurrentStep.Value = chickenGameModel.GameSteps.OnCharacterSelection;

                        }

                    else
                        {
                        uiView.goToMenu("login");
                        uiView.observeLogin();
                        }
                    break;
                case chickenGameModel.GameSteps.Onlogged:

                    uiView.goToMenu("main");
                    break;
                case chickenGameModel.GameSteps.OnPlayMenu:

                    uiView.goToMenu("main");

                    break;
                case chickenGameModel.GameSteps.OnLeaderBoard:

                    uiView.goToMenu("leaderboeard");
                    break;
                case chickenGameModel.GameSteps.OnCharacterSelection:
                    uiView.goToMenu("characterSelection");
                    webView.checkUSerLoggedAtStart(); /// condisder when start load again .....  !!!! 
                    break;
                case chickenGameModel.GameSteps.OnCharacterSelected:

                    scenesView.AddScene(chickenGameModel.singlePlayerSceneName);
                    break;
                case chickenGameModel.GameSteps.OnStartGame:
                    Observable.Timer(TimeSpan.Zero)
                        .Delay(TimeSpan.FromSeconds(500))
                        .Do(_ => gameView.StartGame())
                        .Subscribe()
                        .AddTo(this);

                    chickenGameModel.gameCurrentStep.Value = chickenGameModel.GameSteps.OnGameRunning;

                    break;
                case chickenGameModel.GameSteps.OnGameRunning:
                    Debug.Log("game Is running");
                    break;
                case chickenGameModel.GameSteps.OnGameEnded:
                    gameView.EndGame();
                    break;
                case chickenGameModel.GameSteps.OnBackToCharacterSelection:
                    scenesView.AddScene(chickenGameModel.mainSceneLoadname);
                    break;


            }

            }
        }
    }


