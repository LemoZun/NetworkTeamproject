using UnityEngine;

namespace _1.Private.ParkJM.Scripts.States
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            Debug.Log("Idle 진입");
            player.view.SetBoolParameter(E_AniParameters.Idling, true);
            player.rb.velocity = Vector3.zero;
            player.isJumpable = true;
        }

        public override void Update()
        {
            if (player.jumpBufferCounter > 0f && player.isJumpable)
            {
                player.ChangeState(E_PlayeState.Jump);
            }
            else if (player.moveDir != Vector3.zero && player.isGrounded )
            {
                //Debug.Log("이동 입력 idle 상태에 들어옴");
                player.ChangeState(E_PlayeState.Run);
            }
            else if(!player.isGrounded && player.rb.velocity.y != 0)
            {
                player.ChangeState(E_PlayeState.Fall);
            }
            else if(RemoteInput.inputs[player.model.playerNumber].divingInput)
            {
                player.ChangeState(E_PlayeState.Diving);
            }
            else if (RemoteInput.inputs[player.model.playerNumber].grabInput)
            {
                player.ChangeState(E_PlayeState.Grabbing);
            }
            // 임시 테스트용 잡힌상태
            else if (Input.GetKeyDown(KeyCode.G))
            {
                player.ChangeState(E_PlayeState.Grabbed);
            }
        }

        public override void FixedUpdate()
        {
            player.MoveOnConveyor();
        }

        public override void Exit()
        {
            player.view.SetBoolParameter(E_AniParameters.Idling, false);
        }

    }
}
