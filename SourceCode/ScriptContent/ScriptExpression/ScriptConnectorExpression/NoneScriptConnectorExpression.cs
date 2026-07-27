namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression.ScriptConnectorExpression
{
    using System;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class NoneScriptConnectorExpression : ScriptConnectorExpressionBase
    {
        public const String ComponentName = "None";
        public const String ComponentName1 = "!!";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal IScriptExpression ScriptExpressionLeft;
        internal IScriptExpression ScriptExpressionRight;

        public override String GetImplementationType() => ComponentSignature;

        public NoneScriptConnectorExpression(IScriptExpression ScriptExpressionLeft, IScriptExpression ScriptExpressionRight)
        {
            this.ScriptExpressionLeft = ScriptExpressionLeft;
            this.ScriptExpressionRight = ScriptExpressionRight;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase ValueContainer = ScriptExpressionLeft.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionLeft));
            if (ValueContainer.GetBoolean())
            {
                return ValueContainerBase.False;
            }

            ValueContainer = ScriptExpressionRight.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionRight));
            return (ValueContainer.GetBoolean() == false ? ValueContainerBase.True : ValueContainerBase.False);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            ValueContainerBase ValueContainer = ScriptExpressionLeft.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionLeft));
            if (ValueContainer.GetBoolean())
            {
                return ValueContainerBase.False;
            }

            ValueContainer = ScriptExpressionRight.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionRight));
            return (ValueContainer.GetBoolean() == false ? ValueContainerBase.True : ValueContainerBase.False);
        }
    }
}