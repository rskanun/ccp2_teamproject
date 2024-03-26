using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 참조 컴포넌트
    private Rigidbody2D rigid;

    // 참조 스크립터블 오브젝트
    private PlayerStatus status;

    // 플레이어 움직임 제어 변수
    private Vector2 position;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        status = PlayerStatus.Instance;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        position = new Vector2(horizontalInput, verticalInput);

    }

    private void FixedUpdate()
    {
        Vector2 movement = position.normalized * status.AGI * Time.deltaTime;

        rigid.MovePosition(rigid.position + movement);
    }



}
