using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PurchaseProcessingPanel : MonoBehaviour
{
    [SerializeField]
    GameObject loadingGif;
    [SerializeField]
    TMP_Text message;
    [SerializeField]
    GameObject button;
    [SerializeField]
    GameObject background;



    public void SetProcessing()
    {
        loadingGif.SetActive(true);
        message.text = "Processing your Purchase".ToUpper();
        button.SetActive(false);
        background.SetActive(true);
    }

    public void PurchseSuccessful()
    {
        loadingGif.SetActive(false);
        message.text = "Purchase successful".ToUpper();
        button.SetActive(true);
    }
    public void Failed(string s)
    {
        loadingGif.SetActive(false);
        message.text = s.ToUpper();
        button.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
        background.SetActive(false);
    }
}
