using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private ClassData classData;

    // 참조 컴포넌트
    private Rigidbody2D rigid;

    // 플레이어 움직임 제어 변수
    private Vector2 position;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        position = new Vector2(horizontalInput, verticalInput);

    }

    void FixedUpdate()
    {
        Vector2 movement = position.normalized * classData.Speed * Time.deltaTime;

        rigid.MovePosition(rigid.position + movement);
    }



}
