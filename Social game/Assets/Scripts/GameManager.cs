﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpeechBubble m_speechBubble;

    public List<GameObject> fruits;

    Camera m_cam;

    private void Start()
    {
        m_cam = Camera.main;
    }

    void Update()
    {
    }


}

public enum FruitsEnum
{
    apple,
    peach
}
