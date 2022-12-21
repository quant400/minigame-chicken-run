using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.IO;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
public class gameEndView : MonoBehaviour
{
    [SerializeField]
    Transform characterDisplay;
    [SerializeField]
    GameObject charDisplayPrefab;
    GameObject[] characters;
    [SerializeField]
    TMP_Text currentScore, dailyScore, allTimeScore, sessionCounterText;
    [SerializeField]
    GameObject canvasToDisable;
    [SerializeField]
    AudioClip gameOverClip;
    NFTInfo currentNFT;
    [SerializeField]
    GameObject sessionsLeft, sessionsNotLeft;
    ReactiveProperty<int> scorereactive = new ReactiveProperty<int>();
    ReactiveProperty<int> sessions = new ReactiveProperty<int>();
    ReactiveProperty<bool> gameEnded = new ReactiveProperty<bool>();
    [SerializeField] Button tryAgain, back;
    GameObject localDisplay;
    // [SerializeField]
    //SinglePlayerSpawner spawner;
    [SerializeField]
    GameObject tryoutCanvas;
    [SerializeField]
    GameObject endCharDisplay;
    [SerializeField]
    TMP_Text gameOverNftId, GameOverEmail;
    private void OnEnable()
    {
        if (gameplayView.instance.isTryout)
            GameObject.FindGameObjectWithTag("PlayerUI").SetActive(false);
        if (gameplayView.instance.isTryout)
        {
            tryAgain.gameObject.SetActive(false);
            currentScore.text = "CHICKENS CAUGHT : " + SinglePlayerScoreBoardScript.instance.GetScore().ToString();
            dailyScore.text = "DAILY SCORE : " + 0;
            allTimeScore.text = "WEEKLY SCORE : " + 0;
            sessionCounterText.text = "NFT DAILY RUNS : " + 0 + "/10";
        }
        else
        {
            tryAgain.gameObject.SetActive(true);
        }
        AudioSource ad = GetComponent<AudioSource>();
        ad.clip = gameOverClip;
        ad.loop = false;
        ad.volume = 0.2f;
        ad.Play();

        SinglePlayerScoreBoardScript.instance.gamePad.SetActive(false);
    }
    public void Start()
    {
        observeScoreChange();
        endGameAfterValueChange();
        ObserveGameObverBtns();
       
    }
    public void setScoreAtStart()
    {
        
        tryAgain.gameObject.SetActive(false);
        if (canvasToDisable == null)
        {
            canvasToDisable = gameplayView.instance.gameObject.transform.GetChild(0).gameObject;
        }
        currentNFT = gameplayView.instance.chosenNFT;
        if (gameplayView.instance.GetSessions() <= 10)
        {
            if (gameplayView.instance.isRestApi && !gameplayView.instance.usingOtherChainNft && !gameplayView.instance.usingFreemint)
            {
                Debug.Log("before Score");
                DatabaseManagerRestApi._instance.setScoreRestApiMain(currentNFT.id.ToString(), SinglePlayerScoreBoardScript.instance.GetScore());
                Debug.Log("posted Score");
            }
            else if(gameplayView.instance.usingFreemint)
            {
                Debug.Log("before Score");
                DatabaseManagerRestApi._instance.setScoreRestApiMain(gameplayView.instance.GetLoggedPlayerString(), SinglePlayerScoreBoardScript.instance.GetScore());
                Debug.Log("posted Score");
            }
        }
        if (endCharDisplay != null)
            Destroy(endCharDisplay);
        SetSkin();

    }
    public void initializeValues()
    {
        scorereactive.Value = -1;
        sessions.Value = -1;
        gameEnded.Value = false;
    }
    public void ObserveGameObverBtns()
    {

        tryAgain.OnClickAsObservable()
            .Where(_=>gameEnded.Value==true)
            .Do(_ => TryAgain())
            .Where(_ => PlaySounds.instance != null)
            .Do(_ => PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);
       
    }
    public void updateResults()
    {
        if (gameplayView.instance.GetSessions() < 10)
        {

            sessionsLeft.SetActive(true);
            sessionsNotLeft.SetActive(false);
            currentScore.text = "CHICKENS CAUGHT : " + SinglePlayerScoreBoardScript.instance.GetScore().ToString();
            dailyScore.text = "DAILY SCORE : " + (gameplayView.instance.GetDailyScore());
            allTimeScore.text = "WEEKLY SCORE : " + (gameplayView.instance.GetAllTimeScore());
            sessionCounterText.text = "NFT DAILY RUNS : " + (gameplayView.instance.GetSessions()) + "/10";

        }
        else if (gameplayView.instance.GetSessions() >= 10)
        {
            sessionsLeft.SetActive(false);
            sessionsNotLeft.SetActive(true);
            dailyScore.text = "DAILY SCORE : " + (gameplayView.instance.GetDailyScore());
            allTimeScore.text = "WEEKLY SCORE : " + (gameplayView.instance.GetAllTimeScore());
            sessionCounterText.text = "NFT DAILY RUNS : " + (gameplayView.instance.GetSessions()) + "/10";

        }
        SinglePlayerScoreBoardScript.instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);

    }
    public void setScoreResutls()
    {

        if (gameplayView.instance.GetSessions() < 10)
        {

            sessionsLeft.SetActive(true);
            sessionsNotLeft.SetActive(false);
            currentScore.text = "CHICKENS CAUGHT : " + SinglePlayerScoreBoardScript.instance.GetScore().ToString();
            dailyScore.text = "DAILY SCORE : " + (gameplayView.instance.GetDailyScore());
            allTimeScore.text = "WEEKLY SCORE : " + (gameplayView.instance.GetAllTimeScore());
            sessionCounterText.text = "NFT DAILY RUNS : " + (gameplayView.instance.GetSessions()) + "/10";

        }
        else if (gameplayView.instance.GetSessions() >= 10)
        {
            sessionsLeft.SetActive(false);
            sessionsNotLeft.SetActive(true);
            dailyScore.text = "DAILY SCORE : " + (gameplayView.instance.GetDailyScore());
            allTimeScore.text = "WEEKLY SCORE : " + (gameplayView.instance.GetAllTimeScore());
            sessionCounterText.text = "NFT DAILY RUNS : " + (gameplayView.instance.GetSessions()) + "/10";

        }

       
        //upddate other values here form leaderboard
        SinglePlayerScoreBoardScript.instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);


    }
    public void endGameAfterValueChange()
    {
        gameEnded
            .Where(_ => _ == true)
            .Do(_ => updateResults())
            .Subscribe()
            .AddTo(this);
    }
    public void observeScoreChange()
    {
        scorereactive
            .Do(_ => setScoreToUI())
            .Subscribe()
            .AddTo(this);

        sessions
            .Do(_ => setScoreToUI())
            .Subscribe()
            .AddTo(this);

    }
    public void resetDisplay()
    {
        if (localDisplay!=null)
        Destroy(localDisplay);
    }
    private void Update()
    {
        if (chickenGameModel.gameCurrentStep.Value == chickenGameModel.GameSteps.OnGameEnded)
        {
            scorereactive.Value = gameplayView.instance.dailyScore;
            sessions.Value = gameplayView.instance.sessions;
        }
    }
    public void setScoreToUI()
    {
        gameEnded.Value = true;
        if (gameplayView.instance.GetSessions() < 10)
        {

            sessionsLeft.SetActive(true);
            sessionsNotLeft.SetActive(false);
            currentScore.text = "CHICKENS CAUGHT : " + SinglePlayerScoreBoardScript.instance.GetScore().ToString();
            dailyScore.text = "DAILY SCORE : " + (gameplayView.instance.GetDailyScore());
            allTimeScore.text = "WEEKLY SCORE : " + (gameplayView.instance.GetAllTimeScore());
            sessionCounterText.text = "NFT DAILY RUNS : " + (gameplayView.instance.GetSessions()) + "/10";

        }
        else if (gameplayView.instance.GetSessions() >= 10)
        {
            sessionsLeft.SetActive(false);
            sessionsNotLeft.SetActive(true);
            dailyScore.text = "DAILY SCORE : " + (gameplayView.instance.GetDailyScore());
            allTimeScore.text = "WEEKLY SCORE : " + (gameplayView.instance.GetAllTimeScore());
            sessionCounterText.text = "NFT DAILY RUNS : " + (gameplayView.instance.GetSessions()) + "/10";

        }

    }
    public void TryAgain()
    { 
            chickenGameModel.gameCurrentStep.Value = chickenGameModel.GameSteps.OnCharacterSelected;
   
    }
    public void goToMain()
    {
        scenesView.LoadScene(chickenGameModel.mainSceneLoadname.sceneName);
        chickenGameModel.gameCurrentStep.Value = chickenGameModel.GameSteps.OnBackToMenu;
        
    }

    void SetSkin()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        endCharDisplay=Instantiate(charDisplayPrefab, characterDisplay);
        endCharDisplay.GetComponent<SetUpSkin>().SetUpChar(NameToSlugConvert(gameplayView.instance.chosenNFT.name));
        endCharDisplay.GetComponent<Animator>().SetBool("Ended", true);

        endCharDisplay.transform.localPosition = Vector3.zero;
        endCharDisplay.transform.localRotation = Quaternion.identity;
        endCharDisplay.transform.localScale = Vector3.one * 2;
        localDisplay = endCharDisplay;

        //set nft ID Display 
        string n = NameToSlugConvert(gameplayView.instance.chosenNFT.name);
        if (n == "average-joe" || n == "billy-basic" || n == "mary-jane")
        {
            gameOverNftId.text = "";
            string email = gameplayView.instance.chosenNFT.id.Split('$')[0].ToUpper();
            string[] emailParts = email.Split('@');
            GameOverEmail.text = emailParts[0] + "<font=\"LiberationSans SDF\">@</font>" + emailParts[1];
        }
        else
        {
            GameOverEmail.text = "";
            gameOverNftId.text = "NFT ID: " + gameplayView.instance.chosenNFT.id;
        }
    }

    string NameToSlugConvert(string name)
    {
        string slug;
        slug = name.ToLower().Replace(".", "").Replace("'", "").Replace(" ", "-");
        return slug;
    }
}
