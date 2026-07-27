namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public abstract class ValueContainerBase
    {
        public const String ComponentType = "Value Container";

        public static readonly Int64LiteralValueContainer BooleanDataType = new Int64LiteralValueContainer(0);
        public static readonly Int64LiteralValueContainer CharDataType = new Int64LiteralValueContainer(1);
        public static readonly Int64LiteralValueContainer DateTimeDataType = new Int64LiteralValueContainer(2);
        public static readonly Int64LiteralValueContainer DoubleDataType = new Int64LiteralValueContainer(3);
        public static readonly Int64LiteralValueContainer Int32DataType = new Int64LiteralValueContainer(4);
        public static readonly Int64LiteralValueContainer Int64DataType = new Int64LiteralValueContainer(5);
        public static readonly Int64LiteralValueContainer SingleDataType = new Int64LiteralValueContainer(6);
        public static readonly Int64LiteralValueContainer StringDataType = new Int64LiteralValueContainer(7);
        public static readonly Int64LiteralValueContainer TimeSpanDataType = new Int64LiteralValueContainer(8);
        //---
        public static readonly Int64LiteralValueContainer NullDataType = new Int64LiteralValueContainer(9);
        //---
        public static readonly Int64LiteralValueContainer AttributeListDataType = new Int64LiteralValueContainer(10);
        public static readonly Int64LiteralValueContainer CancelationTokenDataType = new Int64LiteralValueContainer(11);
        public static readonly Int64LiteralValueContainer ClassReferenceDataType = new Int64LiteralValueContainer(12);
        public static readonly Int64LiteralValueContainer DelegateDataType = new Int64LiteralValueContainer(13);
        public static readonly Int64LiteralValueContainer FunctionDataType = new Int64LiteralValueContainer(14);
        public static readonly Int64LiteralValueContainer LambdaDataType = new Int64LiteralValueContainer(15);
        public static readonly Int64LiteralValueContainer LazyDataType = new Int64LiteralValueContainer(16);
        public static readonly Int64LiteralValueContainer ObjectReferenceDataType = new Int64LiteralValueContainer(17);
        public static readonly Int64LiteralValueContainer ObjectDataType = new Int64LiteralValueContainer(18);
        public static readonly Int64LiteralValueContainer SemaphoreDataType = new Int64LiteralValueContainer(19);
        public static readonly Int64LiteralValueContainer SpawnTokenDataType = new Int64LiteralValueContainer(20);
        public static readonly Int64LiteralValueContainer StringerDataType = new Int64LiteralValueContainer(21);
        public static readonly Int64LiteralValueContainer TimerDataType = new Int64LiteralValueContainer(22);
        public static readonly Int64LiteralValueContainer ValueListDataType = new Int64LiteralValueContainer(23);
        public static readonly Int64LiteralValueContainer VolatileDataType = new Int64LiteralValueContainer(24);
        //---
        public static readonly Int64LiteralValueContainer EnumDataType = new Int64LiteralValueContainer(25);

        public static readonly LiteralValueContainerBase Empty = null;
        public static readonly BooleanLiteralValueContainer True = new BooleanLiteralValueContainer(true);
        public static readonly BooleanLiteralValueContainer False = new BooleanLiteralValueContainer(false);
        public static readonly NullLiteralValueContainer NullString = new NullLiteralValueContainer(typeof(String));
        public static readonly Int32LiteralValueContainer ZeroInt32 = new Int32LiteralValueContainer(0);
        public static readonly Int32LiteralValueContainer MinusOneInt32 = new Int32LiteralValueContainer(-1);
        public static readonly SingleLiteralValueContainer ZeroSingle = new SingleLiteralValueContainer(0);
        public static readonly SingleLiteralValueContainer MinusOneSingle = new SingleLiteralValueContainer(-1);

        public abstract ValueContainerBase GetFrameworkType();
        public abstract String GetFrameworkTypeText();
        public abstract String GetInternalTypeText();
        public abstract String GetImplementationType();

        public virtual ValueContainerBase CallMethod(Context Context, ScriptResources ScriptResources, String Name, List<IScriptExpression> Parameters)
        {
            //like base class methods
            switch (Name)
            {
                case nameof(IsNotIn):
                    if (Parameters != null && Parameters.Count >= 1)
                    {
                        return IsNotIn(Context, ScriptResources, Parameters);
                    }
                    break;
                case nameof(IsIn):
                    if (Parameters != null && Parameters.Count >= 1)
                    {
                        return IsIn(Context, ScriptResources, Parameters);
                    }
                    break;
                case nameof(IsNull):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return (IsNull() ? True : False);
                    }
                    break;
                case "IsNotNull":
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return (!IsNull() ? True : False);
                    }
                    break;

                case nameof(IfNull):
                    if (Parameters != null && Parameters.Count == 1)
                    {
                        return IfNull(Context, ScriptResources, Parameters[0]);
                    }
                    break;

                case nameof(AsInt32):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return AsInt32();
                    }
                    break;
                case nameof(AsSingle):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return AsSingle();
                    }
                    break;
                case nameof(AsString):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return AsString();
                    }
                    break;
            }

            List<ValueContainerBase> EvaluatedParameters = RunTimeUtils.EvaluateParameters(Parameters, Context, ScriptResources);
            (Object[] Objects, Type[] Types) = PrepareParametersToInvoke(EvaluatedParameters);
            return Invoke(GetUnderlyingType(), GetUnspecified(), Name, Objects, Types);
        }
        public virtual ValueContainerBase GetProperty(String Name)
        {
            //like base class properties
            switch (Name)
            {
                case "Type":
                    return GetFrameworkType();
                case "UnderlyingType":
                    return new ObjectReferenceValueContainer(GetUnderlyingType());
            }

            return Invoke(GetUnderlyingType(), GetUnspecified(), Name);
        }
        public virtual void SetProperty(String Name, ValueContainerBase Value)
        {
            Invoke(GetUnderlyingType(), GetUnspecified(), Name, Value.GetUnspecified());
        }
        public virtual ValueContainerBase GetItem(ValueContainerBase Key)
        {
            return Invoke(GetUnderlyingType(), GetUnspecified(), Key.GetUnspecified());
        }
        public virtual void SetItem(ValueContainerBase Key, ValueContainerBase Value)
        {
            Invoke(GetUnderlyingType(), GetUnspecified(), Key.GetUnspecified(), Value.GetUnspecified(), Value.GetUnderlyingType());
        }

        internal ValueContainerBase Invoke(Type Type, Object Object, String MethodName, Object[] Parameters, Type[] Types)
        {
            MethodInfo MI = Type.GetRuntimeMethod(MethodName, Types);
            if (MI == null)
            {
                throw new OperationNotFoundException(GetImplementationType() + ": " + MethodName + "()");
            }

            return RunTimeUtils.Box(MI.Invoke(Object, Parameters), MI.ReturnType);
        }
        internal ValueContainerBase Invoke(Type Type, Object Object, String AttributeName)
        {
            FieldInfo FI = Type.GetRuntimeField(AttributeName);
            if (FI != null)
            {
                return RunTimeUtils.Box(FI.GetValue(Object), FI.FieldType);
            }

            PropertyInfo PI = Type.GetRuntimeProperty(AttributeName);
            if (PI != null)
            {
                return RunTimeUtils.Box(PI.GetValue(Object, null), PI.PropertyType);
            }

            throw new AttributeNotFoundException(GetImplementationType() + ": " + AttributeName);
        }
        internal void Invoke(Type Type, Object Object, String AttributeName, Object Value)
        {
            FieldInfo FI = Type.GetRuntimeField(AttributeName);
            if (FI != null)
            {
                FI.SetValue(Object, Value);
                return;
            }

            PropertyInfo PI = Type.GetRuntimeProperty(AttributeName);
            if (PI != null)
            {
                PI.SetValue(Object, Value, null);
                return;
            }

            throw new AttributeNotFoundException(GetImplementationType() + ": " + AttributeName);
        }
        internal ValueContainerBase Invoke(Type Type, Object Object, Object Key)
        {
            MethodInfo MI = Type.GetRuntimeMethod("get_Item", new Type[] { Key.GetType() });
            if (MI == null)
            {
                throw new OperationNotFoundException(GetImplementationType() + ": [" + Key.GetType().Name + "]");
            }

            return RunTimeUtils.Box(MI.Invoke(Object, new Object[] { Key }), MI.ReturnType);
        }
        internal void Invoke(Type Type, Object Object, Object Key, Object Value, Type ValueType)
        {
            MethodInfo MI = Type.GetRuntimeMethod("set_Item", new Type[] { Key.GetType(), ValueType });
            if (MI == null)
            {
                throw new OperationNotFoundException(GetImplementationType() + ": Item(" + Key.GetType().Name + ")");
            }

            MI.Invoke(Object, new Object[] { Key, Value });
        }

        internal (Object[] Objects, Type[] Types) PrepareParameterToInvoke(ValueContainerBase Parameter)
        {
            Object[] InvokeParameters = new Object[1];
            Type[] InvokeTypes = new Type[1];

            InvokeParameters[0] = Parameter.GetUnspecified();
            InvokeTypes[0] = Parameter.GetUnderlyingType();

            return (InvokeParameters, InvokeTypes);
        }
        internal (Object[] Objects, Type[] Types) PrepareParametersToInvoke(List<ValueContainerBase> Parameters)
        {
            if (Parameters == null)
            {
                return (new Object[0], Type.EmptyTypes);
            }

            Object[] InvokeParameters = new Object[Parameters.Count];
            Type[] InvokeTypes = new Type[Parameters.Count];

            for (Int32 i = 0; i < Parameters.Count; i++)
            {
                ValueContainerBase Parameter = Parameters[i];
                InvokeParameters[i] = Parameter.GetUnspecified();
                InvokeTypes[i] = Parameter.GetUnderlyingType();
            }

            return (InvokeParameters, InvokeTypes);
        }

        #region Methods
        private BooleanLiteralValueContainer IsNotIn(Context Context, ScriptResources ScriptResources, List<IScriptExpression> Options)
        {
            foreach (IScriptExpression Option in Options)
            {
                ValueContainerBase OptionValue = Option.EvaluateElement(Context, ScriptResources, nameof(IsNotIn) + " [Method] / " + nameof(Option));
                if (this.IsEqualTo(OptionValue))
                {
                    return ValueContainerBase.False;
                }
            }

            return ValueContainerBase.True;
        }
        private BooleanLiteralValueContainer IsIn(Context Context, ScriptResources ScriptResources, List<IScriptExpression> Options)
        {
            foreach (IScriptExpression Option in Options)
            {
                ValueContainerBase OptionValue = Option.EvaluateElement(Context, ScriptResources, nameof(IsIn) + " [Method] / " + nameof(Option));
                if (this.IsEqualTo(OptionValue))
                {
                    return ValueContainerBase.True;
                }
            }

            return ValueContainerBase.False;
        }

        private ValueContainerBase IfNull(Context Context, ScriptResources ScriptResources, IScriptExpression Then)
        {
            if (this.IsNull())
            {
                return Then.EvaluateElement(Context, ScriptResources);
            }

            return this;
        }

        private Int32LiteralValueContainer AsInt32()
        {
            return new Int32LiteralValueContainer(GetInt32());
        }
        private SingleLiteralValueContainer AsSingle()
        {
            return new SingleLiteralValueContainer(GetSingle());
        }
        private StringLiteralValueContainer AsString()
        {
            return new StringLiteralValueContainer(GetString());
        }
        #endregion

        #region Internal
        internal virtual IEnumerable<ValueContainerBase> Enumerate()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }

        public abstract Object GetUnspecified();
        internal abstract Type GetUnderlyingType();

        internal abstract String GetString();

        public virtual Boolean GetBoolean()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual Char GetChar()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual Int32 GetInt32()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual Int64 GetInt64()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual Single GetSingle()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual Double GetDouble()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual DateTime GetDateTime()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual TimeSpan GetTimeSpan()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }

        public virtual Boolean? GetBooleanOrNull()
        {
            return null;
        }
        internal virtual Char? GetCharOrNull()
        {
            return null;
        }
        internal virtual Int32? GetInt32OrNull()
        {
            return null;
        }
        internal virtual Int64? GetInt64OrNull()
        {
            return null;
        }
        internal virtual Single? GetSingleOrNull()
        {
            return null;
        }
        internal virtual Double? GetDoubleOrNull()
        {
            return null;
        }
        internal virtual DateTime? GetDateTimeOrNull()
        {
            return null;
        }
        internal virtual TimeSpan? GetTimeSpanOrNull()
        {
            return null;
        }

        public virtual Boolean IsNull()
        {
            return false;
        }
        public virtual Boolean IsLiteral()
        {
            return false;
        }
        public virtual Boolean IsVolatile()
        {
            return false;
        }
        #endregion

        #region Implicit
        internal virtual Boolean IsGreaterThan(ValueContainerBase Other)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual Boolean IsGreaterThanOrEqualTo(ValueContainerBase Other)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual Boolean IsLowerThan(ValueContainerBase Other)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual Boolean IsLowerThanOrEqualTo(ValueContainerBase Other)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual Boolean IsEqualTo(ValueContainerBase Other)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual Boolean IsDifferentFrom(ValueContainerBase Other)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }

        internal virtual ValueContainerBase Add(ValueContainerBase Value)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual ValueContainerBase Subtract(ValueContainerBase Value)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual ValueContainerBase Multiply(ValueContainerBase Value)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual ValueContainerBase Divide(ValueContainerBase Value)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }

        internal virtual ValueContainerBase And(ValueContainerBase Value)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual ValueContainerBase Or(ValueContainerBase Value)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual ValueContainerBase XAnd(ValueContainerBase Value)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual ValueContainerBase XOr(ValueContainerBase Value)
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }

        internal virtual ValueContainerBase Increment()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal virtual ValueContainerBase Decrement()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        #endregion
    }
}