using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;

public class FirebaseAuthManager : MonoBehaviour
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

    private void Awake()
    {
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

    public void Login()
    {
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
            user = loginTask.Result.User;

        }
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(name, emailSignUp.text, passwordSignUp.text));
    }

    public IEnumerator RegisterAsync(string name, string email, string password)
    {
        ShowToast("START");
        if (name == "")
        {
            Debug.LogError("Unsername is empty");
        }
        else if(email == "")
        {
            Debug.Log("email is empty");
        }
        else
        {
            ShowToast("GO REGISTER");
            var signupTask= auth.CreateUserWithEmailAndPasswordAsync(email, password);
            //yield return new WaitUntil(() => signupTask.IsCompleted);

            signupTask.ContinueWith(task => {

                ShowToast("END REGISTER");
                if (task.IsFaulted || task.IsCanceled)
                {
                    ShowToast("FULL ERROR REGISTER");
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
                    ShowToast("ALL GOOD");
                    user = signupTask.Result.User;
                    UserProfile userProfile = new UserProfile { DisplayName = name };
                    var updatedProfileTask = user.UpdateUserProfileAsync(userProfile);
                  //  yield return new WaitUntil(() => updatedProfileTask.IsCompleted);

                    ShowToast("PROFILEEE");
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

                }

            });
            yield return null;

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
