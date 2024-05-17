using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/ShieldClass/NormalAttack", fileName = "Shield Swing")]
public class ShieldAttack : Skill
{
    [Header("타격 오브젝트")]
    [SerializeField] private GameObject swingPrefab;

    public override void UseSkill(Player caster)
    {
        string prefabName = SKILL_OBJECT_DIRECTORY + swingPrefab.name;
        GameObject obj = PhotonNetwork.Instantiate(prefabName, CasterData.Position, Quaternion.identity);
        obj.transform.SetParent(caster.transform);

        // 스킬 오브젝트 생성 위치 설정
        obj.transform.position = CasterData.Position;

        // 타격 방향 설정
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 casterPos = CasterData.Position;
        Vector2 direction = mousePos - casterPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        SwingObject swingObj = obj.GetComponent<SwingObject>();

        swingObj.OnSwing(angle, CasterData.AttackSpeed, false, (monster) =>
        {
            float damage = CasterData.DEF * 0.75f + CasterData.STR * 0.25f;

            caster.OnNormalAttack(monster, damage);
        });
    }
}