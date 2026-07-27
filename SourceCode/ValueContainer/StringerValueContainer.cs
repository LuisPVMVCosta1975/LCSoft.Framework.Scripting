namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class StringerValueContainer : ValueContainerBase
    {
        public const String ComponentName = "Stringer";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly StringBuilder Value;
        private readonly Type ValueType;

        internal StringerValueContainer()
        {
            Value = new StringBuilder();
            ValueType = typeof(StringBuilder);
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.StringerDataType;
        public override String GetFrameworkTypeText() => ComponentName;
        public override String GetInternalTypeText() => "...";
        public override String GetImplementationType() => ComponentSignature;

        public override ValueContainerBase CallMethod(Context Context, ScriptResources ScriptResources, String FunctionName, List<IScriptExpression> Parameters)
        {
            switch (FunctionName)
            {
                case nameof(Clear):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        Clear();
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(Append):
                    if (Parameters != null && Parameters.Count != 0)
                    {
                        Append(Context, ScriptResources, Parameters);
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(AppendSeparated):
                    if (Parameters != null && Parameters.Count >= 2)
                    {
                        AppendSeparated(Context, ScriptResources, Parameters[0], Parameters.Skip(1));
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(PrefixIfNeeded):
                    if (Parameters != null && Parameters.Count == 1)
                    {
                        PrefixIfNeeded(Context, ScriptResources, Parameters[0]);
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(SuffixIfNeeded):
                    if (Parameters != null && Parameters.Count == 1)
                    {
                        SuffixIfNeeded(Context, ScriptResources, Parameters[0]);
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(DecorateIfNeeded):
                    if (Parameters != null && Parameters.Count == 2)
                    {
                        DecorateIfNeeded(Context, ScriptResources, Parameters[0], Parameters[1]);
                        return ValueContainerBase.Empty;
                    }
                    break;
            }

            return base.CallMethod(Context, ScriptResources, FunctionName, Parameters);
        }
        public override ValueContainerBase GetProperty(String Name)
        {
            switch (Name)
            {
                case "IsEmpty":
                    return (Value.Length == 0 ? ValueContainerBase.True : ValueContainerBase.False);
                case "IsNotEmpty":
                    return (Value.Length != 0 ? ValueContainerBase.True : ValueContainerBase.False);
            }

            return base.GetProperty(Name);
        }

        #region Methods
        public void Clear()
        {
            Value.Clear();
        }

        public void Append(Context Context, ScriptResources ScriptResources, IEnumerable<IScriptExpression> Values)
        {
            foreach (IScriptExpression Value in Values)
            {
                this.Value.Append(Value.EvaluateElement(Context, ScriptResources).GetString());
            }
        }
        public void AppendSeparated(Context Context, ScriptResources ScriptResources, IScriptExpression Separator, IEnumerable<IScriptExpression> Values)
        {
            if (Value.Length != 0)
            {
                ValueContainerBase SeparatorValue = Separator.EvaluateElement(Context, ScriptResources);
                Value.Append(SeparatorValue.GetString());
            }

            Append(Context, ScriptResources, Values);
        }

        public void PrefixIfNeeded(Context Context, ScriptResources ScriptResources, IScriptExpression Prefix)
        {
            if (Value.Length == 0)
            {
                return;
            }

            ValueContainerBase PrefixValue = Prefix.EvaluateElement(Context, ScriptResources);
            Value.Insert(0, PrefixValue.GetString());
        }
        public void SuffixIfNeeded(Context Context, ScriptResources ScriptResources, IScriptExpression Suffix)
        {
            if (Value.Length == 0)
            {
                return;
            }

            ValueContainerBase SuffixValue = Suffix.EvaluateElement(Context, ScriptResources);
            Value.Append(SuffixValue.GetString());
        }
        public void DecorateIfNeeded(Context Context, ScriptResources ScriptResources, IScriptExpression Prefix, IScriptExpression Suffix)
        {
            if (Value.Length == 0)
            {
                return;
            }

            ValueContainerBase PrefixValue = Prefix.EvaluateElement(Context, ScriptResources);
            Value.Insert(0, PrefixValue.GetString());

            ValueContainerBase SuffixValue = Suffix.EvaluateElement(Context, ScriptResources);
            Value.Append(SuffixValue.GetString());
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
            return Value.ToString();
        }
        #endregion

        #region Implicit
        #endregion
    }
}