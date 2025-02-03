namespace _1.Private.ParkJM.Scripts.States
{
    public class StandUpState : PlayerState
    {
        public StandUpState(PlayerController player) : base(player)
        {
            //animationIndex = (int)E_PlayeState.StandUp;
        }

        public override void Enter()
        {
            player.view.BroadCastTriggerParameter(E_AniParameters.StandingUp);
        }

        public override void Update()
        {
            if (!player.view.IsAnimationFinished())
                return;
            player.ChangeState(E_PlayeState.Idle);
        }

        public override void Exit()
        {
        }
    }
}
