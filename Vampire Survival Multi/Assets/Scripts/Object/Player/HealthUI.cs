using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private TextMeshProUGUI tmpHP;

    public void UpdateHP()
    {
        PlayerStatus status = PlayerStatus.Instance;

        int curHP = status.HP;
        int MaxHP = status.MaxHP;

        tmpHP.text = string.Format("HP : {0} / {1}", curHP, MaxHP);
    }
}