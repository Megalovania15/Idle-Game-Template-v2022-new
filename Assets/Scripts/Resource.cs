using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//a resource class that can be added to any game object that you want to
//produce, drain, or store a resource. Everything can be changed in the
//inspector
public class Resource : MonoBehaviour
{
    public string resourceName;
    //this is the amount that will be added to global or local pools when 
    //when resources are produced
    public float addAmount;
    //this is the unit of measurement for the resource
    public string unit;
    //use if you want to simulate limited space. It's representative of the 
    //amount of resources you currently have
    public float currentResourceAmount;
    //this is the one that does not change on local resource pools on the buildings
    //themselves. This will change on the global pool though
    public float totalResourceAmount = 0;
    //decide whether the resource is local or global. If it is local, this
    //will be false. Decide on whether the resource is local or global in
    //the inspector. Everything else should figure it out automatically.
    public bool isGlobal = false;

    //make sure the Text Mesh Pro asset is attached to your resource somewhere
    public TMP_Text resourceText;

    void Update()
    {
        UpdateResourceAmount();
    }

    //automatically updates the current amount of resources.
    void UpdateResourceAmount()
    {
        if (isGlobal == true)
        {
            //global resources are tracked with the totalResourceAmount
            resourceText.text = resourceName + totalResourceAmount + unit;
        }
        else 
        {
            //local resources are tracked with the currentResourceAmount
            resourceText.text = resourceName + currentResourceAmount + unit;
        }
    }
}
