using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isMenuUp = false;

    public int tileSize = 32;
    public int maxEnemies = 5;
    public int wood = 1500;
    public int gold = 50;
    public int gathererCost = 49;
    public int turretCost = 100;
    [HideInInspector]
    public int aliveEnemies, roundNum = 1;

    public float timeBetweenRounds = 5.0f;
    public float health = 100;

    public Transform charactersParent;
    public Transform turretsParent;

    public Transform gatherPoint;
    public Transform recolectPoint;
    public Transform recolectorSpawnPoint;

    public GameObject turretPrefab;
    public GameObject enemyPrefab;
    public GameObject gathererPrefab;
    public GameObject pathEndPrefab;

    public Sprite interactableTileSprite;

    [HideInInspector]
    public List<GameObject> enemies;

    public Grid map;

    public GameObject interactTileMap;

    private bool m_inPause = true;
    private bool m_spawningEnemies = false;

    private int spawnedGatherers = 1;
    private int spawnedEnemies;

    private float currTime = .0f;

    [Header("UI")]
    public TextMeshProUGUI timer;
    public TextMeshProUGUI roundNumber;
    public TextMeshProUGUI woodIndicator;
    public TextMeshProUGUI goldIndicator;
    public TextMeshProUGUI turretCostText;
    public TextMeshProUGUI gathererCostText;
    public Button buyGatherer;
    public Toggle placingTurretToggle;
    public Slider healthSlider;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ManageSpawn", .0f, 1.0f);

        Instantiate(pathEndPrefab, EnemyBase.path[EnemyBase.path.Length - 1], new Quaternion());

        roundNumber.text = "Round: " + 1;

        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        if(health <= 0)
        {
            isMenuUp = true;
            gameOverMenu.SetActive(true);
            return;
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            isMenuUp = true;
            pauseMenu.SetActive(true);
        }

        if (isMenuUp)
        {
            return;
        }

        currTime += Time.deltaTime;

        woodIndicator.text = wood.ToString();
        goldIndicator.text = gold.ToString();

        turretCostText.text = turretCost.ToString();
        gathererCostText.text = gathererCost.ToString();

        healthSlider.value = health;

        if (timer.gameObject.activeSelf)
        {
            timer.text = (int)(timeBetweenRounds - currTime + 1) + "s";
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(!placingTurretToggle.isOn)
            {
                return;
            }

            if (wood < turretCost)
            {
                return;
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int localPlace = map.WorldToCell(mousePos);

            Vector3 tilePos = map.CellToWorld(localPlace);

            tilePos.x += .5f;
            tilePos.y += .325f;

            //print(map.GetComponentInChildren<Tilemap>().GetTile(localPlace).GetTileData();

            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(r, out hit))
            {
                Turret t = hit.transform.gameObject.GetComponent<Turret>();
                if (t != null)
                {
                    return;
                }
            }

            //BoundsInt bounds = interactTileMap.GetComponent<Tilemap>().cellBounds;
            //TileBase[] allTiles = interactTileMap.GetComponent<Tilemap>().GetTilesBlock(bounds);

            //print(allTiles.Length);
            //foreach(var a in allTiles)
            //{
            //    print(a.name);
            //}

            //if (!interactTileMap.HasTile(localPlace))
            //{
            //    Debug.Log("Nope");
            //    return;
            //}

            GameObject turret = Instantiate(turretPrefab, tilePos, new Quaternion(), turretsParent);
            turret.GetComponentInChildren<MeshFilter>().transform.rotation.eulerAngles.Set(-180, 0, 90);
        }
    }

    private void ManageSpawn()
    {
        if (isMenuUp)
        {
            return;
        }

        if (!m_spawningEnemies)
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
            enemies.Add(Instantiate(enemyPrefab, EnemyBase.path[0], new Quaternion(), charactersParent));
            ++spawnedEnemies;
            yield return new WaitForSeconds(1);
        }
    }

    public void SpawnGatherer()
    {
        if(gold >= gathererCost)
        {
            Instantiate(gathererPrefab, charactersParent);
            gold -= gathererCost;
            ++spawnedGatherers;
        }
    }

    public void Resume()
    {
        isMenuUp = false;
        pauseMenu.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
