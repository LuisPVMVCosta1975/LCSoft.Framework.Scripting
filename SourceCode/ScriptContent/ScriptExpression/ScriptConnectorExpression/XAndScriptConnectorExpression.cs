namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression.ScriptConnectorExpression
{
    using System;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class XAndScriptConnectorExpression : ScriptConnectorExpressionBase
    {
        public const String ComponentName = "XAnd";
        public const String ComponentName1 = "!";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal IScriptExpression ScriptExpressionLeft;
        internal IScriptExpression ScriptExpressionRight;

        public override String GetImplementationType() => ComponentSignature;

        public XAndScriptConnectorExpression(IScriptExpression ScriptExpressionLeft, IScriptExpression ScriptExpressionRight)
        {
            this.ScriptExpressionLeft = ScriptExpressionLeft;
            this.ScriptExpressionRight = ScriptExpressionRight;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase Value1 = ScriptExpressionLeft.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionLeft));
            ValueContainerBase Value2 = ScriptExpressionRight.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionRight));
            return Value1.XAnd(Value2);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            ValueContainerBase Value1 = ScriptExpressionLeft.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionLeft));
            ValueContainerBase Value2 = ScriptExpressionRight.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpressionRight));
            return Value1.XAnd(Value2);
        }
    }
}