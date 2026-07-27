namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class IdentifierScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "Identifier";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal String Identifier;

        public override String GetImplementationType() => ComponentSignature;

        public IdentifierScriptExpression(String Identifier)
        {
            //sintaxe: §Identifier
            //see: GetVariableScriptExpression
            this.Identifier = Identifier;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return Context.GetVariable(Identifier);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            return Context.GetVariable(Identifier);
        }
    }
}