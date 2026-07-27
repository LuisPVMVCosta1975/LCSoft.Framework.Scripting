namespace LCSoft.Framework.Scripting.Exceptions.RunTime.Internal
{
    using System;

    public abstract class InternalExceptionBase : RunTimeExceptionBase
    {
        public InternalExceptionBase(String Message) : base("Internal / " + Message)
        {
        }
    }
}