using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEnd : MonoBehaviour
{
    private GameManager m_gm;

    // Start is called before the first frame update
    void Start()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;
        if (go.GetComponentInParent<EnemyBase>() != null)
        {
            --m_gm.aliveEnemies;
            Destroy(go);
        }
    }
}
