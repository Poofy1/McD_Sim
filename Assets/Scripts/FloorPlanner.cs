using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPlanner : MonoBehaviour
{
    public List<GameObject> staff;
    public GameObject spawn;

    //STAFFING
    public int managers;
    public int crew;
    public int under16;
    public GameObject managerObj;
    public GameObject crewObj;
    public GameObject under16Obj;

    //POSITIONS
    public bool meats;
    public bool backline;
    public bool mcCafe;
    public bool fries;
    public bool present;
    public bool[] register;
    public bool driveRun;
    public bool windowRun;
    public int side1;
    public int side2;


    //AI HELPERS
    public bool floorRunner;
    public int posWaitQ;
    public bool runnerAtDutyCommand;
    public int customerAmount;

    // Start is called before the first frame update
    void Start()
    {
        register = new bool[7];
        staff = new List<GameObject>();

        StartCoroutine(SpawnStaff());

    }

    public IEnumerator SpawnStaff()
    {
        for(int i = 0; i < managers; i++)
        {
            yield return new WaitForSeconds(Random.Range(0f, 1f));
            Vector3 randPos = new Vector3(Random.Range(spawn.GetComponent<BoxCollider2D>().bounds.min.x,
                                    spawn.GetComponent<BoxCollider2D>().bounds.max.x),
                                    Random.Range(spawn.GetComponent<BoxCollider2D>().bounds.min.y,
                                    spawn.GetComponent<BoxCollider2D>().bounds.max.y),
                                    0);
            staff.Add(Instantiate(managerObj, randPos, Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
