namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;

    public class SetVariableScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "SetVariable";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static SetVariableScriptCommand Parse(String VariableName, BookmarkableFileReader BFR)
        {
            //ParserUtils.AssertChar(BFR.Read(), '=', ComponentSignature);

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Value = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Value) + " [Expression]");

            return new SetVariableScriptCommand(VariableName, Value);
        }
        internal static SetVariableScriptCommand Parse(IScriptExpression ScriptExpression, BookmarkableFileReader BFR)
        {
            ParserUtils.AssertChar(BFR.Read(), '=', ComponentSignature);

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Value = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Value) + " [Expression]");

            return new SetVariableScriptCommand(ScriptExpression, Value);
        }

        internal readonly String VariableName;
        internal readonly IScriptExpression ScriptExpression;
        internal readonly IScriptExpression Value;

        public SetVariableScriptCommand(String VariableName, IScriptExpression Value)
        {
            this.VariableName = VariableName;
            this.Value = Value;
        }
        public SetVariableScriptCommand(IScriptExpression ScriptExpression, IScriptExpression Value)
        {
            this.ScriptExpression = ScriptExpression;
            this.Value = Value;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            //TODO: Tech Debt: see GetVariableScriptExpression

            if (ScriptResources.Enums.ContainsKey(VariableName))
            {
                throw new OperationOutOfContextException("Enum.Set");
            }

            Context.SetVariable(VariableName, Value.EvaluateElement(Context, ScriptResources));

            return ExecutionResult.None();
        }
    }
}