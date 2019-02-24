using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        // It was not possible to attach a scene object to the prefab (FindWithTag is not specially efficient...)
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))    // We pop the fruit
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10000))   // Max range for popping the fruit
            {
                if(hit.transform.tag == "fruit")
                {
                    gameManager.money += 10;
                    GetComponent<AudioSource>().Play();
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
