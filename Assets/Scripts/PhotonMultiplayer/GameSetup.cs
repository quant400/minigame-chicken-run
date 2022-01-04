using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameSetup : MonoBehaviourPun
{
    public static GameSetup Gs;

    public Transform[] spawnPoints;
    public Transform[] chickenSpawnPoints;
    public int numChickens;
    public int scoreOneChicken;
    public int scoreToWin;

    private void Awake()
    {
        if (GameSetup.Gs == null)
            GameSetup.Gs = this;
    }


    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;

        SceneManager.LoadScene(0);
    }

   
}
