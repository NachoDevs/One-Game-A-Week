using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static float fireRate = 3.5f;

    public GameObject projectile;

    static GameObject m_player;

    bool m_moving;

    float m_timer;

    Vector3 m_targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("MovementBehaviour", 0, Random.Range(2, 5));

        if (m_player == null)
        {
            m_player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= fireRate)
        {
            m_timer = .0f;
            Shoot();
        }

        if (m_moving)
        {
            transform.Translate((m_targetPosition - transform.position) * Time.deltaTime, Space.World);

            m_moving = !(Vector3.Distance(transform.position, m_targetPosition) < .2f);
        }
    }

    void Shoot()
    {
        Vector3 shootDirection = (m_player.transform.position - transform.position);
        Vector3 instPos = transform.position + (shootDirection * .1f);
        GameObject proj = Instantiate(projectile, instPos, transform.rotation);
        proj.GetComponent<Rigidbody2D>().AddForce(shootDirection * 25);
    }

    void MovementBehaviour()
    {
        Vector3 pos = Random.insideUnitSphere;  pos.z = 0;
        m_targetPosition = pos * 10 + m_player.transform.position;

        m_moving = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Projectile>() != null || collision.gameObject.GetComponentInParent<Shield>() != null)
        {
            return;
        }

        m_targetPosition = /*(Random.Range(-1, 1) **/ Vector2.Perpendicular(collision.gameObject.GetComponentInParent<Rigidbody2D>().velocity).normalized/*)*/;
        m_moving = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
