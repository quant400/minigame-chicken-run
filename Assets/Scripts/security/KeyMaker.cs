using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public struct ConnectObj
{
    public string address;
    public string r;
    public string g;
    public string b;
}
public struct ResponseObject
{
    public NFTInfo[] nfts;
    public string code;
}
public class KeyMaker : MonoBehaviour
{
    public static KeyMaker instance;

    String[] masterKeys = {
    "18fb4964-5b5e-48a5-9057-4e9ca70f730c",
    "407d0378-91a3-4756-99d4-8faf861004f2",
    "3ce01623-bbb8-43a5-922b-9512044eb094",
    "d85626f5-cb88-47db-865d-01d8303cea08",
    "5cd3c30a-a7c4-4d57-b5a3-fd21c95b31ec",
    "ff1de2eb-c3a8-4206-bd6e-d2e4abe46299",
    "1a6f79fb-f24c-41b2-b838-02ea97652de4",
    "7a145394-ad54-4317-826d-ce049a5f7ff3",
    "ccd5e5ef-110b-47a7-8acf-6c6e9fefd5fe",
    "f8cf0175-bc47-4602-8b3e-6438227201fa"
    };

    string currentAddress;
    int currentSequence;
    string currentCode;
    ConnectObj currentConnectObj;
    //connect variables 
    string connectR;
    string connectG;
    string connectB;


    // end variables
    string endR;
    string endG;
    string endB;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        DontDestroyOnLoad(this);

        //StartCoroutine(GetRequest("https://staging-api.cryptofightclub.io/game/sdk/connect"));

    }

    public void SetCode(string code)
    {
        currentCode = code;
    }
    public string GetXSeqConnect(string addr, int seq)
    {
        currentAddress = addr;
        currentSequence = seq;

        string tmst = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        connectR = SHA256Cal(tmst + masterKeys[4] + currentAddress);
        connectG = SHA256Cal(connectR + currentSequence.ToString() + masterKeys[9] + currentAddress);
        connectB = SHA256Cal(connectG + connectR + masterKeys[3] + currentAddress);

        string xSeq = SHA256Cal(connectB + connectR + currentAddress + masterKeys[6]);

        currentConnectObj = new ConnectObj();
        currentConnectObj.address = currentAddress;
        currentConnectObj.r = connectR;
        currentConnectObj.g = connectG;
        currentConnectObj.b = connectB;

        return xSeq;
    }

    public string GetGameEndKey(int score , int nftID)
    {
        string tmst = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        endR = SHA256Cal(currentCode + tmst + masterKeys[0] + score.ToString());
        endG = SHA256Cal(endR + masterKeys[1] + currentSequence + score.ToString());
        endB = SHA256Cal(endG + nftID.ToString() + masterKeys[2] + currentAddress + score.ToString());

        int masterKeyNum = currentSequence + 2;

        string xSeq = SHA256Cal(endB + score.ToString() + masterKeys[masterKeyNum]);


        return xSeq;
    }


    string SHA256Cal(string data)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

            // Convert byte array to a string   
            string encodedText = Convert.ToBase64String(bytes);
            return encodedText.ToString();
            /* StringBuilder builder = new StringBuilder();
             for (int i = 0; i < bytes.Length; i++)
             {
                 builder.Append(bytes[i].ToString("x2"));
             }
             return builder.ToString();*/
        }

    }



    #region Requests
    public IEnumerator GetRequest(string uri)
    {
        int sequence = UnityEngine.Random.Range(1, 8);
        string xseq = GetXSeqConnect(PlayerPrefs.GetString("Account"), sequence);
        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri,JsonUtility.ToJson(currentConnectObj)))
        {
            webRequest.SetRequestHeader("sequence", sequence.ToString());
            webRequest.SetRequestHeader("timestamp", DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
            webRequest.SetRequestHeader("xsequence", xseq);
            webRequest.SetRequestHeader("Content-Type", "application/json");
           
            //webRequest.uploadHandler = new UploadHandlerRaw((System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(currenConnectObj))));
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("no connection");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    //temp = webRequest;
                    //Display();
                    ResponseObject temp = JsonUtility.FromJson<ResponseObject>(webRequest.downloadHandler.text);
                    gameplayView.instance.GetComponent<NFTGetView>().Display(temp.nfts);
                    break;
            }
        }
    }
    #endregion Requests
}
