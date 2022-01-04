using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static GameRoom room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    Player[] photonPlayers;

    private int playersInRoom;
    private int myNumberInRoom;
    public int multiPlayerScene;
    public int playerInGame;

    private bool readyToCount, readyToStart;
    public float startingTime;
    float lessThanMaxPlayers, atMaxPlayers, timeToStart;

    Dictionary<Player, int> playerScores = new Dictionary<Player, int>();
    [SerializeField]
    TMP_InputField nameInput;
    string playerName;
    [HideInInspector]
    public string chosenAvatar;
    [HideInInspector]
    public bool Ended =false;
    [HideInInspector]
    public bool started = false;
    [SerializeField]
    int StartDelay;
    [HideInInspector]
    public int chickensCollected;
    [SerializeField]
    GameObject ChickenCountText;
    private void Awake()
    {
        if (GameRoom.room == null)
            GameRoom.room = this;

        else
        {
            if (GameRoom.room != this)
            {
                Destroy(GameRoom.room.gameObject);
                GameRoom.room = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }





    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    private void CreatePlayer()
    {
        var temp = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayerPla"), transform.position, Quaternion.identity, 0);
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
    }

    private void Update()
    {
        if (MultiplayerSettings.multiplayerSettings.delaydStart)
        {
            if (playersInRoom == 1)
            {
                RestartTimer();
            }
            if (!isGameLoaded)
            {
                if (readyToStart)
                {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                }
                else if (readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }
                if(Debug.isDebugBuild) Debug.Log("Diaplay time to players " + timeToStart);
                if (timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }
        /*if(SpawningChickens)
        {
            timePassed += Time.deltaTime;
            if(timePassed>=10)
            {
                SpawnAChickens();
                timePassed = 0;
            }
        }*/
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if(Debug.isDebugBuild) Debug.Log("Joined room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();//playerName;// //network name

        if (PhotonNetwork.IsMasterClient)
            playerScores.Add(PhotonNetwork.LocalPlayer, 0);
        ScoreUIController.instance.ActivateBoard();
        photonView.RPC("RPC_UpdateBoard", RpcTarget.AllBuffered, PhotonNetwork.NickName, playersInRoom);
        if (MultiplayerSettings.multiplayerSettings.delaydStart)
        {
            if(Debug.isDebugBuild) Debug.Log("Displayer players in room out of max possible (" + playersInRoom + " : " + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                {
                    return;
                }
                else
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if(Debug.isDebugBuild) Debug.Log(newPlayer.NickName + " joined");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        playerScores.Add(newPlayer, 0);
        if (MultiplayerSettings.multiplayerSettings.delaydStart)
        {
            if(Debug.isDebugBuild) Debug.Log("Displayer players in room out of max possible (" + playersInRoom + " : " + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                {
                    return;
                }
                else
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                }
            }
        }
        else
        {
            if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
                StartGame();
        }
    }


    public void StartGame()
    {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        //if (MultiplayerSettings.multiplayerSettings.delaydStart)
        //{
            PhotonNetwork.CurrentRoom.IsOpen = false;

        //}
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.MultiplayerScene);

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if(Debug.isDebugBuild) Debug.Log(otherPlayer.NickName + " Has left");
        playersInRoom--;
    }

    void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSettings.multiplayerSettings.MultiplayerScene)
        {
            isGameLoaded = true;
            if (MultiplayerSettings.multiplayerSettings.delaydStart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
            {
                RPC_CreatePlayer();
                if (PhotonNetwork.IsMasterClient)
                {
                    SpawnChickens();
                    PV.RPC("RPC_StartCountDown", RpcTarget.All);
                }
            }
        }

        /*if (currentScene == multiPlayerScene)
        {
            CreatePlayer();
        }*/
    }


    public void SetName()
    {
        playerName = nameInput.text;
        PhotonNetwork.NickName = playerName;
    }

    private void SpawnChickens()
    {
        PV.RPC("ActivateChickenCounter", RpcTarget.All);
        for (int i = 0; i < GameSetup.Gs.numChickens; i++)
        {
            int point = Random.Range(0, 4);

            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chicken"), GameSetup.Gs.chickenSpawnPoints[i].position, Quaternion.identity, 0);
           
        }
       
    }

    [PunRPC]
    void ActivateChickenCounter()
    {
        ChickenCountText.SetActive(true);
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playerInGame++;
        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);

        }
    }
    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);

    }

    [PunRPC]
    public void RPC_UpdateScore(Player p, int s)
    {
        // if (PhotonNetwork.IsMasterClient)
        // SpawnAChickens();
        chickensCollected++;
        playerScores[p] += s;
        if(Debug.isDebugBuild) Debug.Log(p + "collected chicken");
        photonView.RPC("RPC_UpdateScoreBoard", RpcTarget.All, p.NickName, playerScores[p]);
        if (chickensCollected==GameSetup.Gs.numChickens)
        {
            photonView.RPC("RPC_EndGame", RpcTarget.All);
            photonView.RPC("RPC_DeclareWinner", RpcTarget.All);
        }
    }
    [PunRPC]
    void RPC_DeclareWinner()
    {
        ScoreUIController.instance.RPC_DeclareWinner();
    }

    [PunRPC]
    void RPC_UpdateScoreBoard(string p, int s)
    {
        ScoreUIController.instance.RPC_UpdateScore(p, s);
    }
   [PunRPC]
    void RPC_UpdateBoard(string player ,int index)
    {
        ScoreUIController.instance.RPC_PlayerJoined(player, index);
    }
    [PunRPC]
    void RPC_StartCountDown()
    {
        ScoreUIController.instance.StartCountdown();
    }
   
    public void SetStart()
    {
        started = true;
    }
    [PunRPC]
    void RPC_EndGame()
    {
        Ended = true;
    }

    public void ChooseAvatar(string x)
    {
        chosenAvatar = x;
    }
   
    public int GetMyNumber()
    {
        return myNumberInRoom;
    }

    public int GetStartDelay()
    {
        return StartDelay;
    }
}
