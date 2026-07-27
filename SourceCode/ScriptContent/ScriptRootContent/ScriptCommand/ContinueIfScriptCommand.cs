namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class ContinueIfScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "ContinueIf";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static ContinueIfScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Predicate = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Predicate) + " [Expression]");
            //ParserUtils.AssertExpression(Predicate, BFR.Peek(), ComponentSignature + " / " + nameof(Predicate) + " (Condition)");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new ContinueIfScriptCommand(Predicate);
        }

        internal IScriptExpression Predicate;

        public ContinueIfScriptCommand(IScriptExpression Predicate)
        {
            this.Predicate = Predicate;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase Result = Predicate.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Predicate));
            if (Result.GetBoolean())
            {
                return ExecutionResult.Continue;
            }

            return ExecutionResult.None();
        }
    }
}