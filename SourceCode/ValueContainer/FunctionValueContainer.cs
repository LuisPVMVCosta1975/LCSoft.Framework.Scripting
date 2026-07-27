namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent;

    public class FunctionValueContainer : ValueContainerBase
    {
        public const String ComponentName = "Function";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly FunctionScriptRootContent Function;
        private readonly Context GlobalContext;
        private readonly ScriptResources GlobalScriptResources;

        internal FunctionValueContainer(FunctionScriptRootContent Function, Context GlobalContext, ScriptResources GlobalScriptResources)
        {
            this.Function = Function;
            this.GlobalContext = GlobalContext;
            this.GlobalScriptResources = GlobalScriptResources;
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.LambdaDataType;
        public override String GetFrameworkTypeText() => ComponentName;
        public override String GetInternalTypeText() => "...";
        public override String GetImplementationType() => ComponentSignature;

        public override ValueContainerBase CallMethod(Context Context, ScriptResources ScriptResources, String FunctionName, List<IScriptExpression> Parameters)
        {
            switch (FunctionName)
            {
                case nameof(Eval):
                    return Eval(Context, ScriptResources, Parameters);
            }

            return base.CallMethod(Context, ScriptResources, FunctionName, Parameters);
        }

        #region Methods
        public ValueContainerBase Eval(Context Context, ScriptResources ScriptResources, List<IScriptExpression> Parameters)
        {
            ExecutionResult ExecutionResult = RunTimeUtils.CallFunction(Function, Parameters, GlobalContext, GlobalScriptResources, Context, ScriptResources);
            return ExecutionResult.Value;
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