namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class FailIfScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "FailIf";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static FailIfScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Predicate = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Predicate) + " [Expression]");
            //ParserUtils.AssertExpression(Predicate, BFR.Peek(), ComponentSignature + " / " + nameof(Predicate) + " (Condition)");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new FailIfScriptCommand(Predicate);
        }

        internal IScriptExpression Predicate;

        public FailIfScriptCommand(IScriptExpression Predicate)
        {
            this.Predicate = Predicate;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase Result = Predicate.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Predicate));
            if (Result.GetBoolean())
            {
                return ExecutionResult.Fail;
            }

            return ExecutionResult.None();
        }
    }
}