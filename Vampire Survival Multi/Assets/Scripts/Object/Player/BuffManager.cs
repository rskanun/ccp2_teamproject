using System.Collections.Generic;

public enum Buff
{
    Invisible // 은신
}
public class BuffManager
{
    // 현재 버프 목록
    private Dictionary<Buff, float> buffs;

    public BuffManager()
    {
        buffs = new Dictionary<Buff, float>();
    }

    public void AddBuff(Buff buff, float duration)
    {
        // 이미 해당 버프가 걸려있을 경우
        if (buffs.ContainsKey(buff))
        {
            // 해당 버프 지속시간 갱신
            buffs[buff] = duration;
        }
        else
        {
            // 버프 추가
            buffs.Add(buff, duration);
        }
    }

    public bool HasBuff(Buff buff)
    {
        return buffs.ContainsKey(buff);
    }

    public void BuffTimer(float time)
    {
        foreach (Buff key in new List<Buff>(buffs.Keys))
        {
            // 지속시간 감소
            buffs[key] -= time;

            if (buffs[key] <= 0)
            {
                // 0초 이하가 되면 목록에서 삭제
                buffs.Remove(key);
            }
        }
    }
}