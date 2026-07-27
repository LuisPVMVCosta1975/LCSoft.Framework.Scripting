namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using LCSoft.Framework.Scripting.Classes;

    public class ClassReferenceValueContainer : ValueContainerBase
    {
        public const String ComponentName = "ClassReference";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;

        public ClassReferenceValueContainer(Type Value)
        {
            this.ValueType = Value;
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.ClassReferenceDataType;
        public override String GetFrameworkTypeText() => ComponentName; // + " (" + ValueType.Name + ")";
        public override String GetInternalTypeText() => ValueType.IsGenericType ? RunTimeUtils.GetGenericTypeName(ValueType) : ValueType.Name;
        public override String GetImplementationType() => ComponentSignature;

        #region Internal
        public override Object GetUnspecified()
        {
            return ValueType;
        }
        internal override Type GetUnderlyingType()
        {
            return ValueType;
        }

        internal override String GetString()
        {
            return ValueType.ToString();
        }
        #endregion

        #region Implicit
        #endregion
    }
}