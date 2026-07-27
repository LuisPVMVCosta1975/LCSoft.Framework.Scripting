namespace LCSoft.Framework.Scripting.Exceptions.ParseTime.OutOfContext
{
    using System;

    public class ExpressionOutOfContextException : OutOfContextExceptionBase
    {
        public ExpressionOutOfContextException(String Name, String ParserPath) : base(ParserPath + ": " + Name)
        {
        }
    }
}