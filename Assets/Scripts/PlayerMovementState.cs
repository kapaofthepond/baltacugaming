using UnityEngine;
using System;

public class PlayerMovementState : MonoBehaviour
{
    public enum MoveState
    {
        idle,
        run,
        jump,
        fall,
        double_jump,
        wall_jump,

    }

    public MoveState CurrentMoveState { get; private set; }

    [SerializeField] private Animator animator;

    [SerializeField] private Rigidbody2D rigidBody;
    private const string idleAnim = "idle";
    private const string runAnim = "run";
    private const string jumpAnim = "jump";
    private const string fallAnim = "fall";
    private const string doubleJumpAnim = "double jump";
    private const string wallJumpAnim = "wall jump";
    public static Action<MoveState> OnPlayerMoveStateChange;
    private float xPosLastFrame;

    private void Update()
    {
        if (transform.position.x == xPosLastFrame && rigidBody.linearVelocity.y == 0)
        {
            SetMoveState(MoveState.idle);
        }
        else if (transform.position.x != xPosLastFrame && rigidBody.linearVelocity.y == 0)
        {
            SetMoveState(MoveState.run);
        }
        else if (rigidBody.linearVelocity.y < 0)
        {
            SetMoveState(MoveState.fall);
        }

        xPosLastFrame = transform.position.x;

    }

    public void SetMoveState(MoveState moveState)
    {
        if (moveState == CurrentMoveState) return;

        switch (moveState)
        {
            case MoveState.idle:
                HandleIdle();
                break;

            case MoveState.jump:
                HandleJump();
                break;

            case MoveState.fall:
                HandleFall();
                break;

            case MoveState.run:
                HandleRun();
                break;

            case MoveState.double_jump:
                HandleDoubleJump();
                break;

            case MoveState.wall_jump:
                HandleWallJump();
                break;

            default:
                Debug.LogError($"Invalid movement state: {moveState}");
                break;

        }

        OnPlayerMoveStateChange?.Invoke(moveState);
        CurrentMoveState = moveState;
    }


    private void HandleIdle()
    {
        animator.Play(idleAnim);
    }

    private void HandleJump()
    {
        animator.Play(jumpAnim);
    }
    private void HandleFall()
    {
        animator.Play(fallAnim);
    }
    private void HandleRun()
    {
        animator.Play(runAnim);
    }
    private void HandleDoubleJump()
    {
        animator.Play(doubleJumpAnim);
    }
    private void HandleWallJump()
    {
        animator.Play(wallJumpAnim);
    }

}