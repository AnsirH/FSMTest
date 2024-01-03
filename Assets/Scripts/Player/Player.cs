using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3f;

    Vector2 InputVector;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 nextVector = InputVector * moveSpeed * Time.deltaTime;

        rigid.MovePosition(rigid.position + nextVector);
    }

    void OnMove(InputValue value)
    {
        InputVector = value.Get<Vector2>();
    }
}
