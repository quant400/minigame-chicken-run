using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LeaderBoardController : MonoBehaviour
{
    [SerializeField]
    GameObject leaderBoard;
    [SerializeField]
    GameObject leaderboardEntryPrefab;
    [SerializeField]
    Transform layoutGroup;
 

    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }


    public void ToggleLeaderBoard(bool b)
    {
        if (b)
        {
            leaderBoard.SetActive(true);
            UpDateLeaderBoardDaily();
            leaderBoard.GetComponent<LeaderBoardScript>().Activate();
        }
        else
        {
            leaderBoard.GetComponent<LeaderBoardScript>().Deactivate();

        }
    }

    public void UpDateLeaderBoardDaily()
    {
        Clean();
        for(int i=0;i<20;i++)
        {
            var temp = Instantiate(leaderboardEntryPrefab, layoutGroup);
            temp.GetComponent<LeaderBoardEntry>().Set((i+1).ToString(), "0x134...F7H92", "0123", "10");
        }
       

        //will load records from api call later;
    }
    public void UpDateLeaderBoardMonthly()
    {
        Clean();
        for (int i = 0; i < 20; i++)
        {
            var temp = Instantiate(leaderboardEntryPrefab, layoutGroup);
            temp.GetComponent<LeaderBoardEntry>().Set((i + 1).ToString(), "0x134...F7H92", "0123", "10");
        }

        //will load records from api call later;
    }

    void Clean()
    {
        for (int i = layoutGroup.childCount - 1; i >= 0; i--)
        {
            Destroy(layoutGroup.GetChild(i).gameObject);
        }
    }
}
