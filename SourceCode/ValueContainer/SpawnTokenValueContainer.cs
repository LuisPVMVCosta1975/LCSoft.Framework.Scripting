namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class SpawnTokenValueContainer : ValueContainerBase
    {
        public const String ComponentName = "SpawnToken";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly CancelationTokenValueContainer CancelationToken;

        private Boolean RunningFlag = true;
        public ValueContainerBase Result;

        public SpawnTokenValueContainer(CancelationTokenValueContainer CancelationToken)
        {
            this.CancelationToken = CancelationToken;
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.SpawnTokenDataType;
        public override String GetFrameworkTypeText() => ComponentName;
        public override String GetInternalTypeText() => "...";
        public override String GetImplementationType() => ComponentSignature;

        public override ValueContainerBase CallMethod(Context Context, ScriptResources ScriptResources, String FunctionName, List<IScriptExpression> Parameters)
        {
            switch (FunctionName)
            {
                case nameof(Cancel):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        Cancel();
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(Wait):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        Wait();
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(CancelAndWait):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        CancelAndWait();
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
                case "IsFinished":
                    lock (this)
                    {
                        return (RunningFlag ? ValueContainerBase.False : ValueContainerBase.True);
                    }
                case "IsRunning":
                    lock (this)
                    {
                        return (RunningFlag ? ValueContainerBase.True : ValueContainerBase.False);
                    }
                case "Result":
                    lock (this)
                    {
                        return Result;
                    }
            }

            return base.GetProperty(Name);
        }

        #region Methods
        public void Cancel()
        {
            CancelationToken.Cancel();
        }
        public void Wait()
        {
            while (true)
            {
                lock (this)
                {
                    if (!RunningFlag)
                    {
                        return;
                    }
                }

                Thread.Sleep(50);
            }
        }
        public void CancelAndWait()
        {
            Cancel();
            Wait();
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

        internal void Finish(ExecutionResult Result)
        {
            lock (this)
            {
                RunningFlag = false;
                this.Result = Result.Value;
            }
        }
        #endregion

        #region Implicit
        #endregion
    }
}