using UnityEngine;

namespace _1.Private.ParkJM.Scripts.States
{
    public class RunState : PlayerState
    {
        private Vector3 targetVelocity;
        private float _rotationSpeed;
        public RunState(PlayerController player) : base(player)
        {
            _rotationSpeed = 15.0f;
        }

        public override void Enter()
        {
            player.view.SetBoolParameter(E_AniParameters.Running, true);
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

            else if (player.moveDir.sqrMagnitude < 0.1f)
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
            if (player.moveDir == Vector3.zero)
                return;
            Quaternion targetRotation = Quaternion.LookRotation(player.moveDir);
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

            player.transform.rotation = Quaternion.Slerp(
                player.transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * _rotationSpeed
            );
        }
    }
}
