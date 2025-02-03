using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        Debug.Log("점프진입");
        player.view.BroadCastTriggerParameter(E_AniParameters.Jumping);
        player.model.InvokePlayerJumped();
        if(player.isJumpable)
            Jump();
        
    }

    public override void Update()
    {
        // 임시
        if (player.rb.velocity.y < 0.1f)// && !player.isGrounded)
        {
            player.ChangeState(E_PlayeState.Fall);
        }
        else if (RemoteInput.inputs[player.model.playerNumber].divingInput)
        {
            player.ChangeState(E_PlayeState.Diving);
        }
    }
    private void Jump()
    {
        Debug.Log("점프");
        Vector3 targetVel = player.rb.velocity;
        targetVel.y = player.model.jumpForce;
        player.rb.velocity = targetVel;
        player.isJumpable = false;
    }

    public override void Exit()
    {
        
    }
}
