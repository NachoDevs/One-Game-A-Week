using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    Camera m_cam;

    static PathfindingManager m_pfm; // Pathfinding manager

    List<WorldTile> m_path;

    RaycastHit2D m_hit;

    void Awake()
    {
        m_cam = Camera.main;
        m_pfm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PathfindingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (m_hit.collider.gameObject.GetComponentInParent<WorldTile>() != null)
            {
                m_path = Pathfinding.FindPath(m_pfm.GetTile((int)transform.position.x, (int)transform.position.y)
                                                        , m_pfm.GetTile((int)m_hit.point.x, (int)m_hit.point.y));
                StartCoroutine(Move(m_path));
            }
        }
    }

    IEnumerator Move(List<WorldTile> t_path)
    {
        foreach(WorldTile wt in t_path)
        {
            transform.position = new Vector3(wt.gridX + .5f, wt.gridY + .5f, -1);

            yield return new WaitForSeconds(.2f);
        }
    }
}
