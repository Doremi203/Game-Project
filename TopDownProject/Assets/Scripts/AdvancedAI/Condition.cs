namespace AdvancedAI
{

    public abstract class Condition
    {

        public Condition(string targetParameter) => this.targetParameter = targetParameter;

        protected string targetParameter;

        public abstract bool IsFunctional(StateMachine stateMachine);

    }

}