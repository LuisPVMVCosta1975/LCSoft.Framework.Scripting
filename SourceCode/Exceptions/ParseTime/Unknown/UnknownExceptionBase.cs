namespace LCSoft.Framework.Scripting.Exceptions.ParseTime.Unknown
{
    using System;

    public abstract class UnknownExceptionBase : ParseTimeExceptionBase
    {
        public UnknownExceptionBase(String Message) : base("Unknown / " + Message)
        {
        }
    }
}