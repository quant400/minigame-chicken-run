using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSreenScript : MonoBehaviour
{
    [SerializeField]
    GameObject characterSelectionPanel;


    public void PlayButton()
    {
        characterSelectionPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
