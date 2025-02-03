namespace _1.Private.ParkJM.Scripts.States
{
    public class FallState : PlayerState
    {
        public FallState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            player.view.BroadCastTriggerParameter(E_AniParameters.Falling);
        }

        public override void Update()
        {
            if (RemoteInput.inputs[player.model.playerNumber].divingInput)
            {
                player.ChangeState(E_PlayeState.Diving);
            }
            else if (player.isGrounded)
            {
                player.ChangeState(E_PlayeState.Idle);
            }
        }

        public override void Exit()
        {
        }
    }
}
