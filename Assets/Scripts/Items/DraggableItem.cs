using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    /*[HideInInspector]*/
    public Transform parentAfterDrag;
    public Image image;
    public int indexOfParent;
    public Item item;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        

        transform.SetParent(InventoryManager.inventoryUI.spaceForDraggableItems);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

    public void SetImage(Sprite texture)
    {
        if (texture == null)
        {
            Debug.Log("Texture is null");
            return;
        }

        image.sprite = texture;
    }

    // tooltip

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipManager.Show(item.stackSize, item.maxStackSize, item.description, item.itemName);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Hide();
    }
}

