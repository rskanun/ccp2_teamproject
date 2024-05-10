using UnityEngine;

public class BossMonster : Monster
{
    [Header("참조 이벤트")]
    [SerializeField] private GameEvent bossClearEvent;

    protected override void OnDead(Player killPlayer)
    {
        base.OnDead(killPlayer);

        bossClearEvent.NotifyUpdate();
    }

    protected override int GetMonsterExp()
    {
        // 보스 몹 경험치 지급
        int level = GameData.Instance.Level;
        int requireExp = LevelResource.Instance.GetRequireExp(level);

        return (int)(requireExp * 1.1f);
    }
}