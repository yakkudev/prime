using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ToolTip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI headerField;
    [SerializeField] TextMeshProUGUI descriptionField;
    [SerializeField] TextMeshProUGUI stackField;
    [SerializeField] LayoutElement layoutElement;

    public RectTransform rect;
        
    [SerializeField] int characterWrapLimit;

    public void SetText(int stack, int maxStack, string desc, string nameOfItem)
    {
        headerField.text = nameOfItem;
        descriptionField.text = desc;
        //stackField.text = stack + "/" + maxStack;
        int headerLength = headerField.text.Length;
        int descriptionLength = descriptionField.text.Length;
            
        layoutElement.enabled = (headerLength > characterWrapLimit || descriptionLength > characterWrapLimit) ? true : false;
    }

    void Update()
    {
        Vector2 position = Input.mousePosition;
        float x = position.x / Screen.width;
        float y = position.y / Screen.height;
        if (x <= y && x <= 1 - y) //left
            rect.pivot = new Vector2(-0.15f, y);
        else if (x >= y && x <= 1 - y) //bottom
            rect.pivot = new Vector2(x, -0.1f);
        else if (x >= y && x >= 1 - y) //right
            rect.pivot = new Vector2(1.1f, y);
        else if (x <= y && x >= 1 - y) //top
            rect.pivot = new Vector2(x, 1.3f);
        transform.position = position;
    }
}

