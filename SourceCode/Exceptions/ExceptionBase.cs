namespace LCSoft.Framework.Scripting.Exceptions
{
    using System;

    public abstract class ExceptionBase : Exception
    {
        public ExceptionBase(String Message) : base(Message)
        {
        }
    }
}