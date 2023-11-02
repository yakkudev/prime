using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private const int HotbarSize = 6;
    public Item[] hotbarItems = new Item[HotbarSize];
    public Item[] backpackItems = new Item[HotbarSize * 5]; // grid w plecaku 5 na 6
    private int _equippedItemIndex = -1;
    [SerializeField] GameObject hotbar;
    [SerializeField] GameObject backpack;
    public float money;
    [SerializeField] Item emptyItem;
    InventoryUI inventoryUI;
    [SerializeField] Transform itemsSpace;

    private void Start()
    {
        InventoryManager.playerInventory = this;
        inventoryUI = GameObject.Find("InventoryPanelUI").GetComponent<InventoryUI>();

        /*        hotbarItems[_equippedItemIndex].isEquipped = true;
                hotbarItems[_equippedItemIndex].gameObject.SetActive(true);*/

        /*inventoryUI.ChangeActiveItemInHotbar(_equippedItemIndex, -1);*/
        ChangeItem(0);
        for (int i = 0; i < HotbarSize; i++)
        {
            if (hotbarItems[i] != null)
            {
                hotbarItems[i].gameObject.SetActive(false);
                Debug.Log(inventoryUI == null);
                inventoryUI.ChangeHotbarIcons(i, hotbarItems[i].icon);
                hotbarItems[i].isEquipped = false;
            }
        }

        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null) continue;
            hotbarItems[i].index = i;
            hotbarItems[i].listWhereItemIs = hotbarItems;
        }
        for (int i = 0; i < backpackItems.Length; i++)
        {
            if (backpackItems[i] == null) continue;
            backpackItems[i].index = i;
            backpackItems[i].listWhereItemIs = backpackItems;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeItem(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeItem(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) ChangeItem(5);
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.ToggleUI();
        }
    }

    public bool CanAfford(float price)
    {
        return !(money - price < 0);
    }
    public void ChangeMoney(float amount)
    {
        money += amount;
        inventoryUI.RefreshMoney();
    }
    public void SetActiveItemToNullItem()
    {
        if (hotbarItems[_equippedItemIndex] != null) hotbarItems[_equippedItemIndex].isEquipped = false;

        _equippedItemIndex = -1;

        Debug.Log("Changed item to null");
    }
    private void ChangeItem(int index)
    {
        if (hotbarItems[index] == null || index == _equippedItemIndex) return;
        Item itemToHide;
        int indexToHide = _equippedItemIndex;
        if (_equippedItemIndex != -1)
        {
            itemToHide = hotbarItems[_equippedItemIndex];
        }
        else
        {
            itemToHide = null;
        }
        Item itemToTakeOut = hotbarItems[index];

        if (itemToHide != null)
        {
            itemToHide.gameObject.SetActive(false);
            itemToHide.isEquipped = false;
        }

        itemToTakeOut.isEquipped = true;
        itemToTakeOut.gameObject.SetActive(true);

        _equippedItemIndex = index;
        //inventoryUI.ChangeActiveItemInHotbar(_equippedItemIndex, indexToHide);
        Debug.Log("Changed item to " + itemToTakeOut.name);

    }
    public void ChangePositionOfItems(InventorySlot slotFrom, InventorySlot slotTo, Transform transformFrom, Transform transformTo)
    {
        int indexFrom = slotFrom.index;
        int indexTo = slotTo.index;

        Item[] listFrom = slotFrom.itemList;
        Item[] listTo = slotTo.itemList;

        // check if item moved from is not active 
        //
        //

        listFrom[indexFrom].gameObject.transform.SetParent(transformTo);
        if (listTo[indexTo] != null) listTo[indexTo].gameObject.transform.SetParent(transformFrom);

        if (listFrom == hotbarItems)
        {
            Debug.Log(indexFrom);
            inventoryUI.ChangeHotbarIcons(indexFrom, null);
        }

        if (listTo == hotbarItems)
        {
            inventoryUI.ChangeHotbarIcons(indexTo, listFrom[indexFrom].icon);
        }
        SwapArrayItems(listFrom, listTo, indexFrom, indexTo);


        void SwapArrayItems(Item[] arrayA, Item[] arrayB, int indexA, int indexB)
        {
            Item tempValue = arrayA[indexA];
            arrayA[indexA] = arrayB[indexB];
            arrayB[indexB] = tempValue;
        }
    }

    public bool CanAddItemToInventory(Item item, int amountOfStack)
    {
        //check if can stack in hotbar
        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null) continue;
            if (hotbarItems[i].itemName == item.itemName)
            {
                if (hotbarItems[i].stackSize + amountOfStack <= item.maxStackSize) return true;
                Debug.Log("1");
            };
        }
        //check if can stack in backpack
        for (int i = 0; i < backpackItems.Length; i++)
        {
            if (backpackItems[i] == null) continue;
            if (backpackItems[i].itemName == item.itemName)
            {
                if (backpackItems[i].stackSize + amountOfStack <= item.maxStackSize) return true;
                Debug.Log("2");
            };
        }
        // chceck if is empty space in backpack
        for (int i = 0; i < backpackItems.Length; i++)
        {
            if (backpackItems[i] == null) return true;
            Debug.Log("3");

        }
        // chceck if is empty space in hotbar
        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null) return true;
            Debug.Log("4");
        }

        return false;
    }

    public void AddItemToInventory(Item item, int amountOfStack)
    {
        item.stackSize = amountOfStack;
        //check if can stack in hotbar
        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null) continue;
            if (hotbarItems[i].itemName == item.itemName)
            {
                if (hotbarItems[i].stackSize + amountOfStack <= item.maxStackSize)
                {
                    hotbarItems[i].stackSize += amountOfStack;
                    item.gameObject.transform.SetParent(hotbar.transform);
                    inventoryUI.ChangeHotbarIcons(i, item.icon);
                    UpdateInventory(hotbarItems, i);
                    Destroy(item);
                    return;
                }
            };
        }
        //check if can stack in backpack
        for (int i = 0; i < backpackItems.Length; i++)
        {
            if (backpackItems[i] == null) continue;
            if (backpackItems[i].itemName == item.itemName)
            {
                if (backpackItems[i].stackSize + amountOfStack <= item.maxStackSize)
                {
                    backpackItems[i].stackSize += amountOfStack;
                    item.gameObject.transform.SetParent(backpack.transform);
                    UpdateInventory(backpackItems, i);
                    Destroy(item);
                    return;
                }
            };
        }
        // chceck if is empty space in backpack
        for (int i = 0; i < backpackItems.Length; i++)
        {
            if (backpackItems[i] == null)
            {
                backpackItems[i] = item;
                item.gameObject.transform.SetParent(backpack.transform);
                UpdateInventory(backpackItems, i);
                return;
            }
        }
        // chceck if is empty space in hotbar
        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null)
            {
                hotbarItems[i] = item;
                item.gameObject.transform.SetParent(hotbar.transform);
                UpdateInventory(hotbarItems, i);
                inventoryUI.ChangeHotbarIcons(i, item.icon);
                return;
            }
        }
        Debug.Log("Inventory is full");
        void UpdateInventory(Item[] list, int index)
        {
            if (inventoryUI.isInventoryOpen)
            {
                list[index].gameObject.transform.SetParent(itemsSpace);
                InventorySlot[] listOfSlots = inventoryUI.GetSlotListFromItemList(list);
                inventoryUI.CreateItemInUI(listOfSlots, index, list[index]);
            }
        }
    }

    public void RemoveItemFromInventory(int index, Item[] list)
    {
        if (list == hotbarItems && index == _equippedItemIndex) SetActiveItemToNullItem();
        if (list == hotbarItems) inventoryUI.ChangeHotbarIcons(index, null);
        list[index] = null;
    }


}