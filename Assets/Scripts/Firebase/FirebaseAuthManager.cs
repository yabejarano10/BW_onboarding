using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;
using Google;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class FirebaseAuthManager : Singleton<FirebaseAuthManager>
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Header("Login")]
    public TMP_InputField email;
    public TMP_InputField password;

    [Space]
    [Header("Sign Up")]
    public TMP_InputField nameSignUp;
    public TMP_InputField emailSignUp;
    public TMP_InputField passwordSignUp;

    [Space]
    [Header("Google Sign Up")]
    public string webClientId;


    GoogleSignInConfiguration configuration;


    protected override void Awake()
    {
        base.Awake();
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if(dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not load all dependencies: " + dependencyStatus);
            }
        });

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    void AuthStateChanged(object sender, System.EventArgs eventargs)
    {
        if(auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if(!signedIn && user != null)
            {
                Debug.Log("Signed out");
            }
            if(signedIn)
            {
                Debug.Log("Signed in");
            }
        }
    }
    public void LogOut()
    {
        auth.SignOut();
        GoogleSignIn.DefaultInstance.SignOut();

        SceneManager.LoadScene("Main");
    }

    public void Login()
    {
        email = email != null ?  email : GameObject.Find("LoginEmail").GetComponent<TMP_InputField>();
        password = password != null ? password : GameObject.Find("LoginPassword").GetComponent<TMP_InputField>();
        StartCoroutine(LoginAsync(email.text, password.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if(loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string errorMessage = "Login failed: ";
            switch(authError)
            {
                case AuthError.InvalidEmail:
                    errorMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    errorMessage += "Password is incorrect";
                    break;
                case AuthError.MissingEmail:
                    errorMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    errorMessage += "Password is missing";
                    break;
                default:
                    errorMessage = "Login failed";
                    break;

            }

            ShowToast(errorMessage);
        }
        else
        {
            Debug.Log("LOG IN");
            ShowToast("Login Succesfull");
            user = loginTask.Result.User;
            SceneManager.LoadScene("LogedIn");

        }
    }

    public void Register()
    {
        nameSignUp = nameSignUp != null ? nameSignUp : GameObject.Find("name").GetComponent<TMP_InputField>();
        emailSignUp = emailSignUp != null ? emailSignUp : GameObject.Find("emailSignUp").GetComponent<TMP_InputField>();
        passwordSignUp = passwordSignUp != null ? passwordSignUp : GameObject.Find("passSignUp").GetComponent<TMP_InputField>();
        StartCoroutine(RegisterAsync(nameSignUp.text, emailSignUp.text, passwordSignUp.text));
    }

    public IEnumerator RegisterAsync(string name, string email, string password)
    {
        if (name == "")
        {
            Debug.LogError("Unsername is empty");
            ShowToast("Unsername is empty");
        }
        else if(email == "")
        {
            Debug.Log("email is empty");
            ShowToast("email is empty");
        }
        else
        {
            var signupTask= auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => signupTask.IsCompleted);

                if (signupTask.Exception != null)
                {
                    Debug.LogError(signupTask.Exception);

                    FirebaseException firebaseException = signupTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string errorMessage = "Registration failed: ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            errorMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            errorMessage += "Password is incorrect";
                            break;
                        case AuthError.MissingEmail:
                            errorMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            errorMessage += "Password is missing";
                            break;
                        default:
                            errorMessage = "Login failed";
                            break;

                    }
                    Debug.LogError(errorMessage);
                    ShowToast(errorMessage);
                }
                else
                {
                    ShowToast("Registered Successfully");  
                    user = signupTask.Result.User;
                    UserProfile userProfile = new UserProfile { DisplayName = name };
                    var updatedProfileTask = user.UpdateUserProfileAsync(userProfile);
                    yield return new WaitUntil(() => updatedProfileTask.IsCompleted);

                    if (updatedProfileTask.Exception != null)
                    {
                        user.DeleteAsync();
                        Debug.LogError(signupTask.Exception);

                        FirebaseException firebaseException = updatedProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError authError = (AuthError)firebaseException.ErrorCode;

                        string errorMessage = "profile Update failed: ";
                        switch (authError)
                        {
                            case AuthError.InvalidEmail:
                                errorMessage += "Email is invalid";
                                break;
                            case AuthError.WrongPassword:
                                errorMessage += "Password is incorrect";
                                break;
                            case AuthError.MissingEmail:
                                errorMessage += "Email is missing";
                                break;
                            case AuthError.MissingPassword:
                                errorMessage += "Password is missing";
                                break;
                            default:
                                errorMessage = "Login failed";
                                break;

                        }
                        ShowToast(errorMessage);
                    }
                    SceneManager.LoadScene("LogedIn");

            }

            yield return null;

        }
    }

    public void GoogleLogIn()
    {
        
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }
    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        try
        {

            if (task.IsFaulted)
            {
                using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                        ShowToast(error.Message);
                    }
                }
            }
            else
            {
                SignInWithGoogleOnFirebase(task.Result.IdToken);
            }
        }
        catch(Exception ex)
        {
            ShowToast(ex.Message);
        }
    }
    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        try
        {

            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    ShowToast("SignInWithCredentialAsync was canceled.");
                }
                AggregateException ex = task.Exception;
                if (ex != null)
                {
                    ShowToast(ex.Message);
                }
                else
                {
                    user = task.Result;
                    UnityMainThreadDispatcher.Instance().EnqueueAction(() =>
                    {
                        ShowToast("Log in Succesfull");
                        SceneManager.LoadScene("LogedIn");
                    });
                }
            });


        }
        catch(Exception ex)
        {
            ShowToast(ex.Message);
        }
    }

    public void ShowToast(string message)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");

            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
        else
        {
            Debug.LogWarning("Toasts are only supported on Android.");
        }
    }
}
