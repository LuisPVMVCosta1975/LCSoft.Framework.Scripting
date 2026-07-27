namespace LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound
{
    using System;

    public class VariableNotFoundException : NotFoundExceptionBase
    {
        public VariableNotFoundException(String Name) : base("Variable / " + Name)
        {
        }
    }
}