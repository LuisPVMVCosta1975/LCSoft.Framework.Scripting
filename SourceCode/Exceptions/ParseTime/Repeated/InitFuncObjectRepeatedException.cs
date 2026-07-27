namespace LCSoft.Framework.Scripting.Exceptions.ParseTime.Repeated
{
    using System;


    public class InitFuncObjectRepeatedException : RepeatedExceptionBase
    {
        public InitFuncObjectRepeatedException(String Name) : base("Object InitFunc / " + Name)
        {
        }
    }
}