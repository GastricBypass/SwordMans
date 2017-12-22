using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopMenuManager : MonoBehaviour
{
    public MainMenuManager manager;

    public Text goldAmount;

    public Image itemSelector;
    public Image itemListingRowPrefab;
    public Button itemListingPrefab;
    public Button confirmButtonPrefab;
    public Scrollbar scrollbar;

    public int numRandomItems;

    private Button confirmButton;
    private List<string> availableItems; // to ensure there are no duplicate items in the shop

	// Use this for initialization
	void Start ()
    {
        if (numRandomItems > GameConstants.Unlocks.purchasableHats.Count + GameConstants.Unlocks.purchasableMisc.Count)
        {
            numRandomItems = GameConstants.Unlocks.purchasableHats.Count + GameConstants.Unlocks.purchasableMisc.Count;
        }
        StartCoroutine(RepeatedlyTryToSetUpShopItems(.1f));
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            ScrollItemMenu();
        }
	}

    public void ScrollItemMenu()
    {
        GameObject currentlySelected = EventSystem.current.currentSelectedGameObject;
        if (currentlySelected == null)
        {
            return;
        }

        PurchaseButtonData selectedItemData = currentlySelected.GetComponent<PurchaseButtonData>();
        if (selectedItemData != null)
        {
            string name = selectedItemData.itemName;
            int index = availableItems.IndexOf(name);

            if (index != -1)
            {
                int row = index / 4;
                int totalRows = (int)(((float)availableItems.Count / 4) + .9f);
                float adjustedTotalRows = (float)(totalRows - 1);
                if (adjustedTotalRows <= 0)
                {
                    adjustedTotalRows = 1;
                }

                float invScrollValue = row / adjustedTotalRows;

                scrollbar.value = 1 - invScrollValue;
            }
        }
    }

    public IEnumerator RepeatedlyTryToSetUpShopItems(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        if (manager.gsm.data == null)
        {
            StartCoroutine(RepeatedlyTryToSetUpShopItems(time));
        }
        else
        {
            availableItems = manager.gsm.data.shopItems;

            if (availableItems == null || availableItems.Count == 0)
            {
                SetupShopItems(); // adds to availableItems when there are no items in the shop
            }

            SetUpShopFromItemList(availableItems); // places available items into the shop
        }
    }

    public void SetGoldValue()
    {
        goldAmount.text = "x " + manager.gsm.data.gold.ToString();
    }

    public void SetupShopItems()
    {
        for (int item = 0; item < numRandomItems; item++)
        {
            int index = Random.Range(0, GameConstants.Unlocks.purchasableHats.Count + GameConstants.Unlocks.purchasableMisc.Count);

            string itemName = "";
            float itemPrice = 0;

            if (index < GameConstants.Unlocks.purchasableHats.Count)
            {
                itemName = GameConstants.Unlocks.purchasableHats[index];
                GameConstants.Unlocks.hatPrices.TryGetValue(itemName, out itemPrice);
            }
            else
            {
                itemName = GameConstants.Unlocks.purchasableMisc[index - GameConstants.Unlocks.purchasableHats.Count];
                GameConstants.Unlocks.miscPrices.TryGetValue(itemName, out itemPrice);
            }

            if (availableItems.Contains(itemName))
            {
                item--;
                continue;
            }
            else
            {
                availableItems.Add(itemName);
            }
        }
    }

    public void SetUpShopFromItemList(List<string> items)
    {
        for (int row = 0; row < (int)(((float)items.Count / 4) + .9f); row++)
        {
            Image newItemListingRow = Instantiate(itemListingRowPrefab);

            for (int item = 0; item < 4 && item + (row * 4) < items.Count; item++)
            {

                string itemName = "";
                float itemPrice = 0;

                if (GameConstants.Unlocks.purchasableHats.Contains(items[item + (row * 4)]))
                {
                    itemName = items[item + (row * 4)];
                    GameConstants.Unlocks.hatPrices.TryGetValue(itemName, out itemPrice);
                }
                else if (GameConstants.Unlocks.purchasableMisc.Contains(items[item + (row * 4)]))
                {
                    itemName = items[item + (row * 4)];
                    GameConstants.Unlocks.miscPrices.TryGetValue(itemName, out itemPrice);
                }

                AddItem(itemName, itemPrice, newItemListingRow);
            }

            newItemListingRow.transform.SetParent(itemSelector.transform);
        }
    }

    public void AddItem(string itemName, float itemPrice, Image newItemListingRow)
    {
        Button newItemListing = Instantiate(itemListingPrefab);
        newItemListing.transform.SetParent(newItemListingRow.transform);

        PurchaseButtonData data = newItemListing.GetComponent<PurchaseButtonData>();
        if (data != null)
        {
            data.SetNameAndPrice(itemName, itemPrice);
        }

        Sprite newImage = (Sprite)Resources.Load("ItemImages/" + itemName, typeof(Sprite));

        if (newImage != null)
        {
            newItemListing.GetComponent<Image>().sprite = newImage;
        }

        newItemListing.transform.Find("Text").GetComponent<Text>().text = itemPrice + "g";
        newItemListing.onClick.AddListener(delegate { DisplayPurchaseConfirmation(itemName, itemPrice, newItemListing); });
        newItemListing.onClick.AddListener(delegate { manager.PlayClickSound(); });

        if (manager.gsm.data.hats.Contains(itemName) || manager.gsm.data.misc.Contains(itemName))
        {
            newItemListing.transform.Find("Text").GetComponent<Text>().text = "owned";
            newItemListing.GetComponent<Image>().color = new Color(.1f, .1f, .1f, .1f);
        }
    }

    public void DisplayPurchaseConfirmation(string itemName, float itemPrice, Button itemListing)
    {
        if (confirmButton != null)
        {
            Destroy(confirmButton.gameObject);
        }

        confirmButton = Instantiate(confirmButtonPrefab, itemListing.transform);
        bool itemOwned = itemListing.transform.Find("Text").GetComponent<Text>().text == "owned";

        confirmButton.onClick.AddListener(delegate { manager.PlayClickSound(); });

        if (!itemOwned && manager.gsm.data.gold >= itemPrice)
        {
            confirmButton.transform.Find("Text").GetComponent<Text>().text = "Purchase " + itemName + " for " + itemPrice + " gold?";
            confirmButton.onClick.AddListener(delegate { PurchaseItem(itemName, itemPrice, itemListing); });
        }
        else if (itemOwned)
        {
            confirmButton.transform.Find("Text").GetComponent<Text>().text = "You cannot buy this item again";
            confirmButton.onClick.AddListener(delegate { CancelPurchase(itemListing); });
        }
        else
        {
            confirmButton.transform.Find("Text").GetComponent<Text>().text = "Not enough gold";
            confirmButton.onClick.AddListener(delegate { CancelPurchase(itemListing); });
        }

        confirmButton.Select();
    }

    public void PurchaseItem(string itemName, float itemPrice, Button itemListing)
    {
        if (GameConstants.Unlocks.purchasableHats.Contains(itemName))
        {
            manager.gsm.data.UnlockHat(itemName);
        }
        else if (GameConstants.Unlocks.purchasableMisc.Contains(itemName))
        {
            manager.gsm.data.UnlockMisc(itemName);
        }
        else
        {
            return;
        }
        
        manager.gsm.data.AddGold(-itemPrice);

        Destroy(confirmButton.gameObject);

        itemListing.GetComponent<Image>().color = new Color(.1f, .1f, .1f, .1f);
        itemListing.interactable = false;
        itemListing.transform.Find("Text").GetComponent<Text>().text = "owned";
        itemListing.Select();

        SetGoldValue();
    }

    public void CancelPurchase(Button itemListing)
    {
        Destroy(confirmButton.gameObject);
        itemListing.Select();
    }
}
