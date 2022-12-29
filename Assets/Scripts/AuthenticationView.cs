
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using Google;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class AuthenticationView : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    //Login variables
    [Header("Login")]
    public Transform SignInPanel;
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

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
    GameObject currentOpenWindiow;
    [SerializeField]
    TMP_Text infoDisplay;

    //for Google
    public string GoogleWebAPI;
    private GoogleSignInConfiguration configuration;

    void Awake()
    {
        //for Google
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
                Debug.Log("Signed in " + User.UserId);
                Debug.Log(User.Email);
                Debug.Log(0);
                SignedIn(User.Email);
            }
        }
    }

    void OnDisable()
    {
        auth.StateChanged -= AuthStateChanged;
    }


    #region email/pass
    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text));
    }

    public void ResetPasswordButton()
    {
        StartCoroutine(ResetPassword(emailPasswordReset.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        if(!IsValidEmail(_email))
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
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
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
        else if (!accpetedTos)
        {
            warningRegisterText.text = "please read and accept the terms of servive and privacy policy".ToUpper();
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
    private IEnumerator ResetPassword(string _email)
    {
        if (!IsValidEmail(_email))
        {
           warningEmailReset.text = "Please enter a valid email!".ToUpper();
        }
        var ResetTask = auth.SendPasswordResetEmailAsync(_email);
        yield return new WaitUntil(predicate: () => ResetTask.IsCompleted);

        if (ResetTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {ResetTask.Exception}");
            FirebaseException firebaseEx = ResetTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = errorCode.ToString();
            warningEmailReset.text = message.ToUpper();
            warningEmailReset.color = Color.red;
        }
        else
        {
            warningEmailReset.text = "Password reset email sent".ToUpper();
            warningEmailReset.color = Color.green;
        }
    }
    #endregion email/pass

    #region Google
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
                Debug.Log(User.Email);
                Debug.Log(User.DisplayName);
                infoDisplay.text = User.Email.ToUpper() + "\n\n" + User.DisplayName.ToUpper();
                infoDisplay.color = Color.green;
                infoDisplay.gameObject.SetActive(true);

                //load game into skip
                SignedIn(User.Email);

            });
        }
    }
    #endregion Google*/


  /*  #region utility
    void SignedIn(string info)
    {
        infoDisplay.text = info.ToUpper();
        infoDisplay.color = Color.green;
        currentOpenWindiow.SetActive(false);
        currentOpenWindiow = methodSelect;
        PlayerPrefs.SetString("Account", "0xD408B954A1Ec6c53BE4E181368F1A54ca434d2f3");
        gameplayView.instance.isTryout = false;
        //GetComponentInParent<NFTGetView>().Skip();
        GetComponentInParent<NFTGetView>().Display(new NFTInfo[0]);
    }

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
        }
    }

    public void SignOut()
    {
        auth.SignOut();
        infoDisplay.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
        warningRegisterText.text = "";
        emailLoginField.text = "";
        passwordLoginField.text = "";
        warningLoginText.text = "";
        GetComponentInParent<uiView>().goToMenu("login");
    }
    bool IsValidEmail(string email)
    {
        Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);

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
    #endregion utility 
}*/
