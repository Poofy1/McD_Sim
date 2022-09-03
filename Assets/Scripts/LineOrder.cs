using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineOrder
{
    public int[] code;

    public float time = 0;
    public int orderNum;
    public GameObject textObj;

    private GameObject timeText;
    private int orderLength;


    //Codes
    public string[] foodKey = {"Cheeseburger", //0
                                "Hamburger",
                                "Big Mac",
                                "McDouble",
                                "Bacon McDouble",
                                "Double Cheese", //5
                                "McChicken",
                                "Quarter Pounder",
                                "Double Quarter",
                                "Bacon Quarter",
                                "Deluxe Quarter",//10
                                "Filet-O-Fish", 
                                "Crispy Chicken",
                                "Deluxe Cripsy",
                                "Spicy Crispy",
                                "Deluxe Spicy", //15
                                "Cinnamon Roll",
                                "10 Nugget",
                                "6 Nugget",
                                "4 Nugget" //19
                                };

    public string[] mcCafeKey = {"Orange Juice", //0
                                 "Smoothie",
                                 "Hot Chocolate",
                                 "Shake",
                                 "Macchiato",
                                 "Cappuccino",//5
                                 "Mocha",
                                 "Iced Mocha",
                                 "Latte",
                                 "Iced Latte",
                                 "Americano",//10
                                 "Coffee",
                                 "Decaf",
                                 "Frappe",
                                 "Iced Coffee",
                                 "Flurry",//15
                                 "Cone",
                                 "Sundae"
                                 };

    public LineOrder(bool mode, int orderNumIn, int[] foodNeeded, GameObject textObjInput, GameObject parentPos, Vector3 offset)
    {
        //Initialize
        //mode T = line //mode F = McCafe
        textObj = textObjInput;
        orderNum = orderNumIn;

        code = new int[foodNeeded.Length];
        code = foodNeeded;

        textObj.gameObject.name = "Order " + orderNum;
        textObj.transform.parent = parentPos.transform;
        textObj.transform.localPosition = offset;
        textObj.transform.localScale = new Vector3(1, 1, 1);

        if (mode) UpdateCanvas(foodKey);
        else UpdateCanvas(mcCafeKey);
    }

    public void UpdateTime()
    {
        time += Time.deltaTime;
        textObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Time: " + System.Math.Truncate(time);
    }

    public void UpdateCanvas(string[] key)
    {
        for (int i = 0; i < key.Length; i++)
        {
            if (code[i] != 0)
            {
                orderLength++;
                textObj.GetComponent<TextMeshProUGUI>().text += "" + code[i] + " " + key[i] + "\n";
            }
        }

    }

    public int[] ReadOrder()
    {
        return code;
    }

    public int QuarterCheck()
    {
        return code[7] + (2 * code[8]) + code[9] + code[10];
    }

    public int GetOrderNum()
    {
        return orderNum;
    }

    public int GetTime()
    {
        return (int)time;
    }

    public void Move()
    {
        if(textObj.transform.localPosition.x < -400)
        {
            textObj.transform.localPosition += new Vector3(900, 215, 0);
        }
        else
        {
            textObj.transform.localPosition += new Vector3(-180, 0, 0);
        }
    }
}
