namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class ValueListValueContainer : ValueContainerBase
    {
        public const String ComponentName = "ValueList";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly List<ValueContainerBase> Value;
        private readonly Type ValueType;

        private Boolean IsLocked;
        private Boolean IsProtected;

        internal ValueListValueContainer(List<ValueContainerBase> Values, Boolean IsShortcut)
        {
            this.Value = Values;
            ValueType = typeof(List<ValueContainerBase>);

            if (IsShortcut)
            {
                IsLocked = true;
                IsProtected = true;
            }
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.ValueListDataType;
        public override String GetFrameworkTypeText() => ComponentName;
        public override String GetInternalTypeText() => "...";
        public override String GetImplementationType() => ComponentSignature;

        public override ValueContainerBase CallMethod(Context Context, ScriptResources ScriptResources, String FunctionName, List<IScriptExpression> Parameters)
        {
            switch (FunctionName)
            {
                case nameof(Append):
                    if (Parameters != null && Parameters.Count != 0)
                    {
                        Append(Context, ScriptResources, Parameters);
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(AppendRange):
                    if (Parameters != null && Parameters.Count != 0)
                    {
                        AppendRange(Context, ScriptResources, Parameters);
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(Swap):
                    if (Parameters != null && Parameters.Count == 2)
                    {
                        Swap(Context, ScriptResources, Parameters[0], Parameters[1]);
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(TakeRange):
                    if (Parameters != null && Parameters.Count == 2)
                    {
                        return TakeRange(Context, ScriptResources, Parameters[0], Parameters[1]);
                    }
                    break;
                case nameof(DropRange):
                    if (Parameters != null && Parameters.Count == 2)
                    {
                        return DropRange(Context, ScriptResources, Parameters[0], Parameters[1]);
                    }
                    break;
                case nameof(Insert):
                    if (Parameters != null && Parameters.Count >= 2)
                    {
                        Insert(Context, ScriptResources, Parameters[0], Parameters.Skip(1));
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(InsertRange):
                    if (Parameters != null && Parameters.Count >= 2)
                    {
                        InsertRange(Context, ScriptResources, Parameters[0], Parameters.Skip(1));
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(Clear):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        Clear();
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(Add):
                    if (Parameters != null && Parameters.Count != 0)
                    {
                        return Add(Context, ScriptResources, Parameters);
                    }
                    break;
                case nameof(AddRange):
                    if (Parameters != null && Parameters.Count != 0)
                    {
                        return AddRange(Context, ScriptResources, Parameters);
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
            switch (Name)
            {
                case "Length":
                    return new Int32LiteralValueContainer(Value.Count);
                case "HasItems":
                    return (Value.Count != 0 ? ValueContainerBase.True : ValueContainerBase.False);
            }

            return base.GetProperty(Name);
        }
        public override ValueContainerBase GetItem(ValueContainerBase Index)
        {
            if (Index == null)
            {
                throw new EmptyValueException(ComponentSignature + " / " + nameof(GetItem) + " [Operation] / " + nameof(Index) + " [Parameter]");
            }

            return Value[Index.GetInt32()];
        }
        public override void SetItem(ValueContainerBase Index, ValueContainerBase Value)
        {
            if (IsProtected)
            {
                throw new InvalidOperationException("Protected!");
            }

            if (Index == null)
            {
                throw new EmptyValueException(ComponentSignature + " / " + nameof(SetItem) + " [Operation] / " + nameof(Index));
            }

            this.Value[Index.GetInt32()] = Value;
        }

        private List<ValueContainerBase> Clone()
        {
            List<ValueContainerBase> Result = new List<ValueContainerBase>();
            foreach (ValueContainerBase Item in Value)
            {
                Result.Add(Item);
            }
            return Result;
        }

        #region Methods
        public void Append(Context Context, ScriptResources ScriptResources, List<IScriptExpression> Values)
        {
            if (IsLocked)
            {
                throw new InvalidOperationException("Locked!");
            }

            foreach (IScriptExpression Value in Values)
            {
                ValueContainerBase ValueValue = Value.EvaluateElement(Context, ScriptResources);
                this.Value.Add(ValueValue);
            }
        }
        public void AppendRange(Context Context, ScriptResources ScriptResources, List<IScriptExpression> Values)
        {
            if (IsLocked)
            {
                throw new InvalidOperationException("Locked!");
            }

            foreach (IScriptExpression Value in Values)
            {
                ValueContainerBase ValueValue = Value.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(AppendRange) + " [Method] / " + nameof(Value));
                foreach (ValueContainerBase Value1 in ValueValue.Enumerate())
                {
                    this.Value.Add(Value1);
                }
            }
        }

        public void Swap(Context Context, ScriptResources ScriptResources, IScriptExpression Index1, IScriptExpression Index2)
        {
            if (IsProtected)
            {
                throw new InvalidOperationException("Protected!");
            }

            ValueContainerBase Index1Value = Index1.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Swap) + " [Method] / " + nameof(Index1));
            ValueContainerBase Index2Value = Index2.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Swap) + " [Method] / " + nameof(Index2));

            Int32 Index1Int32 = Index1Value.GetInt32();
            Int32 Index2Int32 = Index2Value.GetInt32();

            ValueContainerBase Aux = Value[Index1Int32];
            Value[Index1Int32] = Value[Index2Int32];
            Value[Index2Int32] = Aux;
        }

        public ValueListValueContainer TakeRange(Context Context, ScriptResources ScriptResources, IScriptExpression Start, IScriptExpression Length)
        {
            ValueContainerBase StartValue = Start.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(TakeRange) + " [Method] / " + nameof(Start));
            ValueContainerBase LengthValue = Length.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(TakeRange) + " [Method] / " + nameof(Length));
            return new ValueListValueContainer(Value.GetRange(StartValue.GetInt32(), LengthValue.GetInt32()), false);
        }
        public ValueListValueContainer DropRange(Context Context, ScriptResources ScriptResources, IScriptExpression Start, IScriptExpression Length)
        {
            ValueContainerBase StartValue = Start.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(DropRange) + " [Method] / " + nameof(Start));
            ValueContainerBase LengthValue = Length.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(DropRange) + " [Method] / " + nameof(Length));

            Int32 StartInt32 = StartValue.GetInt32();
            Int32 LengthInt32 = LengthValue.GetInt32();

            List<ValueContainerBase> Before = Value.GetRange(0, StartInt32);
            List<ValueContainerBase> After = Value.GetRange(StartInt32 + LengthInt32, Value.Count - StartInt32 - LengthInt32);

            Before.AddRange(After);

            return new ValueListValueContainer(Before, false);
        }

        public void Insert(Context Context, ScriptResources ScriptResources, IScriptExpression Index, IEnumerable<IScriptExpression> Values)
        {
            if (IsLocked)
            {
                throw new InvalidOperationException("Locked!");
            }

            ValueContainerBase IndexValue = Index.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Insert) + " [Method] / " + nameof(Index));

            Int32 IndexInt32 = IndexValue.GetInt32();

            foreach (IScriptExpression Value in Values)
            {
                ValueContainerBase ValueValue = Value.EvaluateElement(Context, ScriptResources);
                this.Value.Insert(IndexInt32++, ValueValue);
            }
        }
        public void InsertRange(Context Context, ScriptResources ScriptResources, IScriptExpression Index, IEnumerable<IScriptExpression> Values)
        {
            if (IsLocked)
            {
                throw new InvalidOperationException("Locked!");
            }

            ValueContainerBase IndexValue = Index.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(InsertRange) + " [Method] / " + nameof(Index));

            Int32 IndexInt32 = IndexValue.GetInt32();

            foreach (IScriptExpression Value in Values)
            {
                ValueContainerBase ValueValue = Value.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(AppendRange) + " [Method] / " + nameof(Value));
                foreach (ValueContainerBase Value1 in ValueValue.Enumerate())
                {
                    this.Value.Insert(IndexInt32++, Value1);
                }
            }
        }

        public void Clear()
        {
            Value.Clear();
        }

        public ValueContainerBase Add(Context Context, ScriptResources ScriptResources, List<IScriptExpression> Values)
        {
            List<ValueContainerBase> Result = Clone();
            foreach (IScriptExpression Value in Values)
            {
                ValueContainerBase ValueValue = Value.EvaluateElement(Context, ScriptResources);
                Result.Add(ValueValue);
            }
            return new ValueListValueContainer(Result, false);
        }
        public ValueContainerBase AddRange(Context Context, ScriptResources ScriptResources, List<IScriptExpression> Values)
        {
            List<ValueContainerBase> Result = Clone();
            foreach (IScriptExpression Value in Values)
            {
                ValueContainerBase ValueValue = Value.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(AddRange) + " [Method] / " + nameof(Value));
                foreach (ValueContainerBase Value1 in ValueValue.Enumerate())
                {
                    Result.Add(Value1);
                }
            }
            return new ValueListValueContainer(Result, false);
        }

        public void Lock()
        {
            IsLocked = true;
        }
        public void Protect()
        {
            IsProtected = true;
        }
        #endregion

        #region Internal
        internal override IEnumerable<ValueContainerBase> Enumerate()
        {
            return Value;
        }

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
        internal override ValueContainerBase Add(ValueContainerBase Value)
        {
            List<ValueContainerBase> Result = Clone();
            Result.Add(Value);
            return new ValueListValueContainer(Result, false);
        }
        #endregion
    }
}