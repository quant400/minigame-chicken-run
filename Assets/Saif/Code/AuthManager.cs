using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Tamarin.Common;
using Tamarin.FirebaseX;

public class AuthManager : MonoBehaviour
{
    //This firebase user is not the same as the native api!
    public static AuthManager _instance;
    public FirebaseUser user;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        // SignInAnonymously();
    }

    private void Update() {
        
    }

    //Call this from code, do not reference async functions from the UI thread. 
    async void SignInAnonymously()
    {
        //Is best to always wait for the api to be ready, if the time of the execution cannot be controlled. 
        await Waiter.Until(() => FirebaseAPI.Instance.ready == true);

        //For different auth methods. and references please visit the docs. https://twistedtamarin.com/docs/firebase
        user = await FirebaseAPI.Instance.auth.SignInWithAnon();
        if (user == null || !user.Status)
        {
            Debug.LogError("SignInAnonymouslyAsync was canceled.");
            return;
        }
        Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
        PlayerPrefs.SetString("UserID",user.UserId);
    }

    public FirebaseUser getcurrentUser()
    {
       return user;
    }

}
