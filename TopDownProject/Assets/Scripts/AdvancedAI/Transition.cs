namespace AdvancedAI
{

    public struct Transition
    {

        public Transition(IState to, Condition[] conditions)
        {
            To = to;
            this.conditions = conditions;
        }

        public IState To { get; }

        private Condition[] conditions;

        public bool ShouldTransist(StateMachine stateMachine)
        {
            foreach (var item in conditions)
                if (!item.IsFunctional(stateMachine)) return false;
            return true;
        }

    }

}