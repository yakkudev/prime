using System;
using UnityEngine;


public class Storage : Item, IUsable, IRefreshInteractionTypes
{
    void Start()
    {
        Usable = this;
        RefreshInteractionTypes = this;
    }
    public void RefreshInteractions() { }
    public void Use(InteractionType interactionType, InteractionData interactionData)
    {
        if (interactionType == InteractionType.OpeningUI)
        {
            InventoryManager.inventoryUI.OpenStorage(itemsInStorage, itemName);
        }
        
    }
}

