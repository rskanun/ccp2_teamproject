using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour, IControlState
{
    [Header("이벤트")]
    [SerializeField] private GameEvent deadEvent;
    [SerializeField] private GameEvent reviveEvent;

    // 참조 컴포넌트
    private Rigidbody2D rigid;
    private Player player;

    // 참조 스크립터블 오브젝트
    private PlayerData playerData;
    private ClassData classData;

    // 이동 변수
    private Vector2 moveVec;

    // 스킬 변수
    private Skill autoAttack;
    private float attackCooldown;
    private Skill skill;
    private float skillCooldown;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerData = LocalPlayerData.Instance.PlayerData;

        // Init Position In PlayerData
        playerData.Position = transform.position;

        // Init Skill & Normal Attack
        InitSkill();
    }

    private void InitSkill()
    {
        autoAttack = classData.PassiveSkill;
        attackCooldown = 0;

        skill = classData.ActiveSkill;
        skillCooldown = 0;
    }

    private void Update()
    {
        // 기본 공격
        OnNormalAttack();

        // 기본 공격 및 스킬 쿨다운
        CooldownSkills();
    }

    private void OnNormalAttack()
    {
        if (attackCooldown <= 0)
        {
            autoAttack.UseSkill(player);

            // 공격 후 쿨다운 적용
            attackCooldown = autoAttack.Cooldown;
        }
    }

    private void CooldownSkills()
    {
        attackCooldown -= Time.deltaTime;
        skillCooldown -= Time.deltaTime;
    }

    private void OnEnable()
    {
        // Set Control State
        ControlContext.Instance.SetState(this);

        // Notify Revive Event
        reviveEvent.NotifyUpdate();
    }

    private void OnDisable()
    {
        if (WaveData.Instance.IsRunning)
        {
            // Notify Dead Event
            deadEvent.NotifyUpdate();
        }
    }

    /***************************************************************
    * [ 키 입력 ]
    * 
    * 키 입력에 따른 행동 조정
    ***************************************************************/

    public void OnControlKeyPressed()
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
        if (Input.GetButtonDown("Skill"))
        {
            if (skillCooldown <= 0)
            {
                skill.UseSkill(player);

                // 스킬 사용 후 쿨다운 적용
                skillCooldown = skill.Cooldown;
            }
        }
    }

    private void FixedUpdate()
    {
        // 키 입력에 따른 플레이어 움직임
        Vector2 movement = moveVec.normalized * playerData.MoveSpeed * Time.deltaTime;

        rigid.MovePosition(rigid.position + movement);

        // 플레이어 좌표 갱신
        playerData.Position = transform.position;
    }
}
