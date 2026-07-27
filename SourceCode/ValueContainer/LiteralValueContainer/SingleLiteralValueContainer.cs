namespace LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer
{
    using System;

    public class SingleLiteralValueContainer : LiteralValueContainerBase
    {
        public const String ComponentName = "Single";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;
        private readonly Single Value;

        public SingleLiteralValueContainer(Single Value)
        {
            this.Value = Value;
            this.ValueType = typeof(Single);
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.SingleDataType;
        public override String GetFrameworkTypeText() => ComponentName;
        public override String GetImplementationType() => ComponentSignature;

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

        public override Boolean GetBoolean()
        {
            return (Value != 0);
        }
        internal override Char GetChar()
        {
            return (Char)Value;
        }
        internal override Int32 GetInt32()
        {
            return (Int32)Value;
        }
        internal override Int64 GetInt64()
        {
            return (Int64)Value;
        }
        internal override Single GetSingle()
        {
            return (Single)Value;
        }
        internal override Double GetDouble()
        {
            return (Double)Value;
        }
        internal override DateTime GetDateTime()
        {
            return new DateTime((Int64)Value);
        }
        internal override TimeSpan GetTimeSpan()
        {
            return new TimeSpan((Int64)Value);
        }

        public override Boolean? GetBooleanOrNull()
        {
            return (Value != 0);
        }
        internal override Char? GetCharOrNull()
        {
            return (Char)Value;
        }
        internal override Int32? GetInt32OrNull()
        {
            return (Int32)Value;
        }
        internal override Int64? GetInt64OrNull()
        {
            return (Int64)Value;
        }
        internal override Single? GetSingleOrNull()
        {
            return (Single)Value;
        }
        internal override Double? GetDoubleOrNull()
        {
            return (Double)Value;
        }
        internal override DateTime? GetDateTimeOrNull()
        {
            return new DateTime((Int64)Value);
        }
        internal override TimeSpan? GetTimeSpanOrNull()
        {
            return new TimeSpan((Int64)Value);
        }
        #endregion

        #region Implicit
        internal override Boolean IsGreaterThan(ValueContainerBase Other)
        {
            return Value > Other.GetSingle();
        }
        internal override Boolean IsGreaterThanOrEqualTo(ValueContainerBase Other)
        {
            return Value >= Other.GetSingle();
        }
        internal override Boolean IsLowerThan(ValueContainerBase Other)
        {
            return Value < Other.GetSingle();
        }
        internal override Boolean IsLowerThanOrEqualTo(ValueContainerBase Other)
        {
            return Value <= Other.GetSingle();
        }
        internal override Boolean IsEqualTo(ValueContainerBase Other)
        {
            return Value == Other.GetSingleOrNull();
        }
        internal override Boolean IsDifferentFrom(ValueContainerBase Other)
        {
            return Value != Other.GetSingleOrNull();
        }

        internal override ValueContainerBase Add(ValueContainerBase Value)
        {
            return new SingleLiteralValueContainer(this.Value + Value.GetSingle());
        }
        internal override ValueContainerBase Subtract(ValueContainerBase Value)
        {
            return new SingleLiteralValueContainer(this.Value - Value.GetSingle());
        }
        internal override ValueContainerBase Multiply(ValueContainerBase Value)
        {
            return new SingleLiteralValueContainer(this.Value * Value.GetSingle());
        }
        internal override ValueContainerBase Divide(ValueContainerBase Value)
        {
            return new SingleLiteralValueContainer(this.Value / Value.GetSingle());
        }

        internal override ValueContainerBase Increment()
        {
            return new SingleLiteralValueContainer(Value + 1);
        }
        internal override ValueContainerBase Decrement()
        {
            return new SingleLiteralValueContainer(Value - 1);
        }
        #endregion
    }
}