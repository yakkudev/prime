using UnityEngine;


public class ToolTipManager : MonoBehaviour
{
    [SerializeField] ToolTip toolTip;
    private static ToolTipManager current;
    void Awake()
    {
        current = this;
    }

    public static void Show(int stack, int maxStack, string desc, string name)
    {
        current.toolTip.gameObject.SetActive(true);
        current.toolTip.SetText(stack, maxStack, desc, name);
    }
    public static void Hide()
    {
        current.toolTip.gameObject.SetActive(false);
    }
}

