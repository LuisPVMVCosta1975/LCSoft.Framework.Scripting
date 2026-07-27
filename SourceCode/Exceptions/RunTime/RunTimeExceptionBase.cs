namespace LCSoft.Framework.Scripting.Exceptions.RunTime
{
    using System;

    public abstract class RunTimeExceptionBase : ExceptionBase
    {
        public RunTimeExceptionBase(String Message) : base("Run Time / " + Message)
        {
        }
    }
}