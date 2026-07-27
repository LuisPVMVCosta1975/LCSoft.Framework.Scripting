namespace LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer
{
    using System;

    public abstract class LiteralValueContainerBase : ValueContainerBase
    {
        public new const String ComponentType = "Literal Value Container";

        public override String GetInternalTypeText() => "";

        #region Internal
        public override Boolean IsLiteral()
        {
            return true;
        }
        #endregion
    }
}