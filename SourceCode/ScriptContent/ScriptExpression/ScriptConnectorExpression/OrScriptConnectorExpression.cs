namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression.ScriptConnectorExpression
{
    using System;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class OrScriptConnectorExpression : ScriptConnectorExpressionBase
    {
        public const String ComponentName = "Or";
        public const String ComponentName1 = "|";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal IScriptExpression ScriptExpressionLeft;
        internal IScriptExpression ScriptExpressionRight;

        public override String GetImplementationType() => ComponentSignature;

        public OrScriptConnectorExpression(IScriptExpression ScriptExpressionLeft, IScriptExpression ScriptExpressionRight)
        {
            this.ScriptExpressionLeft = ScriptExpressionLeft;
            this.ScriptExpressionRight = ScriptExpressionRight;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase Value1 = ScriptExpressionLeft.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionLeft));
            ValueContainerBase Value2 = ScriptExpressionRight.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionRight));
            return Value1.Or(Value2);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            ValueContainerBase Value1 = ScriptExpressionLeft.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionLeft));
            ValueContainerBase Value2 = ScriptExpressionRight.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionRight));
            return Value1.Or(Value2);
        }
    }
}