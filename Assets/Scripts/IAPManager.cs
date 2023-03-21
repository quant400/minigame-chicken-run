using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing;
using UnityEngine.UI;
public struct paymentObject
{
    public string id;
    public string amount;
    public string name;
    public payload payload;

}
struct Temp1
{
    public string Payload;
}
struct temp2
{
    public string json;
}
[System.Serializable]
public struct payload
{
    public string orderId;
    public string packageName;
    public string productId;
    public int purchaseTime;
    public int purchaseState;
    public string purchaseToken;
}
struct responce
{
    public string status;
}
public class IAPManager : MonoBehaviour//, IStoreListener
{
    public TMPro.TMP_Text tempText;
    [SerializeField]
    PurchaseProcessingPanel purchasePanel;
    [SerializeField]
    Image currentplayer;

    private void OnEnable()
    {
        UpdatePlayer(gameplayView.instance.chosenNFT.name);
    }
    public void PurchaseCompleted(Product p)
    {
        Debug.Log("Unity_IAP:"+p.receipt);
        Debug.Log("success");
        purchasePanel.gameObject.SetActive(true);
        purchasePanel.SetProcessing();
        string ID = gameplayView.instance.chosenNFT.id;
        string n = "android-chicken-run";
        string am="0";
        Debug.Log(p.definition.storeSpecificId);
        switch(p.definition.id)
        {
            case "1kjuice":
                am = "1000";
                break;

            case "5kjuice":
                am = "5000";
                break;

            case "10kjuice":
                am = "10000";
                break;
        }
       
        Temp1 a= JsonUtility.FromJson<Temp1>(p.receipt);
        temp2 b= JsonUtility.FromJson<temp2>(a.Payload);
        payload payL = JsonUtility.FromJson<payload>(b.json);
        paymentObject pay = new paymentObject
        {
            id = ID,
            name = n,
            amount = am,
            payload = payL
        };
        StartCoroutine(PaymentRequest(pay));
    }
    public void PurchaseFailed()
    {
        purchasePanel.gameObject.SetActive(true);
        purchasePanel.Failed("Payment failed");
        Debug.Log("Fail");
    }


    public IEnumerator PaymentRequest(paymentObject p)
    {
        string uri = "";
        if (KeyMaker.instance.buildType == BuildType.staging)
            uri = "https://staging-api.cryptofightclub.io/game/sdk/payment";
        else if (KeyMaker.instance.buildType == BuildType.production)
            uri = "https://api.cryptofightclub.io/game/sdk/payment";

        using (UnityWebRequest request = UnityWebRequest.Put(uri, JsonUtility.ToJson(p)))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(p));
            //request.method = "POST";
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.error == null)
            {
                string result = Encoding.UTF8.GetString(request.downloadHandler.data);
                responce r = JsonUtility.FromJson<responce>(request.downloadHandler.text);
                if (KeyMaker.instance.buildType == BuildType.staging)
                    Debug.Log(request.downloadHandler.text);
                if (r.status == "true")
                    DatabaseManagerRestApi._instance.getJuiceFromRestApi(gameplayView.instance.chosenNFT.id);
                purchasePanel.PurchseSuccessful();

            }
            else
            {
                PurchaseFailed();
            }
        }
    }
    public void UpdatePlayer(string name)
    {
        Sprite sprite = Resources.Load(Path.Combine("SinglePlayerPrefabs/DisplaySprites/Display", NameToSlugConvert(name)), typeof(Sprite)) as Sprite;
        currentplayer.sprite = sprite;
    }

    string NameToSlugConvert(string name)
    {
        string slug;
        slug = name.ToLower().Replace(".", "").Replace("'", "").Replace(" ", "-");
        return slug;

    }
}
