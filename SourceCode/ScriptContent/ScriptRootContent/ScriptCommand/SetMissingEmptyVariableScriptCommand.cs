namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;

    public class SetMissingEmptyVariableScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "SetMissingEmptyVariable";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static SetMissingEmptyVariableScriptCommand Parse(String VariableName, BookmarkableFileReader BFR)
        {
            //ParserUtils.AssertChar(BFR.Read(), '=', ComponentSignature);

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Value = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Value) + " [Expression]");

            return new SetMissingEmptyVariableScriptCommand(VariableName, Value);
        }
        //internal static SetMissingEmptyVariableScriptCommand Parse(IScriptExpression ScriptExpression, BookmarkableFileReader BFR)
        //{
        //    ParserUtils.AssertChar(BFR.Read(), '=', ComponentSignature);

        //    ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
        //    IScriptExpression Value = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Value) + " [Expression]");

        //    return new SetMissingEmptyVariableScriptCommand(ScriptExpression, Value);
        //}

        internal readonly String VariableName;
        //internal readonly IScriptExpression ScriptExpression;
        internal readonly IScriptExpression Value;

        public SetMissingEmptyVariableScriptCommand(String VariableName, IScriptExpression Value)
        {
            this.VariableName = VariableName;
            this.Value = Value;
        }
        //public SetMissingEmptyVariableScriptCommand(IScriptExpression ScriptExpression, IScriptExpression Value)
        //{
        //    this.ScriptExpression = ScriptExpression;
        //    this.Value = Value;
        //}

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            //TODO: Tech Debt: see GetVariableScriptExpression

            if (ScriptResources.Enums.ContainsKey(VariableName))
            {
                throw new OperationOutOfContextException("Enum.Set");
            }

            Context VariableContext = Context.GetVariableContextOrNull(VariableName);

            if (VariableContext == null)
            {
                Context.SetVariable(VariableName, Value.EvaluateElement(Context, ScriptResources));

                return ExecutionResult.None();
            }

            if (VariableContext.Variables[VariableName] != null)
            {
                return ExecutionResult.None();
            }

            VariableContext.Variables[VariableName] = Value.EvaluateElement(Context, ScriptResources);

            return ExecutionResult.None();
        }
    }
}