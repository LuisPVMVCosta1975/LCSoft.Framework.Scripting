namespace LCSoft.Framework.Scripting.Exceptions.ParseTime.Unknown
{
    using System;

    public class ConnectorCommandUnknownException : UnknownExceptionBase
    {
        public ConnectorCommandUnknownException(String Name) : base("Connector : " + Name)
        {
        }
    }
}