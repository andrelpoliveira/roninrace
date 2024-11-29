using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using System.Threading.Tasks;

public class GoogleLogin : MonoBehaviour
{
    public StartController _startController;
    [SerializeField]private string googleIdToken;
    [SerializeField]private string googleAccessToken;
    FirebaseAuth auth;

    public async void Login()
    {
        await LoginGoogle();
    }

    async Task LoginGoogle()
    {
        auth = FirebaseAuth.DefaultInstance;

        Credential credential =
        GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
        await auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            GuestLoginSuccess();
        });
    }

    void GuestLoginSuccess()
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            _startController.SwitchLoginPanels(2);

            _startController.tmpUsername.text = user.DisplayName;
            _startController.tmpEmail.text = user.Email;
            System.Uri photo_url = user.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = user.UserId;
        }
    }
}
