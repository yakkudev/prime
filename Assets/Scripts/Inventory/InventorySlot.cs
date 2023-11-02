using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public int index;

    public Item[] itemList;
    public Item[] listOfAllowedItems;
    [SerializeField] bool canStoreAnything = true;
    public Transform transformWhereItemsAreStored;
    public DoSomethingWhenSlotChanged doSomethingWhenSlotChanged;
    public bool canUserPutInItems = true;
    public DraggableItem draggableItemInSlot;
    public void OnDrop(PointerEventData eventData)
    {
        if (!canUserPutInItems) return;


        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();

        if (!CheckIfItemCanBePlacedInSlot() || !canUserPutInItems) return;

        draggableItem.indexOfParent = index;
        InventorySlot inventorySlotOfItem = draggableItem.parentAfterDrag.gameObject.GetComponent<InventorySlot>();
        if (transform.childCount != 0)
        {
            if (draggableItem.item.itemName == draggableItemInSlot.item.itemName)
            {
                if (draggableItemInSlot.item.stackSize + draggableItem.item.stackSize <=
                    draggableItemInSlot.item.maxStackSize)
                {
                    draggableItemInSlot.item.stackSize += draggableItem.item.stackSize;
                    Destroy(draggableItem.gameObject);
                    return;
                }
                else
                {
                    int amount = draggableItemInSlot.item.maxStackSize - draggableItemInSlot.item.stackSize;
                    draggableItemInSlot.item.stackSize += amount;
                    draggableItem.item.stackSize -= amount;
                    return;
                }
            }
            transform.GetChild(0).GetComponent<DraggableItem>().parentAfterDrag = draggableItem.parentAfterDrag;
            transform.GetChild(0).transform.SetParent(draggableItem.parentAfterDrag);
            inventorySlotOfItem.draggableItemInSlot = draggableItemInSlot;
        }
        draggableItem.parentAfterDrag = transform;

        // add inventory manager here

        draggableItemInSlot = draggableItem;
        InventoryManager.playerInventory.ChangePositionOfItems(inventorySlotOfItem, this, inventorySlotOfItem.transformWhereItemsAreStored, transformWhereItemsAreStored);
        if (doSomethingWhenSlotChanged != null) doSomethingWhenSlotChanged.DoSomething(true);
        if (inventorySlotOfItem.doSomethingWhenSlotChanged != null) inventorySlotOfItem.doSomethingWhenSlotChanged.DoSomething(false);
        bool CheckIfItemCanBePlacedInSlot()
        {
            // here you can build some logic to check if item can be placed in slot if needed

            if (canStoreAnything) return true;

            for (int i = 0; i < listOfAllowedItems.Length; i++)
            {
                if (listOfAllowedItems[i].itemName == draggableItem.item.itemName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

