namespace AdvancedAI
{

    public class IntCondition : Condition
    {

        public IntCondition(string targetParameter, string compareTo, IntConditionType conditionType) : base(targetParameter)
        {
            this.conditionType = conditionType;
            this.compareTo = compareTo;
        }

        private IntConditionType conditionType;
        private string compareTo;

        public override bool IsFunctional(StateMachine stateMachine)
        {
            int a = stateMachine.ReadInt(targetParameter);
            int b = stateMachine.ReadInt(compareTo);
            switch (conditionType)
            {
                case IntConditionType.Greater:
                    return a > b;
                case IntConditionType.Less:
                    return a < b;
                case IntConditionType.Equals:
                    return a == b;
                case IntConditionType.NotEqual:
                    return a != b;
                default:
                    return false;
            }
        }

    }

    public enum IntConditionType
    {
        Greater,
        Less,
        Equals,
        NotEqual
    }

}