using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if PLAYGROUND_DISABLED // COMMENT THIS AFTER INSTALL
//#if !PLAYGROUND_DISABLED // UNCOMMENT THIS AFTER INSTALL

using Newtonsoft.Json;
using Firebase.Firestore;

using Tamarin.Common;
using Tamarin.FirebaseX;
public class FirebaseExamples : Singleton<FirebaseExamples>
{
    Text result;
    Button button;
    FirebaseAPI firebase;

    async void Start()
    {
        button = transform.Find("TEST").GetComponent<Button>();
        result = transform.Find("RESULT/Viewport/Content/Text").GetComponent<Text>();
        result.text = "";

        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);
        firebase = FirebaseAPI.Instance;

        firebase.auth.AuthChanged((user) =>
        {
            result.text += $"\n\n AUTH LISTENER: \n {JsonConvert.SerializeObject(user)}";
        });

        button.onClick.AddListener(async () =>
        {
            await TestSignIn();
            await TestFirestore();
            await TestRealtime();
            await TestStorage();
            await TestConfig();
            await TestFunctions();

            TestAnalytics();
            //TestMessaging();
        });
    }

    async Task<bool> TestSignIn()
    {
        var user = await firebase.auth.SignInWithAnon();
        result.text += $"\n\n ANON AUTH: \n {JsonConvert.SerializeObject(user)}";
        return true;
    }

    async Task<bool> TestFirestore()
    {
        firebase.firestore.QueryListenAs<FirestoreModel>("test", "testId", (res) =>
        {
            result.text += $"\n\n FIRESTORE LISTENER: \n {JsonConvert.SerializeObject(res)}";
        });

        var data = new FirestoreModel() { test = "test" };
        await firebase.firestore.SetAsync("test", "testId", data);
        await firebase.firestore.UpdateAsync("test", "testId", new Dictionary<string, object> { { "test", "test updated" } });

        var res = await firebase.firestore.QueryAs<FirestoreModel>("test", "testId");
        result.text += $"\n\n FIRESTORE: \n {JsonConvert.SerializeObject(res)}";
        return true;
    }

    async Task<bool> TestRealtime()
    {
        firebase.database.QueryListenAsync<RealtimeModel>("test", (res) =>
        {
            result.text += $"\n\n REALTIME LISTENER: \n {JsonConvert.SerializeObject(res)}";
        });

        var data = new RealtimeModel() { test = "test" };
        await firebase.database.SetAsync("test", new Dictionary<string, object> { { "test", "set without raw" } });
        await firebase.database.SetRawAsync("test", data);
        await firebase.database.UpdateAsync("test", new Dictionary<string, object> { { "test", "test updated" } });

        var res = await firebase.database.QueryAsync<RealtimeModel>("test");
        result.text += $"\n\n REALTIME: \n {JsonConvert.SerializeObject(res)}";
        return true;
    }

    async Task<bool> TestStorage()
    {
        var buffer = await TestImage();
        var upload = await firebase.storage.Upload("/test/test.jpg", buffer, "image/jpeg");
        result.text += $"\n\n STORAGE UPLOADED: \n {upload}";

        var url = await firebase.storage.DownloadUrl("/test/test.jpg");
        result.text += $"\n\n STORAGE DOWNLOAD URL: \n {url}";

        //var upload2 = await firebase.storage.Upload("gs://***.appspot.com", "/test/test.jpg", buffer, "image/jpeg");
        //result.text += $"\n\n STORAGE UPLOADED: \n {upload}";

        //var url2 = await firebase.storage.DownloadUrl("gs://***.appspot.com", "/test/test.jpg");
        //result.text += $"\n\n STORAGE DOWNLOAD URL: \n {url}";

        return true;
    }

    async Task<bool> TestFunctions()
    {
        var res = await firebase.functions.HttpsCall<Dictionary<string, object>>("fooBar", new Dictionary<string, object> { { "foo", "bar" } });
        result.text += $"\n\n FUNCTIONS: \n {res}";

        return true;
    }

    public bool TestAnalytics()
    {
        firebase.analytics.Setup(true, 300);
        firebase.analytics.LogEvent("TEST_EVENT");
        result.text += $"\n\n ANALYTICS SENT";

        return true;
    }

/*
    public bool TestMessaging()
    {
        firebase.messaging.Setup((token) =>
        {
            result.text += $"\n\n MESSAGING: \n {token}";
        });
        firebase.messaging.Subscribe("xxx");
        firebase.messaging.OnReceived((message) =>
        {
            result.text += $"\n\n MESSAGING ONRECEIVED: \n {JsonConvert.SerializeObject(message)}";
        });

        return true;
    }
*/

    async Task<bool> TestConfig()
    {
        var conf = new Dictionary<string, object> { { "TEST", "VALUE" } };
        await firebase.config.Setup(conf, 3600000);
        await firebase.config.FetchActivate();
        var res = await firebase.config.GetValue("TEST");
        result.text += $"\n\n REMOTE CONFIG: \n {res}";

        return true;
    }

    public async Task<byte[]> TestImage()
    {
        await new WaitForEndOfFrame();

        Texture2D tex = new Texture2D(256, 256, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToJPG();
        UnityEngine.Object.Destroy(tex);
        return bytes;
    }

}

[FirestoreData]
public class FirestoreModel
{
    public string docId { get; set; }
    [FirestoreProperty] public object test { get; set; }
}

public class RealtimeModel
{
    public string test { get; set; }
}
#endif