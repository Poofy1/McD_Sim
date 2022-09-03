using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusButton : MonoBehaviour
{
    
    public GameObject menu;

    public void ButtonPressed()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        menu.SetActive(!menu.activeSelf);
    }
}
