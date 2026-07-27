namespace LCSoft.Framework.Scripting.Exceptions.ParseTime
{
    using System;

    public class InvalidSintaxeException : ParseTimeExceptionBase
    {
        public InvalidSintaxeException(String Message) : base("Invalid Sintaxe / " + Message)
        {
        }
    }
}