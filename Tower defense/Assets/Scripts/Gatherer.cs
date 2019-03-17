using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : MonoBehaviour
{
    public bool isChoppingWood = false;

    public int maxWood = 5;

    public float wood;
    public float speed = 5;

    private GameManager m_gm;

    private Rigidbody m_rigidbody;
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        transform.position = m_gm.recolectorSpawnPoint.position;
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
        ++m_gm.gathererCost;
    }

    // Update is called once per frame
    void Update()
    {

        if (m_gm.isMenuUp)
        {
            return;
        }

        if (wood <= maxWood)
        {
            if (!isChoppingWood)
            {
                if(!PositionIsClose(transform.position, m_gm.gatherPoint.position))
                {
                    //m_rigidbody.MovePosition(m_gm.gatherPoint.position * Time.deltaTime);
                    transform.position = Vector3.Lerp(transform.position, m_gm.gatherPoint.position, speed * Time.deltaTime);
                    transform.LookAt(m_gm.gatherPoint);
                }
                else
                {
                    isChoppingWood = true;
                    m_animator.SetBool("isChopping", isChoppingWood);
                }
            }
            else
            {
                wood += Time.deltaTime;
            }
        }
        else
        {
            isChoppingWood = false;
            m_animator.SetBool("isChopping", isChoppingWood);

            if (!PositionIsClose(transform.position, m_gm.recolectPoint.position))
            {
                //m_rigidbody.MovePosition(m_gm.recolectPoint.position * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, m_gm.recolectPoint.position, speed * Time.deltaTime);
                transform.LookAt(m_gm.recolectPoint);
            }
            else
            {
                wood = maxWood;
                m_gm.wood += (int) wood;
                wood = 0;
            }
        }
    }

    bool PositionIsClose(Vector3 t_posA, Vector3 t_posB)
    {
        if(Mathf.Abs(t_posA.x - t_posB.x) >= .1f)
        {
            return false;
        }

        if (Mathf.Abs(t_posA.y - t_posB.y) >= .1f)
        {
            return false;
        }

        return true;
    }
}
