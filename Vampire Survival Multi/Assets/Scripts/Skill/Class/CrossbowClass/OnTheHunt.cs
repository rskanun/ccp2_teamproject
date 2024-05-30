using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/CrossbowClass/ActiveSkill", fileName = "On The Hunt")]
public class OnTheHunt : Skill
{
    public override void OnKillEvent(Player caster)
    {
        // 킬 당 공격력 증가 수치
        float increaseSTR = 1; 

        // 플레이어의 공격력에 증가 시킴
        caster.SetBuffSTR(caster.PlayerData.BuffSTR + increaseSTR);
    }
}