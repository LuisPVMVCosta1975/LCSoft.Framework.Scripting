namespace LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound
{
    using System;

    public class OperationNotFoundException : NotFoundExceptionBase
    {
        public OperationNotFoundException(String Name) : base("Operation / " + Name)
        {
        }
    }
}