using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjMove : MonoBehaviour
{
    ///////////////PATHFINDING
    public Transform target;
    public IAstarAI ai;

    void OnEnable()
    {
        ai = GetComponent<IAstarAI>();
        ai.onSearchPath += Update;
    }

    void OnDisable()
    {
        ai.onSearchPath -= Update;
    }

    void Update()
    {
        if (target != null) ai.destination = target.position;
    }
    ///////////////////////////
    ///


    public void UpdateTitle(string input)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = input;
    }

}
