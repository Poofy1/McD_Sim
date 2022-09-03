using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pathfinding;

public class StaffAI : MonoBehaviour
{
    private Store storeComp;
    private FloorPlanner staffing;
    private Inventory inventory;
    public Transform self;
    public GameObject emptyPos;
    public GameObject carryObj;

    //MANAGER
    private Transform overlook;
    private Transform[] desk;
    private Transform complaint;
    private Transform help;

    //GRILL
    private Transform[] meats;
    private Transform[] fries;
    private Transform[] backline;
    private Transform[] S1;
    private Transform[] S2;
    private Transform[] buns;
    private Transform[] grillFridge;

    //FRONT
    private Transform[] driveRun;
    private Transform windowRun;
    private Transform windowPresent;
    private Transform[] present;
    private Transform drinks;
    private Transform[] happyMeals;
    private Transform frappe;
    private Transform expresso;
    private Transform oj;
    private Transform coffee;
    private Transform[] ice;
    private Transform iceCoffeeM;
    private Transform[] iceCreamM;
    private Transform flurry;
    private Transform slush;
    private Transform[] storage;
    private Transform[] register;
    private Transform[] holds;
    private Transform mcCafeIdle;

    //STORAGE
    private Transform[] dish;
    private Transform walkInF;
    private Transform freezer;
    private Transform[] skids;
    private Transform[] soda;

    //OTHER
    private Transform[] handWash;
    private Transform clock;
    private Transform[] prep;
    private Transform breaks;
    private Transform posWait;
    private GameObject foodItem;

    //VARS
    private bool clockedIn;
    private Color defaultCol;
    private bool isShiftLead;
    private bool carryingInv;


    //PATHFINDING
    private Transform target;
    IAstarAI ai;

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


    void Start()
    {
        //INITIALIZE
        Transform crewLoc = GameObject.Find("CrewLocations").transform;
        Transform grillLoc = crewLoc.Find("GRILL").transform;
        Transform frontLoc = crewLoc.Find("FRONT").transform;
        Transform storeLoc = crewLoc.Find("STORAGE").transform;
        Transform manLoc = crewLoc.Find("MANAGER").transform;
        Transform otherLoc = crewLoc.Find("OTHER").transform;
        storeComp = GameObject.Find("StoreComputer").GetComponent<Store>();
        staffing = GameObject.Find("Staff").GetComponent<FloorPlanner>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        foodItem = storeComp.foodItem;

        //MANAGER 
        overlook = manLoc.Find("Overlook").transform;
        complaint = manLoc.Find("Complaint0").transform;
        help = manLoc.Find("Help").transform;
        desk = new Transform[3];
        for (int i = 0; i <= 2; i++) desk[i] = manLoc.Find("Desk"+i).transform;

        //GRILL
        meats = new Transform[4];
        for (int i = 0; i <= 3; i++) meats[i] = grillLoc.Find("Meats" + i).transform;
        fries = new Transform[3];
        for (int i = 0; i <= 2; i++) fries[i] = grillLoc.Find("Fries" + i).transform;
        backline = new Transform[6];
        for (int i = 0; i <= 5; i++) backline[i] = grillLoc.Find("Backline" + i).transform;
        S1 = new Transform[8];
        for (int i = 0; i <= 7; i++) S1[i] = grillLoc.Find("S1_" + i).transform;
        S2 = new Transform[8];
        for (int i = 0; i <= 7; i++) S2[i] = grillLoc.Find("S2_" + i).transform;
        buns = new Transform[2];
        for (int i = 0; i <= 1; i++) buns[i] = grillLoc.Find("Bun" + i).transform;
        grillFridge = new Transform[2];
        for (int i = 0; i <= 1; i++) grillFridge[i] = grillLoc.Find("Fridge" + i).transform;

        //FRONT
        windowRun = frontLoc.Find("Windowrun").transform;
        windowPresent = frontLoc.Find("WindowPresent").transform;
        drinks = frontLoc.Find("Drinks").transform;
        frappe = frontLoc.Find("Frappe").transform;
        expresso = frontLoc.Find("Expresso").transform;
        oj = frontLoc.Find("OJ").transform;
        coffee = frontLoc.Find("Coffee").transform;
        flurry = frontLoc.Find("Flurry").transform;
        slush = frontLoc.Find("Slush").transform;
        iceCoffeeM = frontLoc.Find("IceCoffeeM").transform;
        mcCafeIdle = frontLoc.Find("McCafeIdle").transform;
        driveRun = new Transform[2];
        for (int i = 0; i <= 1; i++) driveRun[i] = frontLoc.Find("Driverun" + i).transform;
        present = new Transform[2];
        for (int i = 0; i <= 1; i++) present[i] = frontLoc.Find("Present" + i).transform;
        happyMeals = new Transform[2];
        for (int i = 0; i <= 1; i++) happyMeals[i] = frontLoc.Find("Happymeals" + i).transform;
        ice = new Transform[2];
        for (int i = 0; i <= 1; i++) ice[i] = frontLoc.Find("Ice" + i).transform;
        iceCreamM = new Transform[2];
        for (int i = 0; i <= 1; i++) iceCreamM[i] = frontLoc.Find("IceC" + i).transform;
        storage = new Transform[9];
        for (int i = 0; i <= 8; i++) storage[i] = frontLoc.Find("Storage" + i).transform;
        register = new Transform[7];
        for (int i = 0; i <= 6; i++) register[i] = frontLoc.Find("R" + i).transform;
        holds = new Transform[4];
        for (int i = 0; i <= 3; i++) holds[i] = frontLoc.Find("RH" + i).transform;

        //STORAGE
        walkInF = storeLoc.Find("Fridge").transform;
        freezer = storeLoc.Find("Freezer").transform;
        skids = new Transform[2];
        for (int i = 0; i <= 1; i++) skids[i] = storeLoc.Find("Skid" + i).transform;
        soda = new Transform[2];
        for (int i = 0; i <= 1; i++) soda[i] = storeLoc.Find("Soda" + i).transform;

        //OTHER
        breaks = otherLoc.Find("Breaks").transform;
        posWait = otherLoc.Find("PositionWait").transform;
        clock = otherLoc.Find("Clock");
        dish = new Transform[3];
        for (int i = 0; i <= 2; i++) dish[i] = otherLoc.Find("Dish" + i).transform;
        handWash = new Transform[2];
        for (int i = 0; i <= 1; i++) handWash[i] = otherLoc.Find("HW" + i).transform;
        prep = new Transform[3];
        for (int i = 0; i <= 2; i++) prep[i] = otherLoc.Find("Prep" + i).transform;

        defaultCol = GameObject.Find("Circle").GetComponentInChildren<SpriteRenderer>().color;

        //Start Pathing
        StartCoroutine(Spawned());
    }





    /////////////////FUNCTIONS
    IEnumerator TravelWait(Transform pos, float wait)
    {
        target = pos;
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => ai.reachedDestination);
        yield return new WaitForSeconds(wait);
    }

    IEnumerator TravelRandWait(Transform pos, float wait)
    {
        GameObject tempPos = Instantiate(emptyPos, RandPos(pos), Quaternion.identity);
        yield return StartCoroutine(TravelWait(tempPos.transform, 1));
        Destroy(tempPos);
    }

    IEnumerator BrainTick()
    {
        yield return new WaitForSeconds(1);
    }

    private Vector3 RandPos(Transform pos)
    {
        return new Vector3(Random.Range(pos.GetComponent<BoxCollider2D>().bounds.min.x,
                           pos.GetComponent<BoxCollider2D>().bounds.max.x),
                           Random.Range(pos.GetComponent<BoxCollider2D>().bounds.min.y,
                           pos.GetComponent<BoxCollider2D>().bounds.max.y),
                           0);
    }

    public IEnumerator Spawned()
    {
        //ClockIn
        yield return StartCoroutine(ShiftClock());
        //Is Shift Lead?
        if (!staffing.floorRunner) StartCoroutine(ShiftLead());
        else StartCoroutine(WaitForDuty());
    }

    IEnumerator ShiftClock()
    {
        yield return StartCoroutine(TravelRandWait(clock, 2));
        yield return StartCoroutine(TravelRandWait(handWash[0], 5));
    }

    IEnumerator CarryItem(string title)
    {
        //Spawn
        carryingInv = true;
        var item = Instantiate(carryObj, new Vector3(0,0,0), Quaternion.identity);
        item.transform.parent = self;
        item.transform.localPosition = new Vector3(0, .1f, 0);
        item.transform.localEulerAngles = new Vector3(0, 0, 0);
        GetComponentInChildren<TextMeshProUGUI>().text = title;

        //Wait
        yield return new WaitUntil(() => !carryingInv);
        Destroy(item);
    }






    //////////LOGIC 
    IEnumerator ShiftLead()
    {
        isShiftLead = true;
        staffing.floorRunner = true;
        self.Find("Circle").Find("Color").GetComponent<SpriteRenderer>().color = new Color(1, .5f, .6f);
        yield return StartCoroutine(TravelWait(overlook, 3));

        int totalCrew = staffing.managers - 1 + staffing.crew;
        int totalUnder = staffing.under16;

        while (isShiftLead)
        {
            //Brain tick
            StartCoroutine(BrainTick());

            //People waiting for position?
            if (staffing.posWaitQ > 0)
            {
                yield return StartCoroutine(TravelRandWait(posWait, 1));

                staffing.runnerAtDutyCommand = true;
                //Pretend to give orders
                yield return new WaitUntil(() => staffing.posWaitQ == 0);
                yield return new WaitForSeconds(Random.Range(1f, 4f));
                staffing.runnerAtDutyCommand = false;
            }

            if(totalCrew <= 9 && !staffing.driveRun)
            {
                staffing.driveRun = true;
                yield return StartCoroutine(TravelWait(driveRun[0], 1));
            }
            else if (totalCrew <= 12 && !staffing.windowRun)
            {
                staffing.windowRun = true;
                yield return StartCoroutine(TravelWait(windowRun, 1));
            }
            else yield return StartCoroutine(TravelWait(overlook, 1));



        }
    }
    
    IEnumerator WaitForDuty()
    {
        StartCoroutine(TravelRandWait(posWait, 1));

        staffing.posWaitQ++;
        yield return new WaitUntil(() => staffing.runnerAtDutyCommand);
        yield return new WaitForSeconds(Random.Range(2f, 6f));
        staffing.posWaitQ--;
        if (!staffing.register[5])
        {
            staffing.register[5] = true;
            yield return StartCoroutine(TravelWait(register[5], 1));
        }
        if (!staffing.register[4])
        {
            staffing.register[4] = true;
            yield return StartCoroutine(TravelWait(register[4], 1));
        }
        else if (!staffing.meats)
        {
            staffing.meats = true;
            yield return StartCoroutine(TravelWait(meats[0], 1));
            StartCoroutine(Meat1Pos());
        }
        else if (staffing.side1 == 0)
        {
            staffing.side1++;
            yield return StartCoroutine(Line(1, storeComp.side1Pos));
        }
        else if (!staffing.mcCafe)
        {
            staffing.mcCafe = true;
            yield return StartCoroutine(TravelWait(mcCafeIdle, 1));
        }
        else if (staffing.side2 == 0)
        {
            staffing.side2++;
            yield return StartCoroutine(Line(2, storeComp.side2Pos));
        }
        else if (!staffing.fries)
        {
            staffing.fries = true;
            yield return StartCoroutine(TravelWait(fries[0], 1));
        }
        else if (staffing.side1 == 1)
        {
            staffing.side1++;
            yield return StartCoroutine(Line(1, storeComp.side1Pos));
        }
        else if (!staffing.present)
        {
            staffing.present = true;
            yield return StartCoroutine(TravelWait(present[0], 1));
        }
        //10 \/
        else if (!staffing.driveRun)
        {
            staffing.driveRun = true;
            yield return StartCoroutine(TravelWait(driveRun[0], 1));
        }
        else if (!staffing.backline)
        {
            staffing.backline = true;
            yield return StartCoroutine(TravelWait(backline[3], 1));
            StartCoroutine(BacklinePos());
        }
        else if (!staffing.register[0])
        {
            staffing.register[0] = true;
            yield return StartCoroutine(TravelWait(register[0], 1));
        }
        else if (!staffing.register[6])
        {
            staffing.register[6] = true;
            yield return StartCoroutine(TravelWait(register[6], 1));
        }
        else if (!staffing.windowRun)
        {
            staffing.windowRun = true;
            yield return StartCoroutine(TravelWait(windowRun, 1));
        }
    }


    //POSITION MEATS
    IEnumerator Meat1Pos()
    {
        int[] levelsTarget = new int[2];
        int[] amount = new int[4];

        //Initial Check Levels
        yield return StartCoroutine(TravelWait(S2[6], 3));

        while (true)
        {
            yield return StartCoroutine(BrainTick());

            //Determine Levels
            if (staffing.customerAmount == 0 && storeComp.orders.Count == 0) levelsTarget = new int[2] { 8, 2 };
            else if (staffing.customerAmount <= 4) levelsTarget = new int[2] { 12, 2 };
            else if (staffing.customerAmount <= 8) levelsTarget = new int[2] { 12, 3 };
            else if (staffing.customerAmount <= 12) levelsTarget = new int[2] { 16, 3 };
            else if (staffing.customerAmount <= 16) levelsTarget = new int[2] { 16, 4 };
            else levelsTarget = new int[2] { 24, 4 };

            //Current orders on screen = more inticipation
            if (storeComp.orders.Count <= 5 && levelsTarget[0] != 24) levelsTarget[0] = 16;

            int currentTrays = 0;
            for (int i = 2; i <= 5; i++)
            {
                if (inventory.beef[i] != 0) currentTrays++;
            }

            //Drop quarter?
            for (int i = 2; i >= 0; i--)
            {
                if (inventory.StoveStatus[i+1] == 0 && storeComp.ingredientNeed[2] > 0)
                {
                    //if 3? else
                    if (storeComp.ingredientNeed[2] >= 3) amount[i+1] = 3;
                    else amount[i + 1] = storeComp.ingredientNeed[2];

                    
                    storeComp.ingredientNeed[2] -= amount[i + 1];
                    inventory.StoveStatus[i+1] = 1;


                    //drop
                    yield return StartCoroutine(TravelWait(meats[3], 3));
                    inventory.UpdateInv(inventory.quarterTEXT, inventory.quarter, 1, -amount[i + 1], new int[] { 255 }, 3);
                    StartCoroutine(inventory.StoveCountdown(i+3, 110, amount[i + 1]));
                    StartCoroutine(CarryItem("Quarter"));
                    yield return StartCoroutine(TravelWait(meats[2], 5));
                    carryingInv = false;
                }
            }

            //Pickup Beef
            for (int a = 0; a <= 3; a++)
            {
                if (inventory.StoveStatus[a] == 2)
                {
                    inventory.StoveStatus[a] = 0;
                    if (a == 0)
                    {
                        yield return StartCoroutine(TravelWait(meats[1], 5));
                        for (int i = 0; i <= 2; i++)
                        {
                            inventory.beefIndicator[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                            inventory.beefIndicator[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                        }
                        StartCoroutine(CarryItem("10:1"));
                        yield return StartCoroutine(TravelWait(S2[7], 2));
                        carryingInv = false;
                        inventory.UpdateCabnietInv(inventory.beefTEXT, inventory.beef, GetNextTray(inventory.beef, 5), amount[0], 900, 500, 5);
                    }
                    else
                    {
                        //pickup
                        yield return StartCoroutine(TravelWait(meats[2], 5));
                        inventory.beefIndicator[a+2].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                        inventory.beefIndicator[a+2].GetComponentInChildren<TextMeshProUGUI>().text = "";
                        storeComp.quarterNeed2 -= amount[a];
                        
                        //Need more on this order?
                        int stillNeed = storeComp.quarterNeed2;
                        for (int i = 1; i < storeComp.quarterOrders.Count; i++) stillNeed -= storeComp.quarterOrders[i].QuarterCheck();
                      

                        //serve?
                        if (stillNeed <= 0 && storeComp.quarterOrders.Count > 0)
                        {
                            storeComp.Serve(4, storeComp.orderQuarter, storeComp.quarterOrders);
                            while (stillNeed < 0)
                            {
                                storeComp.Serve(4, storeComp.orderQuarter, storeComp.quarterOrders);
                                stillNeed += storeComp.quarterOrders[0].QuarterCheck();       
                            }
                        }


                        //move
                        StartCoroutine(CarryItem("Quarter"));
                        yield return StartCoroutine(TravelWait(S2[6], 2));
                        carryingInv = false;
                        inventory.UpdateInv(inventory.quarterTEXT, inventory.quarter, 2, amount[a], new int[] { 255 }, 3);
                    }

                    //Transport Meat
                    yield return StartCoroutine(TravelWait(meats[0], 1));
                }
            }


            //Determine how much to put down
            if (inventory.beef[1] > 100)
            {
                if(inventory.StoveStatus[0] == 0 && levelsTarget[1] - currentTrays > 0)
                {
                    if (inventory.beef[GetNextTray(inventory.beef, 5)] != 0) yield return StartCoroutine(TravelWait(meats[0], 1));
                    else
                    {
                        yield return StartCoroutine(DropBeef(levelsTarget[0]));
                        amount[0] = levelsTarget[0];
                    }
                }
            }


            if (inventory.beef[0] > 0 && inventory.beef[1] < 100)
            {
                int grab = 500 - inventory.beef[1];
                if (grab > inventory.beef[0]) grab = inventory.beef[0];
                yield return StartCoroutine(TravelRandWait(freezer, 5));
                inventory.UpdateCabnietInv(inventory.beefTEXT, inventory.beef, 0, -grab, 900, 500, 5);
                StartCoroutine(CarryItem("10:1"));
                yield return StartCoroutine(TravelWait(meats[0], 5));
                carryingInv = false;
                inventory.UpdateCabnietInv(inventory.beefTEXT, inventory.beef, 1, grab, 900, 500, 5);
            }
        }
    }

    IEnumerator DropBeef(int amount)
    {
        yield return StartCoroutine(TravelWait(meats[0], 5));
        if (inventory.beef[1] > amount)
        {
            inventory.UpdateCabnietInv(inventory.beefTEXT, inventory.beef, 1, -amount, 900, 500, 5);
            inventory.StoveStatus[0] = 1;

            //Put down meat
            inventory.BeefMachine(amount);
            StartCoroutine(CarryItem("10:1"));
            yield return StartCoroutine(TravelWait(meats[1], 5));
            carryingInv = false;
        }
    }




    //POSITION BACKLINE
    IEnumerator BacklinePos()
    {
        int[] levelsTarget = new int[3];
        int[] trayTarget = new int[4];
        int[] amount = new int[3];

        //Initial Check Levels
        yield return StartCoroutine(TravelWait(S1[7], 4));

        int[] currentTrays = new int[4];
        for (int i = 2; i <= 4; i++) if (inventory.fish[i] != 0) currentTrays[0]++;
        for (int i = 2; i <= 5; i++) if (inventory.crispy[i] != 0) currentTrays[1]++;
        for (int i = 2; i <= 4; i++) if (inventory.mcChicken[i] != 0) currentTrays[2]++;
        for (int i = 2; i <= 5; i++) if (inventory.nuggets[i] != 0) currentTrays[3]++;

        while (true)
        {
            yield return StartCoroutine(BrainTick());

            //Determine Levels
            if (staffing.customerAmount == 0 && storeComp.orders.Count == 0)
            {
                levelsTarget = new int[3] { 2, 2, 6};
                trayTarget = new int[4] { 2, 2, 2, 2};
            }
            else if (staffing.customerAmount <= 4)
            {
                levelsTarget = new int[3] { 3, 4, 6 };
                trayTarget = new int[4] { 2, 2, 2, 2 };
            }
            else if (staffing.customerAmount <= 8)
            {
                levelsTarget = new int[3] { 4, 4, 6 };
                trayTarget = new int[4] { 2, 2, 2, 3 };
            }
            else if (staffing.customerAmount <= 12)
            {
                levelsTarget = new int[3] { 4, 4, 6 };
                trayTarget = new int[4] { 2, 2, 3, 3 };
            }
            else if (staffing.customerAmount <= 16)
            {
                levelsTarget = new int[3] { 4, 4, 6 };
                trayTarget = new int[4] { 3, 2, 3, 4 };
            }
            else
            {
                levelsTarget = new int[3] { 6, 4, 6 };
                trayTarget = new int[4] { 3, 2, 4, 4 };
            }




            //Drop Fish?
            if (inventory.fish[1] > levelsTarget[0] && trayTarget[0] - currentTrays[0] > 0 && inventory.fish[GetNextTray(inventory.fish, 4)] == 0)
            {
                int vat = -1;
                if (inventory.BacklineStatus[11] == 0) vat = 11;
                else if (inventory.BacklineStatus[10] == 0) vat = 10;

                if (vat != -1)
                {
                    yield return StartCoroutine(TravelWait(backline[2], 10));
                    StartCoroutine(CarryItem("Fish"));
                    inventory.UpdateCabnietInv(inventory.fishTEXT, inventory.fish, 1, -levelsTarget[0], 1800, 200, 4);
                    yield return StartCoroutine(DropBackline(vat, 430, levelsTarget[0]));
                    carryingInv = false;
                    currentTrays[0]++;
                    amount[0] = levelsTarget[0];
                }
            }

            //Drop Nuggets?
            if (inventory.nuggets[1] > 0 && trayTarget[3] - currentTrays[3] > 0 && inventory.nuggets[GetNextTray(inventory.nuggets, 5)] == 0)
            {
                int vat = -1;
                if (inventory.BacklineStatus[9] == 0) vat = 9;
                else if (inventory.BacklineStatus[8] == 0) vat = 8;
                else if (inventory.BacklineStatus[6] == 0) vat = 6;
                else if (inventory.BacklineStatus[7] == 0) vat = 7;

                if (vat != -1)
                {
                    yield return StartCoroutine(TravelWait(backline[2], 10));
                    StartCoroutine(CarryItem("Nuggets"));
                    inventory.UpdateCabnietInv(inventory.nuggetTEXT, inventory.nuggets, 1, -1, 1200, 50, 5);
                    yield return StartCoroutine(DropBackline(vat, 430, 40));
                    carryingInv = false;
                    currentTrays[3]++;
                }
            }

            //Drop Crispy?
            if (inventory.crispy[1] > levelsTarget[1] && trayTarget[1] - currentTrays[1] > 0 && inventory.crispy[GetNextTray(inventory.crispy, 5)] == 0)
            {
                int vat = -1;
                if (inventory.BacklineStatus[4] == 0) vat = 4;
                else if (inventory.BacklineStatus[5] == 0) vat = 5;

                if (vat != -1)
                {
                    yield return StartCoroutine(TravelWait(backline[2], 10));
                    StartCoroutine(CarryItem("Crispy"));
                    inventory.UpdateCabnietInv(inventory.crispyTEXT, inventory.crispy, 1, -levelsTarget[1], 3600, 200, 4);
                    yield return StartCoroutine(DropBackline(vat, 430, levelsTarget[1]));
                    carryingInv = false;
                    currentTrays[1]++;
                    amount[1] = levelsTarget[1];
                }
            }

            //Drop McChicken?
            if (inventory.mcChicken[1] > levelsTarget[2] && trayTarget[2] - currentTrays[2] > 0 && inventory.mcChicken[GetNextTray(inventory.mcChicken, 4)] == 0)
            {
                int vat = -1;
                if (inventory.BacklineStatus[2] == 0) vat = 2;
                else if (inventory.BacklineStatus[3] == 0) vat = 3;
                else if (inventory.BacklineStatus[1] == 0) vat = 1;
                else if (inventory.BacklineStatus[0] == 0) vat = 0;

                if (vat != -1)
                {
                    yield return StartCoroutine(TravelWait(backline[2], 10));
                    StartCoroutine(CarryItem("McChicken"));
                    inventory.UpdateCabnietInv(inventory.mcChickenTEXT, inventory.mcChicken, 1, -levelsTarget[2], 1800, 200, 4);
                    yield return StartCoroutine(DropBackline(vat, 430, levelsTarget[2]));
                    carryingInv = false;
                    currentTrays[2]++;
                    amount[2] = levelsTarget[2];
                }
            }


            //PickUp?
            for(int a = 0; a <= 11; a++)
            {
                if (inventory.BacklineStatus[a] == 2)
                {
                    inventory.BacklineStatus[a] = 0;
                    yield return StartCoroutine(TravelWait(backline[4], 2));

                    //Reset
                    inventory.backlineIndicator[a+1].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    inventory.backlineIndicator[a+1].GetComponentInChildren<TextMeshProUGUI>().text = "";

                    //Transport
                    StartCoroutine(CarryItem("Cooked"));
                    yield return StartCoroutine(TravelWait(S1[7], 2));
                    carryingInv = false;

                    if (a > 9) inventory.UpdateCabnietInv(inventory.fishTEXT, inventory.fish, GetNextTray(inventory.fish, 4), amount[0], 1800, 200, 4);
                    else if (a > 5) inventory.UpdateCabnietInv(inventory.nuggetTEXT, inventory.nuggets, GetNextTray(inventory.nuggets, 5), 40, 1200, 50, 5);
                    else if (a > 3) inventory.UpdateCabnietInv(inventory.crispyTEXT, inventory.crispy, GetNextTray(inventory.crispy, 4), amount[1], 3600, 200, 5);
                    else inventory.UpdateCabnietInv(inventory.mcChickenTEXT, inventory.mcChicken, GetNextTray(inventory.mcChicken, 4), amount[2], 1800, 200, 4);
                }
            }

            //Restock??
            if (inventory.fish[0] > 0 && inventory.fish[1] < 25)
            {
                int grab = 250 - inventory.fish[1];
                if (grab > inventory.fish[0]) grab = inventory.fish[0];
                yield return StartCoroutine(TravelRandWait(freezer, 5));
                inventory.UpdateCabnietInv(inventory.fishTEXT, inventory.fish, 0, -grab, 1800, 200, 4);
                yield return StartCoroutine(TravelWait(backline[2], 5));
                inventory.UpdateCabnietInv(inventory.fishTEXT, inventory.fish, 1, grab, 1800, 200, 4);
            }
        }
    }

    IEnumerator DropBackline(int vat, int cookTime, int amount)
    {
        StartCoroutine(inventory.BacklineCountdown(vat, cookTime, amount));
        yield return StartCoroutine(TravelWait(backline[3], 4));
        yield return StartCoroutine(TravelWait(backline[4], 2));
    }

    //TRAYS
    private int GetNextTray(int[] var, int max)
    {
        for (int i = 2; i <= max; i++)
        {
            if (var[i] != 0)
            {
                if (i != max && var[i + 1] == 0) return i + 1;
            }
        }

        return 2;
    }




    //UNIVERSAL STOCK


    //POSITION LINE heavy wip
    IEnumerator Line(int side, bool[] sidePos)
    {
        //Initialize
        int myPos = 0;
        int[] foodCode = new int[18];
        Transform[] crewPos;
        Transform[] foodPos = new Transform[11];
        Transform foodReady = storeComp.foodParent.transform.Find("ReadyFood").transform;
        List<LineOrder> orders;

        if (side == 1)
        {
            crewPos = S1;
            orders = storeComp.side1Orders;
            for (int i = 0; i <= 10; i++) foodPos[i] = storeComp.foodParent.transform.Find("S1_F" + i).transform;
        }
        else
        {
            crewPos = S2;
            orders = storeComp.side2Orders;
            for (int i = 0; i <= 10; i++) foodPos[i] = storeComp.foodParent.transform.Find("S2_F" + i).transform;
        }

        //Position
        if (!sidePos[0])
        {
            sidePos[0] = true;
            myPos = 0;
            yield return StartCoroutine(TravelWait(crewPos[0], 1));
        }
        else if (!sidePos[2])
        {
            sidePos[2] = true;
            myPos = 2;
            yield return StartCoroutine(TravelWait(crewPos[6], 1));
        }
        else if (!sidePos[1])
        {
            sidePos[1] = true;
            myPos = 1;
            yield return StartCoroutine(TravelWait(crewPos[4], 1));
        }
        

        while (true)
        {
            yield return StartCoroutine(BrainTick());

            if(myPos == 0)
            {
                int bookmark = 0;

                if(orders.Count > 0)
                {
                    foodCode = orders[0].ReadOrder();
                    
                    //Start food
                    for(int i = 0; i < foodCode.Length; i++)
                    {
                        if(foodCode[i] > 0)
                        {
                            if (i <= 15)
                            {
                                storeComp.CreateFood(side, i);
                                yield return StartCoroutine(TravelWait(crewPos[0], 1));

                                //Take bun
                                if (i == 2) inventory.UpdateInv(inventory.bunsBmTEXT, inventory.bigMacBuns, 2, -1, new int[] { 6, 36 }, 3);
                                else if (i >= 7 && i <= 10) inventory.UpdateInv(inventory.bunsQuarterTEXT, inventory.quarterBuns, 2, -1, new int[] { 6, 72 }, 3);
                                else if (i >= 12 && i <= 15) inventory.UpdateInv(inventory.bunsPotTEXT, inventory.potatoBuns, 2, -1, new int[] { 4, 72 }, 3);
                                else inventory.UpdateInv(inventory.bunsRegTEXT, inventory.regBuns, side + 1, -1, new int[] { 16, 96, 96 }, 4);

                                //Move Food/Player
                                if (i == 11)
                                {
                                    storeComp.foodItems[side - 1][bookmark].UpdateTarget(foodPos[3]);
                                    yield return StartCoroutine(TravelWait(crewPos[3], 1));
                                }
                                else
                                {
                                    StartCoroutine(storeComp.foodItems[side - 1][bookmark].UpdateSpeed(.1f));
                                    storeComp.foodItems[side - 1][bookmark].UpdateTarget(foodPos[2]);
                                    yield return StartCoroutine(TravelWait(crewPos[1], 1));
                                }

                                foodCode[i]--;
                                bookmark++;

                            }
                            else
                            {
                                storeComp.CreateFood(side, i);
                                storeComp.foodItems[side - 1][bookmark].UpdateTarget(foodPos[9]);
                                yield return StartCoroutine(TravelWait(crewPos[6], 3));
                                TravelRandWait(foodReady, 1);
                                StartCoroutine(storeComp.foodItems[side - 1][bookmark].TravelRand(Instantiate(emptyPos, RandPos(foodReady), Quaternion.identity)));
                                yield return StartCoroutine(TravelWait(crewPos[7], 1));
                                foodCode[i]--;
                                bookmark++;
                            }
                        }
                    }
                }
            }
            

            for (int i = 7; i >= 0; i--)
            {
                
            }
        }

    }

}
