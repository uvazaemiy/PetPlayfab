using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance;
    public UI_Controller Ui_Controller;

    private string loggedPlayfabID;

    private void Start()
    {
        instance = this;

        if (PlayerPrefs.GetString("email") != "" && PlayerPrefs.GetString("password") != "")
            SignIn(PlayerPrefs.GetString("email"), PlayerPrefs.GetString("password"));
        else
            LoginWithCustodID();
    }



    //Signing in and registration methods
    private void LoginWithCustodID()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginIDSuccess => Debug.Log("Successfull login/account with ID create!"), OnError);
    }

    public void SignUp(string email, string password)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = false,
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess =>
        {
            Ui_Controller.SwitchButtons(true);
            Ui_Controller.ShowCloseEnteringName_(1);
            Debug.Log("Register and logged in!");
        },
            RegisterAndLoginError);
    }

    public void SignIn(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, RegisterAndLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Logged in!");
        loggedPlayfabID = result.PlayFabId;
        try { Ui_Controller.SwitchButtons(true); } catch { }
    }

    public void SignOut()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        Ui_Controller.GetComponent<UI_Controller>().SwitchButtons(false);
        LoginWithCustodID();
    }

    private void RegisterAndLoginError(PlayFabError error) 
    {
        Debug.Log(error.ErrorMessage);
        if (error.ErrorMessage == "User not found")
            error.ErrorMessage = "Invalid input parameters";
        try { Ui_Controller.PrintError(error.ErrorMessage); } catch { } 
    }

    public void EnterTheName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate => Debug.Log("Name successfully updated!"), OnErrorDisplayName);
    }

    private void OnErrorDisplayName(PlayFabError error)
    {
        Ui_Controller.ShowErrorOffName(error.ErrorMessage);
    }

    //Recieving and using information
    public void SendLeaderboard(int score, string StatisticName)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new System.Collections.Generic.List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = StatisticName,
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate =>
        {
            Debug.Log("Successfull leaderboard sent!");
            GetLeaderboard();
        },
        OnError);
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = SceneManager.GetActiveScene().name,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        var requestOffPlayer = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = SceneManager.GetActiveScene().name,
            MaxResultsCount = 1
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
        PlayFabClientAPI.GetLeaderboardAroundPlayer(requestOffPlayer, OnLeaderboardAroundPlayerGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        int n = 1;
        foreach (var item in result.Leaderboard)
        {
            if (item.PlayFabId == loggedPlayfabID && item.DisplayName != null)
            {
                Ui_Controller.CreateLeaderboard(item.Position + 1, item.DisplayName, item.StatValue, true);
                n++;
            }
            else if (item.DisplayName != null)
                Ui_Controller.CreateLeaderboard(n++, item.DisplayName, item.StatValue);
        }
        Debug.Log("Successfully leaderboard get!");
    }

    private void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        foreach (var item in result.Leaderboard)
            if (item.Position > 9)
                Ui_Controller.CreateLeaderboard(item.Position + 1, item.DisplayName, item.StatValue, true);
    }

    //Errors and other logic
    private void OnError(PlayFabError error) { Debug.Log(error.ErrorMessage); }

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
