using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    public int movesLeft;

    Camera m_cam;

    static PathfindingManager m_pfm; // Pathfinding manager

    List<WorldTile> m_path;

    void Awake()
    {
        m_cam = Camera.main;
        m_pfm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PathfindingManager>();

        movesLeft = 3;
    }

    public void MoveTo(WorldTile t_destination)
    {
        m_path = Pathfinding.FindPath(m_pfm.GetTile((int)transform.position.x, (int)transform.position.y)
                                        , t_destination);
        StartCoroutine(MoveCoroutine(m_path));
    }

    IEnumerator MoveCoroutine(List<WorldTile> t_path)
    {
        foreach(WorldTile wt in t_path)
        {
            transform.position = new Vector3(wt.gridX + .5f, wt.gridY + .5f, -1);

            yield return new WaitForSeconds(.2f);
        }
    }
}
