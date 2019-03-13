using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public int tileSize = 32;
    public int maxEnemies = 5;
    [HideInInspector]
    public int aliveEnemies, roundNum = 1;

    public float timeBetweenRounds = 5.0f;

    public Transform enemyParent;

    public GameObject enemyPrefab;
    public GameObject pathEndPrefab;

    public Tilemap map;

    public TileBase tile;

    private bool m_inPause = true;
    private bool m_spawningEnemies = false;

    private int spawnedEnemies;

    private float currTime = .0f;

    [Header("UI")]
    public TextMeshProUGUI timer;
    public TextMeshProUGUI roundNumber;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ManageSpawn", .0f, 1.0f);

        Instantiate(pathEndPrefab, EnemyBase.path[EnemyBase.path.Length - 1], new Quaternion());

        roundNumber.text = "Round: " + 1;
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;

        if(timer.gameObject.activeSelf)
        {
            timer.text = (int)(timeBetweenRounds - currTime + 1) + "s";
        }

        if(Input.GetMouseButtonUp(0))
        {
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();


            //TileBase tb = map.GetTile(new Vector3Int((int)Input.mousePosition.x, (int)Input.mousePosition.y, (int)Input.mousePosition.z));

            if (Physics.Raycast(r, out hit))
            {
                if(hit.transform.gameObject.GetComponent<EnemyBase>() != null)
                {
                    hit.transform.gameObject.GetComponent<EnemyBase>().TakeDamage(25);
                }
                //tb = hit.transform.gameObject.GetComponent<TileBase>();
            }

            //TileBase tb = map.GetTile(new Vector3Int((int)mousePos.x, (int)mousePos.y, (int)mousePos.z));

           // map.SwapTile(tb, tile); //.SetTile(new Vector3Int((int)Input.mousePosition.x, (int)Input.mousePosition.y, (int)Input.mousePosition.z), tile);
        }
    }

    private void ManageSpawn()
    {
        // TODO:
        //if(m_menuIsUp)
        //{
        //    return;
        //}

        if(!m_spawningEnemies)
        {
            spawnedEnemies = 0;
            if (m_inPause)
            {
                timer.gameObject.SetActive(true);
                if (currTime >= timeBetweenRounds)
                {
                    m_inPause = false;
                    m_spawningEnemies = true;
                    roundNumber.text = "Round: " + roundNum;
                    ++roundNum;
                    currTime = .0f;
                }
            }
            else
            {
                timer.gameObject.SetActive(false);
                if (aliveEnemies == 0)
                {
                    timeBetweenRounds += 1;
                    m_inPause = true;
                    currTime = .0f;
                }
            }
        }
        else
        {
            timer.gameObject.SetActive(false);
            aliveEnemies = maxEnemies;
            StartCoroutine(SpawnEnemies(aliveEnemies));
            ++maxEnemies;
            m_spawningEnemies = false;
        }
    }

    IEnumerator SpawnEnemies(int t_enemyNum)
    {
        for(int i = 0; i < t_enemyNum; ++i)
        {
            Instantiate(enemyPrefab, EnemyBase.path[0], new Quaternion(), enemyParent);
            ++spawnedEnemies;
            yield return new WaitForSeconds(1);
        }
    }

}
