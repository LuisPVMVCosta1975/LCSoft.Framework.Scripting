namespace LCSoft.Framework.Scripting.Exceptions.ParseTime.Unknown
{
    using System;

    public class EscapeSequenceUnknownException : UnknownExceptionBase
    {
        public EscapeSequenceUnknownException(String Name) : base("Escape Sequence : " + Name)
        {
        }
    }
}