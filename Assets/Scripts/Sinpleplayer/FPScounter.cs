using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPScounter : MonoBehaviour
{
    [SerializeField]
    TMP_Text counter;
   
    void Update()
    {
        float fps = 1 / Time.unscaledDeltaTime;
        counter.text = "" + fps;
    }

}
