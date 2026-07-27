namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class ReturnIfScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "ReturnIf";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static ReturnIfScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Predicate = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Predicate) + " [Expression]");
            //ParserUtils.AssertExpression(Predicate, BFR.Peek(), ComponentSignature + " / " + nameof(Predicate) + " (Condition)");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ',', ComponentSignature + " / [Parameter Separator]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Value = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Value) + " [Expression]");
            //ParserUtils.AssertExpression(Value, BFR.Peek(), ComponentSignature + " / " + nameof(Value) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new ReturnIfScriptCommand(Predicate, Value);
        }

        internal IScriptExpression Predicate;
        internal IScriptExpression Value;

        public ReturnIfScriptCommand(IScriptExpression Predicate, IScriptExpression Value)
        {
            this.Predicate = Predicate;
            this.Value = Value;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase Result = Predicate.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Predicate));
            if (Result.GetBoolean())
            {
                return ExecutionResult.Return(Value == null ? ValueContainerBase.Empty : Value.EvaluateElement(Context, ScriptResources));
            }

            return ExecutionResult.None();
        }
    }
}