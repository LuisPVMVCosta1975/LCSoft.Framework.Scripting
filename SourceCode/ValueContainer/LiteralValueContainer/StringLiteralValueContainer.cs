namespace LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.ScriptContent;

    public class StringLiteralValueContainer : LiteralValueContainerBase
    {
        public const String ComponentName = "String";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;
        private readonly String Value;

        public StringLiteralValueContainer(String Value)
        {
            this.Value = Value;
            this.ValueType = typeof(String);
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.StringDataType;
        public override String GetFrameworkTypeText() => ComponentName;
        public override String GetImplementationType() => ComponentSignature;

        public override ValueContainerBase CallMethod(Context Context, ScriptResources ScriptResources, String FunctionName, List<IScriptExpression> Parameters)
        {
            switch (FunctionName)
            {
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
            }

            return base.CallMethod(Context, ScriptResources, FunctionName, Parameters);
        }
        public override ValueContainerBase GetProperty(String Name)
        {
            switch (Name)
            {
                case "Length":
                    return new Int32LiteralValueContainer(Value.Length);
            }

            return base.GetProperty(Name);
        }
        public override ValueContainerBase GetItem(ValueContainerBase Index)
        {
            if (Index == null)
            {
                throw new EmptyValueException(ComponentSignature + " / " + nameof(GetItem) + " [Operation] / " + nameof(Index) + " [Parameter]");
            }

            return new CharLiteralValueContainer(Value[Index.GetInt32()]);
        }

        #region Methods
        public StringLiteralValueContainer TakeRange(Context Context, ScriptResources ScriptResources, IScriptExpression Start, IScriptExpression Length)
        {
            ValueContainerBase StartValue = Start.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(TakeRange) + " [Method] / " + nameof(Start));
            ValueContainerBase LengthValue = Length.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(TakeRange) + " [Method] / " + nameof(Length));
            return new StringLiteralValueContainer(Value.Substring(StartValue.GetInt32(), LengthValue.GetInt32()));
        }
        public StringLiteralValueContainer DropRange(Context Context, ScriptResources ScriptResources, IScriptExpression Start, IScriptExpression Length)
        {
            ValueContainerBase StartValue = Start.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(DropRange) + " [Method] / " + nameof(Start));
            ValueContainerBase LengthValue = Length.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(DropRange) + " [Method] / " + nameof(Length));

            Int32 StartInt32 = StartValue.GetInt32();
            Int32 LengthInt32 = LengthValue.GetInt32();

            String Before = Value.Substring(0, StartInt32);
            String After = Value.Substring(StartInt32 + LengthInt32, Value.Length - StartInt32 - LengthInt32);

            return new StringLiteralValueContainer(Before + After);
        }
        #endregion

        #region Internal
        internal override IEnumerable<ValueContainerBase> Enumerate()
        {
            foreach (Char Char in Value)
            {
                yield return new CharLiteralValueContainer(Char);
            }
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
            return Value;
        }

        public override Boolean GetBoolean()
        {
            return Boolean.Parse(Value);
        }
        internal override Char GetChar()
        {
            return Char.Parse(Value);
        }
        internal override Int32 GetInt32()
        {
            return Int32.Parse(Value);
        }
        internal override Int64 GetInt64()
        {
            return Int64.Parse(Value);
        }
        internal override Single GetSingle()
        {
            return Single.Parse(Value);
        }
        internal override Double GetDouble()
        {
            return Double.Parse(Value);
        }
        internal override DateTime GetDateTime()
        {
            return DateTime.Parse(Value);
        }
        internal override TimeSpan GetTimeSpan()
        {
            return TimeSpan.Parse(Value);
        }

        public override Boolean? GetBooleanOrNull()
        {
            if (Boolean.TryParse(Value, out Boolean Result))
            {
                return Result;
            }

            return null;
        }
        internal override Char? GetCharOrNull()
        {
            if (Char.TryParse(Value, out Char Result))
            {
                return Result;
            }

            return null;
        }
        internal override Int32? GetInt32OrNull()
        {
            if (Int32.TryParse(Value, out Int32 Result))
            {
                return Result;
            }

            return null;
        }
        internal override Int64? GetInt64OrNull()
        {
            if (Int64.TryParse(Value, out Int64 Result))
            {
                return Result;
            }

            return null;
        }
        internal override Single? GetSingleOrNull()
        {
            if (Single.TryParse(Value, out Single Result))
            {
                return Result;
            }

            return null;
        }
        internal override Double? GetDoubleOrNull()
        {
            if (Double.TryParse(Value, out Double Result))
            {
                return Result;
            }

            return null;
        }
        internal override DateTime? GetDateTimeOrNull()
        {
            if (DateTime.TryParse(Value, out DateTime Result))
            {
                return Result;
            }

            return null;
        }
        internal override TimeSpan? GetTimeSpanOrNull()
        {
            if (TimeSpan.TryParse(Value, out TimeSpan Result))
            {
                return Result;
            }

            return null;
        }
        #endregion

        #region Implicit
        internal override Boolean IsGreaterThan(ValueContainerBase Other)
        {
            return Value.CompareTo(Other.GetString()) > 0;
        }
        internal override Boolean IsGreaterThanOrEqualTo(ValueContainerBase Other)
        {
            return Value.CompareTo(Other.GetString()) >= 0;
        }
        internal override Boolean IsLowerThan(ValueContainerBase Other)
        {
            return Value.CompareTo(Other.GetString()) < 0;
        }
        internal override Boolean IsLowerThanOrEqualTo(ValueContainerBase Other)
        {
            return Value.CompareTo(Other.GetString()) <= 0;
        }
        internal override Boolean IsEqualTo(ValueContainerBase Other)
        {
            return Value == Other.GetString();
        }
        internal override Boolean IsDifferentFrom(ValueContainerBase Other)
        {
            return Value != Other.GetString();
        }

        internal override ValueContainerBase Add(ValueContainerBase Value)
        {
            return new StringLiteralValueContainer(this.Value + Value.GetString());
        }
        internal override ValueContainerBase Multiply(ValueContainerBase Value)
        {
            Int32 Count = Value.GetInt32();
            StringBuilder Result = new StringBuilder(Count * this.Value.Length);
            while (Count-- > 0)
            {
                Result.Append(this.Value);
            }
            return new StringLiteralValueContainer(Result.ToString());
        }
        #endregion
    }
}