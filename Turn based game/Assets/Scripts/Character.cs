using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterMovement m_cm;

    void Awake()
    {
        m_cm = GetComponent<CharacterMovement>();
    }

    public void MoveTo(WorldTile t_destination)
    {
        m_cm.MoveTo(t_destination);
    }
}
