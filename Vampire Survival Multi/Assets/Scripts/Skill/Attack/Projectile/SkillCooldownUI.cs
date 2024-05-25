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
        lastUseTime = -skill.Cooldown; // ó���� ��ų�� �ٷ� ����� �� �ֵ��� ����
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) // ���÷� �����̽��ٸ� ��ų ���Ű�� ����
        {
            if (Time.time - lastUseTime >= skill.Cooldown)
            {
                // ��ų ��� ����
                // ��: skill.UseSkill(player);
                lastUseTime = Time.time;
            }
        }
    }
}