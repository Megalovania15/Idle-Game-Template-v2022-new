using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//place this on an empty game object. You'll have to drag it onto the button
//component of the button you want to take control of what happens with 
//specific upgrades. You may have to modify the code a bit to create new upgrade
//types that affect different aspects of your buildings and production.
//Keep in mind that methods on buttons can only take one parameter.
public class UpgradeController : MonoBehaviour
{
    public Resource currency;
    public GameObject warningText;

    //You can buy an upgrade of an upgrade type. This will require you to drag
    //the object with the upgrade script on it into the button component in the
    //inspector. When buying an upgrade, we remove resources from the "currency"
    //or money resource pool. Increase the cost of future upgrades, update the
    //text mesh pro assets to reflect new costs and level of upgrade and apply
    //the upgrade
    public void BuyUpgrade(Upgrade upgradeType)
    {
        if(currency.totalResourceAmount >= upgradeType.upgradeCost)
        {
            upgradeType.upgradeLevel++;
            currency.totalResourceAmount -= upgradeType.upgradeCost;
            upgradeType.upgradeCost *= upgradeType.costMultiplier;
            upgradeType.upgradeLvlText.text = "Lvl " + upgradeType.upgradeLevel;
            upgradeType.upgradeCostTxt.text = upgradeType.upgradeCost.ToString();
            ApplyUpgrade(upgradeType);
        }
        
    }

    //we can also sell the upgrade in much the same way as buying an upgrade.
    //It simply reverses the code in buying it, except the cost modifier.
    //Money is returned to the player at half the rate. This formula can be 
    //changed for your balancing needs.
    public void SellUpgrade(Upgrade upgradeType)
    {
        if (upgradeType.upgradeLevel > 0)
        {
            upgradeType.upgradeLevel--;
            currency.totalResourceAmount += upgradeType.upgradeCost / 2;
            upgradeType.upgradeLvlText.text = "Lvl " + upgradeType.upgradeLevel;
            RemoveUpgrade(upgradeType);
        }
        else 
        {
            StartCoroutine(DisplayWarning(3f, "No upgrades to sell"));
        }
    }

    //Removing and applying upgrades are not working as intended and will need
    //to be fixed in an update to the code. At the moment the applied upgrade
    //is hardcoded, so just add additional upgrades as methods and parse in the
    //upgrade type if it applies to specific buildings.

    //The remove upgrade method should reverse the upgrade made. In this case
    //it is reversing the upgrade to the building's level.
    void RemoveUpgrade(Upgrade upgradeType)
    {
        foreach (Building building in upgradeType.linkedBuildings)
        {
            building.level--;
            building.UpdateLevelUI();
        }
    }

    //Currently only applies one type of upgrade. To create additional upgrades
    //I'd suggest you rename this one and just add the rest as methods, as this
    //is currently hard-coded.

    //The below cycles through the upgrade's linked buildings and applies an
    //upgrade to the building level
    void ApplyUpgrade(Upgrade upgradeType)
    {
        foreach (Building building in upgradeType.linkedBuildings)
        {
            building.level++;
            building.UpdateLevelUI();
        }
    }

    IEnumerator DisplayWarning(float time, string textToDisplay)
    {
        warningText.SetActive(true);
        warningText.GetComponent<TMP_Text>().text = textToDisplay;
        yield return new WaitForSeconds(time);
        warningText.SetActive(false);

    }
}
