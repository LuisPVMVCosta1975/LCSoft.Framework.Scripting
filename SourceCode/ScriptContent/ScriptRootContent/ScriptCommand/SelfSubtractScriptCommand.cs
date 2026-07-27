namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Reflection;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class SelfSubtractScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "SelfSubtract";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal IScriptExpression ScriptExpression;
        internal String Name;
        internal IScriptExpression Key;
        internal IScriptExpression Value;

        public SelfSubtractScriptCommand(IScriptExpression ScriptExpression, String Name, IScriptExpression Key, IScriptExpression Value)
        {
            this.ScriptExpression = ScriptExpression;
            this.Name = Name;
            this.Key = Key;
            this.Value = Value;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            if (Key != null)
            {
                return RunElementItem(Context, ScriptResources);
            }

            if (ScriptExpression == null)
            {
                return RunElementVariable(Context, ScriptResources);
            }

            return RunElementProperty(Context, ScriptResources);
        }

        private ExecutionResult RunElementVariable(Context Context, ScriptResources ScriptResources)
        {
            Context VariableContext = Context.GetVariableContext(Name);

            ValueContainerBase Value1 = VariableContext.Variables[Name];
            if (Value1 == ValueContainerBase.Empty)
            {
                throw new EmptyValueException(ComponentSignature + " / " + nameof(Name));
            }

            if (Value != null)
            {
                ValueContainerBase Value2 = Value.EvaluateElement(Context, ScriptResources);
                if (Value2 == null)
                {
                    throw new EmptyValueException(ComponentSignature + " / " + nameof(Value));
                }

                VariableContext.Variables[Name] = Value1.Subtract(Value2);
            }
            else
            {
                VariableContext.Variables[Name] = Value1.Decrement();
            }

            return ExecutionResult.None();
        }
        private ExecutionResult RunElementProperty(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase ValueContainer = ScriptExpression.EvaluateElement(Context, ScriptResources);
            if (ValueContainer == ValueContainerBase.Empty)
            {
                throw new EmptyValueException(ComponentSignature + " / " + nameof(ScriptExpression));
            }

            ValueContainerBase Value1 = ValueContainer.GetProperty(Name);
            if (Value1 == ValueContainerBase.Empty)
            {
                throw new EmptyValueException(ComponentSignature + " / " + nameof(Name));
            }

            if (Value != null)
            {
                ValueContainerBase Value2 = Value.EvaluateElement(Context, ScriptResources);
                if (Value2 == null)
                {
                    throw new EmptyValueException(ComponentSignature + " / " + nameof(Value));
                }

                ValueContainer.SetProperty(Name, Value1.Subtract(Value2));
            }
            else
            {
                ValueContainer.SetProperty(Name, Value1.Decrement());
            }

            return ExecutionResult.None();
        }
        private ExecutionResult RunElementItem(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase ValueContainer = ScriptExpression.EvaluateElement(Context, ScriptResources);
            if (ValueContainer == ValueContainerBase.Empty)
            {
                throw new EmptyValueException(ComponentSignature + " / " + nameof(ScriptExpression));
            }

            ValueContainerBase KeyValue = Key.EvaluateElement(Context, ScriptResources);
            if (KeyValue == ValueContainerBase.Empty)
            {
                throw new EmptyValueException(ComponentSignature + " / " + nameof(Key));
            }

            ValueContainerBase Value1 = ValueContainer.GetItem(KeyValue);
            if (Value1 == ValueContainerBase.Empty)
            {
                throw new EmptyValueException(ComponentSignature + " / " + nameof(Key));
            }

            if (Value != null)
            {
                ValueContainerBase Value2 = Value.EvaluateElement(Context, ScriptResources);
                if (Value2 == null)
                {
                    throw new EmptyValueException(ComponentSignature + " / " + nameof(Value));
                }

                ValueContainer.SetItem(KeyValue, Value1.Subtract(Value2));
            }
            else
            {
                ValueContainer.SetItem(KeyValue, Value1.Decrement());
            }

            return ExecutionResult.None();
        }
    }
}