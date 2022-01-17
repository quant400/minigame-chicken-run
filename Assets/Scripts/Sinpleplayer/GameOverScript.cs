using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.IO;

public class GameOverScript : MonoBehaviour
{
    [SerializeField]
    Transform characterDisplay;
    GameObject[] characters;
    [SerializeField]
    TMP_Text currentScore, dailyScore, allTimeScore;
    [SerializeField]
    GameObject canvasToDisable;
    [SerializeField]
    AudioClip gameOverClip;
    NFTInfo currentNFT;
    [SerializeField]
    GameObject sessionsLeft, sessionsNotLeft;
   // [SerializeField]
    //SinglePlayerSpawner spawner;
    private void OnEnable()
    {
        currentNFT = SingleplayerGameControler.instance.chosenNFT;
        AudioSource ad = GameObject.FindGameObjectWithTag("SFXPlayer").GetComponent<AudioSource>();
        ad.clip = gameOverClip;
        ad.loop = false;
        ad.volume = 0.2f;
        ad.Play();
        //characters = spawner.GetCharacterList();
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        GameObject displayChar = Resources.Load(Path.Combine("SinglePlayerPrefabs/Characters", currentNFT.name)) as GameObject;
        var temp=Instantiate(displayChar, characterDisplay.position, Quaternion.identity,characterDisplay);
        
        //destroying all player related components
        Destroy(temp.transform.GetChild(1).gameObject);
        Destroy(temp.transform.GetChild(0).gameObject);
        Destroy(temp.transform.GetChild(2).gameObject);
        Destroy(temp.transform.GetChild(3).gameObject);
        Destroy(temp.GetComponent<StarterAssetsInputs>());
        Destroy(temp.GetComponent<ThirdPersonController>());
        Destroy(temp.GetComponent<CharacterController>());
        Destroy(temp.GetComponent<PlayerInput>());
        temp.GetComponent<Animator>().SetBool("Ended", true);

        temp.transform.localPosition = Vector3.zero;
        temp.transform.localRotation = Quaternion.identity;
        temp.transform.localScale = Vector3.one * 2;
        

        if (SingleplayerGameControler.instance.GetSessions()<10)
        {
            DatabaseManager._instance.setScore(currentNFT.id.ToString(), currentNFT.name, SinglePlayerScoreBoardScript.instance.GetScore());
            sessionsLeft.SetActive(true);
            sessionsNotLeft.SetActive(false);
            currentScore.text = "CHICKENS CAUGHT : " + SinglePlayerScoreBoardScript.instance.GetScore().ToString();
            dailyScore.text = "DAILY SCORE : " + (SingleplayerGameControler.instance.GetDailyScore() + SinglePlayerScoreBoardScript.instance.GetScore());
            allTimeScore.text = "ALL TIME SCORE : " + (SingleplayerGameControler.instance.GetAllTimeScore() + SinglePlayerScoreBoardScript.instance.GetScore());
        }
        else if(SingleplayerGameControler.instance.GetSessions() >=10)
        {
            sessionsLeft.SetActive(false);
            sessionsNotLeft.SetActive(true);
            dailyScore.text = "DAILY SCORE : " + (SingleplayerGameControler.instance.GetDailyScore());
            allTimeScore.text = "ALL TIME SCORE : " + (SingleplayerGameControler.instance.GetAllTimeScore());
        }

       
        //upddate other values here form leaderboard
        canvasToDisable.SetActive(false);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SingleplayerGameControler.instance.GetSinglePlayerScene());
    }
}
