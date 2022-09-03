using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Store : MonoBehaviour
{
    public float timeScale;

    //ORDERS
    public int customerSpawnQueue;
    public int carSpawnQueue;
    public int ordersBeingTaken;
    public int ordersWaiting;
    public List<Order> orders;
    public List<LineOrder> side1Orders;
    public List<LineOrder> side2Orders;
    public List<LineOrder> quarterOrders;
    public List<LineOrder> mcCafeOrders;
    public GameObject orderText;
    public GameObject orderSideText;
    public GameObject orderDT;
    public GameObject orderLobby;
    public GameObject orderSide1;
    public GameObject orderSide2;
    public GameObject orderQuarter;
    public GameObject orderMcCafe;
    private int[] sidePriority;

    //CUSTOMERS
    public GameObject preLane;
    public GameObject customerCar;

    public GameObject spawn;
    public GameObject customer;

    private int orderNum;
    //private float timeAvg;
    private Vector3[] offset;


    //Order Calls
    public int[] foodCode;
    public int[] fryCode;
    public int[] drinkCode;
    public int[] dessertCode;
    public int[] mcCafeCode;

    public int[] ingredientNeed;
    public int quarterNeed2;


    //LINE
    public bool[] side1Pos;
    public bool[] side2Pos;
    public List<FoodItem>[] foodItems;
    private int foodNum = -1;
    public GameObject foodItem;
    public GameObject foodParent;

    // Start is called before the first frame update
    void Start()
    {
        orders = new List<Order>();
        side1Orders = new List<LineOrder>();
        side2Orders = new List<LineOrder>();
        quarterOrders = new List<LineOrder>();
        mcCafeOrders = new List<LineOrder>();
        foodItems = new List<FoodItem>[2];
        foodItems[0] = new List<FoodItem>();
        foodItems[1] = new List<FoodItem>();
        //Make a new list to handle ready food probably <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        offset = new[] { new Vector3(-630, 80, 0), new Vector3(-630, 80, 0), new Vector3(-450, 90, 0), new Vector3(-450, 90, 0), new Vector3(-450, 90, 0), new Vector3(-450, 90, 0) };

        sidePriority = new int[2];

        foodCode = new int[20];
        drinkCode = new int[13];
        dessertCode = new int[4];
        fryCode = new int[4];
        mcCafeCode = new int[18];

        ingredientNeed = new int[10];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Time.timeScale = timeScale;

        //UpdateCanvas();

        //SPAWNING
        if(carSpawnQueue > 0 && !preLane.GetComponent<ColliderTrigger>().taken)
        {
            Instantiate(customerCar, preLane.transform.position, Quaternion.identity);
            carSpawnQueue--;
        }

        if(customerSpawnQueue > 0)
        {
            Vector3 randPos = new Vector3(Random.Range(spawn.GetComponent<BoxCollider2D>().bounds.min.x,
                                    spawn.GetComponent<BoxCollider2D>().bounds.max.x),
                                    Random.Range(spawn.GetComponent<BoxCollider2D>().bounds.min.y,
                                    spawn.GetComponent<BoxCollider2D>().bounds.max.y),
                                    0);
            Instantiate(customer, randPos, Quaternion.identity);
            customerSpawnQueue--;
        }

        //UPDATING ORDERS TIME
        for (int i = 0; i < orders.Count; i++) orders[i].UpdateTime();
        for (int i = 0; i < side1Orders.Count; i++) side1Orders[i].UpdateTime();
        for (int i = 0; i < side2Orders.Count; i++) side2Orders[i].UpdateTime();
        for (int i = 0; i < quarterOrders.Count; i++) quarterOrders[i].UpdateTime();
        for (int i = 0; i < mcCafeOrders.Count; i++) mcCafeOrders[i].UpdateTime();
    }

    //Handle Screen Orders
    public IEnumerator CreateOrder(float waitTime, bool DT)
    {
        //Stat Update
        Debug.Log("ORDERING!!");
        ordersBeingTaken++;
        yield return new WaitForSeconds(waitTime);
        ordersBeingTaken--;
        ordersWaiting++;
        
        orderNum++;
        if (orderNum == 100) orderNum = 0;

        //DT AND LOBBY ORDERS
        if (DT)
        {
            Offsets(0, 500, -450);
            orders.Add(new Order(orderNum, Instantiate(orderText, new Vector3(0, 0, 0), Quaternion.identity), orderDT, offset[0]));
        }
        else
        {
            Offsets(1, 500, -450);
            orders.Add(new Order(orderNum, Instantiate(orderText, new Vector3(0, 0, 0), Quaternion.identity), orderLobby, offset[1]));
        }


        //Count newest order
        int[][] package = orders[orders.Count - 1].ReadOrder();
        drinkCode = MergeArray(drinkCode, package[0]);
        dessertCode = MergeArray(dessertCode, package[1]);
        foodCode = MergeArray(foodCode, package[2]);
        mcCafeCode = MergeArray(mcCafeCode, package[3]);
        fryCode = MergeArray(fryCode, package[4]);

        //10:1
        ingredientNeed[0] += package[2][0] + package[2][1] + (2 * package[2][2]) + (2 * package[2][3]) + (2 * package[2][4]) + (2 * package[2][6]);
        //Bacon
        ingredientNeed[1] += 2 * package[2][4] + (3 * package[2][9]);
        //Quarter
        ingredientNeed[2] += package[2][7] + (2 * package[2][8]) + package[2][9] + package[2][10];
        quarterNeed2 += package[2][7] + (2 * package[2][8]) + package[2][9] + package[2][10];

        //GRILL SIDE SCREENS
        bool foodOnOrder = false;
        for (int i = 0; i < package[2].Length; i++)
        {
            if (package[2][i] != 0) foodOnOrder = true;
        }

        if (foodOnOrder)
        {
            if (sidePriority[0] <= sidePriority[1])
            {
                sidePriority[0]++;
                Offsets(2, 300, -270);
                side1Orders.Add(new LineOrder(true, orderNum, package[2], Instantiate(orderSideText, new Vector3(0, 0, 0), Quaternion.identity), orderSide1, offset[2]));
            }
            else
            {
                sidePriority[1]++;
                Offsets(3, 300, -270);
                side2Orders.Add(new LineOrder(true, orderNum, package[2], Instantiate(orderSideText, new Vector3(0, 0, 0), Quaternion.identity), orderSide2, offset[3]));
            }
        }

        //Quarter screen
        if (package[2][7] + package[2][8] + package[2][9] + package[2][10] != 0)
        {
            Offsets(4, 300, -270);
            int[] temp = package[2];
            for (int i = 0; i < package[2].Length; i++) if (i != 7 && i != 8 && i != 9 && i != 10) temp[i] = 0;
            quarterOrders.Add(new LineOrder(true, orderNum, temp, Instantiate(orderSideText, new Vector3(0, 0, 0), Quaternion.identity), orderQuarter, offset[4]));
        }

        //McCafe screen
        bool mcCafeOnOrder = false;
        for (int i = 0; i < package[3].Length; i++)
        {
            if (package[3][i] != 0) mcCafeOnOrder = true;
        }
        if (mcCafeOnOrder)
        {
            Offsets(5, 300, -270);
            mcCafeOrders.Add(new LineOrder(false, orderNum, package[3], Instantiate(orderSideText, new Vector3(0, 0, 0), Quaternion.identity), orderMcCafe, offset[5]));
        }

    }


    //Handle Food Assembly
    public void CreateFood(int side, int foodCode)
    {
        foodNum++;
        if (foodNum == 1000) foodNum = 0;
        if (side == 1) foodItems[0].Add(new FoodItem(foodCode, side, foodNum, Instantiate(foodItem, new Vector3(0, 0, 0), Quaternion.identity), foodParent));

        if (side == 2) foodItems[1].Add(new FoodItem(foodCode, side, foodNum, Instantiate(foodItem, new Vector3(0, 0, 0), Quaternion.identity), foodParent));
        

    }






    private void Offsets(int DT, int maxX, int minX)
    {
        offset[DT].x += 180;
        if (offset[DT].x > maxX)
        {
            offset[DT].x = minX;
            offset[DT].y -= 215;
        }
        if (offset[DT].x < minX) offset[DT].x = -minX;
    }

    public void Serve(int offset_pointer, GameObject location, List<LineOrder> type)
    {
        Destroy(location.transform.Find("Order " + type[0].GetOrderNum()).gameObject);
        type.RemoveAt(0);
        offset[offset_pointer].x -= 180;
        for (int i = 0; i < type.Count; i++)
        {
            type[i].Move();
        }
    }

    private int[] MergeArray(int[] a, int[] b)
    {
        for(int i = 0; i < a.Length; i++)
        {
            a[i] += b[i];
        }
        return a;
    }

    private void Fries()
    {

    }

}
