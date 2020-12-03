namespace AdvancedAI
{

    public class TriggerCondition : Condition
    {

        public TriggerCondition(string targetParameter) : base(targetParameter) { }

        public override bool IsFunctional(StateMachine stateMachine)
        {
            return stateMachine.IsTriggerActive(targetParameter) == true;
        }

    }

}