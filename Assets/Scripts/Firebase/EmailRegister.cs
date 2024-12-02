using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Firebase
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Mail;
using WebSocketSharp;


public class EmailRegister : MonoBehaviour
{
    [Header("Componetes de Login")]
    public TMP_InputField tmpEmailLogin;
    public TMP_InputField tmpPasswordLogin;
    [Space]
    [Header("Componentes de Registro")]
    public TMP_InputField tmpUsernameRegister;
    public TMP_InputField tmpEmailRegister;
    public TMP_InputField tmpPasswordRegister;
    public TMP_InputField tmpPasswordConfirmRegister;
    public TMP_Text tmpWarning;
    [Space]
    [Header("Componentes de Recuperação de senha")]
    public TMP_InputField tmpForgetEmail;
    [Space]
    [Header("Scripts")]
    public StartController _startController;
    public FirebaseDataServer _server;
    public InfoFirestoreJogador _jogador;
    [Space]
    [Header("Retorno de dados")]
    public string urlPhoto;
    public FirebaseAuth auth;
    public FirebaseUser user;

    #region Unity Methods
    private void Awake()
    {
        tmpWarning.text = "";
        _startController = FindObjectOfType<StartController>();
        _server = FindObjectOfType<FirebaseDataServer>();
        _jogador = FindObjectOfType<InfoFirestoreJogador>();
    }
    #endregion

    #region Firebase
    
    /// <summary>
    /// Verifica se o usuário está autenticado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventsArgs"></param>
    void AuthStateChanged(object sender, EventArgs eventsArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                _startController.SwitchLoginPanels(0);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                _startController.SwitchLoginPanels(2);
                _startController.tmpUsername.text = user.DisplayName ?? "";
                _startController.tmpEmail.text = user.Email;
            }
        }
    }
    #endregion

    #region Email Register
    /// <summary>
    /// Criação de usuários no firebase
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="username"></param>
    void CreateUser(string email, string password, string username)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) 
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled");
                return; 
            }
            if (task.IsFaulted) 
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception.ToString());

                foreach(Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    FirebaseException firebaseException = exception as FirebaseException;
                    if(firebaseException != null)
                    {
                        var errorCode = (AuthError)firebaseException.ErrorCode;
                        tmpWarning.color = Color.magenta;
                        tmpWarning.text = "Error: " + GetErrorMessage(errorCode);
                        StartCoroutine(ClearWarningandInfo());
                    }
                }
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            StartCoroutine(ClearFields());
            //Save Firestore DB
            _server.userId = result.User.UserId;
            _server.username = username;
            _server.SaveData();

            UpdateUser(username);
        });
    }
    #endregion

    #region Login
    /// <summary>
    /// Autenticar o usuário do Firebase
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    public void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled");
                return;
            }
            if (task.IsFaulted) 
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error" + task.Exception.ToString());

                foreach(Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    FirebaseException firebaseException = exception as FirebaseException;
                    if(firebaseException != null)
                    {
                        var errorCode = (AuthError)firebaseException.ErrorCode;
                        tmpWarning.color = Color.magenta;
                        tmpWarning.text = "Error: " + GetErrorMessage(errorCode);
                        StartCoroutine(ClearWarningandInfo());
                    }
                }
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            
            _server.userId = result.User.UserId;
            _server.LoadData();
            string email = result.User.Email;
            StartCoroutine(FillInFields(email));
        });
    }
    /// <summary>
    /// Tratamentos de erros do Firebase
    /// </summary>
    /// <param name="errorCode"></param>
    /// <returns></returns>
    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "There is already an account with these credentials";
                break;
            case AuthError.MissingPassword:
                message = "Password required";
                break;
            case AuthError.WeakPassword:
                message = "The password is weak";
                break;
            case AuthError.WrongPassword:
                message = "Incorrect password";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "There is already an account with this email";
                break;
            case AuthError.InvalidEmail:
                message = "Invalid email";
                break;
            case AuthError.MissingEmail:
                message = "Email is missing";
                break;
            default:
                message = "An error occurred";
                break;
        }
        return message;
    }
    /// <summary>
    /// Função para habilitar o profile do jogador
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    void LoginSuccessfully(string email)
    {
        tmpWarning.color = Color.green;
        tmpWarning.text = "Login success";
        _startController.SwitchLoginPanels(2);
        _startController.tmpUsername.text = _server.username;
        _startController.tmpEmail.text = email;
        _jogador.userId = _server.userId;
        _jogador.username = _server.username;
        StartCoroutine(ClearWarningandInfo());
    }
    #endregion

    #region Update user and Password recory
    /// <summary>
    /// Update das informações do usuário após se registrar
    /// </summary>
    /// <param name="username"></param>
    public void UpdateUser(string username)
    {
        user = auth.CurrentUser;
        if (user != null) 
        {
            UserProfile profile = new UserProfile 
            {
                DisplayName = username,
            };
            user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SUpdateUserProfileAsync was canceled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error" + task.Exception.ToString());
                    return;
                }

                Debug.Log("User profile update successfully.");
                tmpWarning.color = Color.green;
                tmpWarning.text = "Alert," + username + " account successfully created!";
                _startController.tmpUsername.text = _server.username;
                _startController.tmpEmail.text = auth.CurrentUser.Email;
                _startController.SwitchLoginPanels(2);
                StartCoroutine(ClearWarningandInfo());
            });
        }
    }
    /// <summary>
    /// Reset de senha do usuário
    /// </summary>
    /// <param name="forgetPassword"></param>
    public void ForgetPasswordSubmit(string forgetPassword)
    {
        auth.SendPasswordResetEmailAsync(forgetPassword).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);

                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    FirebaseException firebaseEx = exception as FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        tmpWarning.color = Color.red;
                        tmpWarning.text = "Error: " + GetErrorMessage(errorCode);
                        StartCoroutine(ClearWarningandInfo());
                    }
                }
                return;
            }

            Debug.Log("Password reset email sent successfully.");
            tmpWarning.color = Color.green;
            tmpWarning.text = "Password reset email sent successfully";
            StartCoroutine(ClearWarningandInfo());
            _startController.SwitchLoginPanels(1);
        });
    }
    #endregion

    #region My Methods
    /// <summary>
    /// Botão StartGame
    /// </summary>
    public void StartGame()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if(dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.Log(String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }
    /// <summary>
    /// Inicializa o Firebase
    /// </summary>
    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        _startController.SwitchPanels(4);
    }
    /// <summary>
    /// Função do botão de criação de usuário - verifica as informações antes de criar o usuário
    /// </summary>
    public void UserSignUp()
    {
        if (string.IsNullOrEmpty(tmpEmailRegister.text) || string.IsNullOrEmpty(tmpPasswordRegister.text) || string.IsNullOrEmpty(tmpPasswordConfirmRegister.text)
            || string.IsNullOrEmpty(tmpUsernameRegister.text))
        {
            tmpWarning.color = Color.red;
            tmpWarning.text = "There are empty fields!";
        }
        else if (tmpPasswordRegister.text != tmpPasswordConfirmRegister.text)
        {
            tmpWarning.color = Color.yellow;
            tmpWarning.text = "Passwords don't match";
        }
        else
        {
            CreateUser(tmpEmailRegister.text, tmpPasswordRegister.text, tmpUsernameRegister.text);
        }
        StartCoroutine(ClearWarningandInfo());
    }
    /// <summary>
    /// Função do botão de login - verifica se os campos estão preenchidos ou não
    /// </summary>
    public void UserLogin()
    {
        if (string.IsNullOrEmpty(tmpEmailLogin.text) && string.IsNullOrEmpty(tmpPasswordLogin.text))
        {
            tmpWarning.color = Color.red;
            tmpWarning.text = "There are empty fields!";
        }
        else
        {
            SignInUser(tmpEmailLogin.text, tmpPasswordLogin.text);
            //_server.LoadData();
        }
    }
    /// <summary>
    /// Função do botão de recuperação de senha - verifica se os campos estão preenchidos ou não
    /// </summary>
    public void UserForgetPassword()
    {
        if (string.IsNullOrEmpty(tmpForgetEmail.text)) 
        {
            tmpWarning.color = Color.red;
            tmpWarning.text = "Forget email empty!";
        }
        else
        {
            ForgetPasswordSubmit(tmpForgetEmail.text);
        }
        StartCoroutine(ClearWarningandInfo());
    }
    /// <summary>
    /// Botão de deslogar o usuário
    /// </summary>
    public void LogoutUser()
    {
        auth.SignOut();
        tmpWarning.color = Color.cyan;
        tmpWarning.text = "user successfully logged out";
        StartCoroutine(ClearWarningandInfo());
        _startController.SwitchLoginPanels(0);
    }
    /// <summary>
    /// Destrói a instância de login
    /// </summary>
    private void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
    //////////////////////////////Corrotinas gerais ///////////////////////////////////////////////////////
    /// Corrotina de limpeza das informações e avisos
    /// </summary>
    /// <returns></returns>
    IEnumerator ClearWarningandInfo()
    {
        yield return new WaitForSeconds(2f);
        tmpWarning.text = "";
    }
    /// <summary>
    /// Corrotina de limpeza de campos preenchidos
    /// </summary>
    /// <returns></returns>
    IEnumerator ClearFields()
    {
        yield return new WaitForSeconds(2f);
        tmpUsernameRegister.text = "";
        tmpEmailRegister.text = "";
        tmpPasswordRegister.text = "";
        tmpPasswordConfirmRegister.text = "";
    }
    /// <summary>
    /// Corrotina para apresentar o perfil do jogador
    /// </summary>
    /// <param name="userEmail"></param>
    /// <returns></returns>
    IEnumerator FillInFields(string userEmail)
    {
        yield return new WaitForSeconds(1f);
        LoginSuccessfully(userEmail);
    }
    #endregion
}
