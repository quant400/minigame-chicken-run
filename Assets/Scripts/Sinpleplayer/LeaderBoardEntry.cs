using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoardEntry : MonoBehaviour
{
    [SerializeField]
    TMP_Text pName, score;


    public void Set(string PName,string Score)
    {
        pName.text = PName;
        score.text = Score;
    }
        
}
