namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression.ScriptConnectorExpression
{
    using System;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class EqualScriptConnectorExpression : ScriptConnectorExpressionBase
    {
        public const String ComponentName = "Equal";
        public const String ComponentName1 = "==";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal IScriptExpression ScriptExpressionLeft;
        internal IScriptExpression ScriptExpressionRight;

        public override String GetImplementationType() => ComponentSignature;

        public EqualScriptConnectorExpression(IScriptExpression ScriptExpressionLeft, IScriptExpression ScriptExpressionRight)
        {
            this.ScriptExpressionLeft = ScriptExpressionLeft;
            this.ScriptExpressionRight = ScriptExpressionRight;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase ValueContainerLeft = ScriptExpressionLeft.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionLeft));
            ValueContainerBase ValueContainerRight = ScriptExpressionRight.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionRight));
            return (ValueContainerLeft.IsEqualTo(ValueContainerRight) ? ValueContainerBase.True : ValueContainerBase.False);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            ValueContainerBase ValueContainerLeft = ScriptExpressionLeft.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionLeft));
            ValueContainerBase ValueContainerRight = ScriptExpressionRight.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionRight));
            return (ValueContainerLeft.IsEqualTo(ValueContainerRight) ? ValueContainerBase.True : ValueContainerBase.False);
        }
    }
}