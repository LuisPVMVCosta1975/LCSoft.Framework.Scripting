namespace LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer
{
    using System;

    public class CharLiteralValueContainer : LiteralValueContainerBase
    {
        public const String ComponentName = "Char";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;
        private readonly Char Value;

        public CharLiteralValueContainer(Char Value)
        {
            this.Value = Value;
            this.ValueType = typeof(Char);
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.CharDataType;
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
            return (Value == 'T');
        }
        internal override Char GetChar()
        {
            return Value;
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

        public override Boolean? GetBooleanOrNull()
        {
            return (Value == 'T');
        }
        internal override Char? GetCharOrNull()
        {
            return Value;
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
        #endregion

        #region Implicit
        internal override Boolean IsGreaterThan(ValueContainerBase Other)
        {
            return Value > Other.GetChar();
        }
        internal override Boolean IsGreaterThanOrEqualTo(ValueContainerBase Other)
        {
            return Value >= Other.GetChar();
        }
        internal override Boolean IsLowerThan(ValueContainerBase Other)
        {
            return Value < Other.GetChar();
        }
        internal override Boolean IsLowerThanOrEqualTo(ValueContainerBase Other)
        {
            return Value <= Other.GetChar();
        }
        internal override Boolean IsEqualTo(ValueContainerBase Other)
        {
            return Value == Other.GetCharOrNull();
        }
        internal override Boolean IsDifferentFrom(ValueContainerBase Other)
        {
            return Value != Other.GetCharOrNull();
        }

        internal override ValueContainerBase Multiply(ValueContainerBase Value)
        {
            return new StringLiteralValueContainer(new String(this.Value, Value.GetInt32()));
        }
        #endregion
    }
}