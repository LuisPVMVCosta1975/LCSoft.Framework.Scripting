namespace LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext
{
    using System;

    public abstract class OutOfContextExceptionBase : RunTimeExceptionBase
    {
        public OutOfContextExceptionBase(String Message) : base("Out Of Context / " + Message)
        {
        }
    }
}