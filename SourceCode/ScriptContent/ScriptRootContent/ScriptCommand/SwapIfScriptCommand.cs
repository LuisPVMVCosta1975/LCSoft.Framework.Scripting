namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class SwapIfScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "SwapIf";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static SwapIfScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Predicate = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Predicate) + " [Expression]");
            //ParserUtils.AssertExpression(Predicate, BFR.Peek(), ComponentSignature + " / " + nameof(Predicate) + " (Condition)");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ',', ComponentSignature + " / [Parameter Separator]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String VariableName1 = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(VariableName1, BFR.Peek(), ComponentName + " / " + nameof(VariableName1) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ',', ComponentSignature + " / [Parameter Separator]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String VariableName2 = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(VariableName2, BFR.Peek(), ComponentName + " / " + nameof(VariableName2) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new SwapIfScriptCommand(Predicate, VariableName1, VariableName2);
        }

        internal IScriptExpression Predicate;
        internal String VariableName1;
        internal String VariableName2;

        public SwapIfScriptCommand(IScriptExpression Predicate, String VariableName1, String VariableName2)
        {
            this.Predicate = Predicate;
            this.VariableName1 = VariableName1;
            this.VariableName2 = VariableName2;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase Result = Predicate.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Predicate));
            if (Result.GetBoolean())
            {
                Context Variable1Contexxt = Context.GetVariableContext(VariableName1);
                Context Variable2Contexxt = Context.GetVariableContext(VariableName2);

                ValueContainerBase Value1 = Variable1Contexxt.Variables[VariableName1];
                ValueContainerBase Value2 = Variable2Contexxt.Variables[VariableName2];

                Variable1Contexxt.Variables[VariableName1] = Value2;
                Variable2Contexxt.Variables[VariableName2] = Value1;
            }

            return ExecutionResult.None();
        }
    }
}