using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    TileSelecting,
    Building,
}

public enum ResourceType
{
    wood,
    stone,
    wheat,
    bread,
}

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject buttonPrefab;
    public GameObject resourcePanelPrefab;
    public List<GameObject> buildings;
    public List<Sprite> resources;

    [Header("Parents")]
    public Transform buildingParent;
    public Transform buildingButtonParent;
    public Transform resourcesParent;

    public Dictionary<ResourceType, int> resourceCount;

    GameState m_currGameState;

    RaycastHit2D m_hit;

    Camera m_cam;

    Dictionary<ResourceType, TextMeshProUGUI> resourceText;

    GameObject m_selectedObject;
    GameObject m_hoveredObject;
    GameObject m_selectedBulding;

    TileManager m_tman;

    WorldTile m_previousTile;
    WorldTile m_currentTile;

    void Awake()
    {
        m_cam = Camera.main;

        m_tman = GetComponent<TileManager>();

        m_currGameState = GameState.TileSelecting;
    }

    void Start()
    {
        m_currentTile = m_tman.GetTile(0, 0);

        resourceCount = new Dictionary<ResourceType, int>();
        resourceText = new Dictionary<ResourceType, TextMeshProUGUI>();

        foreach (GameObject building in buildings)
        {
            GameObject button = Instantiate(buttonPrefab, buildingButtonParent);
            button.GetComponentsInChildren<Image>()[1].sprite = building.GetComponentInChildren<SpriteRenderer>().sprite;

            button.GetComponent<Button>().onClick.AddListener(delegate
            {
                switch(building.GetComponentInChildren<SpriteRenderer>().sprite.name)
                {
                    default:
                    case "bakery":
                        m_selectedBulding = buildings[0];
                        break;
                    case "deliveryHouse":
                        m_selectedBulding = buildings[1];
                        break;
                    case "farm":
                        m_selectedBulding = buildings[2];
                        break;
                    case "lumber":
                        m_selectedBulding = buildings[3];
                        break;
                    case "mine":
                        m_selectedBulding = buildings[4];
                        break;
                    case "warehouse":
                        m_selectedBulding = buildings[5];
                        break;
                    case "windmill":
                        m_selectedBulding = buildings[6];
                        break;
                }

                m_currGameState = GameState.Building;
            });
        }

        foreach(Sprite res in resources)
        {
            GameObject panel = Instantiate(resourcePanelPrefab, resourcesParent);
            panel.GetComponentsInChildren<Image>()[1].sprite = res;
            panel.GetComponentInChildren<TextMeshProUGUI>().text = 0.ToString();
            ResourceType rt;
            Enum.TryParse(res.name, out rt);
            resourceCount.Add(rt, 0);
            resourceText.Add(rt, panel.GetComponentInChildren<TextMeshProUGUI>());
        }

        UpdateResource(ResourceType.wood, 50);
    }

    // Update is called once per frame
    void Update()
    {
        HandleGameState();
    }

    public void UpdateResource(ResourceType t_rType, int t_amount)
    {
        resourceCount[t_rType] += t_amount;
        resourceText[t_rType].text = resourceCount[t_rType].ToString();
    }

    private void HandleGameState()
    {
        switch(m_currGameState)
        {
            case GameState.Building:
                TileSelectionBehaviour();

                if(!m_selectedBulding.GetComponent<Building>().CanAfford())
                {
                    m_currGameState = GameState.TileSelecting;
                    break;
                }

                if (!m_selectedBulding.GetComponent<Building>().CanBuildHere(m_hoveredObject))
                {
                    break;
                }

                WorldTile hitWT = null;
                try
                {
                    hitWT = m_hit.collider.transform.parent.gameObject.GetComponent<WorldTile>();
                    if(hitWT == null)
                    {
                        return;
                    }
                }
                catch (Exception e) { /*print("The mouse is not over a tile");*/ return; }

                Destroy(Instantiate(m_selectedBulding, m_currentTile.transform.position, Quaternion.identity, buildingParent), .02f);
                if(Input.GetMouseButtonUp(0))
                {
                    GameObject building = Instantiate(m_selectedBulding, m_currentTile.transform.position, Quaternion.identity, buildingParent);
                    building.GetComponent<Building>().Build();
                    m_hoveredObject.GetComponent<WorldTile>().UpdateTIle(TileState.Default);
                    m_currGameState = GameState.TileSelecting;
                }

                break;
            default:
            case GameState.TileSelecting:
                TileSelectionBehaviour();
                break;
        }
    }

    void TileSelectionBehaviour()
    {
        m_hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        // Not checking if it is a WorldTile might cause bugs?

        try
        {
            m_hoveredObject = m_hit.collider.transform.parent.gameObject;
        }
        catch (Exception e) { /*PrintException(e);*/ return; }

        m_previousTile = m_currentTile;

        m_currentTile = m_tman.GetTile(m_hoveredObject.transform.position.x, m_hoveredObject.transform.position.y);

        if (!m_currentTile.selected)
        {
            m_currentTile.UpdateTIle(TileState.Hovered);
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_selectedObject = m_hoveredObject;

            HandleSelection();
        }

        if (!m_previousTile.selected)
        {
            if (m_previousTile != m_currentTile)
            {
                m_previousTile.UpdateTIle(TileState.Default);
            }
        }
    }

    void HandleSelection()
    {
        if(m_selectedObject.GetComponent<WorldTile>() != null)
        {
            m_currentTile.UpdateTIle(TileState.Selected);

            // Only one tile can be selected
            foreach (GameObject wt in m_tman.unsortedNodes)
            {
                if (wt.GetComponent<WorldTile>() != m_currentTile)
                {
                    wt.GetComponent<WorldTile>().UpdateTIle(TileState.Default);
                }
            }

            return;
        }

        if (m_selectedObject.GetComponent<Building>() != null)
        {
            // If we are placing the building
            if(!m_selectedObject.GetComponent<Building>().isBuilded)
            {
                m_hoveredObject = m_tman.GetTile(m_hoveredObject.transform.position.x, m_hoveredObject.transform.position.y).gameObject;
                return;
            }

            m_selectedObject.GetComponent<Building>().Clicked();

            return;
        }
    }

    public void GoToBuildingMode()
    {
        switch(m_currGameState)
        {
            default:
            case GameState.TileSelecting:
                m_currGameState = GameState.Building;
                break;
            case GameState.Building:
                m_currGameState = GameState.TileSelecting;
                break;
        }
    }

    public static void PrintException(Exception t_e)
    {
        print
            (
            "---------Exception---------\n"
            + "Message: \t" + t_e.Message + "\n"
            + "Source: \t" + t_e.Source + "\n"
            + "Data: \t" + t_e.Data + "\n"
            + "Trace: \t" + t_e.StackTrace + "\n"
            );
    }
}
