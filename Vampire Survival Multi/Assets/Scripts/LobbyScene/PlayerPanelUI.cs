using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerPanelUI : MonoBehaviour
{
    [Header("구성 오브젝트")]
    [SerializeField] private GameObject playerMenu;
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private GameObject closedMark;
    [SerializeField] private GameObject playerName;
    [SerializeField] private TextMeshProUGUI className;

    public void SetActiveCloseMark(bool isActive)
    {
        closedMark.SetActive(isActive);
    }

    public void SetActiveCharacter(bool isActive)
    {
        playerCharacter.SetActive(isActive);
    }

    public void SetActivePlayerMenu(bool isActive)
    {
        playerMenu.SetActive(isActive);
    }

    public void SetActivePlayerName(bool isActive)
    {
        playerName.SetActive(isActive);
    }

    [PunRPC]
    public void SetClassName(string name)
    {
        className.text = name;
    }
}