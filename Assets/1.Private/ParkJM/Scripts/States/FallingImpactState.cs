namespace _1.Private.ParkJM.Scripts.States
{
    public class FallingImpact : PlayerState
    {
        public FallingImpact(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            player.view.BroadCastTriggerParameter(E_AniParameters.FallingImpact);
            player.model.InvokePlayerFloorImpacted();
        }

        public override void Update()
        {
            if (!player.view.IsAnimationFinished())
                return;
        
            player.ChangeState(E_PlayeState.StandUp);
        }

        public override void Exit()
        {
        }
    }
}
