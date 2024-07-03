using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement1 : MonoBehaviour
{

    public Animator animator;
    
    public RectTransform healthTransform;
    private float cachedY;
    private float minXValue;
    private float maxXValue;
    private int currentHealth;
    public float speed;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;
    
    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 8f;

    private enum MovementState { idle, running, jumping, falling }
    private MovementState state = MovementState.idle;

    public int maxHealth;
    public Text healthText;
    public Image visualHealth;
    public float coolDown;
    private bool onCD;

    [SerializeField] public KeyCode left;
    [SerializeField] public KeyCode right;
    [SerializeField] public KeyCode jump;
    [SerializeField] public KeyCode attack;

    // Start is called before the first frame update
    private void Start()
    {
        cachedY = healthTransform.position.y;
        maxXValue = healthTransform.position.x;
        minXValue = healthTransform.position.x - healthTransform.rect.width;
        currentHealth = maxHealth;
        onCD = false;

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    private void Update()
    {

        HandleMovement();

        if (healthTransform.GetComponent<CanvasRenderer>().cullTransparentMesh != true)
        {
        }
        else
        {
            healthTransform.GetComponent<CanvasRenderer>().cullTransparentMesh = false;
        }

        dirX = Input.GetAxisRaw("Horizontal1");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if(Input.GetButtonDown("Jump1") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }

    private void HandleMovement()
    {
        float translation = speed * Time.deltaTime;

        transform.Translate(new Vector3(Input.GetAxis("Horizontal1")* translation ,0, Input.GetAxis("Vertical1") * translation));
    }


    private float Mapvalues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if(dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
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

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}

