﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
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
    float currentTime;
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
       
    }
    public void StartGame(float timeOfGame)
    {
        Debug.Log("timeSetted");
        time = timeOfGame;
        currentTime = time;
        started = true;

    }
    private void Update()
    {
        if (started)

        {
            timerFill.fillAmount -= Time.deltaTime / time;
            currentTime -= Time.deltaTime;
            if(currentTime>20)
            {
                timerValue.text = ((int)(currentTime)).ToString();
            }
            else if(currentTime<=20)
            {
                timerValue.text = "<color=red>" + ((int)(currentTime)).ToString()+"</color>";
            }
           
        }

        if(timerFill.fillAmount<=0)
        {
            if(started)
            {
             if (gameplayView.instance != null)
                {
                    gameplayView.instance.EndGame();

                }
                chickenGameModel.gameCurrentStep.Value = chickenGameModel.GameSteps.OnGameEnded;
                DisplayScore();
            }
            started = false;
            //SingleplayerGameControler.instance.EndGame();
            
        }    
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
            chickenCount.text = chickensCollected.ToString("00");
            DOTween.To(() =>chickenCount.fontSize, x => chickenCount.fontSize = x, 75, 0.5f).OnComplete(
              () => DOTween.To(() => chickenCount.fontSize, x => chickenCount.fontSize = x, 60, 0.5f));

        });
    }

    public int GetScore()
    {
        return chickensCollected;
    }
}


//used for more than one player with a scoreboard.
/*  public void PlayerJoined(string player, int index)
  {
      players++;
      playerIndex.Add(player, index);
      scorePanel.GetChild(index).GetComponent<TMP_Text>().text = player;
      scorePanel.GetChild(index).GetChild(0).GetComponent<TMP_Text>().text = "0";
  }



  public void UpdateScore(string player, int score)
  {
      chickensCollected++;
      scorePanel.GetChild(playerIndex[player]).GetChild(0).GetComponent<TMP_Text>().text = (int.Parse(scorePanel.GetChild(playerIndex[player]).GetChild(0).GetComponent<TMP_Text>().text) + score).ToString();
      DOTween.To(() => scorePanel.GetChild(playerIndex[player]).GetComponent<TMP_Text>().fontSize, x => scorePanel.GetChild(playerIndex[player]).GetComponent<TMP_Text>().fontSize = x, 30, 0.5f).OnComplete(
          () => DOTween.To(() => scorePanel.GetChild(playerIndex[player]).GetComponent<TMP_Text>().fontSize, x => scorePanel.GetChild(playerIndex[player]).GetComponent<TMP_Text>().fontSize = x, 20, 0.5f));
      DOTween.To(() => scorePanel.GetChild(playerIndex[player]).GetChild(0).GetComponent<TMP_Text>().fontSize, x => scorePanel.GetChild(playerIndex[player]).GetChild(0).GetComponent<TMP_Text>().fontSize = x, 30, 0.5f).OnComplete(
         () => DOTween.To(() => scorePanel.GetChild(playerIndex[player]).GetChild(0).GetComponent<TMP_Text>().fontSize, x => scorePanel.GetChild(playerIndex[player]).GetChild(0).GetComponent<TMP_Text>().fontSize = x, 20, 0.5f));


      chickenCount.text = (10 - chickensCollected).ToString();
      if(chickensCollected==10)
      {
          DeclareWinner();
          SingleplayerGameControler.instance.EndGame();
      }

  }

  public void DeclareWinner()
  {
      string maxPoints = "";
      int currentMax = 0;

      for (int i = 1; i <= players; i++)
      {

          if (int.Parse(scorePanel.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text) > currentMax)
          {
              currentMax = int.Parse(scorePanel.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text);
              maxPoints = scorePanel.GetChild(i).GetComponent<TMP_Text>().text;
          }
      }

      winnerText.text = "Player " + maxPoints + " Wins";
      winnerText.gameObject.SetActive(true);

  }


  public void ActivateBoard()
  {
      scorePanel.gameObject.SetActive(true);
  }
  */