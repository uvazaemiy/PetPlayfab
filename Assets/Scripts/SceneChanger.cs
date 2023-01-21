using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Transform Choose_Your_Game;
    [SerializeField] private Image Back;
    private int counter = 0;

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;

        Choose_Your_Game.DORotate(new Vector3(0, 0, 2), 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
        Back.DOFade(0.5f, 10).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        GameObject.Find("UI Controller").GetComponent<UI_Controller>(); //Showing Lunar Console in Nox
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            counter++;
            StartCoroutine(Timer());
            if (counter == 2)
                Exit();
            SSTools.ShowMessage("Click more to exit", SSTools.Position.top, SSTools.Time.twoSecond);
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(2);
        counter--;
    }

    public void ChooseScene(string scene)
    {
        DOTween.KillAll();
        SceneManager.LoadScene(scene);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
