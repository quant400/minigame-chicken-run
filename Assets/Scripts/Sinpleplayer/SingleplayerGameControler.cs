using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class SingleplayerGameControler : MonoBehaviour
{
    public static SingleplayerGameControler instance;
    [SerializeField]
    int singleplayerScene;
    public int toSpawn;
    //public int chosenAvatar; changed to nft object 
    [SerializeField]
    GameObject[] toDestroyOnload;
    GameObject player;
    //public int startDelay;
    [SerializeField]
    float timeForOneGame;
    [SerializeField]
    int initialChickenCount;
    [SerializeField]
    float spawnIntervals;
    [SerializeField]
    float mushroomPowerUpChance;

    public NFTInfo chosenNFT;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

   
    public void LoadSingleplayer()
    {
        SceneManager.LoadScene(singleplayerScene);
        foreach(GameObject g in toDestroyOnload)
        {
            Destroy(g);
        }
    }

    public void StartGame()
    {
        SinglePlayerScoreBoardScript.instance.StartGame();
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ThirdPersonController>().SetStarted(true);
    }
    public void EndGame()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ThirdPersonController>().SetEnded(true);
    }

    public float GetTimeForGame()
    {
        return timeForOneGame;
    }
    public int GetChickenCount()
    {
        return initialChickenCount;
    }
    public float GetSpawnInterval()
    {
        return spawnIntervals;
    }

    public int GetSinglePlayerScene()
    {
        return singleplayerScene;
    }

    public float GetMushroomPowerUpChance()
    {
        return mushroomPowerUpChance;
    }

}
