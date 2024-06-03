using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeUI : MonoBehaviour
{
    [Header("스킬 아이콘")]
    [SerializeField] private Image skillIcon;

    [Header("쿨타임")]
    [SerializeField] private GameObject skillLockedPanel;
    [SerializeField] private Image cooldownTimer;
    [SerializeField] private TextMeshProUGUI coolTimeText;

    // 스킬 정보
    private Skill activeSkill;

    private void Start()
    {
        activeSkill = LocalPlayerData.Instance.Class.ActiveSkill;

        skillIcon.sprite = activeSkill.Icon;
    }

    public void UpdateTime(float time)
    {
        if (time > 0)
        {
            // 스킬이 쿨타임이라면 쿨타임 UI 적용
            skillLockedPanel.SetActive(true);
            coolTimeText.gameObject.SetActive(true);
            cooldownTimer.gameObject.SetActive(true);

            // 남은 시간에 비례해서 시각화
            cooldownTimer.fillAmount = time / activeSkill.Cooldown;
            coolTimeText.text = ((int)time).ToString();
        }
        else
        {
            // 스킬의 쿨타임이 끝났다면 쿨타임 UI 적용 해제
            skillLockedPanel.SetActive(false);
            coolTimeText.gameObject.SetActive(false);
            cooldownTimer.gameObject.SetActive(false);
        }
    }
}