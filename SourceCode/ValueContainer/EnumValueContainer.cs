namespace LCSoft.Framework.Scripting.ValueContainer
{
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class EnumValueContainer : ValueContainerBase
    {
        public const String ComponentName = "Enum";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal readonly Dictionary<String, LiteralValueContainerBase> Values;
        private readonly String Name;

        internal EnumValueContainer(String Name)
        {
            this.Values = new Dictionary<String, LiteralValueContainerBase>();
            this.Name = Name;
        }
        internal EnumValueContainer(String Name, Dictionary<String, LiteralValueContainerBase> Value)
        {
            this.Values = Value;
            this.Name = Name;
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.EnumDataType;
        public override String GetFrameworkTypeText() => ComponentName; // + " (" + Name + ")";
        public override String GetInternalTypeText() => Name;
        public override String GetImplementationType() => ComponentSignature;

        public override ValueContainerBase GetProperty(String Name)
        {
            if (Values.TryGetValue(Name, out LiteralValueContainerBase Literal))
            {
                return Literal;
            }

            return base.GetProperty(Name);
        }

        #region Internal
        public override Object GetUnspecified()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal override Type GetUnderlyingType()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }

        internal override String GetString()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        #endregion

        #region Implicit
        #endregion
    }
}