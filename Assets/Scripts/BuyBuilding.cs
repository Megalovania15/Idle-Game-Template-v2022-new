using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//a script to hold all of the functions that can be added to buttons for buying
//buildings. Put this script on the button that you will use to buy buildings.
//It also includes a method for selling buildings, which you can adapt for other
//purposes if needed. For example, if you need to sell some resources instead
//of buildings themselves. The sell feature is to prevent players from soft
//locking themselves if they don't build mines, but build other things
public class BuyBuilding : MonoBehaviour
{
    //use the global resource that would be your currency, money, or adapt it
    //to include other resources
    public Resource currency;
    //in the inspector, drag in one type of building into this array. It should
    //be either power plants or mines (or whatever buildings/resources you're 
    //using) do not put both building types into the array. One button, one
    //building type
    public GameObject[] buildings;
    public TMP_Text buildingCountTxt;
    public TMP_Text buildingCostTxt;
    public GameObject warningText;
    //this can be moved to the building script, as it makes more sense there
    public float buildingCost;

    //for keeping track of the number of buildings (either mines or power
    //plants) currently in play. This will depend on the button you've assigned
    //it to
    [SerializeField]
    private int buildingCount = 0;    

    // Start is called before the first frame update
    void Start()
    {

        UpdateBuildingCounts();
        UpdateBuildingCost();

        if (warningText != null)
        {
            warningText.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        UpdateBuildingCounts();
        UpdateBuildingCost();
    }

    //a function that can be added to a button with an adjustable cost modifier
    //that can be changed in the inspector. The cost modifier is something that
    //can currently be set in the inspector when you assign the method to the
    //button, but should actually be kept track of on the building script
    public void PurchaseBuilding(float costModifier)
    {
        //check to ensure we have enough money to buy the building. Check against
        //the global resource for your money or currency.
        if (buildingCost <= currency.totalResourceAmount)
        {
            //subtracts the cost from the currency
            currency.totalResourceAmount -= buildingCost;   

            //check to ensure we don't build beyond the array of buildings
            if (buildingCount < buildings.Length)
            {
                buildings[buildingCount].SetActive(true);
                buildingCount++;
                //update the cost for the building
                buildingCost *= costModifier;
            }
            else
            {
                StartCoroutine(DisplayText(3f, "Maxmimum buildings reached"));
            }

        }
        else
        {
            StartCoroutine(DisplayText(3f, "Insufficient funds"));
        }
    }

    //a method that can be selected on your sell button, that reverses the
    //buying of a building
    public void SellBuilding()
    {
        if (buildingCount <= buildings.Length && buildingCount > 0)
        {
            //we add the currency back to the global resource at half the price
            //paid. This formula can be changed to something that fits more with
            //your progression. For example, you could change it so the player
            //receives 75% of the building cost back or something. You can choose
            //whether the cost multiplier should be reversed as well. But this will
            //have to be coded in
            currency.totalResourceAmount += buildingCost / 2;
            //turns off the last built building in the inspector
            buildings[buildingCount - 1].SetActive(false);
            //decreases the current building count
            buildingCount--;
        }
        else 
        {
            StartCoroutine(DisplayText(3f, "No buildings to sell"));
        }
    }

    //mines and power plants were separate functions. It's been combined into
    //one function that can be used for either since then. Can ignore this code
    //a function that can be added to a button with an adjustable cost that can be changed in the
    //inspector. For mines specifically
    /*public void PurchaseMine(float costModifier)
    {
        //check to ensure we have enough money to buy the building
        if (buildingCost <= currency.totalResourceAmount)
        {
            currency.totalResourceAmount -= buildingCost;   //minus the cost from the currency

            //check to ensure we don't build beyond the array of buildings
            if (buildingCount < mines.Length)
            {
                mines[buildingCount].SetActive(true);
                buildingCount++;
                buildingCost *= costModifier;
            }
            else 
            {
                //StartCoroutine(DisplayText(3f, "Maxmimum mines reached"));
            }
        }
        else
        {
            //StartCoroutine(DisplayText(3f, "Insufficient funds"));
        }
    }*/

    //updates the text mesh pro asset to reflect the number of buildings that
    //have been placed. This works automatically
    void UpdateBuildingCounts()
    {
        buildingCountTxt.text = buildingCount + "/" + buildings.Length;
    }

    //updates the text mesh pro asset to reflect the current cost of the
    //building
    void UpdateBuildingCost()
    {
        buildingCostTxt.text = currency.unit + buildingCost.ToString();
    }

    //displays a specified warning after a specific amount of time. Include
    //the text to display as part of the parameters when starting the coroutine
    IEnumerator DisplayText(float time, string textToDisplay)
    {
        warningText.SetActive(true);
        warningText.GetComponentInChildren<TMP_Text>().text = textToDisplay;
        yield return new WaitForSeconds(time);
        warningText.SetActive(false);

    }
}
