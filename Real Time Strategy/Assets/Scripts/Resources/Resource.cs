using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public static Transform resourcesParent;

    public int resourcesLeft;
    public int maxResources;

    public ResourceType resourceType;

    static GameManager m_gm;

    Renderer m_rend;

    // Start is called before the first frame update
    void Start()
    {
        if (m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        m_rend = GetComponentInChildren<Renderer>();

        ResourceSetUp();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ResourceSetUp()
    {
        resourcesLeft = maxResources;
        
        Material resMat;
        switch (resourceType)
        {
            default:
                resMat = m_gm.resourceMats[0];
                break;
        }
        m_rend.material = resMat;

        // Finding if the parent for this team has already been created
        if(resourcesParent == null)
        {
            if(GameObject.FindGameObjectWithTag("ResourcesParent") != null)
            {
                resourcesParent = GameObject.FindGameObjectWithTag("ResourcesParent").transform;
            }
            else
            {
                GameObject tParent = new GameObject
                {
                    name = "resourcesParent",
                    tag = "ResourcesParent"
                };
                resourcesParent = tParent.transform;
            }
        }

        transform.parent = resourcesParent;

    }
}
