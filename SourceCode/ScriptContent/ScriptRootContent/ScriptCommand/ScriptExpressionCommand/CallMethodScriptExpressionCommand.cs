namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand.ScriptExpressionCommand
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptExpression;
    using LCSoft.Framework.Scripting.ValueContainer;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class CallMethodScriptExpressionCommand : ScriptExpressionCommandBase
    {
        public const String ComponentName = "CallMethod";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CallMethodScriptExpressionCommand Parse(IScriptExpression ScriptExpression, String MethodName, BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<IScriptExpression> Parameters = ParserUtils.ParseListOfExpressions(BFR, ComponentSignature + " / " + nameof(Parameters) + " [List Of Expressions]", ')');

            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature);

            return new CallMethodScriptExpressionCommand(ScriptExpression, MethodName, Parameters);
        }


        internal IScriptExpression ScriptExpression;
        internal String MethodName;
        internal List<IScriptExpression> Parameters;

        public CallMethodScriptExpressionCommand(IScriptExpression ScriptExpression, String MethodName)
        {
            this.ScriptExpression = ScriptExpression;
            this.MethodName = MethodName;
        }
        public CallMethodScriptExpressionCommand(IScriptExpression ScriptExpression, String MethodName, List<IScriptExpression> Parameters)
        {
            this.ScriptExpression = ScriptExpression;
            this.MethodName = MethodName;
            this.Parameters = Parameters;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            switch (MethodName)
            {
                case "IsMissing":
                    if (ScriptExpression is GetVariableScriptExpression && (Parameters == null || Parameters.Count == 0))
                    {
                        String VariableName = ((GetVariableScriptExpression)ScriptExpression).VariableName;
                        return ExecutionResult.None(!Context.CheckVariable(VariableName) ? ValueContainerBase.True : ValueContainerBase.False);
                    }
                    break;
                case "IsNotMissing":
                    if (ScriptExpression is GetVariableScriptExpression && (Parameters == null || Parameters.Count == 0))
                    {

                        String VariableName = ((GetVariableScriptExpression)ScriptExpression).VariableName;
                        return ExecutionResult.None(Context.CheckVariable(VariableName) ? ValueContainerBase.True : ValueContainerBase.False);
                    }
                    break;
                case "IfMissing":
                    if (ScriptExpression is GetVariableScriptExpression && Parameters != null && Parameters.Count == 1)
                    {
                        String VariableName = ((GetVariableScriptExpression)ScriptExpression).VariableName;
                        (ValueContainerBase Value, Boolean IsMissing) = Context.GetVariableOrNull(VariableName);
                        if (IsMissing)
                        {
                            return ExecutionResult.None(Parameters[0].EvaluateElement(Context, ScriptResources));
                        }
                        return ExecutionResult.None(Value);
                    }
                    break;
            }

            ValueContainerBase ValueContainer = ScriptExpression.EvaluateElement(Context, ScriptResources);

            //like extension methods
            switch (MethodName)
            {
                case "IsEmpty":
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return ExecutionResult.None(ValueContainer == null ? ValueContainerBase.True : ValueContainerBase.False);
                    }
                    break;
                case "IsNotEmpty":
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return ExecutionResult.None(ValueContainer != null ? ValueContainerBase.True : ValueContainerBase.False);
                    }
                    break;
                case "IfEmpty":
                    if (Parameters != null && Parameters.Count == 1)
                    {
                        if (ValueContainer != null)
                        {
                            return ExecutionResult.None(ValueContainer);
                        }
                        return ExecutionResult.None(Parameters[0].EvaluateElement(Context, ScriptResources));
                    }
                    break;
            }

            if (ValueContainer == null)
            {
                throw new EmptyValueException(ComponentSignature + " / " + nameof(ScriptExpression));
            }

            return ExecutionResult.None(ValueContainer.CallMethod(Context, ScriptResources, MethodName, Parameters));
        }

        private void Optimize(IScriptExpression ScriptExpression, String MethodName, List<IScriptExpression> Parameters)
        {
        }
    }
}