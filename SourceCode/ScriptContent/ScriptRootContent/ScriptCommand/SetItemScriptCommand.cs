namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class SetItemScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "SetItem";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static SetItemScriptCommand Parse(IScriptExpression ScriptExpression, IScriptExpression Key, BookmarkableFileReader BFR)
        {
            //ParserUtils.AssertChar(BFR.Read(), '=', ComponentSignature);

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Value = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Value) + " [Expression]");

            return new SetItemScriptCommand(ScriptExpression, Key, Value);
        }

        internal IScriptExpression ScriptExpression;
        internal IScriptExpression Key;
        internal IScriptExpression Value;

        public SetItemScriptCommand(IScriptExpression ScriptExpression, IScriptExpression Key, IScriptExpression Value)
        {
            this.ScriptExpression = ScriptExpression;
            this.Key = Key;
            this.Value = Value;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase TargetValueContainer = ScriptExpression.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpression));
            ValueContainerBase KeyValueContainer = Key.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Key));

            TargetValueContainer.SetItem(KeyValueContainer, Value.EvaluateElement(Context, ScriptResources));

            return ExecutionResult.None();
        }
    }
}