namespace LCSoft.Framework.Scripting.Exceptions.RunTime
{
    using System;

    public class EmptyValueException : RunTimeExceptionBase
    {
        public EmptyValueException(String Name) : base("Empty Value / " + Name)
        {
        }
    }
}