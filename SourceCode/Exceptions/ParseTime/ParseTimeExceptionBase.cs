namespace LCSoft.Framework.Scripting.Exceptions.ParseTime
{
    using System;

    public abstract class ParseTimeExceptionBase : ExceptionBase
    {
        public ParseTimeExceptionBase(String Message) : base("Parse Time / " + Message)
        {
        }
    }
}