using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Transform hotbarGrid;
    [SerializeField] Transform backpackGrid;

    [SerializeField] InventorySlot[] slotsOfHotbar;
    [SerializeField] InventorySlot[] slotsOfBackpack;
    public bool isInventoryOpen { get; private set; }
    Inventory playerInventory;
    [SerializeField] GameObject inventoryONOFF;
    [SerializeField] GameObject itemIconPrefab;
    public Transform spaceForDraggableItems;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] TMP_Text moneyText;
    [Header("Storage")]
    [SerializeField] GameObject fillStorageSpace;
    [SerializeField] Transform storageGrid;
    [SerializeField] GameObject storageUI;
    [SerializeField] TMP_Text storageNameText;

    [Header("Hotbar")]
    [SerializeField] Image[] hotbarImages;
    [SerializeField] GameObject hotbar;
    [SerializeField] TMP_Text[] hotbarNumbers;
    // Additional GUI
    GameObject additionalGUI;

    void Start()
    {
        InventoryManager.inventoryUI = this;
        Debug.Log("sus");
    }

    public InventorySlot[] GetSlotListFromItemList(Item[] list)
    {
        if (list == playerInventory.hotbarItems) return slotsOfHotbar;
        return slotsOfBackpack;
    }

    public void ToggleUI()
    {
        playerInventory = InventoryManager.playerInventory;

        inventoryONOFF.SetActive(!isInventoryOpen);

        if (isInventoryOpen)
            CloseInventory();
        else
            OpenInventory();

        isInventoryOpen = !isInventoryOpen;
        hotbar.SetActive(hotbar.activeSelf ? false : true);
    }
    void OpenInventory()
    {
        RefreshInventory();
    }

    public void ChangeHotbarIcons(int index, Sprite icon)
    {
        hotbarImages[index].sprite = icon;
    }

    public void ChangeActiveItemInHotbar(int indexToTurnOn, int indexToTurnOff)
    {
        if (indexToTurnOff == -1)
        {
            foreach (var t in hotbarNumbers)
            {
                t.color = Color.white;
            }

            indexToTurnOff = 0;
        }
        hotbarNumbers[indexToTurnOff].color = Color.white;
        hotbarNumbers[indexToTurnOn].color = Color.yellow;
    }

    void CloseInventory()
    {
        storageUI.SetActive(false);
        fillStorageSpace.SetActive(true);
        if (additionalGUI != null) additionalGUI.SetActive(false);

        storageNameText.text = "";
        for (int i = 0; i < storageGrid.transform.childCount; i++)
        {
            Destroy(storageGrid.transform.GetChild(i).gameObject);
        }

        ToolTipManager.Hide();
    }

    public void OpenBuildingGUI(GameObject GUI)
    {
        ToggleUI();
        additionalGUI = GUI;
        fillStorageSpace.SetActive(false);
        additionalGUI.SetActive(true);
    }
    public void OpenStorage(Item[] itemsInStorage, string storageName)
    {
        ToggleUI();
        fillStorageSpace.SetActive(false);
        storageUI.SetActive(true);
        storageNameText.text = storageName;

        InventorySlot[] slotsOfStorage = new InventorySlot[itemsInStorage.Length];

        for (int i = 0; i < slotsOfStorage.Length; i++)
        {
            slotsOfStorage[i] = Instantiate(slotPrefab, storageGrid).GetComponent<InventorySlot>();
            slotsOfStorage[i].index = i;
            slotsOfStorage[i].itemList = itemsInStorage;
        }

        for (int i = 0; i < itemsInStorage.Length; i++)
        {
            if (itemsInStorage[i] != null)
            {
                CreateItemInUI(slotsOfStorage, i, itemsInStorage[i]);
            }
        }
    }

    public void RefreshMoney()
    {
        moneyText.text = playerInventory.money + "$";
    }
    void RefreshInventory()
    {
        /*ClearCrateFields();*/ // update it later

        RefreshMoney();

        for (int i = 0; i < slotsOfHotbar.Length; i++)
        {
            if (slotsOfHotbar[i] != null) Destroy(slotsOfHotbar[i].gameObject);
        }
        for (int i = 0; i < slotsOfBackpack.Length; i++)
        {
            if (slotsOfBackpack[i] != null) Destroy(slotsOfBackpack[i].gameObject);
        }

        CreateSlots(playerInventory.hotbarItems, slotsOfHotbar, hotbarGrid);
        CreateSlots(playerInventory.backpackItems, slotsOfBackpack, backpackGrid);

        for (int i = 0; i < playerInventory.hotbarItems.Length; i++)
        {

            if (playerInventory.hotbarItems[i] != null)
            {
                CreateItemInUI(slotsOfHotbar, i, playerInventory.hotbarItems[i]);
            }
        }

        for (int i = 0; i < playerInventory.backpackItems.Length; i++)
        {
            if (slotsOfBackpack[i].gameObject.transform.childCount != 0)
            {
                Destroy(slotsOfBackpack[i].gameObject.transform.GetChild(0).gameObject);
            }

            if (playerInventory.backpackItems[i] != null)
            {
                CreateItemInUI(slotsOfBackpack, i, playerInventory.backpackItems[i]);
            }
        }

        void CreateSlots(Item[] items, InventorySlot[] slots, Transform grid)
        {
            for (int i = 0; i < items.Length; i++)
            {
                slots[i] = Instantiate(slotPrefab, grid).GetComponent<InventorySlot>();
                slots[i].index = i;
                slots[i].itemList = items;
            }
        }
    }
    public void CreateItemInUI(InventorySlot[] inventorySlots, int index, Item item)
    {
        if (inventorySlots[index].transform.childCount != 0) Destroy(inventorySlots[index].transform.GetChild(0).gameObject);
        GameObject itemGameObject = Instantiate(itemIconPrefab, inventorySlots[index].transform);
        DraggableItem UIItem = itemGameObject.GetComponent<DraggableItem>();
        UIItem.indexOfParent = index;
        UIItem.item = item;
        UIItem.SetImage(item.icon);
        inventorySlots[index].draggableItemInSlot = UIItem;
    }
}
