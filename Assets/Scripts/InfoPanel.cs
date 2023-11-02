using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InfoPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI costText;
    public Item item;
    int index;
    [SerializeField] Image image;
    public Button button;
    public Shop shop;
    public void SetItem(Item item, float cost, int index)
    {
        UpdatePrice(cost);
        this.item = item;
        this.index = index;
        image.sprite = item.icon;
    }

    public void UpdatePrice(float cost)
    {
        costText.text = cost.ToString();
    }

    public void OnClick()
    {
        shop.SetBuyItemInfo(item);
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

