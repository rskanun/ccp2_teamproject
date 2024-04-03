using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 참조 컴포넌트
    private Rigidbody2D rigid;

    // 참조 스크립터블 오브젝트
    private LocalPlayerData status;

    // 이동 변수
    private Vector2 moveVec;

    // 스킬 변수
    private Skill normalAttack;
    private Skill skill;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        status = LocalPlayerData.Instance;

        // Init Skill
        ClassData classData = status.Class;

        normalAttack = classData.NormalAttack;
        skill = classData.ClassSkill;
    }

    private void Update()
    {
        // 키 입력 받기
        OnControlKeyPressed();

        // 기본 공격
        normalAttack.OnUseSkill();

        // 기본 공격 및 스킬 쿨다운
        CooldownSkills();
    }

    private void CooldownSkills()
    {
        normalAttack.CooldownSkill();
        skill.CooldownSkill();
    }

    /***************************************************************
    * [ 키 입력 ]
    * 
    * 키 입력에 따른 행동 조정
    ***************************************************************/

    private void OnControlKeyPressed()
    {
        OnMoveKeyPressed();
        OnSkillKeyPressed();
    }

    private void OnMoveKeyPressed()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveVec = new Vector2(horizontalInput, verticalInput);
    }

    private void OnSkillKeyPressed()
    {

    }

    private void FixedUpdate()
    {
        // 키 입력에 따른 플레이어 움직임
        Vector2 movement = moveVec.normalized * status.AGI * Time.deltaTime;

        rigid.MovePosition(rigid.position + movement);

        // 플레이어 좌표 갱신
        LocalPlayerData.Instance.Position = transform.position;
    }



}
