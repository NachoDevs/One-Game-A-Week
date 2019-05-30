using UnityEngine;

public class Unit : MonoBehaviour
{
    public int team;

    public string teamName;

    public UnitType unitType;

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

        UnitSetUp();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void UnitSetUp()
    {
        Material teamMat;
        switch (team)
        {
            default:
                teamMat = m_gm.teamMats[0];
                teamName = "neutral";
                break;
            case 1:
                teamMat = m_gm.teamMats[1];
                teamName = "red";
                break;
            case 2:
                teamMat = m_gm.teamMats[2];
                teamName = "blue";
                break;
        }
        m_rend.material = teamMat;

        // Finding if the parent for this team has already been created
        bool parentFound = false;
        foreach (GameObject teamParent in GameObject.FindGameObjectsWithTag("TeamParent"))
        {
            if (teamParent.name == "unitsParent_" + teamName)
            {
                transform.parent = teamParent.transform;
                parentFound = true;
                break;
            }
        }
        // If not found, create it
        if (!parentFound)
        {
            GameObject tParent = new GameObject
            {
                name = "unitsParent_" + teamName,
                tag = "TeamParent"
            };

            transform.parent = tParent.transform;
        }
    }

}
