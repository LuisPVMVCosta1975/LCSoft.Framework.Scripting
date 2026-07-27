namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;

    public class AttributeListValueContainer : ValueContainerBase
    {
        public const String ComponentName = "AttributeList";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Dictionary<String, ValueContainerBase> Value;
        private readonly Type ValueType;

        private Boolean IsLocked;
        private Boolean IsProtected;

        public AttributeListValueContainer(Dictionary<String, ValueContainerBase> Value, Boolean IsType)
        {
            this.Value = Value;
            ValueType = typeof(Dictionary<String, ValueContainerBase>);

            if (IsType)
            {
                IsLocked = true;
            }
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.AttributeListDataType;
        public override String GetFrameworkTypeText() => ComponentName;
        public override String GetInternalTypeText() => "...";
        public override String GetImplementationType() => ComponentSignature;

        public override ValueContainerBase CallMethod(Context Context, ScriptResources ScriptResources, String FunctionName, List<IScriptExpression> Parameters)
        {
            switch (FunctionName)
            {
                case nameof(HasProperty):
                    if (Parameters != null && Parameters.Count == 1)
                    {
                        return HasProperty(Context, ScriptResources, Parameters[0]);
                    }
                    break;
                case nameof(Lock):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        Lock();
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(Protect):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        Protect();
                        return ValueContainerBase.Empty;
                    }
                    break;
            }

            return base.CallMethod(Context, ScriptResources, FunctionName, Parameters);
        }
        public override ValueContainerBase GetProperty(String Name)
        {
            if (Value.ContainsKey(Name))
            {
                return Value[Name];
            }

            return base.GetProperty(Name);
        }
        public override void SetProperty(String Name, ValueContainerBase Value)
        {
            if (this.Value.ContainsKey(Name))
            {
                if (IsProtected)
                {
                    throw new InvalidOperationException("Protected!");
                }

                this.Value[Name] = Value;
                return;
            }

            if (IsLocked)
            {
                throw new InvalidOperationException("Locked!");
            }

            this.Value.Add(Name, Value);
        }

        #region Methods
        private ValueContainerBase HasProperty(Context Context, ScriptResources ScriptResources, IScriptExpression Name)
        {
            ValueContainerBase NameValue = Name.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(HasProperty) + " [Method] / " + nameof(Name));
            return (Value.ContainsKey(NameValue.GetString()) ? True : False);
        }
        private void Lock()
        {
            IsLocked = true;
        }
        private void Protect()
        {
            IsProtected = true;
        }
        #endregion

        #region Internal
        public override Object GetUnspecified()
        {
            return Value;
        }
        internal override Type GetUnderlyingType()
        {
            return ValueType;
        }

        internal override String GetString()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        #endregion

        #region Implicit
        #endregion
    }
}