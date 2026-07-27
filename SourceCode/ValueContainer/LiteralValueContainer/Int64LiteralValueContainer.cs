namespace LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer
{
    using System;

    public class Int64LiteralValueContainer : LiteralValueContainerBase
    {
        public const String ComponentName = "Int64";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;
        private readonly Int64 Value;

        public Int64LiteralValueContainer(Int64 Value)
        {
            this.Value = Value;
            this.ValueType = typeof(Int64);
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.Int64DataType;
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
            return Value;
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
            return new DateTime(Value);
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
            return Value;
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
            return new DateTime(Value);
        }
        internal override TimeSpan? GetTimeSpanOrNull()
        {
            return new TimeSpan((Int64)Value);
        }
        #endregion

        #region Implicit
        internal override Boolean IsGreaterThan(ValueContainerBase Other)
        {
            return Value > Other.GetInt64();
        }
        internal override Boolean IsGreaterThanOrEqualTo(ValueContainerBase Other)
        {
            return Value >= Other.GetInt64();
        }
        internal override Boolean IsLowerThan(ValueContainerBase Other)
        {
            return Value < Other.GetInt64();
        }
        internal override Boolean IsLowerThanOrEqualTo(ValueContainerBase Other)
        {
            return Value <= Other.GetInt64();
        }
        internal override Boolean IsEqualTo(ValueContainerBase Other)
        {
            return Value == Other.GetInt64OrNull();
        }
        internal override Boolean IsDifferentFrom(ValueContainerBase Other)
        {
            return Value != Other.GetInt64OrNull();
        }

        internal override ValueContainerBase Add(ValueContainerBase Value)
        {
            return new Int64LiteralValueContainer(this.Value + Value.GetInt64());
        }
        internal override ValueContainerBase Subtract(ValueContainerBase Value)
        {
            return new Int64LiteralValueContainer(this.Value - Value.GetInt64());
        }
        internal override ValueContainerBase Multiply(ValueContainerBase Value)
        {
            return new Int64LiteralValueContainer(this.Value * Value.GetInt64());
        }
        internal override ValueContainerBase Divide(ValueContainerBase Value)
        {
            return new Int64LiteralValueContainer(this.Value / Value.GetInt64());
        }

        internal override ValueContainerBase And(ValueContainerBase Value)
        {
            return new Int64LiteralValueContainer(this.Value & Value.GetInt64());
        }
        internal override ValueContainerBase Or(ValueContainerBase Value)
        {
            return new Int64LiteralValueContainer(this.Value | Value.GetInt64());
        }
        internal override ValueContainerBase XAnd(ValueContainerBase Value)
        {
            return new Int64LiteralValueContainer((this.Value | Value.GetInt64()) ^ Int64.MaxValue);
        }
        internal override ValueContainerBase XOr(ValueContainerBase Value)
        {
            return new Int64LiteralValueContainer(this.Value ^ Value.GetInt64());
        }

        internal override ValueContainerBase Increment()
        {
            return new Int64LiteralValueContainer(Value + 1);
        }
        internal override ValueContainerBase Decrement()
        {
            return new Int64LiteralValueContainer(Value - 1);
        }
        #endregion
    }
}