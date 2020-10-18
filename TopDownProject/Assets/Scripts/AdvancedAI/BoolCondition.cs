namespace AdvancedAI
{

    public class BoolCondition : Condition
    {
        public BoolCondition(string targetParameter, bool passRequirement) : base(targetParameter)
        {
            this.passRequirement = passRequirement;
        }

        private bool passRequirement;

        public override bool IsFunctional(StateMachine stateMachine)
        {
            return stateMachine.ReadBool(targetParameter) == passRequirement;
        }

    }

}