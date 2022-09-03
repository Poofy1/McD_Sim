using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pathfinding;

public class FoodItem : MonoBehaviour
{
    public int foodCode;

    public float time = 0;
    public int orderNum;
    public GameObject Obj;

    public Transform[] position;
    public Transform foodReady;

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

    //Food Codes
    //"Cheeseburger", //0
    //"Hamburger",
    //"Big Mac",
    //"McDouble",
    //"Bacon McDouble",
    //"Double Cheese", //5
    //"McChicken",
    //"Quarter Pounder",
    //"Double Quarter",
    //"Bacon Quarter",
    //"Deluxe Quarter",//10
    //"Filet-O-Fish", 
    //"Crispy Chicken",
    //"Deluxe Cripsy",
    //"Spicy Crispy",
    //"Deluxe Spicy", //15
    //"10 Nugget",
    //"6 Nugget",
    //"4 Nugget" //18

    //ingredientCodes
    //Buns //BM // REG // STEAM // QURT // POT
    //ketc
    //Must
    //Mayo
    //Mac
    //Tarter //5

    //Sliverd
    //Dehids
    //lettuce
    //pickles //1 //2
    //cheese //1 //2     //10

    //quarter
    //nuggets
    //10:1 //1 //2
    //mcChick
    //Crisp
    //Fish    //16


    public FoodItem(int foodInput, int side, int itemNum, GameObject ObjInput, GameObject parentPos)
    {
        //Initialize
        Obj = ObjInput;
        foodCode = foodInput;
        Obj.GetComponent<ObjMove>().UpdateTitle(foodKey[foodCode]);
        //ingredientCode = new int[16];

        //Get positions
        Transform foodPath = GameObject.Find("CrewLocations").transform.Find("GRILL").transform.Find("FoodPath").transform;
        foodReady = foodPath.Find("ReadyFood").transform;

        position = new Transform[11];

        if(side == 1) for (int i = 0; i <= 10; i++) position[i] = foodPath.Find("S1_F" + i).transform;
        else for (int i = 0; i <= 10; i++) position[i] = foodPath.Find("S2_F" + i).transform;

        Obj.gameObject.name = "FoodItem " + itemNum;
        Obj.transform.parent = parentPos.transform;
        Obj.transform.position = position[0].transform.position;




   

    }

    public void UpdateTime()
    {
        time += Time.deltaTime;
        Obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Time: " + System.Math.Truncate(time);
    }

    public IEnumerator UpdateSpeed(float speed)
    {
        Obj.GetComponent<AIPath>().maxSpeed = speed;
        yield return new WaitUntil(() => Obj.GetComponent<ObjMove>().ai.reachedDestination);
        Obj.GetComponent<AIPath>().maxSpeed = 1.5f;
    }

    public void UpdateTarget(Transform input)
    {
        Obj.GetComponent<ObjMove>().target = input;
    }


    public IEnumerator TravelRand(GameObject tempPos)
    {
        UpdateTarget(tempPos.transform);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => Obj.GetComponent<ObjMove>().ai.reachedDestination);
        Destroy(tempPos);
    }

    public int ReadOrder()
    {
        return 0;
    }

    public int GetOrderNum()
    {
        return orderNum;
    }

    public int GetTime()
    {
        return (int)time;
    }
}
