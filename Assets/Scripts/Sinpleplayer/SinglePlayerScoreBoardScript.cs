using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
public class SinglePlayerScoreBoardScript : MonoBehaviour
{
    public static SinglePlayerScoreBoardScript instance;
    [SerializeField]
    Transform scorePanel;

    Dictionary<string, int> playerIndex = new Dictionary<string, int>();
    [SerializeField]
    TMP_Text winnerText;
    int players;
    [SerializeField]
    TMP_Text chickenCount;
    int chickensCollected;
    [SerializeField]
    GameObject chickenCollectedImage;
    public float time;
    [SerializeField]
    Image timerFill;
    public bool started = false;
    [SerializeField]
    GameObject endGameObject;
    [SerializeField]
    TMP_Text timerValue;
    [SerializeField]
    Image barTimer;
    float currentTime;
    public ReactiveProperty<bool> timeIsUp = new ReactiveProperty<bool>();
    public ReactiveProperty<float> reactiveTime = new ReactiveProperty<float>();

    [SerializeField]
    GameObject settingsPanel;
    public GameObject gamePad;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        timeIsUp.Value = true;
#if UNITY_WEBGL
        gamePad.SetActive(false);
#endif

#if UNITY_ANDROID || UNITY_IOS
        gamePad.SetActive(true);
#endif

    }
    public void StartGame(float timeOfGame)
    {
        Debug.Log("timeSetted");
        time = timeOfGame;
        currentTime = time;
        reactiveTime.Value = time;
        started = true;
        timeIsUp.Value = false;
        barTimer.fillAmount = 1;
        barTimer.transform.parent.gameObject.SetActive(false);
        counterObservation();
    }
    public void counterObservation()
    {
        timeIsUp
            .Where(_ => _ == false)
            .Where(_=>reactiveTime.Value>0)
            .Where(_ => started)
            .Do(_ => timeIsUp.Value = true)
            .Do(_=> SetReactiveTime())
            .Delay(TimeSpan.FromSeconds(1))
            .Where(_=>started)
            .Do(_=> timeIsUp.Value = false)
            .Subscribe()
            .AddTo(this);
        
        reactiveTime
           .Where(_ => started)
           .Where(_ => _ == 0)
           .Do(_=>started=false)
           .Do(_ => timerValue.text = "<color=red>" + _.ToString() + "</color>")
           .Delay(TimeSpan.FromSeconds(1))
           .Do(_ => SetTimeEndGame(_))
           .Subscribe()
           .AddTo(this);



    }
    public void SetReactiveTime()
    {
        reactiveTime.Value -= 1;
        currentTime = reactiveTime.Value;
        SetTimeInUI(currentTime);


    }
    public void SetTimeInUI(float time)
    {
        if (time > 20)
        {
            timerValue.text ="TIME: "+(int)(currentTime/60) +":"+((currentTime)%60).ToString("00");
        }
        else if (time <= 20)
        {
            //timerValue.text = "TIME: "+"<color=blue>" + (int)(currentTime / 60) + ":" + ((currentTime) % 60).ToString("00") + "</color>";
            timerValue.text = "TIME: " + (int)(currentTime / 60) + ":" + ((currentTime) % 60).ToString("00");
            if (!barTimer.transform.parent.gameObject.activeInHierarchy)
                barTimer.transform.parent.gameObject.SetActive(true);
            barTimer.fillAmount -= (float)1/(float)20;
        }
        //timmer object size was 1.5f
        //timerFill.fillAmount = (float)currentTime / (float)SinglePlayerScoreBoardScript.instance.time;
    }
    public void SetTimeEndGame(float time)
    {


        timerValue.text = "TIME: "+ "0:00";
        barTimer.fillAmount =0;
        if (gameplayView.instance != null)
                {
                    gameplayView.instance.EndGame();

                }
                chickenGameModel.gameCurrentStep.Value = chickenGameModel.GameSteps.OnGameEnded;
                DisplayScore();
            


        
    }
    private void Update()
    {
       
    }

    public void DisplayScore()
    {
        //winnerText.text = "You collected " + chickensCollected + " chickens";
        //winnerText.gameObject.SetActive(true);
        if (gameplayView.instance != null)
        {
            gameplayView.instance.gameOverObject.SetActive(true);
        }
    }
    public void AnimChickenCollected()
    {
        chickensCollected++;
        var temp = Instantiate(chickenCollectedImage, transform.GetChild(0).position, Quaternion.identity, transform.GetChild(0));
        temp.transform.DOMove(chickenCount.transform.position, 1f).OnComplete(() =>
        {  
            Destroy(temp);
            chickenCount.text = "SCORE:"+ chickensCollected.ToString("00");
            DOTween.To(() =>chickenCount.fontSize, x => chickenCount.fontSize = x, 35, 0.5f).OnComplete(
              () => DOTween.To(() => chickenCount.fontSize, x => chickenCount.fontSize = x, 30, 0.5f));

        });
    }

    public int GetScore()
    {
        return chickensCollected;
    }


    public void OpenSettings()
    {
        gameplayView.instance.isPaused = true;
        Time.timeScale = 0f;
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        gameplayView.instance.isPaused = false;
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}


