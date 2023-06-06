#if UNITY_WEBGL
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using FirebaseWebGL.Examples.Utils;
#endif
#if UNITY_ANDROID || UNITY_IOS
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections;
#if UNITY_IOS
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;
#endif
#endif
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ChickenRun
{
    public struct LoginObject
    {
        public string uid;
        public string email;
        public string emailVerified;
        public string displayName;

    }

    public class FireBaseWebGLAuth : MonoBehaviour
    {
#if UNITY_ANDROID || UNITY_IOS
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    //for google
    [SerializeField]
    string GoogleWebAPI;
    private GoogleSignInConfiguration configuration;
#endif

        [Header("Login")]
        public Transform SignInPanel;
        public TMP_InputField emailLoginField;
        public TMP_InputField passwordLoginField;
        public TMP_Text warningLoginText;

        //Register variables
        [Header("Register")]
        public Transform registerPanel;
        public TMP_InputField emailRegisterField;
        public TMP_InputField passwordRegisterField;
        public TMP_InputField passwordRegisterVerifyField;
        public TMP_Text warningRegisterText;
        public bool accpetedTos;

        [Header("PasswordReset")]
        public Transform passwordResetPanel;
        public TMP_InputField emailPasswordReset;
        public TMP_Text warningEmailReset;

        [Header("Others")]
        [SerializeField]
        GameObject methodSelect;
        [SerializeField]
        GameObject BackgroundBlur;
        GameObject currentOpenWindiow;
        [SerializeField]
        TMP_Text InfoDisplay;
        [SerializeField]
        GameObject loginButton;
        [SerializeField]
        GameObject deleteButton;
        [SerializeField]
        GameObject DeleteCanvas;

#if UNITY_ANDROID || UNITY_IOS
    void Awake()
    {
        //for Google
        GoogleWebAPI = "335488712723-2hqciv7mto3c7eic8vnpphla02ic9qeq.apps.googleusercontent.com";
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleWebAPI,
            RequestIdToken = true
        };

        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

        currentOpenWindiow = methodSelect;



    }
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        Invoke("Test", 1f);

    }
    void Test()
    {
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != User)
        {
            bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && User != null)
            {
                Debug.Log("Signed out " + User.UserId);
            }
            User = auth.CurrentUser;
            if (signedIn)
            {
                SignedIn(User.Email);
            }
        }
    }

    void OnDisable()
    {
        auth.StateChanged -= AuthStateChanged;
    }

#endif
#if UNITY_WEBGL
        private void Start()
        {
            currentOpenWindiow = methodSelect;
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                Debug.Log("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
                return;
            }

            FirebaseAuth.OnAuthStateChanged(gameObject.name, "DisplayUserInfo", "DisplayInfo");
        }
#endif

        public void OnSignInClick()
        {
            warningLoginText.text = "";
            if (emailLoginField.text == "" || !IsValidEmail(emailLoginField.text))
            {
                SignInPanel.DOShakePosition(1, 1);
                warningLoginText.text = "Please enter a valid email".ToUpper();
                warningLoginText.color = Color.red;
            }
            else
            {
#if UNITY_WEBGL
                SignInWithEmailAndPassword();
#endif
#if UNITY_ANDROID || UNITY_IOS
            StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
#endif

            }

        }

        public void OnRegisterClick()
        {
            warningRegisterText.text = "";
            if (emailRegisterField.text == "" || !IsValidEmail(emailRegisterField.text))
            {
                registerPanel.DOShakePosition(1, 1);
                warningRegisterText.text = "Please enter a valid email".ToUpper();
                warningRegisterText.color = Color.red;
            }
            else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
            {
                registerPanel.DOShakePosition(1, 1);
                warningRegisterText.text = "Password does not match".ToUpper();
                warningRegisterText.color = Color.red;
            }
            else if (!accpetedTos)
            {
                registerPanel.DOShakePosition(1, 1);
                warningRegisterText.text = "please read and accept the terms of service and privacy policy".ToUpper();
                warningRegisterText.color = Color.red;
            }
            else
            {
#if UNITY_WEBGL
                CreateUserWithEmailAndPassword();
#endif
#if UNITY_ANDROID || UNITY_IOS
            StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text));
#endif
            }
        }

#if UNITY_WEBGL
        public void SignInWithEmailAndPassword() =>
              FirebaseAuth.SignInWithEmailAndPassword(emailLoginField.text, passwordLoginField.text, gameObject.name, "SignedIn", "DisplayError");
        public void CreateUserWithEmailAndPassword() =>
            FirebaseAuth.CreateUserWithEmailAndPassword(emailRegisterField.text, passwordRegisterField.text, gameObject.name, "SignedIn", "DisplayError");
#endif
        public void SignInWithGoogle()
        {
#if UNITY_WEBGL
            FirebaseAuth.SignInWithGoogle(gameObject.name, "SignedIn", "DisplayError");
#endif
#if UNITY_ANDROID || UNITY_IOS
        GoogleSignInClick();
#endif
        }


        public void ResetPasswordEmail()
        {
#if UNITY_WEBGL
            FirebaseAuth.ResetPassword(emailPasswordReset.text, gameObject.name, "DisplayResetReply", "DisplayResetReply");
#endif
#if UNITY_ANDROID || UNITY_IOS
#endif
        }




#if UNITY_ANDROID || UNITY_IOS //Login functions 
    private IEnumerator Login(string _email, string _password)
    {
        if (!IsValidEmail(_email))
        {
            warningLoginText.text = "Please enter a valid email!".ToUpper();
        }
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message.ToUpper();
            warningRegisterText.color = Color.red;
        }
        else
        {
            warningLoginText.text = "";
            SignedIn(User.Email);
        }
    }

    private IEnumerator Register(string _email, string _password)
    {
        if (!IsValidEmail(_email))
        {
            warningRegisterText.text = "Please enter a valid email!".ToUpper();
        }
        if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!".ToUpper();
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message.ToUpper();
                warningRegisterText.color = Color.red;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile();

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!".ToUpper();
                        warningRegisterText.color = Color.red;
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        SignedIn(User.Email);
                        warningRegisterText.text = "";
                    }
                }
            }
        }
    }

    private void PasswordResetReset()
    {
        auth.SendPasswordResetEmailAsync(emailPasswordReset.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                warningEmailReset.text = "Password Reset Failed";
                return;
            }
            if (task.IsFaulted)
            {
                warningEmailReset.text = "Reset encountered an error: " + task.Exception;
                return;
            }

            warningEmailReset.text = "Password reset email sent successfully.";
        });

    }

    //Google stuff
    public void GoogleSignInClick()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthFinish);
    }

    void OnGoogleAuthFinish(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
            Debug.LogError("Fault" + task.Exception);
        else if (task.IsCanceled)
            Debug.Log("Login canceled");
        else
        {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
            auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(Task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SigninWithCredentials Was Cancled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SigninWithCredentials got error" + task.Exception);
                }
                User = auth.CurrentUser;

                //load game into skip
                gameplayView.instance.usingMeta = false;
                SignedIn(User.Email);

            });
        }
    }

    //for ios
#if UNITY_IOS
    private IAppleAuthManager appleAuthManager;

    void Start()
    {
        // If the current platform is supported
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            var deserializer = new PayloadDeserializer();
            appleAuthManager = new AppleAuthManager(deserializer);
        }
    }

    void Update()
    {
        if (appleAuthManager != null)
        {
           appleAuthManager.Update();
        }

    }
    private string GenerateRandomString(int length)
    {
        if (length <= 0)
        {
            throw new Exception("Expected nonce to have positive length");
        }

        const string charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvwxyz-._";
        var cryptographicallySecureRandomNumberGenerator = new RNGCryptoServiceProvider();
        var result = string.Empty;
        var remainingLength = length;

        var randomNumberHolder = new byte[1];
        while (remainingLength > 0)
        {
            var randomNumbers = new List<int>(16);
            for (var randomNumberCount = 0; randomNumberCount < 16; randomNumberCount++)
            {
                cryptographicallySecureRandomNumberGenerator.GetBytes(randomNumberHolder);
                randomNumbers.Add(randomNumberHolder[0]);
            }

            for (var randomNumberIndex = 0; randomNumberIndex < randomNumbers.Count; randomNumberIndex++)
            {
                if (remainingLength == 0)
                {
                    break;
                }

                var randomNumber = randomNumbers[randomNumberIndex];
                if (randomNumber < charset.Length)
                {
                    result += charset[randomNumber];
                    remainingLength--;
                }
            }
        }

        return result;
    }

    private string GenerateSHA256NonceFromRawNonce(string rawNonce)
    {
        var sha = new SHA256Managed();
        var utf8RawNonce = Encoding.UTF8.GetBytes(rawNonce);
        var hash = sha.ComputeHash(utf8RawNonce);

        var result = string.Empty;
        for (var i = 0; i < hash.Length; i++)
        {
            result += hash[i].ToString("x2");
        }

        return result;
    }
    public void PerformLoginWithAppleIdAndFirebase()
    {
        var rawNonce = GenerateRandomString(32);
        var nonce = GenerateSHA256NonceFromRawNonce(rawNonce);

        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName, nonce);

        appleAuthManager.LoginWithAppleId(loginArgs, credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    PerformFirebaseAuthentication(appleIdCredential, rawNonce);
                }
            },
            error =>
            {
                Debug.Log(error.ToString());
            });
    }
    public void PerformQuickLoginWithFirebase(Action<FirebaseUser> firebaseAuthCallback)
    {
        var rawNonce = GenerateRandomString(32);
        var nonce = GenerateSHA256NonceFromRawNonce(rawNonce);

        var quickLoginArgs = new AppleAuthQuickLoginArgs(nonce);

        appleAuthManager.QuickLogin(
            quickLoginArgs,
            credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    PerformFirebaseAuthentication(appleIdCredential, rawNonce);
                }
            },
            error =>
            {
                Debug.Log(error.ToString());
        });
    }

    private void PerformFirebaseAuthentication(IAppleIDCredential appleIdCredential, string rawNonce)
    {
        var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);
        var authorizationCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode);
        Credential credential =
        OAuthProvider.GetCredential("apple.com", identityToken, rawNonce, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            SignedIn(newUser.Email);
        });
    }
#endif
#endif

        void DisplayInfo(string info)
        {
            Debug.Log(info);
        }

        void DisplayResetReply(string info)
        {
            warningEmailReset.text = info.ToUpper();
        }
        void DisplayUserInfo(string info)
        {
            if (info != "")
            {
                FirebaseUser pl = JsonUtility.FromJson<FirebaseUser>(info);
#if UNITY_WEBGL
                gameplayView.instance.logedPlayer = (pl.email.ToLower(), pl.uid.ToLower());
                gameplayView.instance.usingMeta = false;
                DatabaseManagerRestApi._instance.getJuiceFromRestApi(pl.email);
                SignedIn("Signed in as ".ToUpper() + pl.email.ToUpper() + "\n\n" + pl.providerData);
#endif

            }

        }
        void SignedIn(string info)
        {
#if UNITY_ANDROID || UNITY_IOS
            gameplayView.instance.logedPlayer = (User.Email.ToLower(), User.UserId.ToLower());
            DatabaseManagerRestApi._instance.getJuiceFromRestApi(User.Email);
#endif
#if UNITY_WEBGL
            deleteButton.SetActive(false);
#endif
            Close();

            loginButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
            InfoDisplay.text = info.ToUpper();
            currentOpenWindiow.SetActive(false);
            currentOpenWindiow = methodSelect;
            //PlayerPrefs.SetString("Account", "0xD408B954A1Ec6c53BE4E181368F1A54ca434d2f3");
            gameplayView.instance.isTryout = false;
            //change what loads when mint nft added and stuff linked
            StartCoroutine(KeyMaker.instance.GetRequest());

        }

        public void LogOut()
        {
#if UNITY_WEBGL
            FirebaseAuth.SignOut();
#endif
#if UNITY_ANDROID || UNITY_IOS
        auth.SignOut();
#endif
            GetComponentInParent<uiView>().goToMenu("login");
            loginButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
            chickenGameModel.userIsLogged.Value = false;
            chickenGameModel.currentNFTArray = null;
            gameplayView.instance.usingFreemint = false;
            gameplayView.instance.usingMeta = false;
            gameplayView.instance.isTryout = false;
            gameplayView.instance.usingOtherChainNft = false;
            gameplayView.instance.hasOtherChainNft = false;
            InfoDisplay.text = "";
            emailRegisterField.text = "";
            passwordRegisterField.text = "";
            passwordRegisterVerifyField.text = "";
            warningRegisterText.text = "";
            emailLoginField.text = "";
            passwordLoginField.text = "";
            warningLoginText.text = "";
            warningEmailReset.text = "";
        }

        void DisplayError(string error)
        {
#if UNITY_WEBGL
            var parsedError = StringSerializationAPI.Deserialize(typeof(FirebaseError), error) as FirebaseError;

            if (currentOpenWindiow.name == "Login")
            {

                warningLoginText.text = parsedError.message.ToUpper();
            }
            else if (currentOpenWindiow.name == "Register")
            {

                warningRegisterText.text = parsedError.message.ToUpper();
            }
            else
                Debug.Log(parsedError.message);
#endif
        }


        #region utility

        public void OpenSingin()
        {
            if (currentOpenWindiow == null)
            {
                currentOpenWindiow = methodSelect;
            }
            emailRegisterField.text = "";
            passwordRegisterField.text = "";
            passwordRegisterVerifyField.text = "";
            warningRegisterText.text = "";
            currentOpenWindiow.SetActive(false);
            currentOpenWindiow = SignInPanel.gameObject;
            SignInPanel.gameObject.SetActive(true);
            BackgroundBlur.SetActive(true);
        }

        public void OpenRegister()
        {
            if (currentOpenWindiow == null)
            {
                currentOpenWindiow = methodSelect;
            }
            emailLoginField.text = "";
            passwordLoginField.text = "";
            warningLoginText.text = "";
            currentOpenWindiow.SetActive(false);
            currentOpenWindiow = registerPanel.gameObject;
            registerPanel.gameObject.SetActive(true);
            BackgroundBlur.SetActive(true);
        }

        public void OpenPasswordReset()
        {
            if (currentOpenWindiow == null)
            {
                currentOpenWindiow = methodSelect;
            }
            emailRegisterField.text = "";
            passwordRegisterField.text = "";
            passwordRegisterVerifyField.text = "";
            warningRegisterText.text = "";
            currentOpenWindiow.SetActive(false);
            currentOpenWindiow = passwordResetPanel.gameObject;
            passwordResetPanel.gameObject.SetActive(true);
            BackgroundBlur.SetActive(true);
        }
        public void OpenDelete()
        {
            if (currentOpenWindiow == null)
            {
                currentOpenWindiow = methodSelect;
            }
            emailRegisterField.text = "";
            passwordRegisterField.text = "";
            passwordRegisterVerifyField.text = "";
            warningRegisterText.text = "";
            currentOpenWindiow.SetActive(false);
            currentOpenWindiow = DeleteCanvas;
            DeleteCanvas.gameObject.SetActive(true);
            BackgroundBlur.SetActive(true);
        }

        public void DeleteUser()
        {
#if UNITY_ANDROID || UNITY_IOS
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            user.DeleteAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("DeleteAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                    return;
                }
                
                Debug.Log("User deleted successfully.");
               

            });
            Close();
            chickenGameModel.userIsLogged.Value = false;
            chickenGameModel.gameCurrentStep.Value = chickenGameModel.GameSteps.OnLogin;
            loginButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
#endif
        }
        public void OpenMethodSelect()
        {
            methodSelect.SetActive(true);
            BackgroundBlur.SetActive(true);
        }
        public void Close()
        {

            if (currentOpenWindiow != null)
            {
                currentOpenWindiow.SetActive(false);
                currentOpenWindiow = methodSelect;
                emailRegisterField.text = "";
                passwordRegisterField.text = "";
                passwordRegisterVerifyField.text = "";
                warningRegisterText.text = "";
                emailLoginField.text = "";
                passwordLoginField.text = "";
                warningLoginText.text = "";
                emailPasswordReset.text = "";
                warningEmailReset.text = "";
            }
            BackgroundBlur.SetActive(false);
        }
        bool IsValidEmail(string email)
        {
            Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]+$", RegexOptions.IgnoreCase);

            return emailRegex.IsMatch(email);
        }


        public void ToggleTos(bool val)
        {
            accpetedTos = val;
        }
        public void LoadTos()
        {
            Application.OpenURL("https://www.cryptofightclub.io/terms-of-service");
        }
        public void LoadPrivacy()
        {
            Application.OpenURL("https://www.cryptofightclub.io/privacy-policy");
        }

        public void Skip()
        {
            Debug.Log("skip clicked");
            //for email login
            //gameplayView.instance.logedPlayer = ("test@test.com".ToLower(), "5uU1JCypYMT3EGWTzK3I2EhHqpC3".ToLower());
            //gameplayView.instance.logedPlayer = ("hassan.iqbal@quids.tech".ToLower(), "0tuICf75vGOsrhtbpYWaLKeTugg2".ToLower());

            //for meta login
            gameplayView.instance.usingMeta = true;
            PlayerPrefs.SetString("Account", "0xD408B954A1Ec6c53BE4E181368F1A54ca434d2f3");


            StartCoroutine(KeyMaker.instance.GetRequest());
        }
        #endregion utility

    }

}