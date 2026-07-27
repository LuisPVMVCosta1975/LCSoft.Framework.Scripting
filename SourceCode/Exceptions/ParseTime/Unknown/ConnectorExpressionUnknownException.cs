namespace LCSoft.Framework.Scripting.Exceptions.ParseTime.Unknown
{
    using System;

    public class ConnectorExpressionUnknownException : UnknownExceptionBase
    {
        public ConnectorExpressionUnknownException(String Name) : base("Connector : " + Name)
        {
        }
    }
}