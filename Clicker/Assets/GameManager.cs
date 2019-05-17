using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float currentPoints = 0;

    [Header("UI")]
    public TextMeshProUGUI pointsText;

    Camera m_mainCamera1;
    Clicker m_clicker;

    RaycastHit2D m_hit;

    // Start is called before the first frame update
    void Start()
    {
        m_mainCamera1 = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = ((int)currentPoints).ToString();

        if (Input.GetMouseButtonUp(0))
        {
            m_hit = Physics2D.Raycast(m_mainCamera1.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (m_hit.collider == null)
            {
                return;
            }

            m_clicker = m_hit.collider.GetComponentInParent<Clicker>();
            if(m_clicker != null)
            {
                AddPoints(m_clicker.clickPoints);
            }
        }
    }

    public void AddPoints(float t_points)
    {
        currentPoints += t_points;
    }
}
