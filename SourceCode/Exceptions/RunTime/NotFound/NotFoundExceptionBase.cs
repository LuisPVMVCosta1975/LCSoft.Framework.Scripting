namespace LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound
{
    using System;

    public abstract class NotFoundExceptionBase : RunTimeExceptionBase
    {
        public NotFoundExceptionBase(String Message) : base("Not Found / " + Message)
        {
        }
    }
}