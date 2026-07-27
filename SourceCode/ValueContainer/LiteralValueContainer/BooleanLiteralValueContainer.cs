namespace LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer
{
    using System;

    public class BooleanLiteralValueContainer : LiteralValueContainerBase
    {
        public const String ComponentName = "Boolean";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;
        private readonly Boolean Value;

        public BooleanLiteralValueContainer(Boolean Value)
        {
            this.Value = Value;
            this.ValueType = typeof(Boolean);
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.BooleanDataType;
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
            return (Value ? "True" : "False");
        }

        public override Boolean GetBoolean()
        {
            return Value;
        }
        internal override Char GetChar()
        {
            return (Value ? 'T' : 'F');
        }
        internal override Int32 GetInt32()
        {
            return (Value ? 1 : 0);
        }
        internal override Int64 GetInt64()
        {
            return (Value ? 1 : 0);
        }
        internal override Single GetSingle()
        {
            return (Value ? 1 : 0);
        }
        internal override Double GetDouble()
        {
            return (Value ? 1 : 0);
        }

        public override Boolean? GetBooleanOrNull()
        {
            return Value;
        }
        internal override Char? GetCharOrNull()
        {
            return (Value ? 'T' : 'F');
        }
        internal override Int32? GetInt32OrNull()
        {
            return (Value ? 1 : 0);
        }
        internal override Int64? GetInt64OrNull()
        {
            return (Value ? 1 : 0);
        }
        internal override Single? GetSingleOrNull()
        {
            return (Value ? 1 : 0);
        }
        internal override Double? GetDoubleOrNull()
        {
            return (Value ? 1 : 0);
        }
        #endregion

        #region Implicit
        internal override Boolean IsEqualTo(ValueContainerBase Other)
        {
            return Value == Other.GetBooleanOrNull();
        }
        internal override Boolean IsDifferentFrom(ValueContainerBase Other)
        {
            return Value != Other.GetBooleanOrNull();
        }
        #endregion
    }
}