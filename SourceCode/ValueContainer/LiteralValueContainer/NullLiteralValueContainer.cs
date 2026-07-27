namespace LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer
{
    using System;

    public class NullLiteralValueContainer : LiteralValueContainerBase
    {
        public const String ComponentName = "Null";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;

        public NullLiteralValueContainer(Type ValueType)
        {
            this.ValueType = ValueType;
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.NullDataType;
        public override String GetFrameworkTypeText() => ComponentName;
        public override String GetInternalTypeText() => ValueType.Name;
        public override String GetImplementationType() => ComponentSignature;

        #region Internal
        public override Object GetUnspecified()
        {
            return null;
        }
        internal override Type GetUnderlyingType()
        {
            return ValueType;
        }

        internal override String GetString()
        {
            return "NULL: (" + ValueType.ToString() + ")";
        }

        public override Boolean IsNull()
        {
            return true;
        }
        #endregion

        #region Implicit
        #endregion
    }
}