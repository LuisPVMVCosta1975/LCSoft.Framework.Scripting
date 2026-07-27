namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class BreakIfScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "BreakIf";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static BreakIfScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Predicate = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Predicate) + " [Expression]");
            //ParserUtils.AssertExpression(Predicate, BFR.Peek(), ComponentSignature + " / " + nameof(Predicate) + " (Condition)");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ',', ComponentSignature + " / [Parameter Separator]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            Int64 Count = ParserUtils.GetInt64(BFR, ComponentSignature + " / " + nameof(Count) + " [Int64 Literal]");
            ParserUtils.AssertPositive(Count, ComponentSignature + " / " + nameof(Count) + " [Int64 Literal]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new BreakIfScriptCommand(Predicate, Count);
        }

        internal IScriptExpression Predicate;
        internal Int64 Count;

        public BreakIfScriptCommand(IScriptExpression Predicate, Int64 Count)
        {
            this.Predicate = Predicate;
            this.Count = Count;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase Result = Predicate.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Predicate));
            if (Result.GetBoolean())
            {
                return ExecutionResult.Break(Count);
            }

            return ExecutionResult.None();
        }
    }
}