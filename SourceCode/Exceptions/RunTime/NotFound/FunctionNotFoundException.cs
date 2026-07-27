namespace LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound
{
    using System;

    public class FunctionNotFoundException : NotFoundExceptionBase
    {
        public FunctionNotFoundException(String Name) : base("Function / " + Name)
        {
        }
    }
}