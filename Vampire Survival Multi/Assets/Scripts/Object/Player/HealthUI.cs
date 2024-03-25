using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Header("참조 컴포넌스")]
    [SerializeField] private TextMeshProUGUI tmpHP;

    public void UpdateHP(int currentHP, int hp)
    {
        tmpHP.text = string.Format("HP : {0} / {1}", currentHP, hp);
    }
}