namespace LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer
{
    using System;

    public class DateTimeLiteralValueContainer : LiteralValueContainerBase
    {
        public const String ComponentName = "DateTime";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;
        private readonly DateTime Value;

        public DateTimeLiteralValueContainer(DateTime Value)
        {
            this.Value = Value;
            this.ValueType = typeof(DateTime);
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.DateTimeDataType;
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

        internal override Int64 GetInt64()
        {
            return Value.Ticks;
        }
        internal override Single GetSingle()
        {
            return (Single)Value.Ticks;
        }
        internal override Double GetDouble()
        {
            return (Double)Value.Ticks;
        }
        internal override DateTime GetDateTime()
        {
            return Value;
        }

        internal override Int64? GetInt64OrNull()
        {
            return Value.Ticks;
        }
        internal override Single? GetSingleOrNull()
        {
            return (Single)Value.Ticks;
        }
        internal override Double? GetDoubleOrNull()
        {
            return (Double)Value.Ticks;
        }
        internal override DateTime? GetDateTimeOrNull()
        {
            return Value;
        }
        #endregion

        #region Implicit
        internal override Boolean IsGreaterThan(ValueContainerBase Other)
        {
            return Value > Other.GetDateTime();
        }
        internal override Boolean IsGreaterThanOrEqualTo(ValueContainerBase Other)
        {
            return Value >= Other.GetDateTime();
        }
        internal override Boolean IsLowerThan(ValueContainerBase Other)
        {
            return Value < Other.GetDateTime();
        }
        internal override Boolean IsLowerThanOrEqualTo(ValueContainerBase Other)
        {
            return Value <= Other.GetDateTime();
        }
        internal override Boolean IsEqualTo(ValueContainerBase Other)
        {
            return Value == Other.GetDateTimeOrNull();
        }
        internal override Boolean IsDifferentFrom(ValueContainerBase Other)
        {
            return Value != Other.GetDateTimeOrNull();
        }

        internal override ValueContainerBase Add(ValueContainerBase Value)
        {
            return new DateTimeLiteralValueContainer(this.Value + Value.GetTimeSpan());
        }
        internal override ValueContainerBase Subtract(ValueContainerBase Value)
        {
            return new DateTimeLiteralValueContainer(this.Value - Value.GetTimeSpan());
        }
        #endregion
    }
}