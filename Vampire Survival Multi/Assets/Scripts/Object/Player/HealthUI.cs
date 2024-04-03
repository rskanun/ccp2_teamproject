using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private TextMeshProUGUI tmpHP;

    public void UpdateHP(float currentHP, float maxHP)
    {
        tmpHP.text = string.Format("HP : {0} / {1}", currentHP, maxHP);
    }
}