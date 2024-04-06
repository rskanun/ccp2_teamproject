using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private Transform hpTranceform;
    [SerializeField] private SpriteRenderer hpBar;

    [Header("구간별 HP 색깔")]
    [SerializeField] private Color greenZone;
    [SerializeField] private Color yellowZone;
    [SerializeField] private Color redZone;

    // HP 구간 수치
    private float yellowLine = 0.5f;
    private float redLine = 0.1f;

    public void UpdateHP(float currentHP, float maxHP)
    {
        float percent = currentHP / maxHP;

        hpTranceform.localScale = new Vector3(percent, 1, 1);
        SetHpColor(percent);
    }

    private void SetHpColor(float percent)
    {
        if (percent <= redLine) hpBar.color = redZone;
        else if (percent <= yellowLine) hpBar.color = yellowZone;
        else hpBar.color = greenZone;
    }
}