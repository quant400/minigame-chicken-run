
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class NFTInfo
{
    public string id;
    public string name;
}
public class NFTGetView : MonoBehaviour
{
    [SerializeField] characterSelectionView characterSelectView;
    public static UnityWebRequest temp;
    [SerializeField]
    GameObject noNFTCanvas;


    private void Start()
    {
        if (characterSelectView == null)
        {
            characterSelectView = GameObject.FindObjectOfType<characterSelectionView>();
        }
    }
    public void GetNFT()
    {
        StartCoroutine(KeyMaker.instance.GetOtherNft());
        StartCoroutine(KeyMaker.instance.GetRequest());
    }

    public void Display(NFTInfo[] NFTData)
    {
        NFTInfo[] used;
        if (gameplayView.instance.hasOtherChainNft)
        {
            NFTInfo tempNft = new NFTInfo() { name = "grane", id = 100000000.ToString() };
            NFTInfo[] tempArr = new NFTInfo[NFTData.Length + 1];
            for (int i = 0; i < tempArr.Length; i++)
            {
                if (i == 0)
                    tempArr[i] = tempNft;
                else
                    tempArr[i] = NFTData[i - 1];
            }
            chickenGameModel.currentNFTArray = tempArr;
            used = tempArr;
        }

        else
        {
            chickenGameModel.currentNFTArray = NFTData;
            used = NFTData;
        }
        if (gameplayView.instance.hasOtherChainNft && used.Length == 1)
        {
            /*noNFTCanvas.SetActive(true);
            chickenGameModel.userIsLogged.Value = false;*/
            gameplayView.instance.usingFreemint = true;
            characterSelectView.FreeMint();
            chickenGameModel.userIsLogged.Value = true;
        }
        else if (used.Length == 0)
        {
            gameplayView.instance.usingFreemint = true;
            characterSelectView.FreeMint();
            chickenGameModel.userIsLogged.Value = true;
        }
        else
        {
            noNFTCanvas.SetActive(false);
            characterSelectView.SetData(used);
            chickenGameModel.userIsLogged.Value = true;
        }


    }
    public void savedLoggedDisplay()
    {
        if (gameplayView.nftDataArray.Length == 0)
        {
            Display(new NFTInfo[0]);
            chickenGameModel.userIsLogged.Value = true;
        }
        else
        {
            noNFTCanvas.SetActive(false);
            characterSelectView.SetData(chickenGameModel.currentNFTArray);
            chickenGameModel.userIsLogged.Value = true;
        }
    }

    //temp Fuction for skip
    public void Skip()
    {
        //to enable skipping in test mode
        StartCoroutine(KeyMaker.instance.GetRequestSkip());
        characterSelectView.Skip();
    }



    void OldNFTGet()
    {
        //string acc = PlayerPrefs.GetString("Account");
       // StartCoroutine(GetRequest("https://api.cryptofightclub.io/game/sdk/" + acc)); 

        //testing link
        //StartCoroutine(GetRequest("https://api.cryptofightclub.io/game/sdk/0xbecd7b5cfab483d65662769ad4fecf05be4d4d05"));
    }
}

