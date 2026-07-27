namespace LCSoft.Framework.Scripting.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand;
    using LCSoft.Framework.Scripting.ValueContainer;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    internal static class RunTimeUtils
    {
        internal static ValueContainerBase Box(Object Value, Type Type)
        {
            if (Value == null)
            {
                if (Type == null)
                {
                    throw new ArgumentNullException(nameof(Type));
                }

                return new NullLiteralValueContainer(Type);
            }

            if (Value is Boolean BooleanValue)
            {
                return (BooleanValue ? ValueContainerBase.True : ValueContainerBase.False);
            }
            if (Value is Char CharValue)
            {
                return new CharLiteralValueContainer(CharValue);
            }
            if (Value is DateTime DateTimeValue)
            {
                return new DateTimeLiteralValueContainer(DateTimeValue);
            }
            if (Value is Double DoubleValue)
            {
                return new DoubleLiteralValueContainer(DoubleValue);
            }
            if (Value is Int32 Int32Value)
            {
                return new Int32LiteralValueContainer(Int32Value);
            }
            if (Value is Int64 Int64Value)
            {
                return new Int64LiteralValueContainer(Int64Value);
            }
            if (Value is Single SingleValue)
            {
                return new SingleLiteralValueContainer(SingleValue);
            }
            if (Value is String StringValue)
            {
                return new StringLiteralValueContainer(StringValue);
            }
            if (Value is TimeSpan TimeSpanValue)
            {
                return new TimeSpanLiteralValueContainer(TimeSpanValue);
            }

            if (Value is ValueContainerBase ValueContainerBaseValue)
            {
                return ValueContainerBaseValue;
            }
            //if (Value is Type TypeValue)
            //{
            //    if (Type == null)
            //    {
            //        return new ClassReferenceValueContainer(TypeValue);
            //    }
            //}

            return new ObjectReferenceValueContainer(Value);
        }

        public static List<ValueContainerBase> EvaluateParameters(List<IScriptExpression> Parameters, Context Context, ScriptResources ScriptResources)
        {
            if (Parameters == null)
            {
                return null;
            }

            List<ValueContainerBase> EvaluatedParameters = new List<ValueContainerBase>();
            foreach (IScriptExpression SE in Parameters)
            {
                EvaluatedParameters.Add(SE.EvaluateElement(Context, ScriptResources));
            }

            return EvaluatedParameters;
        }
        public static List<ValueContainerBase> EvaluateParameters(List<IScriptExpression> Parameters, Context Context, ScriptResources ScriptResources, Int32 Count)
        {
            if (Parameters == null)
            {
                return null;
            }

            List<ValueContainerBase> EvaluatedParameters = new List<ValueContainerBase>();
            foreach (IScriptExpression SE in Parameters.Take(Count))
            {
                EvaluatedParameters.Add(SE.EvaluateElement(Context, ScriptResources));
            }

            return EvaluatedParameters;
        }

        public static ExecutionResult RunBlock(List<ScriptCommandBase> Commands, Context ParentContext, ScriptResources ScriptResources)
        {
            if (Commands == null || Commands.Count == 0)
            {
                return ExecutionResult.None();
            }

            return RunBlockPrivate(true, Commands, ParentContext, ScriptResources);
        }
        private static ExecutionResult RunBlockPrivate(Boolean IsChildContext, List<ScriptCommandBase> Commands, Context ParentContext, ScriptResources ScriptResources)
        {
            Context LocalContext;
            if (IsChildContext)
            {
                LocalContext = ParentContext.EnterChildContext();
            }
            else
            {
                LocalContext = ParentContext;
            }

            try
            {
                foreach (ScriptCommandBase SC in Commands)
                {
                    ExecutionResult ElementResult = SC.RunElement(LocalContext, ScriptResources);

                    if (ElementResult.CancelationFlag != ExecutionResult.CancelationMode.None)
                    {
                        return ElementResult;
                    }
                }

                return ExecutionResult.None();
            }
            finally
            {
                if (IsChildContext)
                {
                    LocalContext.LeaveContext();
                }
            }
        }

        public static ExecutionResult CallFunction(FunctionScriptRootContent Function, List<IScriptExpression> Parameters, Context ParentContext, ScriptResources ScriptResources, Context ResolverContext, ScriptResources ResolverScriptResources)
        {
            if (Function.ScriptCommands == null || Function.ScriptCommands.Count == 0)
            {
                return ExecutionResult.None();
            }

            List<ValueContainerBase> EvaluatedParameters = EvaluateParameters(Parameters, ResolverContext, ResolverScriptResources, Parameters != null ? Parameters.Count : 0);

            return CallFunctionPrivate(Function, null, EvaluatedParameters, ParentContext, ScriptResources);
        }
        public static ExecutionResult CallFunction(String FunctionName, List<IScriptExpression> Parameters, Context Context, ScriptResources ScriptResources, Context ResolverContext, ScriptResources ResolverScriptResources)
        {
            FunctionScriptRootContent Function;
            if (ScriptResources.Functions.TryGetValue(FunctionName, out Function) == false)
            {
                throw new FunctionNotFoundException(FunctionName);
            }

            if (Function.ScriptCommands == null || Function.ScriptCommands.Count == 0)
            {
                return ExecutionResult.None();
            }

            List<ValueContainerBase> EvaluatedParameters = EvaluateParameters(Parameters, ResolverContext, ResolverScriptResources, Function.Parameters != null ? Function.Parameters.Count : 0);

            return CallFunctionPrivate(Function, null, EvaluatedParameters, Context, ScriptResources);
        }
        public static ExecutionResult CallFunction(String FunctionName, CancelationTokenValueContainer CancelationToken, List<IScriptExpression> Parameters, Context Context, ScriptResources ScriptResources, Context ResolverContext, ScriptResources ResolverScriptResources)
        {
            FunctionScriptRootContent Function;
            if (ScriptResources.Functions.TryGetValue(FunctionName, out Function) == false)
            {
                throw new FunctionNotFoundException(FunctionName);
            }

            if (Function.ScriptCommands == null || Function.ScriptCommands.Count == 0)
            {
                return ExecutionResult.None();
            }

            List<ValueContainerBase> EvaluatedParameters = EvaluateParameters(Parameters, ResolverContext, ResolverScriptResources, Function.Parameters != null ? Function.Parameters.Count : 0);

            return CallFunctionPrivate(Function, CancelationToken, EvaluatedParameters, Context, ScriptResources);
        }
        public static ExecutionResult CallFunction(String FunctionName, List<ValueContainerBase> EvaluatedParameters, Context Context, ScriptResources ScriptResources)
        {
            FunctionScriptRootContent Function;
            if (ScriptResources.Functions.TryGetValue(FunctionName, out Function) == false)
            {
                throw new FunctionNotFoundException(FunctionName);
            }

            if (Function.ScriptCommands == null || Function.ScriptCommands.Count == 0)
            {
                return ExecutionResult.None();
            }

            return CallFunctionPrivate(Function, null, EvaluatedParameters, Context, ScriptResources);
        }
        private static ExecutionResult CallFunctionPrivate(FunctionScriptRootContent Function, CancelationTokenValueContainer CancelationToken, List<ValueContainerBase> EvaluatedParameters, Context ParentContext, ScriptResources ScriptResources)
        {
            Context LocalContext = ParentContext.EnterSpawnContext();

            try
            {
                if (CancelationToken != null)
                {
                    LocalContext.SetVariable("CancelationToken", CancelationToken);
                }
                ApplyParameters(LocalContext, Function.Parameters, EvaluatedParameters);

                ExecutionResult ElementResult = RunBlockPrivate(false, Function.ScriptCommands, LocalContext, ScriptResources);

                if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Break)
                {
                    throw new CommandOutOfContextException("Break");
                }
                if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Continue)
                {
                    throw new CommandOutOfContextException("Continue");
                }
                if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Fail)
                {
                    throw new CommandOutOfContextException("Fail");
                }
                if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Return)
                {
                    return ElementResult.EndCall();
                }

                return ExecutionResult.None();
            }
            finally
            {
                LocalContext.LeaveContext();
            }
        }

        private static void ApplyParameters(Context Context, List<String> DeclaredParameters, List<ValueContainerBase> EvaluatedParameters)
        {
            Int32 DeclaredParameterCount = (DeclaredParameters?.Count ?? 0);
            Int32 EvaluatedParameterCount = (EvaluatedParameters?.Count ?? 0);
            for (Int32 i = 0; i < DeclaredParameterCount; i++)
            {
                if (i < EvaluatedParameterCount)
                {
                    Context.SetVariable(DeclaredParameters[i], EvaluatedParameters[i]);
                }
                else
                {
                    Context.SetVariable(DeclaredParameters[i], ValueContainerBase.Empty);
                }
            }
        }

        internal static String GetGenericTypeName(Type ValueType)
        {
            StringComposer StringComposer = new StringComposer();
            foreach (Type GenericTypeArgument in ValueType.GenericTypeArguments)
            {
                StringComposer.AppendSeparated(", ", GenericTypeArgument.Name);
            }

            Int32 Posistion = ValueType.Name.IndexOf('`');
            String TypeName = ValueType.Name.Substring(0, Posistion);

            return TypeName + "<" + StringComposer.ToString() + ">";
        }
    }
}