namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class DebuggerScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "Debugger";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static DebuggerScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

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

            return new DebuggerScriptCommand(ID, Values);
        }

        internal readonly String ID;
        internal readonly List<IScriptExpression> Values;

        public DebuggerScriptCommand(String ID, List<IScriptExpression> Values)
        {
            this.ID = ID;
            this.Values = Values;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            if (ScriptingConfiguration.BreakpointHandler != null)
            {
                List<ValueContainerBase> EvaluatedValues = RunTimeUtils.EvaluateParameters(Values, Context, ScriptResources);
                ScriptingConfiguration.BreakpointHandler(Context, ID, EvaluatedValues);
            }

            return ExecutionResult.None();
        }
    }
}