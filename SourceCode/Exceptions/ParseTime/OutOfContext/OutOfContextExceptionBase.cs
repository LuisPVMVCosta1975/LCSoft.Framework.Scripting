namespace LCSoft.Framework.Scripting.Exceptions.ParseTime.OutOfContext
{
    using System;

    public abstract class OutOfContextExceptionBase : ParseTimeExceptionBase
    {
        public OutOfContextExceptionBase(String Message) : base("Out Of Context / " + Message)
        {
        }
    }
}