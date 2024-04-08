using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private ClassData classData;

    Rigidbody2D rb;

    Vector2 position;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        position = new Vector2(horizontalInput, verticalInput).normalized;

    }

    void FixedUpdate()
    {
        Vector2 movement = position * classData.MoveSpeed * Time.deltaTime;

        rb.MovePosition(rb.position + movement);
    }





}
