using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoardEntry : MonoBehaviour
{
    [SerializeField]
    TMP_Text rank, wallet, nftID, score;


    public void Set(string Rank,string Wallet,string ID,string Score)
    {
        rank.text = Rank;
        wallet.text = Wallet;
        nftID.text = ID;
        score.text = Score;

    }
        
}
