using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public int currAmmo;
    public int maxAmmo = 20;

    public float damage = 10.0f;
    public float fireRate = 1.0f;
    public float reloadTime = 5.0f;
    public float fireRange = 5.0f;

    private float m_timer;

    private GameObject currTarget;
    private GameManager m_gm;

    void Start()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        InvokeRepeating("FindTarget", .0f, 1);

        m_gm.wood -= m_gm.turretCost;

        m_gm.turretCost += 5;

        currAmmo = maxAmmo;
    }

    void Update()
    {

        if (m_gm.isMenuUp)
        {
            return;
        }

        m_timer += Time.deltaTime;

        if (currTarget == null)
        {
            FindTarget();
        }
        else
        {
            transform.LookAt(currTarget.transform);

            if (currAmmo > 0)
            {
                if (m_timer >= fireRate)
                {
                    m_timer = .0f;
                    currTarget.GetComponent<EnemyBase>().TakeDamage(damage);
                }
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
    }

    private void FindTarget()
    {
        float minDistance = float.MaxValue;
        foreach(GameObject enemy in m_gm.enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                if(distance < fireRange)
                {
                    currTarget = enemy;
                    minDistance = distance; 
                }
            }
        }
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        currAmmo = maxAmmo;
    }
}
