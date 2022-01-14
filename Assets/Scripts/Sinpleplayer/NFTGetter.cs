using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class NFTInfo
{
    public int id;
    public string name;
}
public class NFTGetter : MonoBehaviour
{
    CharacterSelectionScript cS;
    UnityWebRequest temp;

    

    void Start()
    {
        cS = GetComponent <CharacterSelectionScript> ();
    }
    public void GetNFT()
    {
        string acc = PlayerPrefs.GetString("Account");
        //StartCoroutine(GetRequest("https://api.cryptofightclub.io/game/sdk/"+acc));

        //testing link
        StartCoroutine(GetRequest("https://api.cryptofightclub.io/game/sdk/0xbecd7b5cfab483d65662769ad4fecf05be4d4d05"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    temp = webRequest;
                    Display();
                    break;
            }
        }
    }

    void Display()
    {
        string data = "{\"Items\":" + temp.downloadHandler.text + "}";

        NFTInfo[] NFTData = JsonHelper.FromJson<NFTInfo>(data);
        cS.SetData(NFTData);
     
    }

    //temp Fuction for skip
    public void Skip()
    {
        cS.Skip();
    }
}

