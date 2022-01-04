using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using DG.Tweening;

public class ScoreUIController : MonoBehaviourPun
{
    public static ScoreUIController instance;
    [SerializeField]
    Transform scorePanel;

    Dictionary<string, int> playerIndex= new Dictionary<string, int>();
    [SerializeField]
    TMP_Text winnerText;
    int players;
    [SerializeField]
    TMP_Text chickenCount;
    int chickensCollected;
    [SerializeField]
    GameObject chickenCollectedImage, countDownTimer;
    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void StartCountdown()
    {
        countDownTimer.GetComponent<CountDownScript>().StartCountDown();
    }

   // [PunRPC]
    public void RPC_PlayerJoined(string player, int index)
    {
        players++;
        playerIndex.Add(player, index);
        scorePanel.GetChild(index).GetComponent<TMP_Text>().text = player;
        scorePanel.GetChild(index).GetChild(0).GetComponent<TMP_Text>().text = "0";
    }


    [PunRPC]
    public void RPC_UpdateScore(string player,int score)
    {
        chickensCollected++;
        scorePanel.GetChild(playerIndex[player]).GetChild(0).GetComponent<TMP_Text>().text = score.ToString();
        DOTween.To(() => scorePanel.GetChild(playerIndex[player]).GetComponent<TMP_Text>().fontSize, x => scorePanel.GetChild(playerIndex[player]).GetComponent<TMP_Text>().fontSize = x, 30, 0.5f).OnComplete(
            () => DOTween.To(() => scorePanel.GetChild(playerIndex[player]).GetComponent<TMP_Text>().fontSize, x => scorePanel.GetChild(playerIndex[player]).GetComponent<TMP_Text>().fontSize = x, 20, 0.5f));
        DOTween.To(() => scorePanel.GetChild(playerIndex[player]).GetChild(0).GetComponent<TMP_Text>().fontSize, x => scorePanel.GetChild(playerIndex[player]).GetChild(0).GetComponent<TMP_Text>().fontSize = x, 30, 0.5f).OnComplete(
           () => DOTween.To(() => scorePanel.GetChild(playerIndex[player]).GetChild(0).GetComponent<TMP_Text>().fontSize, x => scorePanel.GetChild(playerIndex[player]).GetChild(0).GetComponent<TMP_Text>().fontSize = x, 20, 0.5f));


        chickenCount.text = (10 - chickensCollected).ToString();

    }
    [PunRPC]
    public void RPC_DeclareWinner()
    {
        string maxPoints="";
        int currentMax=0;
        
        for(int i=1;i<=players;i++)
        {
            
            if(int.Parse(scorePanel.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text) > currentMax)
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

    public void AnimChickenCollected()
    {
        var temp =Instantiate(chickenCollectedImage, transform.GetChild(0).position, Quaternion.identity, transform.GetChild(0));
        temp.transform.DOMove(scorePanel.GetChild(4).position, 1f).OnComplete(() => Destroy(temp));
    }
}