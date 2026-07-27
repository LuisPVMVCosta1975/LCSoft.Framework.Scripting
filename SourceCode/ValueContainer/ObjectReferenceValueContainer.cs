namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Scripting.Classes;

    public class ObjectReferenceValueContainer : ValueContainerBase
    {
        public const String ComponentName = "ObjectReference";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;
        private readonly Object Value;

        public ObjectReferenceValueContainer(Object Value)
        {
            //TODO: Tech Debt: why the if?
            //if (Value != null)
            //{
            this.Value = Value;
            this.ValueType = Value.GetType();
            //}
            //todo
            //else
            //{
            //    ValueType = typeof(Object);
            //}
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.ObjectReferenceDataType;
        public override String GetFrameworkTypeText() => ComponentName; // + " (" + ValueType.Name + ")";
        public override String GetInternalTypeText() => ValueType.IsGenericType ? RunTimeUtils.GetGenericTypeName(ValueType) : ValueType.Name;
        public override String GetImplementationType() => ComponentSignature;

        #region Internal
        internal override IEnumerable<ValueContainerBase> Enumerate()
        {
            IEnumerable<Object> Values = Value as IEnumerable<Object>;
            if (Values == null)
            {
                base.Enumerate();
            }

            foreach (Object Object in Values)
            {
                yield return RunTimeUtils.Box(Object, null);
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
            return Value.ToString();
        }
        #endregion

        #region Implicit
        #endregion
    }
}