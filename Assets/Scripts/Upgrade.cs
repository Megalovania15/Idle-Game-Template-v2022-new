using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//this class works similarly to the resource class. Create an upgrade gameObject
//and attach this script to it to create an upgrade
public class Upgrade : MonoBehaviour
{
    public string upgradeName;
    public float upgradeCost;
    //the cost of the upgrade will be updated by the multiplier everytime the
    //player buys an upgrade
    public float costMultiplier;
    public int upgradeLevel;

    public TMP_Text upgradeLvlText;
    public TMP_Text upgradeCostTxt;

    //at the moment, upgrades are applied to all linked buildings. It may need
    //an update to balance it better. Add the buildings you want to be affected
    //by the upgrade to the array.
    public Building[] linkedBuildings;

    void Start()
    {
        //updates the text assets with the level and cost of the upgrade
        upgradeLvlText.text = "Lvl: " + upgradeLevel;
        upgradeCostTxt.text = upgradeCost.ToString();
    }
}
