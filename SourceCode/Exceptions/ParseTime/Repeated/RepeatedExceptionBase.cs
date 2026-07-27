namespace LCSoft.Framework.Scripting.Exceptions.ParseTime.Repeated
{
    using System;

    public abstract class RepeatedExceptionBase : ParseTimeExceptionBase
    {
        public RepeatedExceptionBase(String Message) : base("Repeated / " + Message)
        {
        }
    }
}