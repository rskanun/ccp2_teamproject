using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("구성 오브젝트")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private GameObject descriptionPanel;

    // 슬롯 상태
    private bool isSlotFilled = false;

    public void SetItem(ItemData item)
    {
        itemIcon.sprite = item.ItemImage;
        itemName.text = item.Name;
        itemDescription.text = item.Description;

        isSlotFilled = true;
    }

    public void ActiveDescription(bool active)
    {
        if (isSlotFilled)
        {
            descriptionPanel.SetActive(active);
        }
    }
}