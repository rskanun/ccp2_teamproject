using UnityEngine;

public abstract class Skill : ScriptableObject
{
    // 스킬 오브젝트 위치
    public const string SKILL_OBJECT_DIRECTORY = "Skills/Object/";

    [Header("표시 정보")]
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon
    {
        get { return _icon; }
    }

    [SerializeField]
    [Multiline]
    private string _description;
    public string Description
    {
        get { return _description; }
    }

    [Header("스킬 상세 정보")]
    [SerializeField]
    private float _cooldown;
    public float Cooldown
    {
        get { return _cooldown; }
    }

    public virtual void InitSkill(Player caster)
    {
        // 게임 시작 시 실행
    }

    public virtual void UseSkill(Player caster, Vector2 direction)
    {
        // 스킬 버튼을 눌렀을 경우 실행
    }

    public virtual void OnKillEvent(Player caster)
    {
        // 플레이어가 킬을 했을 경우 실행
    }
}