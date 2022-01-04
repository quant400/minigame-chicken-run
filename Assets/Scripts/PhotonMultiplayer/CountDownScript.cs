using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CountDownScript : MonoBehaviour
{
    [SerializeField]
    TMP_Text countdownText;
    int time;

    void OnEnable()
    {
        time = GameRoom.room.GetStartDelay();
    }
    public void StartCountDown()
    {
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {
        for (int i = time; i >= 0; i--)
        {
            countdownText.text = i.ToString();
            if(i==0)
                countdownText.text = "Start";
            countdownText.fontSize = 150;
            DOTween.To(() => countdownText.fontSize, x => countdownText.fontSize = x, 0, 1f);
            yield return new WaitForSeconds(1);
        }
        GameRoom.room.SetStart();
        this.gameObject.SetActive(false);
    }

    
}
