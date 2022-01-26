using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Toolkit;
using UniRx.Triggers;
using UniRx.Operators;

public class uiView : MonoBehaviour
{
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject characterSelectionPanel;
    [SerializeField] GameObject loginCanvas;
    [SerializeField] GameObject resultsCanvas;
    [SerializeField] GameObject leaderBoeardCanvas;


    public Button loginBtn, PlayMode, Play, LeaderBoard, Back,Skip,backFromLeaderboard;
    [SerializeField] webLoginView webloginView;
    // Start is called before the first frame update
    void Start()
    {
        ObserveBtns();
    }
    public void observeLogin()
    {
        chickenGameModel.userIsLogged
          .Where(_ => _)
          .Do(_ => chickenGameModel.gameCurrentStep.Value = chickenGameModel.GameSteps.Onlogged)
          .Subscribe()
          .AddTo(this);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ObserveBtns()
    {
        loginBtn.OnClickAsObservable()
            .Do(_=> webloginView.OnLogin(loginBtn, Skip))
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);
        PlayMode.OnClickAsObservable()
           .Do(_ => PlayMainButton())
           .Where(_ => PlaySounds.instance != null)
           .Do(_ => PlaySounds.instance.Play())
           .Subscribe()
           .AddTo(this);

    }
    public void PlayMainButton()
    {

       
        chickenGameModel.gameCurrentStep.Value = chickenGameModel.GameSteps.OnCharacterSelection;

    }
    public void goToMenu(string menuName)
    {
        if (resultsCanvas == null)
        {
            resultsCanvas = GameObject.Find("EndCanvasHolder").transform.GetChild(0).gameObject;
        }
        switch (menuName)
        {
            case "login":
                mainMenuCanvas.gameObject.SetActive(false);
                characterSelectionPanel.SetActive(false);
                loginCanvas.gameObject.SetActive(true);
                leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(false);
                resultsCanvas.SetActive(false);

                break;
            case "main":
                mainMenuCanvas.gameObject.SetActive(true);
                characterSelectionPanel.SetActive(false);
                loginCanvas.gameObject.SetActive(false);
                resultsCanvas.SetActive(false);
                leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(false);
                break;
            case "characterSelection":
                mainMenuCanvas.gameObject.SetActive(false);
                characterSelectionPanel.SetActive(true);
                loginCanvas.gameObject.SetActive(false);
                resultsCanvas.SetActive(false);
                leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(false);
                break;
            case "results":
                mainMenuCanvas.gameObject.SetActive(false);
                characterSelectionPanel.SetActive(false);
                leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(false);
                loginCanvas.gameObject.SetActive(false);
                resultsCanvas.SetActive(true);
                break;
            case "leaderboeard":
                leaderBoeardCanvas.GetComponent<LeaderBoardControllerRestApi>().ToggleLeaderBoard(true);
                break;
        }
    }
}
