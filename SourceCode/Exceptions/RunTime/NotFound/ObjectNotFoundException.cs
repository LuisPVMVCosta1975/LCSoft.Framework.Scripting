namespace LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound
{
    using System;

    public class ObjectNotFoundException : NotFoundExceptionBase
    {
        public ObjectNotFoundException(String Name) : base("Object / " + Name)
        {
        }
    }
}