using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float spawnRate = 2;

    public List<GameObject> asteroids;

    float m_timer;

    Camera m_cam;

    GameObject m_player;

    void Start()
    {
        m_cam = Camera.main;
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        m_timer += Time.deltaTime;

        if(m_timer > spawnRate)
        {
            m_timer = 0;
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = m_cam.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;
        Vector3 spawnPos = m_player.transform.position;

        if(Random.Range(0, 2) > 0)
        {
            spawnPos.x += Random.Range((-cameraWidth) / 2, cameraWidth / 2);
            spawnPos.y += (m_cam.orthographicSize + .5f) * ((Random.Range(0, 2) > 0) ? -1 : 1);
        }
        else
        {
            spawnPos.x += (m_cam.orthographicSize * screenAspect + .5f) * ((Random.Range(0, 2) > 0) ? -1 : 1);
            spawnPos.y += Random.Range((-cameraHeight) / 2, cameraHeight / 2);
        }

        GameObject asteroid = Instantiate(asteroids[Random.Range(0, asteroids.Count)], spawnPos, new Quaternion());
    }
}
