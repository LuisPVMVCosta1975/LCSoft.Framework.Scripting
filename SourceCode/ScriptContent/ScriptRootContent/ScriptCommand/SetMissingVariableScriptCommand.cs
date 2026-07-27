namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;

    public class SetMissingVariableScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "SetMissingVariable";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static SetMissingVariableScriptCommand Parse(String VariableName, BookmarkableFileReader BFR)
        {
            //ParserUtils.AssertChar(BFR.Read(), '=', ComponentSignature);

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Value = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Value) + " [Expression]");

            return new SetMissingVariableScriptCommand(VariableName, Value);
        }
        //internal static SetMissingVariableScriptCommand Parse(IScriptExpression ScriptExpression, BookmarkableFileReader BFR)
        //{
        //    ParserUtils.AssertChar(BFR.Read(), '=', ComponentSignature);

        //    ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
        //    IScriptExpression Value = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Value) + " [Expression]");

        //    return new SetMissingVariableScriptCommand(ScriptExpression, Value);
        //}

        internal readonly String VariableName;
        //internal readonly IScriptExpression ScriptExpression;
        internal readonly IScriptExpression Value;

        public SetMissingVariableScriptCommand(String VariableName, IScriptExpression Value)
        {
            this.VariableName = VariableName;
            this.Value = Value;
        }
        //public SetMissingVariableScriptCommand(IScriptExpression ScriptExpression, IScriptExpression Value)
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

            if (Context.CheckVariable(VariableName))
            {
                return ExecutionResult.None();
            }

            Context.SetVariable(VariableName, Value.EvaluateElement(Context, ScriptResources));

            return ExecutionResult.None();
        }
    }
}