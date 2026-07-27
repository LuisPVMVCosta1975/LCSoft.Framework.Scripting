namespace LCSoft.Framework.Scripting.Exceptions.ParseTime.Unknown
{
    using System;

    public class ExpressionUnknownException : UnknownExceptionBase
    {
        public ExpressionUnknownException(String Name) : base("Expression : " + Name)
        {
        }
    }
}