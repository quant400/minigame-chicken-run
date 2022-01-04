using System.Collections;
using UnityEngine;
using TMPro;

public class WaitingTextAnim : MonoBehaviour
{
    int lenght;
    [SerializeField]
    TMP_Text text;
    bool turnOff=false;
    private void OnEnable()
    {
        lenght = text.text.Length;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        int counter=lenght-4;
        while(!turnOff)
        {
            counter++;
            text.maxVisibleCharacters = counter;
            yield return new WaitForSeconds(0.5f);
            if (counter > lenght)
                counter = lenght - 4;
        }
    }

    private void OnDisable()
    {
        turnOff = true;
    }
}
