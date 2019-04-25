using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damage;

    static GameObject m_player;

    bool m_moving;

    Vector3 m_targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("MovementBehaviour", 0, Random.Range(2, 5));

        if(m_player == null)
        {
            m_player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_moving)
        {
            transform.Translate((m_targetPosition - transform.position) * Time.deltaTime, Space.World);

            m_moving = !(Vector3.Distance(transform.position, m_targetPosition) < .2f);
        }
    }

    void MovementBehaviour()
    {
        Vector3 pos = Random.insideUnitSphere;  pos.z = 0;
        m_targetPosition = pos * 15 + m_player.transform.position;

        m_moving = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("Evasive maneuvers");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
