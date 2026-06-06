using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player_Controller : MonoBehaviour
{
    [Header("movement settings")]
    [SerializeField] private float moveSpeed=5f;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;

    private readonly int animMoveXHash = Animator.StringToHash("moveX");    
    private readonly int animMoveYHash = Animator.StringToHash("moveY");
    private readonly int animSpeedHash = Animator.StringToHash("speed");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.freezeRotation = true;
        rb.gravityScale = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        UpdateAnim();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized;
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void UpdateAnim()
    {
        if(movement != Vector2.zero)
        {
            anim.SetFloat(animMoveXHash, movement.x);   
            anim.SetFloat(animMoveYHash, movement.y);
        }

        anim.SetFloat(animSpeedHash, movement.sqrMagnitude);
    }
}
