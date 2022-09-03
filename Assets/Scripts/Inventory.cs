using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{

    //INVENTORY VARS
    [Header("INVENTORY")]
    public int[] bottledWater;
    public int[] whiteMilk;
    public int[] chocMilk;
    public int[] appleJuice;
    public int[] shakeMix;
    public int[] whippedCream;
    public int[] oreo;
    public int[] MnMs;
    public int[] cones;
    public int[] flurrySpoons;
    public int[] hotFudge;
    public int[] hotCaramel;
    public int[] fudgeDrizzle;
    public int[] caramelDrizzle;
    public int[] frenchVan;
    public int[] frenchVanSF;
    public int[] caramel;
    public int[] hazelnut;
    public int[] liquidSugar;
    public int[] chocolate;
    public int[] chocCaramel;
    public int[] strawberryShake;
    public int[] chocShake;
    public int[] vanShake;
    public int[] hotLids;
    public int[] smallHot;
    public int[] mediumHot;
    public int[] largeHot;
    public int[] xsCups;
    public int[] smallCups;
    public int[] mediumCups;
    public int[] largeCups;
    public int[] xlCups;
    public int[] domeLids;
    public int[] xsLids;
    public int[] mediumLids;
    public int[] largeLids;

    public int[] bbq;
    public int[] honeyMustard;
    public int[] sweetNSour;
    public int[] hotMustard;
    public int[] ranch;
    public int[] buffalo;
    public int[] honey;
    public int[] ketchupPacket;
    public int[] mayoPacket;
    public int[] spoons;
    public int[] forks;

    public int[] caramelFrappe;
    public int[] mochaFrappe;
    public int[] slushieBase;
    public int[] wholeMilk;

    public int[] muffins;
    public int[] appleFritters;
    public int[] pies;
    public int[] cookies;

    public int[] HMBoxes;
    public int[] ice;


    public int[] lettuce;
    public int[] sliveredOnions;
    public int[] dicedOnions;
    public int[] cheese;
    public int[] butter;
    public int[] tomatoes;
    public int[] crinklePickles;

    public int[] nuggets;
    public int[] beef;
    public int[] quarter;
    public int[] mcChicken;
    public int[] crispy;
    public int[] fish;

    public int[] cinRolls;
    public int[] tartarPouch;
    public int[] macPouch;
    public int[] mayoPouch;
    public int[] spicyPouch;
    public int[] pickles;
    public int[] bacon;
    public int[] burritos;
    public int[] fries;

    public int[] bigMacBoxes;
    public int[] quarterBoxes;
    public int[] doubleQuarBoxes;
    public int[] fishBoxes;
    public int[] nug10Boxes;
    public int[] nug6Boxes;
    public int[] nug4Boxes;
    public int[] crispyBoxes;
    public int[] spicyCripsyBoxes;
    public int[] deluxeCripsyBoxes;

    public int[] bigMacBuns;
    public int[] regBuns;
    public int[] quarterBuns;
    public int[] potatoBuns;

    //TEXTBOXES
    [Header("TEXTBOXES")]
    public GameObject frontDryTEXT;
    public GameObject extraCupsTEXT;
    public GameObject extraLidsTEXT;
    public GameObject extraSaucesTEXT;
    public GameObject saucesTEXT;
    public GameObject dryDessertTEXT;
    public GameObject frontCupsTEXT;
    public GameObject packagedDrinks;
    public GameObject[] frontFridgeTEXT;
    public GameObject[] HMBoxesTEXT;

    public GameObject prepFridgeTEXT;
    public GameObject prepTableTEXT;
    public GameObject backFreezerTEXT;
    public GameObject beefFreezerTEXT;
    public GameObject cinRollsTEXT;
    public GameObject[] quarterTEXT;
    public GameObject saucePouchesTEXT;
    public GameObject picklesTEXT;
    public GameObject boxesTEXT;
    public GameObject grillFridgeTEXT;
    public GameObject fryDropperTEXT;
    public GameObject[] bunsBmTEXT;
    public GameObject[] bunsRegTEXT;
    public GameObject[] bunsQuarterTEXT;
    public GameObject[] bunsPotTEXT;
    public GameObject[] nuggetTEXT;
    public GameObject[] beefTEXT;
    public GameObject[] mcChickenTEXT;
    public GameObject[] crispyTEXT;
    public GameObject[] fishTEXT;

    [Header("INDICATORS")]
    public GameObject[] beefIndicator;
    public int[] StoveStatus;
    public GameObject[] backlineIndicator;
    public int[] BacklineStatus;



    private void Start()
    {
        //WRITE INVENTORY AMOUNTS
        UpdateCabnietInv(beefTEXT, beef, 0, 0, 900, 500, 5);
        UpdateCabnietInv(fishTEXT, fish, 0, 0, 1800, 200, 4);
        UpdateCabnietInv(crispyTEXT, crispy, 0, 0, 3600, 200, 5);
        UpdateCabnietInv(mcChickenTEXT, mcChicken, 0, 0, 1800, 200, 4);
        UpdateCabnietInv(nuggetTEXT, nuggets, 0, 0, 1200, 200, 5);
        UpdateInv(quarterTEXT, quarter, 0, 0, new int[] {255}, 3);

        UpdateInv(bunsBmTEXT, bigMacBuns, 0, 0, new int[] { 6, 36 }, 3);
        UpdateInv(bunsRegTEXT, regBuns, 0, 0, new int[] { 16, 96, 96 }, 4);
        UpdateInv(bunsQuarterTEXT, quarterBuns, 0, 0, new int[] { 6, 72 }, 3);
        UpdateInv(bunsPotTEXT, potatoBuns, 0, 0, new int[] { 4, 72 }, 3);
    }

    //UPDATE CAB INV
    public void UpdateCabnietInv(GameObject[] TEXT, int[] item, int index, int add, int qualTime, int maxInv, int maxTray)
    {
        item[index] += add;
        if (index > 1) StartCoroutine(TrayQuality(TEXT[index], qualTime));

        TEXT[0].GetComponent<TextMeshProUGUI>().text = "" + item[0];
        TEXT[1].GetComponent<TextMeshProUGUI>().text = "" + item[1] + "/" + maxInv;
        for (int i = 2; i <= maxTray; i++)
        {
            TEXT[i].GetComponentInChildren<TextMeshProUGUI>().text = "" + item[i];
        }
    }

    //UPDATE UNIVERSAL INVENTORY DISPLAY/AMOUNT
    public void UpdateInv(GameObject[] TEXT, int[] item, int index, int add, int[] maxInv, int maxLoc)
    {
        item[index] += add;
        maxLoc--;

        TEXT[0].GetComponent<TextMeshProUGUI>().text = "" + item[0];
        for (int i = 1; i <= maxInv.Length; i++)
        {
            TEXT[i].GetComponent<TextMeshProUGUI>().text = "" + item[i] + "/" + maxInv[i - 1];
        }

        for (int i = maxInv.Length + 1; i <= maxLoc; i++)
        {
            TEXT[i].GetComponentInChildren<TextMeshProUGUI>().text = "" + item[i];
        }
    }

    public void BeefMachine(int amount)
    {
        if (amount >= 8) StartCoroutine(StoveCountdown(0, 40, amount));
        if (amount >= 12) StartCoroutine(StoveCountdown(1, 40, amount));
        if (amount >= 24) StartCoroutine(StoveCountdown(2, 40, amount));
    }

    public IEnumerator StoveCountdown(int target, int cookTime, int amount)
    {
        beefIndicator[target].GetComponent<Image>().color = new Color(1, .75f, .5f, .5f);
        beefIndicator[target].GetComponentInChildren<TextMeshProUGUI>().text = "Prepping " + amount;
        yield return new WaitForSeconds(5);
        beefIndicator[target].GetComponent<Image>().color = new Color(1, .6f, .3f, .5f);
        beefIndicator[target].GetComponentInChildren<TextMeshProUGUI>().text = "Cooking " + amount;
        yield return new WaitForSeconds(cookTime);
        beefIndicator[target].GetComponent<Image>().color = new Color(1, .5f, 0, .5f);
        beefIndicator[target].GetComponentInChildren<TextMeshProUGUI>().text = "Ready " + amount;
        if (target <= 2) StoveStatus[0] = 2;
        else StoveStatus[target-2] = 2;
    }


    public IEnumerator BacklineCountdown(int target, int cookTime, int amount)
    {
        BacklineStatus[target] = 1;
        backlineIndicator[0].GetComponent<Image>().color = new Color(1, .75f, .5f, .5f);
        backlineIndicator[0].GetComponentInChildren<TextMeshProUGUI>().text = "Prepping " + amount;
        yield return new WaitForSeconds(5);
        backlineIndicator[0].GetComponent<Image>().color = new Color(0,0,0,0);
        backlineIndicator[0].GetComponentInChildren<TextMeshProUGUI>().text = "";
        backlineIndicator[target + 1].GetComponent<Image>().color = new Color(1, .6f, .3f, .5f);
        backlineIndicator[target + 1].GetComponentInChildren<TextMeshProUGUI>().text = "Cooking " + amount;
        yield return new WaitForSeconds(cookTime);
        backlineIndicator[target + 1].GetComponent<Image>().color = new Color(1, .5f, 0, .5f);
        backlineIndicator[target + 1].GetComponentInChildren<TextMeshProUGUI>().text = "Ready " + amount;
        BacklineStatus[target] = 2;
    }






    //Tray Color Shift
    IEnumerator TrayQuality(GameObject var, int seconds)
    {
        Color currentCol = new Color(0, .8f, 0, 1);
        float colorShift = .8f / seconds;

        for(int i = 0; i < seconds; i++)
        {
            var.GetComponent<Image>().color = currentCol;
            currentCol += new Color(colorShift, -colorShift, 0, 1);
            yield return new WaitForSeconds(1);
        }
    }
}
