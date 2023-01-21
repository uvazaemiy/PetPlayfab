using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    private PlayFabManager playFabManager;

    [Header("Gameplay Vars")]
    [Space]
    //[SerializeField] private GameObject Player;
    [SerializeField] private float scaleFactorX, bouncenes = 40;
    [HideInInspector] public int milliseconds = 0, seconds = 0, minutes = 0, score = 0;
    [Space]
    [Header("UI Components")]
    [Header("Win/Lose panel")]
    [Space]
    public Text ClickAnywhere;
    public Text Score;
    public Image toggle;
    [SerializeField] private Image Leaderboard;
    [Space]
    [SerializeField] private Image WinLosePanel;
    [SerializeField] private Image WinLosePanelBorder;
    [SerializeField] Image WinningTextPanel;
    [SerializeField] private Text WinningText;
    [Space]
    [SerializeField] private Image[] ObjectsOfWinLosePanel; // SignInWithGoogle, SignInWithApple, SignInWithCustom, Restart, ExitToMainMenu
    [Space]
    [Header("Login/Registration")]
    [SerializeField] private GameObject SignInUpObject;
    [SerializeField] private InputField e_mail;
    [SerializeField] private InputField password;
    [Space]
    [SerializeField] private Image[] ObjectsOfCustomSignIn; // SignOut, Back, SignIn, SignUp, E_mail, Password
    [Space]
    [SerializeField] private Image EnteringNamePanel;
    [SerializeField] private Image BorderofEnteringName;
    [SerializeField] private InputField EnterNameField;
    [SerializeField] private Text NameText;
    [SerializeField] private Image SaveNameButton;
    [Space]
    [Header("Leaderboard")]
    [SerializeField] private Image LeaderboardTable;
    [SerializeField] private Image LeaderboardBorder;
    [SerializeField] private Image CloseLeaderboardButton;
    [SerializeField] private Image Line;
    [SerializeField] private GameObject FirstRow;
    [SerializeField] private GameObject Grid;
    [SerializeField] private GameObject Row;
    private Image MarkingofPlayer;
    [Space]
    [SerializeField] private Color Not_Logged, Logged, Error;



    private void Start()
    {
        //Settings of padding
        scaleFactorX = (float)Screen.width / 1920 * 80;

        playFabManager = PlayFabManager.instance;
        playFabManager.Ui_Controller = this;

        if (Screen.height > Screen.width)
        {
            scaleFactorX *= transform.localScale.x * 2;
            bouncenes /= 120;
        }
        else
            scaleFactorX *= transform.localScale.x;

        //Auto LogIn
        if (PlayerPrefs.GetString("email") != "" && PlayerPrefs.GetString("password") != "")
        {
            WinningText.text = "You logged in!";
            WinningText.color = Logged;
            for (int i = 2; i < 6; i++)
                ObjectsOfCustomSignIn[i].gameObject.SetActive(false);

            ObjectsOfCustomSignIn[0].gameObject.SetActive(true);
            ObjectsOfCustomSignIn[1].transform.position -= new Vector3(scaleFactorX, 0, 0);
        }

        RemovingAlpha();
        playFabManager.GetLeaderboard();
    }

    private void RemovingAlpha()
    {
        Leaderboard.color = DisableAlpha(Leaderboard.color);
        WinLosePanel.color = DisableAlpha(WinLosePanel.color);
        WinLosePanelBorder.color = DisableAlpha(WinLosePanelBorder.color);
        WinningTextPanel.color = DisableAlpha(WinningTextPanel.color);
        WinningText.color = DisableAlpha(WinningText.color);
        foreach (Image temp in ObjectsOfWinLosePanel)
            temp.color = DisableAlpha(temp.color);
        foreach (Image temp in ObjectsOfCustomSignIn)
            temp.color = DisableAlpha(temp.color);
        EnteringNamePanel.color = DisableAlpha(EnteringNamePanel.color);
        BorderofEnteringName.color = DisableAlpha(BorderofEnteringName.color);
        EnterNameField.GetComponent<Image>().color = DisableAlpha(EnterNameField.GetComponent<Image>().color);
        EnterNameField.GetComponentInChildren<Text>().color = DisableAlpha(EnterNameField.GetComponentInChildren<Text>().color);
        NameText.color = DisableAlpha(NameText.color);
        SaveNameButton.color = DisableAlpha(SaveNameButton.color);
        SaveNameButton.GetComponentInChildren<Text>().color = DisableAlpha(SaveNameButton.GetComponentInChildren<Text>().color);
        LeaderboardTable.color = DisableAlpha(LeaderboardTable.color);
        LeaderboardBorder.color = DisableAlpha(LeaderboardBorder.color);
        CloseLeaderboardButton.color = DisableAlpha(CloseLeaderboardButton.color);
        CloseLeaderboardButton.GetComponentInChildren<Text>().color = DisableAlpha(CloseLeaderboardButton.GetComponentInChildren<Text>().color);
        Line.color = DisableAlpha(Line.color);
        foreach (Text temp in FirstRow.GetComponentsInChildren<Text>())
            temp.color = DisableAlpha(temp.color);
    }

    private Color DisableAlpha(Color current)
    {
        current = new Color(current.r, current.g, current.b, 0);
        return current;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == "NavMesh")
            Invoke(nameof(Timer), 0.1f);
        ClickAnywhere.DOFade(0, 0.75f).OnComplete(() => ClickAnywhere.gameObject.SetActive(false));
    }



    //Rolling the whole WinLosePanel according to game status
    public void RollWinLosePanel(int state)
    {
        if (state == 1) ShowWinLosePanel();

        WinLosePanel.DOFade(0.75f * state, 0.75f);
        WinLosePanelBorder.DOFade(state, 0.75f);
        WinningTextPanel.DOFade(0.4f * state, 0.75f);
        WinningText.DOFade(state, 0.75f);
        Leaderboard.DOFade(state, 0.75f);

        foreach (Image temp in ObjectsOfWinLosePanel)
        {
            temp.DOFade(state, 0.75f);
            temp.GetComponentInChildren<Text>().DOFade(state, 0.75f);
        }

        if (state == 0) StartCoroutine(CloseWinLosePanel());
}

    private void ShowWinLosePanel()
    {
        if (WinningText.text == "You logged in!")
            playFabManager.SendLeaderboard(score, SceneManager.GetActiveScene().name);
        CancelInvoke(nameof(Timer));

        WinLosePanel.gameObject.SetActive(true);
        WinningText.transform.DOMoveX(WinningText.transform.position.x + bouncenes, 0.75f).SetEase(Ease.InOutBounce).From();
    }

    private IEnumerator CloseWinLosePanel()
    {
        yield return new WaitForSeconds(0.75f);

        WinLosePanel.gameObject.SetActive(false);
        ClickAnywhere.gameObject.SetActive(true);
        yield return
        ClickAnywhere.DOFade(1, 0.75f).WaitForCompletion();

        //RESTART
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    //Button Sign in with Email
    public void SignInEmail_(int state) { StartCoroutine(SignInEmail(state)); }
    private IEnumerator SignInEmail(int state)
    {
        e_mail.text = "";
        password.text = "";

        if (state == 1)     //activation buttons of sign in/up
        {
            foreach (Image temp in ObjectsOfWinLosePanel)
            {
                temp.DOFade(-state, 0.75f);
                temp.GetComponentInChildren<Text>().DOFade(-state, 0.75f);
            }
            Leaderboard.DOFade(-1, 0.75f);
            yield return new WaitForSeconds(0.75f);

            foreach (Image temp in ObjectsOfWinLosePanel)
                temp.gameObject.SetActive(false);
            SignInUpObject.SetActive(true);
            Leaderboard.gameObject.SetActive(false);

            for (int i = 0; i < 6; i++)
            {
                ObjectsOfCustomSignIn[i].DOFade(state, 0.75f);
                ObjectsOfCustomSignIn[i].GetComponentInChildren<Text>().DOFade(state, 0.75f);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                ObjectsOfCustomSignIn[i].DOFade(state, 0.75f);
                ObjectsOfCustomSignIn[i].GetComponentInChildren<Text>().DOFade(state, 0.75f);
            }
            yield return new WaitForSeconds(0.75f);

            SignInUpObject.SetActive(false);
            foreach (Image temp in ObjectsOfWinLosePanel)
                temp.gameObject.SetActive(true);
            Leaderboard.gameObject.SetActive(true);

            foreach (Image temp in ObjectsOfWinLosePanel)
            {
                temp.DOFade(-state, 0.75f);
                temp.GetComponentInChildren<Text>().DOFade(-state, 0.75f);
            }
            Leaderboard.DOFade(1, 0.75f);
            yield return new WaitForSeconds(0.75f);
        }

    }



    //Switching buttons SignIn/SinUp to SignOut, email and password fields
    public void SwitchButtons(bool state)
    {
        PlayerPrefs.SetString("email", e_mail.text);
        PlayerPrefs.SetString("password", password.text);

        e_mail.text = "";
        password.text = "";

        if (state)
            StartCoroutine(RemoveEmailAndPasswordFields());
        else
            StartCoroutine(ShowEmailAndPasswordFields());
    }

    private IEnumerator RemoveEmailAndPasswordFields()
    {
        playFabManager.SendLeaderboard(score, SceneManager.GetActiveScene().name);

        WinningText.color = Logged;
        WinningText.text = "You logged in!";
        WinningText.transform.DOMoveX(WinningText.transform.position.x + bouncenes, 0.75f).SetEase(Ease.InOutBounce).From();

        for (int i = 2; i < 6; i++)
        {
            ObjectsOfCustomSignIn[i].DOFade(0, 0.75f);
            ObjectsOfCustomSignIn[i].GetComponentInChildren<Text>().DOFade(-1, 0.75f);
        }
        yield return new WaitForSeconds(0.75f);

        for (int i = 2; i < 6; i++)
            ObjectsOfCustomSignIn[i].gameObject.SetActive(false);
        ObjectsOfCustomSignIn[0].gameObject.SetActive(true);

        ObjectsOfCustomSignIn[0].DOFade(1, 0.75f);
        ObjectsOfCustomSignIn[0].GetComponentInChildren<Text>().DOFade(1, 0.75f);                                           yield return
        ObjectsOfCustomSignIn[1].transform.DOMoveX(ObjectsOfCustomSignIn[1].transform.position.x - 1 * scaleFactorX, 1).WaitForCompletion();
    }

    private IEnumerator ShowEmailAndPasswordFields()
    {
        PlayerPrefs.SetString("email", "");
        PlayerPrefs.SetString("password", "");

        WinningText.color = Not_Logged;
        WinningText.text = "You win! Please sign in or register you account to save your score!";
        WinningText.transform.DOMoveX(WinningText.transform.position.x - bouncenes, 0.75f).SetEase(Ease.InOutBounce).From();

        ObjectsOfCustomSignIn[0].DOFade(0, 0.75f);
        ObjectsOfCustomSignIn[0].GetComponentInChildren<Text>().DOFade(0, 0.75f);                                       yield return

        ObjectsOfCustomSignIn[1].transform.DOMoveX(ObjectsOfCustomSignIn[1].transform.position.x + 1 * scaleFactorX, 1).WaitForCompletion(); 
        ObjectsOfCustomSignIn[0].gameObject.SetActive(false);

        for (int i = 2; i < 6; i++)
        {
            ObjectsOfCustomSignIn[i].gameObject.SetActive(true);
            ObjectsOfCustomSignIn[i].DOFade(1, 0.75f);
            ObjectsOfCustomSignIn[i].GetComponentInChildren<Text>().DOFade(1, 0.75f);
        }
        yield return new WaitForSeconds(0.75f);
    }



    //Signing in and up
    public void SignInUp(bool signin)
    {
        if (!CheckPassword())
            return;
        if (signin)
            playFabManager.SignIn(e_mail.text, password.text);
        else
            playFabManager.SignUp(e_mail.text, password.text);
    }

    private bool CheckPassword()
    {
        if (password.text.Length < 6)
        {
            PrintError("Your password is to short!");
            return false;
        }
        else
            return true;
    }

    public void ShowCloseEnteringName_(int state) { StartCoroutine(ShowCloseEnteringName(state)); }
    private IEnumerator ShowCloseEnteringName(int state)
    {
        if (state == 1) EnteringNamePanel.gameObject.SetActive(true);
        else
        {
            playFabManager.EnterTheName(EnterNameField.text);
            if (NameText.text != "Enter your name")
                StopAllCoroutines();
        }

        EnterNameField.text = "";
        EnterNameField.GetComponent<Image>().DOFade(state, 0.75f);
        EnterNameField.GetComponentInChildren<Text>().DOFade(state, 0.75f);
        EnteringNamePanel.DOFade(state, 0.75f);
        BorderofEnteringName.DOFade(state, 0.75f);
        NameText.DOFade(state, 0.75f);
        SaveNameButton.GetComponentInChildren<Text>().DOFade(state, 0.75f);     yield return
        SaveNameButton.DOFade(state, 0.75f).WaitForCompletion();

        if (state == -1)
            EnteringNamePanel.gameObject.SetActive(false);
    }

    public void ShowErrorOffName(string error)
    {
        NameText.text = error + "!";
        NameText.transform.DOMoveX(WinningText.transform.position.x - bouncenes, 0.75f).SetEase(Ease.InOutBounce).From();
    }

    public void SignOut() { playFabManager.SignOut(); }

    public void PrintError(string error)
    {
        WinningText.text = error + "!";
        WinningText.color = Error;
        WinningText.transform.DOMoveX(WinningText.transform.position.x - bouncenes, 0.75f).SetEase(Ease.InOutBounce).From();
    }



    //LeaderboardTable
    public void ShowCloseLeaderboard_(int state) { StartCoroutine(ShowCloseLeaderboard(state)); }
    private IEnumerator ShowCloseLeaderboard(int state)
    {
        if (state == 1) LeaderboardTable.gameObject.SetActive(true);

        foreach (Text temp in Grid.GetComponentsInChildren<Text>())
            temp.DOFade(state, 0.75f);
        foreach (Text temp in FirstRow.GetComponentsInChildren<Text>())
            temp.DOFade(state, 0.75f);
        MarkingofPlayer.DOFade(state, 0.75f);
        LeaderboardTable.DOFade(state, 0.75f);
        LeaderboardBorder.DOFade(state, 0.75f);
        Line.DOFade(state, 0.75f);
        CloseLeaderboardButton.DOFade(state, 0.75f);    yield return
        CloseLeaderboardButton.GetComponentInChildren<Text>().DOFade(state, 0.75f).WaitForCompletion();

        if (state == -1) LeaderboardTable.gameObject.SetActive(false);
    }

    public void CreateLeaderboard(int position, string name, int score, bool marked = false)
    {
        GameObject NextPlayer = Instantiate(Row, Grid.transform);
        LeaderboardRow DataOfRow = NextPlayer.GetComponent<LeaderboardRow>();

        DataOfRow.Position.text = position.ToString();
        DataOfRow.Name.text = name;
        DataOfRow.Score.text = score.ToString();

        DataOfRow.Position.color = DisableAlpha(DataOfRow.Position.color);
        DataOfRow.Name.color = DisableAlpha(DataOfRow.Name.color);
        DataOfRow.Score.color = DisableAlpha(DataOfRow.Score.color);
        if (marked)
        {
            DataOfRow.Marking.SetActive(true);
            DataOfRow.Marking.GetComponent<Image>().color = DisableAlpha(DataOfRow.Marking.GetComponent<Image>().color);
            MarkingofPlayer = DataOfRow.Marking.GetComponent<Image>();
        }
    }

    public void ClearLeaderboard()
    {
        foreach (GameObject temp in Grid.GetComponentsInChildren<GameObject>())
            Destroy(temp);
    }



    //Other mechanics
    public void Toggle_Start(bool check, Vector3 new_pos = new Vector3()) { StartCoroutine(Toggle(check, new_pos)); }
    private IEnumerator Toggle(bool check, Vector3 new_pos)
    {
        if (check)
        {
            yield return new WaitForSeconds(0.2f);

            toggle.DORestart();
            toggle.DOKill();

            toggle.transform.position = new_pos;

            toggle.fillClockwise = true;
            toggle.DOColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1), 0.75f);
            toggle.DOFillAmount(1, 0.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).OnStepComplete(() =>
            {
                toggle.fillClockwise = !toggle.fillClockwise;
                toggle.DOColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1), 0.75f);
            });
        }
        else
        {
            toggle.DORestart();
            toggle.DOKill();
        }
    }

    private void Timer()
    {
        milliseconds++;
        if (milliseconds == 10)
        {
            milliseconds = 0;
            seconds++;
        }
        if (seconds == 60)
        {
            seconds = 0;
            minutes++;
        }
        Score.text = "Time: " + minutes + ":" + seconds + "." + milliseconds;
        Invoke(nameof(Timer), 0.1f);
    }

    public void ScoreIncrease(int score)
    {
        this.score += score;
        Score.text = "Score: " + this.score;
    }

    public void Exit()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("MainMenu");
    }
}