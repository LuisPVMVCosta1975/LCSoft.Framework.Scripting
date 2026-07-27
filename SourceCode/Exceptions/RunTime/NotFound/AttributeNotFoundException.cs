namespace LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound
{
    using System;

    public class AttributeNotFoundException : NotFoundExceptionBase
    {
        public AttributeNotFoundException(String Name) : base("Property / " + Name)
        {
        }
    }
}