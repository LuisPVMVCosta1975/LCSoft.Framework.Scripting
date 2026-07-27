namespace LCSoft.Framework.Scripting.ValueContainer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class ObjectValueContainer : ValueContainerBase
    {
        public const String ComponentName = "Object";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        public readonly Context Context;
        public readonly ObjectScriptRootContent Template;

        internal ObjectValueContainer(Context Context, ScriptResources ScriptResources, ObjectScriptRootContent Value, List<IScriptExpression> Parameters)
        {
            this.Context = new Context();
            Template = Value;

            if (Value.Fields != null)
            {
                foreach (KeyValuePair<String, LiteralValueContainerBase> Field in Template.Fields)
                {
                    this.Context.SetVariable(Field.Key, Field.Value);
                }
            }

            if (Template.InitializationFunction != null)
            {
                RunTimeUtils.CallFunction(Template.InitializationFunction, Parameters, this.Context, Template.ObjectResources, Context, ScriptResources);
            }
        }

        public override ValueContainerBase GetFrameworkType() => ValueContainerBase.ObjectDataType;
        public override String GetFrameworkTypeText() => ComponentName; // + " (" + Template.Name + ")";
        public override String GetInternalTypeText() => Template.Name;
        public override String GetImplementationType() => ComponentSignature;

        public override ValueContainerBase CallMethod(Context Context, ScriptResources ScriptResources, String FunctionName, List<IScriptExpression> Parameters)
        {
            FunctionScriptRootContent Function;
            if (!Template.PublicFunctions.TryGetValue(FunctionName, out Function))
            {
                return base.CallMethod(Context, ScriptResources, FunctionName, Parameters);
            }

            ExecutionResult ElementResult = RunTimeUtils.CallFunction(Function, Parameters, this.Context, Template.ObjectResources, Context, ScriptResources);
            //TODO: Tech Debt: eval cancelation token (see CallFunc)
            return ElementResult.Value;
        }

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