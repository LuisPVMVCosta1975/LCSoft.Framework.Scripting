namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;

    public class LambdaValueContainer : ValueContainerBase
    {
        public const String ComponentName = "Lambda";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        private readonly IScriptExpression Value;

        internal LambdaValueContainer(IScriptExpression Value)
        {
            this.Value = Value;
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
                    if (Parameters == null || Parameters.Count == 0)
                    {
                        return Eval(Context, ScriptResources);
                    }
                    break;
            }

            return base.CallMethod(Context, ScriptResources, FunctionName, Parameters);
        }

        #region Methods
        public ValueContainerBase Eval(Context Context, ScriptResources ScriptResources)
        {
            return Value.EvaluateElement(Context, ScriptResources);
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