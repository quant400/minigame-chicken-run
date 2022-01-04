using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Lobby : MonoBehaviourPunCallbacks
{
    public static Lobby lobby;

    [SerializeField]
    GameObject joinButton, CancelButton, waitingText, selectionbuttons, singlePlayerButton;
    //[SerializeField]
    TMP_InputField nameInput;

    private void Awake()
    {
        lobby = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        if(Debug.isDebugBuild) Debug.Log("connected");
        PhotonNetwork.AutomaticallySyncScene = true;
        joinButton.SetActive(true);
    }

    public void onBattleButtonClick()
    {
        joinButton.SetActive(false);
        CancelButton.SetActive(true);
        waitingText.SetActive(true);
        singlePlayerButton.GetComponent<Button>().interactable = false;
        selectionbuttons.SetActive(false);
        if(Debug.isDebugBuild) Debug.Log("Battle Clicked");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if(Debug.isDebugBuild) Debug.Log("join failed");
        CreateRoom();

    }

    void CreateRoom()
    {
        if(Debug.isDebugBuild) Debug.Log("room created");
        int randomRoom = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoom, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if(Debug.isDebugBuild) Debug.Log("Create failed");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        if(Debug.isDebugBuild) Debug.Log("Cancle Clicked");
        CancelButton.SetActive(false);
        joinButton.SetActive(true);
        singlePlayerButton.GetComponent<Button>().interactable = true;
        PhotonNetwork.LeaveRoom();
    }

   
}
