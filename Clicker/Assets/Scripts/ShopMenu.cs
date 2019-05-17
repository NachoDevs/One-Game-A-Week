using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [Header("UI")]
    public Image toggleButtonImage;

    [Header("Items")]
    public GameObject itemPrefab;
    public Transform itemsParent;

    [Space]

    [SerializeField]
    Vector3 m_hidePos = new Vector3(-250, 0, 0);
    [SerializeField]
    Vector3 m_visiblePos = new Vector3(0, 0, 0);

    bool m_isVisible = false;
    bool m_isMoving = false;

    Vector3 m_position;
    Vector3 m_movementDirection;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isMoving)
        {
            m_movementDirection = m_position - transform.position;
            m_movementDirection.y = 0;
            transform.Translate(m_movementDirection * Time.deltaTime * 10);



            if(Mathf.Abs(transform.position.x - m_position.x) < .1f)
            {
                m_isMoving = false;
            }
        }
    }

    public void ToggleMenu()
    {
        m_isVisible = !m_isVisible;
        m_position = (m_isVisible) ? m_visiblePos : m_hidePos;
        m_isMoving = true;

        Vector3 theScale = toggleButtonImage.transform.localScale;
        theScale.y *= -1;
        toggleButtonImage.transform.localScale = theScale;
    }
}
