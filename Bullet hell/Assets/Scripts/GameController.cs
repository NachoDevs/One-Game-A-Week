using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int score = 0;

    public float asteroidSpawnRate = 2;
    public float enemySpawnRate = 5;

    public GameObject enemyPrefab;

    public List<GameObject> asteroids;

    [Header("UI")]
    public TextMeshProUGUI scoreText;

    float m_asteroidTimer;
    float m_enemyTimer;
    float m_cameraTimer;

    Vector3 m_originalCamPos;

    Camera m_cam;

    GameObject m_player;

    void Start()
    {
        m_cam = Camera.main;
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        m_asteroidTimer += Time.deltaTime;
        m_enemyTimer += Time.deltaTime;

        scoreText.text = score.ToString();

        if (m_asteroidTimer > Random.Range(asteroidSpawnRate - 1, asteroidSpawnRate + 1))
        {
            m_asteroidTimer = 0;
            SpawnObject(asteroids[Random.Range(0, asteroids.Count)]);
        }

        if (m_enemyTimer > Random.Range(enemySpawnRate - 3, enemySpawnRate + 3))
        {
            m_enemyTimer = 0;
            SpawnObject(enemyPrefab);
        }
    }

    public void IncreaseScore(int t_addScore)
    {
        score += t_addScore;

        if(score > 500)
        {
            asteroidSpawnRate = 1.5f;
            enemySpawnRate = 4.5f;
        }
        if (score > 1000)
        {
            asteroidSpawnRate = 1f;
            enemySpawnRate = 4f;
        }
        if (score > 1500)
        {
            asteroidSpawnRate = .5f;
            enemySpawnRate = 3.5f;
        }
    }

    public void Shake(float t_duration, float t_magnitude)
    {
        StartCoroutine(m_cam.GetComponent<CamShake>().Shake(t_duration, t_magnitude));
    }

    void SpawnObject(GameObject t_toSpawn)
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

        GameObject asteroid = Instantiate(t_toSpawn, spawnPos, new Quaternion());
    }
}
