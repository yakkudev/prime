using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Shop : Item, IRefreshInteractionTypes, IUsable, DoSomethingWhenSlotChanged
{
    [SerializeField] ShopItem[] itemsInShop;
    [SerializeField] bool sellMode;
    [SerializeField] Transform grid;
    [SerializeField] GameObject itemInGridPrefab;
    [SerializeField] Transform itemTransform;
    InfoPanel[] infoPanels = new InfoPanel[10];

    public Item[] itemInSlot = new Item[1];
    [SerializeField] InventorySlot slot;
    [SerializeField] TMP_Text transactionCostText;
        
    [Header("Buy")] 
    public Image image;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text amountText;
    int sliderValue;
    InventoryUI inventoryUI;
    public TMP_Text nameText;
    void Start()
    {
        inventoryUI = GameObject.Find("InventoryPanelUI").GetComponent<InventoryUI>();

        RefreshInteractionTypes = this;
        Usable = this;
        Item[] items = new Item[itemsInShop.Length];
        for (int i = 0; i < itemsInShop.Length; i++)
        {
            items[i] = itemsInShop[i].item;
        }
        if (!sellMode) return;
        slot.itemList = itemInSlot;
        slot.transformWhereItemsAreStored = itemTransform;
        slot.doSomethingWhenSlotChanged = this;
        slot.listOfAllowedItems = items;
    }
    void Update()
    {
        if(Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 5)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                OpenShop();
                inventoryUI = GameObject.Find("InventoryPanelUI").GetComponent<InventoryUI>();

                inventoryUI.OpenBuildingGUI(additionalGUI);
            }
        }
    }
    public void RefreshInteractions() {}

    public void Use(InteractionType interactionType, InteractionData interactionData)
    {
        switch (interactionType)
        {
            case InteractionType.OpeningUI:
                OpenShop();
                inventoryUI.OpenBuildingGUI(additionalGUI);
                    
                break;
        }
    }
    
    void OpenShop()
    {
        //generuj liste crop do sprzedania oraz ustaw ich cene
        sliderValue = 1;
        transactionCostText.text = 0 + "$";
        if (!sellMode)
        {
            nameText.text = "";
            image.sprite = null;
            itemInSlot[0] = null;
            slider.value = 1;
        }
            
        for (int i = 0; i < itemsInShop.Length; i++)
        {
            if (infoPanels[i] == null)
            {
                GameObject panel = Instantiate(itemInGridPrefab, grid);
                infoPanels[i] = panel.GetComponent<InfoPanel>();
                infoPanels[i].SetItem(itemsInShop[i].item, itemsInShop[i].price, i);
                infoPanels[i].shop = this;
            }
            else
            {
                infoPanels[i].UpdatePrice(itemsInShop[i].price);
            }
        }
    }

    public void Sell()
    {
        for (int i = 0; i < itemsInShop.Length; i++)
        {
            if (itemInSlot[0] == null)
            {
                transactionCostText.text = 0 + "$";
                return;
            }
            if(itemInSlot[0].itemName == itemsInShop[i].item.itemName)
            {
                InventoryManager.playerInventory.ChangeMoney(itemsInShop[i].price * itemInSlot[0].stackSize);
                itemInSlot[0] = null;
                Destroy(slot.transform.GetChild(0).gameObject);
                transactionCostText.text = 0 + "$";
                return;
            }
        }
    }

    public void Buy()
    {
        // ZROB JAKOS ZEBY TO MIALO SENS LOL
        for (int i = 0; i < itemsInShop.Length; i++)
        {
            if (itemInSlot[0] == null || sliderValue == 0)
            {
                transactionCostText.text = 0 + "$";
                nameText.text = "";
                return;
            }
            if(itemInSlot[0].itemName == itemsInShop[i].item.itemName)
            {
                if (InventoryManager.playerInventory.CanAddItemToInventory(itemsInShop[i].item, sliderValue) && InventoryManager.playerInventory.CanAfford(itemsInShop[i].price * sliderValue))
                {
                    Item temp = CopyItemToOtherItem(gameObject.transform, itemsInShop[i].item.gameObject).GetComponent<Item>();
                    InventoryManager.playerInventory.ChangeMoney(-itemsInShop[i].price * sliderValue);
                    InventoryManager.playerInventory.AddItemToInventory(temp, sliderValue);
                }
                else
                {
                    transactionCostText.text = "You dont have enought money or space in inventory";
                    nameText.text = "";
                    image.sprite = null;
                }
            }
            nameText.text = "";
        }
    }

    public void SliderChangeValue()
    {
        sliderValue = (int) slider.value;
        amountText.text = "Amount: " + sliderValue;
        if(itemInSlot[0] == null) return;
        SetBuyItemInfo(itemInSlot[0]);
    }
    public void SetBuyItemInfo(Item item)
    {
        if(sellMode) return;
        itemInSlot[0] = item;
        nameText.text = item.itemName;
        image.sprite = item.icon;
        slider.maxValue = item.maxStackSize;
        slider.minValue = 1;
        for (int i = 0; i < itemsInShop.Length; i++)
        {
            if(item.itemName == itemsInShop[i].item.itemName)
            {
                transactionCostText.text =( itemsInShop[i].price * sliderValue ) + "$";
                break;
            }
        }
    }
    public void DoSomething(bool isComingFromThisSlot)
    {
        RefreshPrices(isComingFromThisSlot);
    }

    void RefreshPrices(bool isComingFromThisSlot)
    {
        if (!isComingFromThisSlot)
        {
            transactionCostText.text = 0 + "$";
        }
        for (int i = 0; i < itemsInShop.Length; i++)
        {
            if(itemInSlot[0] == null) return;
            if(itemInSlot[0].itemName == itemsInShop[i].item.itemName)
            {
                transactionCostText.text = itemsInShop[i].price + "$";
                return;
            }
        }
    }
    [Serializable]
    public class ShopItem
    {
        public Item item;
        public float basePrice;
        public float maxPrice;
        public float price;
        public float minPrice;
    }
}

