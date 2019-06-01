using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// https://www.youtube.com/watch?v=OqADgd05fpQ
public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;

    public float boxWidth;
    public float boxHeight;
    public float boxTop;
    public float boxLeft;

    public Vector2 boxStart;
    public Vector2 boxFinish;
    public Vector2 mouseDragStartPosition;

    public Vector3 currentMousePoint;
    public Vector3 mouseDownPoint;

    public GUIStyle mouseDragSkin;

    public List<GameObject> currentlySelectedUnits = new List<GameObject>();

    public bool mouseDragging;

    public GameObject selectedUnit;

    Camera m_cam;

    GameManager m_gm;

    public enum SelectFSM
    {
        clickOrDrag,
        clickSelect,
        clickDeselect
    }
    public SelectFSM selectFSM;

    private void Awake()
    {
        instance = this;

        m_cam = Camera.main;
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if(!m_gm.isBuilding)
        {
            SelectUnitsFSM();
        }
    }

    private void OnGUI()
    {
        if (mouseDragging)
        {
            GUI.Box(new Rect(boxLeft, boxTop, boxWidth, boxHeight), "", mouseDragSkin);
        }
    }

    private void SelectUnitsFSM()
    {
        switch (selectFSM)
        {
            case SelectFSM.clickOrDrag:
                ClickOrDrag();
                break;
            case SelectFSM.clickSelect:
                SelectSingleUnit();
                break;
            case SelectFSM.clickDeselect:
                DeselectAll();
                break;
        }
    }

    private void ClickOrDrag()
    {
        Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) && !EventSystem.current.IsPointerOverGameObject())
        {
            currentMousePoint = hit.point;
            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                mouseDownPoint = hit.point;

                mouseDragStartPosition = Input.mousePosition;

                print(hit.collider.gameObject.name);

                //click to select unit, or click the ground to deselect all
                if (hit.collider.gameObject.tag == "Selectable")
                {
                    selectedUnit = hit.collider.transform.parent.gameObject;
                    selectFSM = SelectFSM.clickSelect;
                }
                else /*if (hit.collider.gameObject.tag == "Terrain")*/
                {
                    selectFSM = SelectFSM.clickDeselect;
                }
            }
            else if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
            {
                //holding shift, click to select units or click selected units to deselect
                if (hit.collider.gameObject.tag == "Selectable" && !currentlySelectedUnits.Contains(hit.collider.gameObject))
                {
                    AddToCurrentlySelectedUnits(hit.collider.transform.parent.gameObject);
                }
                else if (hit.collider.gameObject.tag == "Selectable" && currentlySelectedUnits.Contains(hit.collider.gameObject))
                {
                    RemoveFromCurrentlySelectedUnits(hit.collider.transform.parent.gameObject);
                }
            }
            else if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                if (UserDraggingByPosition(mouseDragStartPosition, Input.mousePosition))
                {
                    mouseDragging = true;
                    DrawDragBox();
                    SelectUnitsInDrag();
                }
            }
            else if (Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                mouseDragging = false;
            }
        }
    }

    private void SelectSingleUnit()
    {
        if (selectedUnit != null)
        {
            if (currentlySelectedUnits.Count > 0)
            {
                for (int i = 0; i < currentlySelectedUnits.Count; i++)
                {
                    currentlySelectedUnits[i].GetComponent<Soldier>().m_canvas.enabled = false;
                    currentlySelectedUnits[i].GetComponent<Soldier>().selected = false;
                    currentlySelectedUnits.Remove(currentlySelectedUnits[i]);
                }
            }
            else if (currentlySelectedUnits.Count == 0)
            {
                AddToCurrentlySelectedUnits(selectedUnit);
                selectFSM = SelectFSM.clickOrDrag;
            }
        }
    }

    private void DrawDragBox()
    {
        boxWidth = m_cam.WorldToScreenPoint(mouseDownPoint).x - m_cam.WorldToScreenPoint(currentMousePoint).x;
        boxHeight = m_cam.WorldToScreenPoint(mouseDownPoint).y - m_cam.WorldToScreenPoint(currentMousePoint).y;
        boxLeft = Input.mousePosition.x;
        boxTop = (Screen.height - Input.mousePosition.y) - boxHeight; //need to invert y as GUI space has 0,0 at top left, but Screen space has 0,0 at bottom left. x is the same. 

        if (boxWidth > 0 && boxHeight < 0f)
        {
            boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (boxWidth > 0 && boxHeight > 0f)
        {
            boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y + boxHeight);
        }
        else if (boxWidth < 0 && boxHeight < 0f)
        {
            boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y);
        }
        else if (boxWidth < 0 && boxHeight > 0f)
        {
            boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y + boxHeight);
        }

        boxFinish = new Vector2(boxStart.x + Mathf.Abs(boxWidth), boxStart.y - Mathf.Abs(boxHeight));
    }

    private bool UserDraggingByPosition(Vector2 dragStartPoint, Vector2 newPoint)
    {
        return ((newPoint.x > dragStartPoint.x || newPoint.x < dragStartPoint.x)
            || (newPoint.y > dragStartPoint.y || newPoint.y < dragStartPoint.y));
    }

    private void SelectUnitsInDrag()
    {
        foreach (GameObject go in m_gm.soldiers)
        {
            Unit goU = go.GetComponent<Unit>();
            Vector2 unitScreenPosition = m_cam.WorldToScreenPoint(go.transform.position);

            if (unitScreenPosition.x < boxFinish.x && unitScreenPosition.y > boxFinish.y && unitScreenPosition.x > boxStart.x && unitScreenPosition.y < boxStart.y)
            {
                AddToCurrentlySelectedUnits(go);
            }
            else
            {
                RemoveFromCurrentlySelectedUnits(go);
            }
        }
    }

    private void AddToCurrentlySelectedUnits(GameObject unitToAdd)
    {
        if (!currentlySelectedUnits.Contains(unitToAdd))
        {
            unitToAdd.GetComponent<Soldier>().m_canvas.enabled = true;
            unitToAdd.GetComponent<Soldier>().selected = true;
            currentlySelectedUnits.Add(unitToAdd);
        }
    }

    private void RemoveFromCurrentlySelectedUnits(GameObject unitToRemove)
    {
        if (currentlySelectedUnits.Count > 0)
        {
            unitToRemove.GetComponent<Soldier>().m_canvas.enabled = false;
            unitToRemove.GetComponent<Soldier>().selected = false;
            currentlySelectedUnits.Remove(unitToRemove);
        }
    }

    private void DeselectAll()
    {
        selectedUnit = null;
        if (currentlySelectedUnits.Count > 0)
        {
            for (int i = 0; i < currentlySelectedUnits.Count; i++)
            {
                currentlySelectedUnits[i].GetComponent<Soldier>().m_canvas.enabled = false;
                currentlySelectedUnits[i].GetComponent<Soldier>().selected = false;
                currentlySelectedUnits.Remove(currentlySelectedUnits[i]);
            }
        }
        else if (currentlySelectedUnits.Count == 0)
        {
            selectFSM = SelectFSM.clickOrDrag;
        }
    }
}
