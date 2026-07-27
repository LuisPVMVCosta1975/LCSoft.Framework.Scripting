namespace LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound
{
    using System;

    public class TypeNotFoundException : NotFoundExceptionBase
    {
        public TypeNotFoundException(String Name) : base("Type / " + Name)
        {
        }
    }
}