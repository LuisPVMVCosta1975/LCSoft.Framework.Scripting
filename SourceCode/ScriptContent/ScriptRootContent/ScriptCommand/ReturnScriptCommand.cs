namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class ReturnScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "Return";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static ReturnScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Value = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Value) + " [Expression]");
            //ParserUtils.AssertExpression(Value, BFR.Peek(), ComponentSignature + " / " + nameof(Value) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new ReturnScriptCommand(Value);
        }

        internal IScriptExpression Value;

        public ReturnScriptCommand(IScriptExpression Value)
        {
            this.Value = Value;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            return ExecutionResult.Return(Value == null ? ValueContainerBase.Empty : Value.EvaluateElement(Context, ScriptResources));
        }
    }
}