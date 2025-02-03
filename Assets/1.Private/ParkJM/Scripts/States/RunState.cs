using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerState
{
    Vector3 targetVelocity;
    public RunState(PlayerController player) : base(player)
    {
        //animationIndex = (int)E_PlayeState.Run;
    }

    public override void Enter()
    {
        //player.view.SetBoolParameter(E_AniParameters.Running, true);

        //player.view.BroadCastBoolParameter(E_AniParameters.Running, true);
        player.view.SetBoolParameter(E_AniParameters.Running, true);



        //player.view.SetAnimationBoolTrue(E_PlayeState.Run);
        //player.view.PlayRun();
        //player.view.PlayAnimation(animationIndex);
    }

    public override void Update()
    {
        if (player.jumpBufferCounter > 0f && player.isJumpable)
        {
            player.ChangeState(E_PlayeState.Jump);
        }
        else if(!player.isGrounded)
        {
            player.ChangeState(E_PlayeState.Fall);
        }

        else if (player.moveDir.sqrMagnitude < 0.1f) //== Vector3.zero)
        {
            player.ChangeState(E_PlayeState.Idle);
        }
        else if (RemoteInput.inputs[player.model.playerNumber].grabInput)
        {
            player.ChangeState(E_PlayeState.Grabbing);
        }

    }

    public override void FixedUpdate()
    {
        Run();
        LookForward();
    }

    public override void Exit()
    {
        player.view.SetBoolParameter(E_AniParameters.Running, false);
        targetVelocity = Vector3.zero;
    }

    private void Run()
    {
        if (player.isSlope)
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(player.moveDir, player.chosenHit.normal).normalized;
            targetVelocity = slopeDirection * player.model.moveSpeed;
        }
        else
        {
            targetVelocity = player.moveDir * player.model.moveSpeed;
            targetVelocity.y = player.rb.velocity.y;
        }

        Vector3 moveForce = targetVelocity - player.rb.velocity;

        if (player.onConveyor)
        {
            player.rb.AddForce(moveForce + player.conveyorVel, ForceMode.VelocityChange);
        }
        else
        {
            player.rb.AddForce(moveForce, ForceMode.VelocityChange);
        }
    }
    private void LookForward()
    {
        if (player.moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(player.moveDir);
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

            player.transform.rotation = Quaternion.Slerp(
                player.transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * 15
            );
        }
    }
}
