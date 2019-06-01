using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soldier : UnitTypeBase
{
    public bool selected;

    [Header("UI")]
    public Canvas m_canvas;
    Slider healthBar;

    GameObject m_body;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        m_canvas = GetComponentInChildren<Canvas>();
        m_canvas.worldCamera = m_unitRef.m_cam;

        healthBar = GetComponentInChildren<Slider>();
        m_canvas.enabled = false;

        Unit.m_gm.soldiers.Add(gameObject);

        SetMaterials();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (m_canvas.enabled)
        {
            healthBar.transform.LookAt(transform.position + m_unitRef.m_cam.transform.rotation * Vector3.forward
                , m_unitRef.m_cam.transform.rotation * Vector3.up);
        }
    }

    protected override void HandleAI()
    {
        base.HandleAI();

        switch (intentions)
        {
            default:
            case AIIntentions.idle:
                if(Input.GetMouseButtonDown(0) && selected)
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f);
                    m_navAgent.SetDestination(hit.point);
                }
                break;
        }
    }

    protected override void HandleInteractions(Collider t_other)
    {
        switch (intentions)
        {
            default:
            case AIIntentions.idle:
                return;
        }
    }

    void SetMaterials()
    {
        foreach (Transform meshPart in GetComponentInChildren<Renderer>().transform)
        {
            meshPart.GetComponent<Renderer>().material = m_unitRef.teamMat;
        }
    }
}
