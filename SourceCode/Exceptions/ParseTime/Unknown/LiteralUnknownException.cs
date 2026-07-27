namespace LCSoft.Framework.Scripting.Exceptions.ParseTime.Unknown
{
    using System;

    public class LiteralUnknownException : UnknownExceptionBase
    {
        public LiteralUnknownException(String Name) : base("Literal : " + Name)
        {
        }
    }
}