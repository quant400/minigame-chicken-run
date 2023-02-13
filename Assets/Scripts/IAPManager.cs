using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour//, IStoreListener
{
    public TMPro.TMP_Text tempText;
    public void DailyRuns5Completed()
    {
        tempText.text = (int.Parse(tempText.text) + 5).ToString();
        Debug.Log("success");
    }
    public void DailyRuns5Fail()
    {
        Debug.Log("Fail");
    }
}
