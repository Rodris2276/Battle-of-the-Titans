using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections;
using Unity.Mathematics;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public SpriteRenderer sprite;
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    public Animator animator;

    float speed = 0;
    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private LayerMask jumpableGround;

    private enum MovementState { idle, running, jumping, falling }
    private MovementState state = MovementState.idle;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();

        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }

    private void HandleMovement()
    {
        float translation = speed * Time.deltaTime;

        transform.Translate(new Vector2(Input.GetAxis("Horizontal") * translation, Input.GetAxis("Vertical") * translation));
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if(dirX > 0f)
        {
            state = MovementState.running;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            state = MovementState.idle;
        }

        if(rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }

        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        animator.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}

