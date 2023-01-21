using UnityEngine;
using System.Collections.Generic;

public class SpawnerPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject PlatformPrefab;
    public List<GameObject> obstacles;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        SpawnObstacles();
    }

    public void SpawnObstacles()
    {
        Vector3 spawner_position = new Vector3(0, -0.26f, 0);
        for (int i = 0; i < 7; i++)
        {
            spawner_position.x = Random.Range(cam.ScreenToWorldPoint(new Vector3(0, 0, -cam.transform.position.z)).x + transform.localScale.x / 4, cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, -cam.transform.position.z)).x - transform.localScale.x / 4);
            spawner_position.y += Random.Range(1f, 4);
            obstacles.Add(Instantiate(PlatformPrefab, spawner_position, Quaternion.identity, transform));
        }
        obstacles.Add(Instantiate(PlatformPrefab, new Vector3(-0.05f, -3.73f, 0), Quaternion.identity, transform));
        obstacles.Add(Instantiate(PlatformPrefab, new Vector3(0.61f, -1.61f, 0), Quaternion.identity, transform));
        obstacles.Add(Instantiate(PlatformPrefab, new Vector3(-0.88f, -0.26f, 0), Quaternion.identity, transform));
    }
}
