using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButtonData : MonoBehaviour
{
    public string itemName;
    public float itemPrice;

    public void SetNameAndPrice(string name, float price)
    {
        itemName = name;
        itemPrice = price;
    }
}
