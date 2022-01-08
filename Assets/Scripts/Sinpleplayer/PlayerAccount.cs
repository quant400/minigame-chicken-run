using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccount : MonoBehaviour
{
    [SerializeField]
    GameObject[] players;
    Info[] myNFT;


    public void SetData(Info[] Pdata)
    {
        myNFT = Pdata;
        SetDefault();
           
    }


    void SetDefault()
    {
        if (myNFT[0].id == 538)
            players[1].SetActive(true);

        else
            players[0].SetActive(true);

    }
}
