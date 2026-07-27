namespace LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext
{
    using System;

    public class CommandOutOfContextException : OutOfContextExceptionBase
    {
        public readonly String Name;

        public CommandOutOfContextException(String Name) : base("Command / " + Name)
        {
            this.Name = Name;
        }
    }
}