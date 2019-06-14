using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    public List<GameObject> enemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderEnemies();
    }

    void RenderEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponentsInChildren<MeshRenderer>()[0].enabled = false;
            enemy.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;

            float result = Vector3.Dot(Vector3.Normalize(player.transform.position - enemy.transform.position), player.GetComponent<Player>().forwardVector);
            if (result < -.45f)
            {
                if (Physics.Raycast(player.transform.position, enemy.transform.position - player.transform.position, out RaycastHit hit))
                {
                    if(hit.transform.gameObject.GetComponentInParent<Enemy>())
                    {
                        Debug.DrawRay(player.transform.position, enemy.transform.position - player.transform.position, Color.blue);
                        enemy.GetComponentsInChildren<MeshRenderer>()[0].enabled = true;
                        enemy.GetComponentsInChildren<MeshRenderer>()[1].enabled = true;
                    }
                }
            }
        }
    }
}
