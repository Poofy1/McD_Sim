using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CustomerCarAI : MonoBehaviour
{
    public GameObject self;
    private GameObject storeComp;
    private FloorPlanner staffing;

    private Transform[] lane1;
    private Transform[] lane2;
    private Transform[] waiting;
    private Transform[] hold;
    private Transform preLane;
    private Transform exit;
    private int laneChoice;
    public int laneLineSpot;
    private int queueSpot;
    private Vector3 rotationStop;



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
    ///////////////////////////



    void Update()
    {
        if (target != null) ai.destination = target.position;


        if (ai.reachedDestination)
        {
            self.transform.eulerAngles = rotationStop;
        }

    }


    void Start()
    {
        //INITIALIZE
        lane1 = new Transform[7];
        lane2 = new Transform[7];
        waiting = new Transform[13];
        hold = new Transform[4];
        rotationStop = new Vector3(0, 0, 180);

        storeComp = GameObject.Find("StoreComputer");
        Transform carLocations = GameObject.Find("CarLocations").transform;

        staffing = GameObject.Find("Staff").GetComponent<FloorPlanner>();

        preLane = carLocations.Find("PreLane");
        exit = carLocations.Find("Exit");
        for (int i = 0; i < 7; i++)
        {
            lane1[i] = carLocations.Find("Lane1." + i);
            lane2[i] = carLocations.Find("Lane2." + i);
        }
        for (int i = 0; i <= 12; i++)
        {
            waiting[i] = carLocations.Find("Waiting" + (i+1));
        }
        for (int i = 0; i <= 3; i++)
        {
            hold[i] = carLocations.Find("Hold" + (i + 1));
        }

        //START PATH
        preLane.GetComponent<ColliderTrigger>().taken = true;
        StartCoroutine(CarSpawned());
    }

    IEnumerator CarSpawned()
    {
        staffing.customerAmount++;

        //Find open spot
        for (int i = 0; i < 7; i++)
        {
            if (!lane1[i].GetComponent<ColliderTrigger>().taken)
            {
                laneChoice = 1;
                lane1[i].GetComponent<ColliderTrigger>().taken = true;
                target = lane1[i];
                laneLineSpot = i;
                yield return new WaitForSeconds(2);
                preLane.GetComponent<ColliderTrigger>().taken = false;
                if (laneLineSpot == 0) StartCoroutine(Order());
                else StartCoroutine(SearchLineSpot());
                yield break;
            }
            else if (!lane2[i].GetComponent<ColliderTrigger>().taken)
            {
                laneChoice = 2;
                lane2[i].GetComponent<ColliderTrigger>().taken = true;
                target = lane2[i];
                laneLineSpot = i;
                yield return new WaitForSeconds(2);
                preLane.GetComponent<ColliderTrigger>().taken = false;
                if (laneLineSpot == 0) StartCoroutine(Order());
                else StartCoroutine(SearchLineSpot());
                yield break;
            }
        }

        //Otherwise wait until spots available
        yield return new WaitUntil(() => !lane1[6].GetComponent<ColliderTrigger>().taken || !lane2[6].GetComponent<ColliderTrigger>().taken);

        if (!lane1[6].GetComponent<ColliderTrigger>().taken)
        {
            laneChoice = 1;
            lane1[6].GetComponent<ColliderTrigger>().taken = true;
            target = lane1[6];
        }
        else if (!lane2[6].GetComponent<ColliderTrigger>().taken)
        {
            laneChoice = 2;
            lane2[6].GetComponent<ColliderTrigger>().taken = true;
            target = lane2[6];
        }

        laneLineSpot = 6;
        yield return new WaitForSeconds(2);
        preLane.GetComponent<ColliderTrigger>().taken = false;

        StartCoroutine(SearchLineSpot());
    }


    IEnumerator SearchLineSpot()
    {
        if (laneChoice == 1)
        {
            yield return new WaitUntil(() => !lane1[laneLineSpot - 1].GetComponent<ColliderTrigger>().taken);
            laneLineSpot--;
            lane1[laneLineSpot].GetComponent<ColliderTrigger>().taken = true;
            target = lane1[laneLineSpot];
            yield return new WaitForSeconds(1);
            lane1[laneLineSpot + 1].GetComponent<ColliderTrigger>().taken = false;
        }
        else if (laneChoice == 2)
        {
            yield return new WaitUntil(() => !lane2[laneLineSpot - 1].GetComponent<ColliderTrigger>().taken);
            laneLineSpot--;
            lane2[laneLineSpot].GetComponent<ColliderTrigger>().taken = true;
            target = lane2[laneLineSpot];
            yield return new WaitForSeconds(1);
            lane2[laneLineSpot + 1].GetComponent<ColliderTrigger>().taken = false;
        }

        if(laneLineSpot == 0) StartCoroutine(Order());
        else StartCoroutine(SearchLineSpot());
    }

    IEnumerator Order()
    {
        //Wait until arrived and then order
        float waitTime = Random.Range(10f, 60f);
        laneLineSpot = -1;

        if (laneChoice == 1) yield return new WaitUntil(() => ai.reachedDestination && staffing.GetComponent<FloorPlanner>().register[6]);
        else yield return new WaitUntil(() => ai.reachedDestination && staffing.GetComponent<FloorPlanner>().register[4]);
        StartCoroutine(storeComp.GetComponent<Store>().CreateOrder(waitTime, true));
        yield return new WaitForSeconds(waitTime);

        //Drive/Wait next spot
        yield return new WaitUntil(() => !waiting[0].GetComponent<ColliderTrigger>().taken);
        waiting[0].GetComponent<ColliderTrigger>().taken = true;
        target = waiting[0];

        yield return new WaitForSeconds(1);
        rotationStop = new Vector3(0, 0, 45);
        if (laneChoice == 1) lane1[0].GetComponent<ColliderTrigger>().taken = false;
        else if (laneChoice == 2) lane2[0].GetComponent<ColliderTrigger>().taken = false;

        staffing.customerAmount--;
        StartCoroutine(WaitQueue());
    }

    IEnumerator WaitQueue()
    {
        //Drive/Wait next spot
        yield return new WaitUntil(() => !waiting[queueSpot + 1].GetComponent<ColliderTrigger>().taken);
        queueSpot++;
        waiting[queueSpot].GetComponent<ColliderTrigger>().taken = true;
        target = waiting[queueSpot];

        yield return new WaitForSeconds(1);
        waiting[queueSpot - 1].GetComponent<ColliderTrigger>().taken = false;

        if(queueSpot < 5) rotationStop = new Vector3(0, 0, 45);
        else if(queueSpot < 7) rotationStop = new Vector3(0, 0, 90);
        else if (queueSpot < 8) rotationStop = new Vector3(0, 0, 125);
        else rotationStop = new Vector3(0, 0, 0);
        
        if(queueSpot != 9 && queueSpot != 12) StartCoroutine(WaitQueue());
        else if (queueSpot == 9) StartCoroutine(Paying());
        else if (queueSpot == 12) StartCoroutine(Receiving());
    }

    IEnumerator Paying()
    {
        //Wait until arrived
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => ai.reachedDestination);
        yield return new WaitForSeconds(Random.Range(5f, 10f));

        StartCoroutine(WaitQueue());
    }

    IEnumerator Receiving()
    {
        //Wait until arrived
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => ai.reachedDestination);
        yield return new WaitForSeconds(Random.Range(5f, 10f));
        
    }

}
