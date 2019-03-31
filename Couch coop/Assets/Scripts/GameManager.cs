using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public bool m_timePaused = false;

    public Texture2D cursor;
    public Texture2D cursorClicked;

    public Camera mainCamera;

    Enemy selectedEnemy;

    RaycastHit2D m_hit;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            m_hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if(m_hit.collider == null)
            {
                return;
            }

            Enemy enemy = m_hit.collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                if(selectedEnemy != null)
                {
                    selectedEnemy.isControlled = false;
                    selectedEnemy.selectedImage.SetActive(false);

                    if (enemy == selectedEnemy)
                    {
                        selectedEnemy = null;
                        return;
                    }
                }

                enemy.isControlled = true;
                enemy.selectedImage.SetActive(true);

                selectedEnemy = enemy;
            }
        }
    }

    void OnMouseDown()
    {
        Cursor.SetCursor(cursorClicked, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseUp()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }
}
