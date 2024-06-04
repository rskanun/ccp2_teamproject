using TMPro;
using UnityEngine;

public class SearchCodePanel : MonoBehaviour
{
    public GameObject roomCodePanel;
    [SerializeField]
    private TMP_InputField searchCodeField;

    public void openSearchCodePanel()
    {
        roomCodePanel.SetActive(true);
    }

    public void closeSearchCodePanel()
    {
        roomCodePanel.SetActive(false);
    }
    public void ClearSearchField()
    {
        searchCodeField.text = "";
    }

}