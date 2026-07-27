namespace LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer
{
    using System;

    public class DoubleLiteralValueContainer : LiteralValueContainerBase
    {
        public const String ComponentName = "Double";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;
        private readonly Double Value;

        public DoubleLiteralValueContainer(Double Value)
        {
            this.Value = Value;
            this.ValueType = typeof(Int64);
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.DoubleDataType;
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
            return Value;
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
            return Value;
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
            return Value > Other.GetDouble();
        }
        internal override Boolean IsGreaterThanOrEqualTo(ValueContainerBase Other)
        {
            return Value >= Other.GetDouble();
        }
        internal override Boolean IsLowerThan(ValueContainerBase Other)
        {
            return Value < Other.GetDouble();
        }
        internal override Boolean IsLowerThanOrEqualTo(ValueContainerBase Other)
        {
            return Value <= Other.GetDouble();
        }
        internal override Boolean IsEqualTo(ValueContainerBase Other)
        {
            return Value == Other.GetDoubleOrNull();
        }
        internal override Boolean IsDifferentFrom(ValueContainerBase Other)
        {
            return Value != Other.GetDoubleOrNull();
        }

        internal override ValueContainerBase Add(ValueContainerBase Value)
        {
            return new DoubleLiteralValueContainer(this.Value + Value.GetDouble());
        }
        internal override ValueContainerBase Subtract(ValueContainerBase Value)
        {
            return new DoubleLiteralValueContainer(this.Value - Value.GetDouble());
        }
        internal override ValueContainerBase Multiply(ValueContainerBase Value)
        {
            return new DoubleLiteralValueContainer(this.Value * Value.GetDouble());
        }
        internal override ValueContainerBase Divide(ValueContainerBase Value)
        {
            return new DoubleLiteralValueContainer(this.Value / Value.GetDouble());
        }

        internal override ValueContainerBase Increment()
        {
            return new DoubleLiteralValueContainer(Value + 1);
        }
        internal override ValueContainerBase Decrement()
        {
            return new DoubleLiteralValueContainer(Value - 1);
        }
        #endregion
    }
}