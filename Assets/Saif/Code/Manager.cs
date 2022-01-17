using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tamarin.Common;
using Tamarin.FirebaseX;

//for managers is better to use singletons, if you are not referencing anything manually from the editor (one should never reference stuff manually btw)!
//The tamarin Singleton class also handles doNotDestroy, and can be referenced as a normal monobehaviour!  
public class Manager : Singleton<Manager>
{
    public FirebaseAPI firebase;

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        Manager.Instance.firebase = FirebaseAPI.Instance;
        Debug.Log("AUTORUN");
    }

    async void Start()
    {
        await Waiter.Until(() => firebase.ready == true);
        //you can add listeners, or anything else which requires the firebase api to be ready beforehand;
    }
}
