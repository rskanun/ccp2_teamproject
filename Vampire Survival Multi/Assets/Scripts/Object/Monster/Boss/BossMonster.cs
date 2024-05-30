using UnityEngine;

public abstract class BossMonster : Monster
{
    protected const string OBJECT_DIRECTION = "Objects/Monster/Boss/SkillObject/";

    [Header("참조 이벤트")]
    [SerializeField] private GameEvent bossClearEvent;

    protected override void OnDead(Player killPlayer)
    {
        bossClearEvent.NotifyUpdate();

        base.OnDead(killPlayer);
    }

    protected override int GetMonsterExp()
    {
        // 보스 몹 경험치 지급
        int level = GameData.Instance.Level;
        int requireExp = LevelResource.Instance.GetRequireExp(level);

        return (int)(requireExp * 1.1f);
    }
}