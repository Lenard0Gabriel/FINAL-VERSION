using UnityEngine;

public class MobilePlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public bool isSafe = false;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float moveDirection = 0f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private PlayerHiding hidingScript; // Reference to hiding script

    public float MoveDirection => moveDirection;

    // 🔧 Added: Expose facing direction
    public bool FacingRight { get; private set; } = true; // 🔧 Added

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hidingScript = GetComponent<PlayerHiding>();
    }

    private void Update()
    {
        // If hiding, ignore all movement
        if (isSafe || (hidingScript != null && hidingScript.isHidden))
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            animator.SetFloat("Speed", 0);
            return;
        }

        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // Flip sprite and set facing direction
        if (moveDirection > 0)
        {
            spriteRenderer.flipX = false;
            FacingRight = true; // 🔧 Added
        }
        else if (moveDirection < 0)
        {
            spriteRenderer.flipX = true;
            FacingRight = false; // 🔧 Added
        }

        animator.SetFloat("Speed", Mathf.Abs(moveDirection));
        animator.SetBool("IsJumping", !isGrounded);
    }

    // BUTTON METHODS
    public void OnLeftButtonDown()
    {
        if (hidingScript != null && hidingScript.isHidden)
        {
            hidingScript.Unhide(); // Automatically unhide on move
        }
        moveDirection = -1f;
    }

    public void OnLeftButtonUp() => moveDirection = 0f;

    public void OnRightButtonDown()
    {
        if (hidingScript != null && hidingScript.isHidden)
        {
            hidingScript.Unhide(); // Automatically unhide on move
        }
        moveDirection = 1f;
    }

    public void OnRightButtonUp() => moveDirection = 0f;

    public void OnJumpButton()
    {
        if (isGrounded && !isSafe && (hidingScript == null || !hidingScript.isHidden))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
