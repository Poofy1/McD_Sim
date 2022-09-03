using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CustomerAI : MonoBehaviour
{
    private Transform bathroomM;
    private Transform bathroomW;
    private Transform foodWait;
    private Transform[] kiosk;
    private GameObject storeComp;
    private FloorPlanner staffing;
    public GameObject self;
    public GameObject emptyPos;

    private bool usedBathroom;
    private bool ordered;

    /////////////////////PATHFINDING
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
        kiosk = new Transform[10];
        storeComp = GameObject.Find("StoreComputer");
        staffing = GameObject.Find("Staff").GetComponent<FloorPlanner>();

        Transform customerLocations = GameObject.Find("CustomerLocations").transform;
        foodWait = customerLocations.Find("FoodWait");
        bathroomM = customerLocations.Find("BathroomM");
        bathroomW = customerLocations.Find("BathroomW");
        for(int i = 1; i < 11; i++)
        {
            kiosk[i-1] = customerLocations.Find("Kiosk"+i);
        }

        staffing.customerAmount++;

        if (Random.Range(0, 8) == 0) StartCoroutine(UseBathroom());
        else StartCoroutine(UseKiosk());
    }

    IEnumerator UseKiosk()
    {
        target = kiosk[Random.Range(0, 10)];
        float waitTime = Random.Range(15f, 60f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => ai.reachedDestination);
        StartCoroutine(storeComp.GetComponent<Store>().CreateOrder(waitTime, false));
        yield return new WaitForSeconds(waitTime);
        ordered = true;

        if(!usedBathroom && Random.Range(0, 8) == 0) StartCoroutine(UseBathroom());
        else StartCoroutine(idleWait());
    }

    IEnumerator idleWait()
    {
        staffing.customerAmount--;
        Vector3 randPos = new Vector3(Random.Range(foodWait.GetComponent<BoxCollider2D>().bounds.min.x,
                                    foodWait.GetComponent<BoxCollider2D>().bounds.max.x),
                                    Random.Range(foodWait.GetComponent<BoxCollider2D>().bounds.min.y,
                                    foodWait.GetComponent<BoxCollider2D>().bounds.max.y),
                                    0);
        GameObject tempPos = Instantiate(emptyPos, randPos, Quaternion.identity);
        target = tempPos.transform;
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => ai.reachedDestination); 
        Destroy(tempPos);
        //THEN WAIT UNTILL FOOD READY
    }

    IEnumerator UseBathroom()
    {
        usedBathroom = true;
        Vector3 randPos;

        if (Random.Range(0, 2) == 0)
        {
            randPos = new Vector3(Random.Range(bathroomM.GetComponent<BoxCollider2D>().bounds.min.x,
                                    bathroomM.GetComponent<BoxCollider2D>().bounds.max.x), 
                                    Random.Range(bathroomM.GetComponent<BoxCollider2D>().bounds.min.y,
                                    bathroomM.GetComponent<BoxCollider2D>().bounds.max.y), 
                                    0);
        }
        else
        {
            randPos = new Vector3(Random.Range(bathroomW.GetComponent<BoxCollider2D>().bounds.min.x,
                                    bathroomW.GetComponent<BoxCollider2D>().bounds.max.x),
                                    Random.Range(bathroomW.GetComponent<BoxCollider2D>().bounds.min.y,
                                    bathroomW.GetComponent<BoxCollider2D>().bounds.max.y),
                                    0);
        }

        GameObject tempPos = Instantiate(emptyPos, randPos, Quaternion.identity);
        target = tempPos.transform;
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => ai.reachedDestination);
        Destroy(tempPos);
        yield return new WaitForSeconds(Random.Range(1, 5));

        if (!ordered) StartCoroutine(UseKiosk());
        else StartCoroutine(idleWait());

    }



}
