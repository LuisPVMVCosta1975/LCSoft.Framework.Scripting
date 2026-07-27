namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;

    internal class VolatileValueContainer : ValueContainerBase
    {
        public const string ComponentName = "Volatile";
        public const string ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;
        private ValueContainerBase Value;

        public VolatileValueContainer(ValueContainerBase Value)
        {
            this.Value = Value;
            ValueType = Value.GetType();
        }

        public override ValueContainerBase GetFrameworkType() => VolatileDataType;
        public override String GetFrameworkTypeText() => ComponentName;
        public override String GetInternalTypeText() => Value.GetFrameworkTypeText();
        public override String GetImplementationType() => ComponentSignature;

        public override ValueContainerBase CallMethod(Context Context, ScriptResources ScriptResources, String FunctionName, List<IScriptExpression> Parameters)
        {
            switch (FunctionName)
            {
                case nameof(Get):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return Get();
                    }
                    break;
                case nameof(Set):
                    if (Parameters != null && Parameters.Count == 1)
                    {
                        return Set(Context, ScriptResources, Parameters[0]);
                    }
                    break;
                case nameof(Increment):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return Increment();
                    }
                    break;
                case nameof(Decrement):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return Decrement();
                    }
                    break;
            }

            return base.CallMethod(Context, ScriptResources, FunctionName, Parameters);
        }

        #region Methods
        public ValueContainerBase Get()
        {
            lock (this)
            {
                return Value;
            }
        }

        public ValueContainerBase Set(Context Context, ScriptResources ScriptResources, IScriptExpression Value)
        {
            lock (this)
            {
                ValueContainerBase Old = this.Value;
                this.Value = Value.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Set) + " [Method] / " + nameof(Value)); ;
                return Old;
            }
        }

        public new ValueContainerBase Increment()
        {
            lock (this)
            {
                ValueContainerBase Old = Value;
                Value = Value.Increment();
                return Old;
            }
        }
        public new ValueContainerBase Decrement()
        {
            lock (this)
            {
                ValueContainerBase Old = Value;
                Value = Value.Decrement();
                return Old;
            }
        }
        #endregion

        #region Internal
        public override Object GetUnspecified()
        {
            return Value.GetUnspecified();
        }
        internal override Type GetUnderlyingType()
        {
            return Value.GetUnderlyingType();
        }

        internal override string GetString()
        {
            return Value.GetString();
        }

        public override bool IsLiteral() //TODO: propagate???
        {
            return Value.IsLiteral();
        }
        public override bool IsVolatile()
        {
            return true;
        }
        #endregion

        #region Implicit
        #endregion
    }
}