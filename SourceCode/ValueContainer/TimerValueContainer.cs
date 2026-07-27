namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    //using System.Timers;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    internal class TimerValueContainer : ValueContainerBase//, IDisposable
    {
        public const string ComponentName = "Timer";
        public const string ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly Type ValueType;
        private readonly Timer Value;
        private readonly String FunctionName;
        private readonly Context GlobalContext;
        private readonly ScriptResources GlobalScriptResources;

        private Boolean IsRunning;
        private Boolean IsEnabled;

        public TimerValueContainer(String FunctionName, Context GlobalContext, ScriptResources GlobalScriptResources)
        {
            //this.Value = new Timer();
            //this.Value.Elapsed += OnTimer;
            //this.Value.AutoReset = true;
            //this.Value.Enabled = false;
            Value = new Timer(OnTimer);
            ValueType = typeof(Timer);
            this.FunctionName = FunctionName;
            this.GlobalContext = GlobalContext;
            this.GlobalScriptResources = GlobalScriptResources;
        }
        //~TimerValueContainer()
        //{
        //    Dispose(false);
        //}

        public override ValueContainerBase GetFrameworkType() => TimerDataType;
        public override String GetFrameworkTypeText() => ComponentName;
        public override String GetInternalTypeText() => GetFrameworkTypeText();
        public override String GetImplementationType() => ComponentSignature;

        public override ValueContainerBase CallMethod(Context Context, ScriptResources ScriptResources, String FunctionName, List<IScriptExpression> Parameters)
        {
            switch (FunctionName)
            {
                case nameof(Start):
                    if (Parameters != null && Parameters.Count == 1)
                    {
                        Start(Context, ScriptResources, Parameters[0]);
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(Stop):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        Stop();
                        return ValueContainerBase.Empty;
                    }
                    break;
                case nameof(Dispose):
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        Dispose();
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
                case "IsRunning":
                    lock (this)
                    {
                        return (IsRunning ? ValueContainerBase.True : ValueContainerBase.False);
                    }
                case "IsEnabled":
                    lock (this)
                    {
                        return (IsEnabled ? ValueContainerBase.True : ValueContainerBase.False);
                    }
            }

            return base.GetProperty(Name);
        }

        //private void OnTimer(Object Sender, ElapsedEventArgs Args)
        private void OnTimer(Object State)
        {
            lock (this)
            {
                if (IsRunning)
                {
                    return;
                }
                IsRunning = true;
            }

            RunTimeUtils.CallFunction(FunctionName, null, GlobalContext, GlobalScriptResources, GlobalContext, GlobalScriptResources);

            lock (this)
            {
                IsRunning = false;
            }
        }

        #region Methods
        public void Start(Context Context, ScriptResources ScriptResources, IScriptExpression Delay)
        {
            ValueContainerBase DelayValue = Delay.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Start) + " [Method] / " + nameof(Delay) + " [Parameter]");

            //Value.Interval = DelayValue.GetDouble();
            //Value.Enabled = true;

            Int32 Period = DelayValue.GetInt32();
            Value.Change(Period, Period);

            lock (this)
            {
                IsEnabled = true;
            }
        }
        public void Stop()
        {
            //Value.Enabled = false;

            Value.Change(Timeout.Infinite, Timeout.Infinite);

            lock (this)
            {
                IsEnabled = false;
            }
        }
        public void Dispose()
        {
            //Value.Enabled = false;
            //Value.Elapsed -= OnTimer;
            //Value.Close();
            //Value.Dispose();
            Value.Change(Timeout.Infinite, Timeout.Infinite);
            Value.Dispose();
            //Dispose(true);
            //GC.SuppressFinalize(this); // here, we ask the garbage collector _not_ to call the finalizer, as we've already done the necessary clean-up above
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

        internal override string GetString()
        {
            throw new OperationOutOfContextException(GetImplementationType() + ": " + MethodBase.GetCurrentMethod().Name);
        }
        #endregion

        #region Implicit
        #endregion

        //private bool isDisposed = false;
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (isDisposed)
        //        return;

        //    if (disposing)
        //    {
        //        Value.Enabled = false;
        //        Value.Elapsed -= OnTimer;
        //        Value.Close();
        //        Value.Dispose();
        //    }

        //    // free unmanaged resources here, if you need to
        //    // you may also want to explicitly set large private fields to null here. If `disposing` is true here, the garbage collector will not have run yet, but we still know that we don't want to use those fields any more (otherwise, why are we disposing?). If `disposing` is false, the garbage collector will clean those fields anyway, but it's no harm.

        //    isDisposed = true;
        //}
    }
}