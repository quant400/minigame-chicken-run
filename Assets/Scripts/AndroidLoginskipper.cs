using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidLoginskipper : MonoBehaviour
{
    [SerializeField]
    GameObject tryoutCanvas;
    public void Skip()
    {
        gameplayView.instance.isTryout = true;
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        tryoutCanvas.SetActive(true);
    }
       
}
