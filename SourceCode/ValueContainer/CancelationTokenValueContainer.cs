namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class CancelationTokenValueContainer : ValueContainerBase
    {
        public const String ComponentName = "CancelationToken";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private Boolean CancelationFlag = false;

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.CancelationTokenDataType;
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
            }

            return base.CallMethod(Context, ScriptResources, FunctionName, Parameters);
        }
        public override ValueContainerBase GetProperty(String Name)
        {
            switch (Name)
            {
                case "IsToCancel":
                    lock (this)
                    {
                        return (CancelationFlag ? ValueContainerBase.True : ValueContainerBase.False);
                    }

                case "IsToContinue":
                    lock (this)
                    {
                        return (CancelationFlag ? ValueContainerBase.False : ValueContainerBase.True);
                    }
            }

            return base.GetProperty(Name);
        }

        #region Methods
        public void Cancel()
        {
            lock (this)
            {
                CancelationFlag = true;
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