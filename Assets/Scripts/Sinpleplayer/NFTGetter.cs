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
    [SerializeField]
    GameObject noNFTCanvas;
    

    void Start()
    {
        cS = GetComponent <CharacterSelectionScript> ();
    }
    public void GetNFT()
    {
        Debug.LogWarningFormat("Change this before final build and also rename youngin, sledghammer and long shot");
        string acc = PlayerPrefs.GetString("Account");
        StartCoroutine(GetRequest("https://api.cryptofightclub.io/game/sdk/"+acc));

        //testing link
        //StartCoroutine(GetRequest("https://api.cryptofightclub.io/game/sdk/0xbecd7b5cfab483d65662769ad4fecf05be4d4d05"));
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
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    temp = webRequest;
                    Display();
                    break;
            }
        }
    }

    void Display()
    {
        string data = "{\"Items\":" + temp.downloadHandler.text + "}";

        NFTInfo[] NFTData = JsonUtilNFT.fromJson<NFTInfo[]>(data);
        if(NFTData.Length==0)
        {
            noNFTCanvas.SetActive(true);
        }
        else
        {
            noNFTCanvas.SetActive(false);
            cS.SetData(NFTData);
        }
        
     
    }
    public static class JsonUtilNFT
    {

        /// <summary> Converts an object to a Json string </summary>.
        /// <param name="obj">object </param>
        public static string toJson<T>(T obj)
        {
            if (obj == null) return "null";

            if (typeof(T).GetInterface("IList") != null)
            {
                Pack<T> pack = new Pack<T>();
                pack.data = obj;
                string json = JsonUtility.ToJson(pack);
                return json.Substring(8, json.Length - 9);
            }

            return JsonUtility.ToJson(obj);
        }

        /// < summary > parse Json </summary >
        /// <typeparam name="T">type</typeparam>
        /// <param name="json">Json string </param>
        public static T fromJson<T>(string json)
        {
            if (json == "null" && typeof(T).IsClass) return default(T);

            if (typeof(T).GetInterface("IList") != null)
            {
                json = "{\"data\":{data}}".Replace("{data}", json);
                Pack<T> Pack = JsonUtility.FromJson<Pack<T>>(json);
                return Pack.data;
            }

            return JsonUtility.FromJson<T>(json);
        }

        /// < summary > inner packaging class </summary >
        private class Pack<T>
        {
            public T data;
        }

    }
    //temp Fuction for skip
    public void Skip()
    {
        cS.Skip();
    }
}

