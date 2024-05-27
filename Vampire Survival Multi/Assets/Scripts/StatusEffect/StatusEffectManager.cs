using System.Collections.Generic;
public enum StatusEffect
{
    Stun, // 기절
    Weakness, // 취약
}
public class StatusEffectManager
{
    // 현재 객체가 가지고 있는 상태이상 목록
    private Dictionary<StatusEffect, float> effects;

    public StatusEffectManager()
    {
        effects = new Dictionary<StatusEffect, float>();
    }

    public void AddEffect(StatusEffect type, float duration)
    {
        // 이미 해당 상태이상이 걸려있을 경우
        if (HasStatusEffect(type))
        {
            // 해당 상태이상 지속시간 갱신
            effects[type] = duration;
        }
        else
        {
            // 상태이상 추가
            effects.Add(type, duration);
        }
    }

    public bool HasStatusEffect(StatusEffect type)
    {
        return effects.ContainsKey(type);
    }

    public void EffectTimer(float time)
    {
        foreach(StatusEffect key in new List<StatusEffect>(effects.Keys))
        {
            // 지속시간 감소
            effects[key] -= time;

            if (effects[key] <= 0)
            {
                // 0초 이하가 되면 목록에서 삭제
                effects.Remove(key);
            }
        }
    }
}