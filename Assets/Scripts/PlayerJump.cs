using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    [SerializeField] private PlayerMovementState playerMovementState;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float jumpforce = 10;
    [SerializeField] private float doublejumpForce = 6f;

    [SerializeField] private Vector2 wallJumpForce = new Vector2(4f, 8f);
    [SerializeField] private float wallJumpMovementCooldown = 0.2f;

    private PlayerMovement playerMovement;

    private float playerHalfHeight;
    private float playerHalfWidth;

    private bool canDoubleJump;

    private void Start()
    {
        playerHalfWidth = spriteRenderer.bounds.extents.x;
        playerHalfHeight = spriteRenderer.bounds.extents.y;
        playerMovement = GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckJumpType();
        }


    }

    private void CheckJumpType()
    {
        bool isGrounded = GetIsGrounded();

        if (isGrounded)
        {
            playerMovementState.SetMoveState(PlayerMovementState.MoveState.jump);
            Jump(jumpforce);
        }
        else
        {
            int direction = GetWallJumpDirection();
            if (direction == 0 && canDoubleJump && rigidBody.linearVelocity.y <= 0.1f)
            {
                DoubleJump();
            }
            else if (direction != 0)
            {
                WallJump(direction);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetIsGrounded();
    }

    private int GetWallJumpDirection()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right, playerHalfWidth + 0.1f, LayerMask.GetMask("Ground")))
        {
            return -1;
        }
        if (Physics2D.Raycast(transform.position, Vector2.left, playerHalfWidth - 0.1f, LayerMask.GetMask("Ground")))
        {
            return 1;
        }
        return 0;
    }

    private bool GetIsGrounded()
    {
        bool hit = Physics2D.Raycast(transform.position, Vector2.down, playerHalfHeight + 0.1f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            canDoubleJump = true;
        }

        return hit;
    }

    private void DoubleJump()
    {
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
        Jump(doublejumpForce);
        canDoubleJump = false;
        playerMovementState.SetMoveState(PlayerMovementState.MoveState.double_jump);

    }

    private void WallJump(int direction)
    {
        Vector2 force = wallJumpForce;
        force.x *= direction;
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
        playerMovement.wallJumpCooldown = wallJumpMovementCooldown;
        rigidBody.AddForce(force, ForceMode2D.Impulse);
        playerMovementState.SetMoveState(PlayerMovementState.MoveState.wall_jump);
    }

    private void Jump(float force)
    {
        rigidBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }
}
