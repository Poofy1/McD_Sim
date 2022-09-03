using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Order : MonoBehaviour
{
    public int[] mealCode;
    public int[] foodCode;
    public int[] fryCode;
    public int[] drinkCode;
    public int[] dessertCode;
    public int[] mcCafeCode;

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

    public string[] drinkKeys = {"Coke", //0
                                 "Diet Coke",
                                 "Sprite",
                                 "DrPepper",
                                 "Fanta Orange",
                                 "Hi-C Orange", //5
                                 "Powerade",
                                 "Sweet Tea",
                                 "Unsweetened Tea",
                                 "Water",
                                 "Apple Juice",//10
                                 "White Milk",
                                 "Choc Milk",
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

    public string[] dessertKey = {"Apple Pie",//0
                                  "Bag 3 Cookies",
                                  "Apple Fritter",
                                  "Blueberry Muffin",//3
                                  };

    public string[] fryKey = {"Kids Fry",//0
                              "Small Fry",
                              "Medium Fry",
                              "Large Fry"//3
                              };


    public Order(int orderNumIn, GameObject textObjInput, GameObject parentPos, Vector3 offset)
    {
        //Initialize
        textObj = textObjInput;
        orderNum = orderNumIn;

        mealCode = new int[7];
        foodCode = new int[foodKey.Length];
        drinkCode = new int[drinkKeys.Length];
        dessertCode = new int[dessertKey.Length];
        fryCode = new int[fryKey.Length];
        mcCafeCode = new int[mcCafeKey.Length];

        int meals = Random.Range(-2, 3);
        int foods = Random.Range(-2, 5);
        int fries = Random.Range(-3, 6);
        int drinks = Random.Range(-5, 4);
        int desserts = Random.Range(-10, 4);
        int mcCafe = Random.Range(-2, 3);

        if (meals <= 0 && foods <= 0 && drinks <= 0 && desserts <= 0 && mcCafe <= 0) drinks = 1;
        else if (Random.Range(0, 50) == 0) foods += Random.Range(0, 4);

        //Choosing meals and drinks
        for (int i = 0; i < meals; i++)
        {
            int mealRand = Random.Range(0, 101);
            if (mealRand < 20) mealRand = 1;
            else if (mealRand >= 20 && mealRand < 30) mealRand = 2;
            else if (mealRand >= 30 && mealRand < 35) mealRand = 3;
            else if (mealRand >= 35 && mealRand < 45) mealRand = 4;
            else if (mealRand >= 45 && mealRand < 75) mealRand = 5;
            else if (mealRand >= 75 && mealRand < 80) mealRand = 6;
            else if (mealRand >= 80) mealRand = 7;

            mealCode[mealRand-1]++;
            fryCode[Random.Range(2, 4)]++;
            drinkCode[Random.Range(0, drinkKeys.Length)]++;
        }

        //Choosing food items and how much
        for (int i = 0; i < foods; i++)
        {
            int foodRepeat = Random.Range(0, 101);
            if (foodRepeat < 35) foodRepeat = 1;
            else if (foodRepeat >= 35 && foodRepeat < 70) foodRepeat = Random.Range(2, 4);
            else if (foodRepeat >= 70 && foodRepeat < 85) foodRepeat = Random.Range(4, 6);
            else if (foodRepeat >= 85 && foodRepeat < 95) foodRepeat = Random.Range(6, 8);
            else if (foodRepeat >= 95 && foodRepeat < 99) foodRepeat = Random.Range(8, 10);
            else  foodRepeat = Random.Range(10, 16);

            int foodRand = Random.Range(0, foodKey.Length);
            for (int a = 0; a < foodRepeat; a++)
            {
                foodCode[foodRand]++;
            }
        }

        //Choosing desserts
        for (int i = 0; i < desserts; i++)
        {
            dessertCode[Random.Range(0, dessertKey.Length)]++;
        }

        //Choosing fries
        for (int i = 0; i < fries; i++)
        {
            fryCode[Random.Range(0, 4)]++;
        }

        //Choosing drinks
        for (int i = 0; i < drinks; i++)
        {
            drinkCode[Random.Range(0, drinkKeys.Length)]++;
        }

        //Choosing McCafe
        for (int i = 0; i < mcCafe; i++)
        {
            mcCafeCode[Random.Range(0, mcCafeKey.Length)]++;
        }


        textObj.gameObject.name = "Order " + orderNum;
        textObj.transform.parent = parentPos.transform;
        textObj.transform.localPosition = offset;
        textObj.transform.localScale = new Vector3(1, 1, 1);

        UpdateCanvas();
    }

    public void UpdateTime()
    {
        time += Time.deltaTime;
        textObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Time: " + System.Math.Truncate(time);
    }

    public void UpdateCanvas()
    {
        string tempText = "";

        tempText += ItemSectionWrite(drinkKeys, drinkCode);
        tempText += ItemSectionWrite(mcCafeKey, mcCafeCode);
        tempText += ItemSectionWrite(dessertKey, dessertCode);
        tempText += ItemSectionWrite(foodKey, foodCode);
        tempText += ItemSectionWrite(fryKey, fryCode);

        textObj.GetComponent<TextMeshProUGUI>().text = tempText;

    }

    private string ItemSectionWrite(string[] keys, int[] codes)
    {
        string temp = "";
        for (int i = 0; i < keys.Length; i++)
        {
            if (codes[i] != 0)
            {
                orderLength++;
                temp += "" + codes[i] + " " + keys[i] + "\n";
            }
        }
        if (temp.Length != 0)
        {
            orderLength++;
            temp += "-----------------\n";
        }
        return temp;
    }

    //Return values
    public int[][] ReadOrder()
    {
        return new int[][] {drinkCode, dessertCode, foodCode, mcCafeCode, fryCode};
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
