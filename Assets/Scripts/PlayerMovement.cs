using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    public float wallJumpCooldown { get; set; }

    private Vector2 movement;
    private Vector2 screenbounds;
    private float playerHalfWidth;
    private float xPosLastFrame;

    private void Start()
    {
        screenbounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        playerHalfWidth = spriteRenderer.bounds.extents.x;
        spriteRenderer.flipX = false;

    }

    void Update()
    // Miscare orizontala right here 
    {
        HandleMovement();
        // ClampedMovement();
        FlipCharacterX();

        if (wallJumpCooldown > 0f)
        {
            wallJumpCooldown -= Time.deltaTime;
        }
    }

    private void FlipCharacterX() // Orientare la mers

    {
        float input = Input.GetAxisRaw("Horizontal");
        if (input > 0 && (transform.position.x > xPosLastFrame))
        { //Mers la dreapta
            spriteRenderer.flipX = false;
        } //Mers la stanga
        else if (input < 0 && (transform.position.x < xPosLastFrame))
        {
            spriteRenderer.flipX = true;
        }

        xPosLastFrame = transform.position.x;
    }

    private void ClampedMovement()
    {
        // In screen position
        float clampedX = Mathf.Clamp(transform.position.x, -screenbounds.x + playerHalfWidth, screenbounds.x - playerHalfWidth);
        Vector2 pos = transform.position; // Geth the player current position
        pos.x = clampedX; // Reassign x value to clamped position
        transform.position = pos; // Reassign clamped position back to the player
    }

    private void HandleMovement()
    {

        if (wallJumpCooldown > 0f) return;

        float input = Input.GetAxisRaw("Horizontal");
        movement.x = input * movementSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
}
