namespace AdvancedAI
{

    public class FloatCondition : Condition
    {

        public FloatCondition(string targetParameter, string compareTo, FloatConditionType conditionType) : base(targetParameter)
        {
            this.conditionType = conditionType;
            this.compareTo = compareTo;
        }

        private FloatConditionType conditionType;
        private string compareTo;

        public override bool IsFunctional(StateMachine stateMachine)
        {
            float a = stateMachine.ReadFloat(targetParameter);
            float b = stateMachine.ReadFloat(compareTo);
            switch (conditionType)
            {
                case FloatConditionType.Greater:
                    return a > b;
                case FloatConditionType.Less:
                    return a < b;
                default:
                    return false;
            }
        }

    }

    public enum FloatConditionType
    {
        Greater,
        Less,
    }

}