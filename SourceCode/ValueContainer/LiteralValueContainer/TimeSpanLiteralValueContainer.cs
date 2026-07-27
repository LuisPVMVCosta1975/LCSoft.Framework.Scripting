namespace LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer
{
    using System;

    public class TimeSpanLiteralValueContainer : LiteralValueContainerBase
    {
        public const String ComponentName = "TimeSpan";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;
        private readonly TimeSpan Value;

        public TimeSpanLiteralValueContainer(TimeSpan Value)
        {
            this.Value = Value;
            this.ValueType = typeof(TimeSpan);
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.TimeSpanDataType;
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
        internal override TimeSpan GetTimeSpan()
        {
            return Value;
        }
        #endregion

        #region Implicit
        internal override Boolean IsGreaterThan(ValueContainerBase Other)
        {
            return Value > Other.GetTimeSpan();
        }
        internal override Boolean IsGreaterThanOrEqualTo(ValueContainerBase Other)
        {
            return Value >= Other.GetTimeSpan();
        }
        internal override Boolean IsLowerThan(ValueContainerBase Other)
        {
            return Value < Other.GetTimeSpan();
        }
        internal override Boolean IsLowerThanOrEqualTo(ValueContainerBase Other)
        {
            return Value <= Other.GetTimeSpan();
        }
        internal override Boolean IsEqualTo(ValueContainerBase Other)
        {
            return Value == Other.GetTimeSpanOrNull();
        }
        internal override Boolean IsDifferentFrom(ValueContainerBase Other)
        {
            return Value != Other.GetTimeSpanOrNull();
        }

        internal override ValueContainerBase Add(ValueContainerBase Value)
        {
            return new TimeSpanLiteralValueContainer(this.Value + Value.GetTimeSpan());
        }
        internal override ValueContainerBase Subtract(ValueContainerBase Value)
        {
            return new TimeSpanLiteralValueContainer(this.Value - Value.GetTimeSpan());
        }
        #endregion
    }
}