using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItemUI : MonoBehaviour
{
    [Header("관련 오브젝트")]
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image image;

    public void SetItem(ItemData item)
    {
        itemName.text = item.Name;
        itemDescription.text = item.Description;
        image.sprite = item.ItemImage;
    }
}