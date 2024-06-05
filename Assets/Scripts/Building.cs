using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//this script to be placed on buildings, functional on both producers and drains
//of resources. It has several methods that allow you to add resources to the
//global pool, and drain resources from the global pool and any specified local
//pool of resources
public class Building : MonoBehaviour
{
    //obsolete, but variables you can use if you want your buildings to generate
    //resources over time. Could be useful for automatic generation which has not
    //been included.
    public float resourceDrainAmount = 10;
    //public float generationTime = 1f;

    //the level of the building that this script is attached to
    public int level = 1;

    public Slider progressSlider;
    public Slider fuelSlider;
    //need a reference to the script of the resource that is being used up. I'd
    //recommend you keep this resource on your building
    public Resource fuel;
    //need a reference to the script of the global resource that is typically
    //used up i.e. coal. This is currently kept separate from the building
    public Resource fuelGlobal;    
    public GameObject warningText;
    public GameObject gameOverScreen;

    //if you have mines or elements that work similarly, then you can set this
    //to true in the inspector
    public bool isMine = false;

    private TMP_Text levelText;

    private float timer;
    private bool isClicked = false;
    private bool runTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        //ensuring that the game doesn't start with these screens active or
        //visible
        warningText.SetActive(false);

        levelText = GetComponentInChildren<TMP_Text>();
        UpdateLevelUI();

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        //check to see if there is a resource drain script attached to the
        //game object. Not all buildings have to drain resources. Resources
        //work a bit differently on the mines, as they previously did not have
        //fuel
        if (fuel != null && isMine == false)
        {
            //sets the current amount of resources available to the "maximum"
            //to begin with. This currently results in free power for the mines
            //and free coal for new power stations, so you'll have to balance
            //that, as it's an exploit.
            fuel.currentResourceAmount = fuel.totalResourceAmount;    
            fuelSlider.value = fuel.currentResourceAmount / fuel.totalResourceAmount; //sets the value of the yellow slider to reflect the resource amount
            print(fuel.currentResourceAmount);
        }
        else 
        {
            fuelSlider.value = fuel.currentResourceAmount / fuel.totalResourceAmount; //sets the value of the yellow slider to reflect the resource amount
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check to see that the building has a resource drain, as not all
        //buildings need to have drains or require fuel
        if (fuel != null)      
        {
            //updates the UI associated with this resource (i.e. coal)
            UpdateResourceUI(fuel.resourceText, fuel.currentResourceAmount, fuel.resourceName, fuel.unit);
        }

        //currently game over is only determined by if the main building
        //runs out of fuel. You can change this if you want
        if (gameOverScreen != null)     
        {
            GameOver();
        }
    }

    //Adds a specified resource to the global pool. You need to drag the resource
    //in in the inspector to the button function. Can also drag a local resource
    //in if you want.

    //Select it on your button. You can add several resources this way if you
    //want your building or object to produce more than one type of resource
    //every click
    public void AddResource(Resource producedResource)
    {
        //the building's fuel must be more than 0 for this to work
        if (fuel.currentResourceAmount > 0)
        {
            //you can change the formula here as part of your balancing. Consider
            //your curve and how you'd like the player to progress. This is currently
            //linear progression
            producedResource.totalResourceAmount += producedResource.addAmount * level;
            UpdateLevelUI();
            //updates the UI associated with this resource (i.e. money/coin)
            UpdateResourceUI(producedResource.resourceText, producedResource.totalResourceAmount, producedResource.resourceName, producedResource.unit);
        }
        else
        {
            StartCoroutine(DisplayWarning(3f, "No fuel"));
        }

    }

    //displays a specified warning after a specific amount of time. You need
    //to include the message as a parameter when starting the coroutine. See
    //how it's used above.
    IEnumerator DisplayWarning(float time, string textToDisplay)
    {
        warningText.SetActive(true);
        warningText.GetComponent<TMP_Text>().text = textToDisplay;
        yield return new WaitForSeconds(time);
        warningText.SetActive(false);

    }

    //updates the resource UI
    void UpdateResourceUI(TMP_Text resourceUI, float resourceTotal, string resourceName, string resourceUnit)
    {
        resourceUI.text = resourceName + ": " + resourceTotal + resourceUnit;
    }

    //updates the level UI
    public void UpdateLevelUI()
    {
        if (gameObject.activeInHierarchy)
        {
            levelText.text = "Level: " + level;
        } 
    }

    //Removes resources of a specified value from the building's local resource
    //pool. Recommended you put this resource on the building in the inspector
    public void DrainResource()
    {
        //checks to ensure we have resources to drain
        if (fuel.currentResourceAmount > 0)
        {
            //remove resources from the building's pool. The amount of resources
            //removed is multiplied by the level of the building. Also updates
            //the text showing what level the building's at
            fuel.currentResourceAmount -= resourceDrainAmount * level; 
            levelText.text = "Level: " + level;
            //updates the slider to show how much resources you have
            fuelSlider.value = fuel.currentResourceAmount / fuel.totalResourceAmount; 
        }
        else
        {
            //keeps the resource from becoming negative if you drain too many
            fuel.currentResourceAmount = 0;    
            fuelSlider.value = fuel.currentResourceAmount / fuel.totalResourceAmount;
        }
    }

    //a button function to add fuel on click by the amount specified in the
    //inspector
    public void AddFuel()
    {
        //this takes from a global resource which should be set up separately
        //and adds it to the local storage of the building's fuel resource pool
        //it also checks to see that we have enough fuel in the global storage
        //and that the amount we're adding to what we currently have doesn't
        //exceed the total amount of storage
        if (fuelGlobal.addAmount + fuel.currentResourceAmount <= fuel.totalResourceAmount && fuelGlobal.totalResourceAmount > 0)
        {
            fuelGlobal.totalResourceAmount -= fuelGlobal.addAmount;
            fuel.currentResourceAmount += fuelGlobal.addAmount;
            fuelSlider.value = fuel.currentResourceAmount / fuel.totalResourceAmount;
        }
        else
        {
            StartCoroutine(DisplayWarning(3f, "Insufficient " + fuelGlobal.resourceName));
        }
    }

    //game ends if the main building (which is the one you start with) runs
    //out of fuel
    void GameOver()
    {
        if (fuel.currentResourceAmount == 0)
        {
            gameOverScreen.SetActive(true);
        }
    }

    //you can ignore, remove, or change this code if you want to. As it is,
    //it simply adds a timer for the resource production

    //a function set in the inspector to work on the click of a button.
    //Starts the coroutine to add a resource
    /*public void StartAddResource()
    {
        Debug.Log("Resources are being added");
        if (!isClicked)
        {
            //checks to ensure the coroutine can still run when there is no draining resource,
            //also checks to ensure we have enough resources to produce a resource
            if (fuel == null || fuel.currentResourceAmount > 0)
            {
                StartCoroutine("AddResource");
            }
            else
            {
                StartCoroutine(DisplayWarning(3f, "No fuel"));
            }
        }
    }*/

    //adds a resource after a certain amount of time and displays it on a
    //green progress meter
    /*public IEnumerator AddResource()    
    {
        //need to check if the button has been clicked so the player can't spam it while the timer is going
        isClicked = true;
        var time = 0f;
        while (time < generationTime)
        {
            yield return new WaitForSeconds(0.05f);
            time += 0.05f;
            progressSlider.value = time / generationTime;   //updates the slider value overtime
        }
        producedResource.totalResourceAmount += producedResource.addBaseAmount; //adds a specified amount to the pool of produced resources
        progressSlider.value = 0;     //resets the slider
        isClicked = false;
    }*/
}
