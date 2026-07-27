namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class SetPropertyScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "SetProperty";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static SetPropertyScriptCommand Parse(IScriptExpression ScriptExpression, String AttributeName, BookmarkableFileReader BFR)
        {
            //ParserUtils.AssertChar(BFR.Read(), '=', ComponentSignature + " / [Assignment]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Value = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Value) + " [Expression]");

            return new SetPropertyScriptCommand(ScriptExpression, AttributeName, Value);
        }

        internal IScriptExpression ScriptExpression;
        internal String AttributeName;
        internal IScriptExpression Value;

        public SetPropertyScriptCommand(IScriptExpression ScriptExpression, String Name, IScriptExpression Value)
        {
            this.ScriptExpression = ScriptExpression;
            this.AttributeName = Name;
            this.Value = Value;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase ValueContainer = ScriptExpression.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpression));

            ValueContainer.SetProperty(AttributeName, Value.EvaluateElement(Context, ScriptResources));

            return ExecutionResult.None();
        }
    }
}