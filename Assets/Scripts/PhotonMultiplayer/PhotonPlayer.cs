using Photon.Pun;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{

    PhotonView Pv;
    public GameObject myAvatar;
    int spawnpoint;

    void Start()
    {
        Pv = GetComponent<PhotonView>();
        
        if (Pv.IsMine)
        {
            spawnpoint = GameRoom.room.GetMyNumber() - 1;
            if (spawnpoint < 0)
                spawnpoint = 0;
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", GameRoom.room.chosenAvatar), GameSetup.Gs.spawnPoints[spawnpoint].position, GameSetup.Gs.spawnPoints[PhotonNetwork.PlayerList.Length].rotation, 0);
            
        }
    }
    
}