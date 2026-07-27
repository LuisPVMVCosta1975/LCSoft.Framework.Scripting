namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand.ScriptExpressionCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;

    public class CallFunctionScriptExpressionCommand : ScriptExpressionCommandBase
    {
        public const String ComponentName = "CallFunction";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CallFunctionScriptExpressionCommand Parse(String FunctionName, BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<IScriptExpression> Parameters = ParserUtils.ParseListOfExpressions(BFR, ComponentSignature + " / " + nameof(Parameters) + " [List Of Expressions]", ')');

            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature);

            return new CallFunctionScriptExpressionCommand(FunctionName, Parameters);
        }

        internal String FunctionName;
        internal List<IScriptExpression> Parameters;

        public CallFunctionScriptExpressionCommand(String FunctionName) : this(FunctionName, null)
        {
        }
        public CallFunctionScriptExpressionCommand(String FunctionName, List<IScriptExpression> Parameters)
        {
            this.FunctionName = FunctionName;
            this.Parameters = Parameters;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            return RunTimeUtils.CallFunction(FunctionName, Parameters, Context, ScriptResources, Context, ScriptResources);
        }
    }
}