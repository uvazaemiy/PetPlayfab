using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerControlNavMesh : MonoBehaviour
{
    [SerializeField] private UI_Controller Ui_Controller;

    private bool flag_of_winning, flag_of_start;
    private Camera mainCamera;
    private NavMeshAgent agent;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if ((int)Vector3.Distance(transform.position, agent.pathEndPosition) == 0)
            Ui_Controller.Toggle_Start(false);

        if (Input.GetMouseButtonDown(0) && !flag_of_winning)
        {
            RaycastHit hit;
            if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
                return;

            if (!flag_of_start)
            {
                flag_of_start = !flag_of_start;
                Ui_Controller.StartGame();
            }

            agent.SetDestination(hit.point);
            Ui_Controller.Toggle_Start(true, new Vector3(agent.destination.x, 0.5f, agent.destination.z));
        }

        if (Input.GetKey(KeyCode.Escape))
            Ui_Controller.Exit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Finish" && !flag_of_winning)
        {
            flag_of_winning = !flag_of_winning;
            Ui_Controller.RollWinLosePanel(1);
        }
    }
}
