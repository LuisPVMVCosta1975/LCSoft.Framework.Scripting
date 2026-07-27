namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class DebuggerIfScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "DebuggerIf";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static DebuggerIfScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Predicate = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Predicate) + " [Expression]");
            //ParserUtils.AssertExpression(Predicate, BFR.Peek(), ComponentSignature + " / " + nameof(Predicate) + " (Condition)");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ',', ComponentSignature + " / [Parameter Separator]");

            String ID;
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '"', ComponentSignature + " / " + nameof(ID) + " [String Literal]");
            ID = ParserUtils.GetString(BFR, ComponentSignature + " / " + nameof(ID) + " [String Literal]");

            List<IScriptExpression> Values;
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            if (BFR.Peek() == ',')
            {
                BFR.Advance();

                ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                Values = ParserUtils.ParseListOfExpressions(BFR, ComponentSignature + " / " + nameof(Values) + " [List Of Expressions]", ')');
                ParserUtils.AssertListOfExpressions(Values, ComponentSignature + " / " + nameof(Values) + " [List Of Expressions]");
            }
            else
            {
                Values = null;
            }

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new DebuggerIfScriptCommand(Predicate, ID, Values);
        }

        internal IScriptExpression Predicate;
        internal readonly String ID;
        internal readonly List<IScriptExpression> Values;

        public DebuggerIfScriptCommand(IScriptExpression Predicate, String ID, List<IScriptExpression> Values)
        {
            this.Predicate = Predicate;
            this.ID = ID;
            this.Values = Values;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            if (ScriptingConfiguration.BreakpointHandler != null)
            {
                ValueContainerBase Result = Predicate.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Predicate));
                if (Result.GetBoolean())
                {
                    List<ValueContainerBase> EvaluatedValues = RunTimeUtils.EvaluateParameters(Values, Context, ScriptResources);
                    ScriptingConfiguration.BreakpointHandler(Context, ID, EvaluatedValues);
                }
            }

            return ExecutionResult.None();
        }
    }
}