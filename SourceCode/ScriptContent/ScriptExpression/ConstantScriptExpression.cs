namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.ValueContainer;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class ConstantScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "Constant";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal LiteralValueContainerBase Value;

        public override String GetImplementationType() => ComponentSignature;

        public ConstantScriptExpression(LiteralValueContainerBase Value)
        {
            this.Value = Value;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return Value;
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            if (Value != null)
            {
                return Value;
            }

            throw new EmptyValueException(Name);
        }
    }
}