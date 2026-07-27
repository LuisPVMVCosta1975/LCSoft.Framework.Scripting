namespace LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext
{
    using System;

    public class OperationOutOfContextException : OutOfContextExceptionBase
    {
        public OperationOutOfContextException(String Name) : base("Operation / " + Name)
        {
        }
    }
}