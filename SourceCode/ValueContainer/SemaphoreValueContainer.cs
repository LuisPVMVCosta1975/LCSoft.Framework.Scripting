namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;

    public class SemaphoreValueContainer : ValueContainerBase
    {
        public const String ComponentName = "Semaphore";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private Boolean IsLocked = false;
        private Int32 ThreadID = 0;

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.SemaphoreDataType;
        public override String GetFrameworkTypeText() => ComponentName;
        public override String GetInternalTypeText() => "...";
        public override String GetImplementationType() => ComponentSignature;

        public override ValueContainerBase CallMethod(Context Context, ScriptResources ScriptResources, String FunctionName, List<IScriptExpression> Parameters)
        {
            switch (FunctionName)
            {
                case nameof(TryCapture):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return TryCapture();
                    }
                    break;
                case nameof(Capture):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        Capture();
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(Release):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        Release();
                        return ValueContainerBase.Empty;
                    }
                    break;
            }

            return base.CallMethod(Context, ScriptResources, FunctionName, Parameters);
        }
        public override ValueContainerBase GetProperty(String Name)
        {
            switch (Name)
            {
                case "IsCaptured":
                    lock (this)
                    {
                        return (IsLocked ? ValueContainerBase.True : ValueContainerBase.False);
                    }
                case "IsReleased":
                    lock (this)
                    {
                        return (IsLocked ? ValueContainerBase.False : ValueContainerBase.True);
                    }
            }

            return base.GetProperty(Name);
        }

        #region Methods
        public ValueContainerBase TryCapture()
        {
            lock (this)
            {
                if (!IsLocked)
                {
                    IsLocked = true;
                    ThreadID = Thread.CurrentThread.ManagedThreadId;
                    return True;
                }
            }

            return False;
        }
        public void Capture()
        {
            while (true)
            {
                lock (this)
                {
                    if (!IsLocked)
                    {
                        IsLocked = true;
                        ThreadID = Thread.CurrentThread.ManagedThreadId;
                        return;
                    }
                }

                Thread.Sleep(50);
            }
        }

        public void Release()
        {
            lock (this)
            {
                if (!IsLocked || Thread.CurrentThread.ManagedThreadId != ThreadID)
                {
                    return;
                }

                IsLocked = false;
                ThreadID = 0;
            }
        }
        #endregion

        #region Internal
        public override Object GetUnspecified()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        internal override Type GetUnderlyingType()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }

        internal override String GetString()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        #endregion

        #region Implicit
        #endregion
    }
}