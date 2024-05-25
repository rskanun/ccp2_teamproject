using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private Skill skill;

    private float lastUseTime;

    void Start()
    {
        skillIcon.sprite = skill.Icon;
        lastUseTime = -skill.Cooldown; // 처음에 스킬을 바로 사용할 수 있도록 설정
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) // 예시로 스페이스바를 스킬 사용키로 설정
        {
            if (Time.time - lastUseTime >= skill.Cooldown)
            {
                // 스킬 사용 로직
                // 예: skill.UseSkill(player);
                lastUseTime = Time.time;
            }
        }
    }
}