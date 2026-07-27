namespace LCSoft.Framework.Scripting.Exceptions.RunTime.Internal
{
    using System;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class ExitInternalException : InternalExceptionBase
    {
        public readonly ValueContainerBase Value;

        public ExitInternalException(ValueContainerBase Value) : base("Exit")
        {
            this.Value = Value;
        }
    }
}