namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.ParseTime.Unknown;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class GetVariableScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "GetVariable";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal readonly String VariableName;
        internal readonly IScriptExpression ScriptExpression;

        public override String GetImplementationType() => ComponentSignature;

        public GetVariableScriptExpression(String VariableName)
        {
            this.VariableName = VariableName;
        }
        public GetVariableScriptExpression(IScriptExpression ScriptExpression)
        {
            this.ScriptExpression = ScriptExpression;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            String VariableName;
            if (ScriptExpression != null)
            {
                VariableName = ScriptExpression.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpression)).GetString();
            }
            else
            {
                VariableName = this.VariableName;
            }

            if (ScriptResources.Enums.TryGetValue(VariableName, out EnumScriptRootContent Enum))
            {
                return new EnumValueContainer(VariableName, Enum.Options);
            }

            //TODO: reference object enums from outside
            //if (ScriptResources.Objects.TryGetValue(VariableName, out ObjectScriptRootContent Object))
            //{
            //    return new ScriptResourcesValueContainer(Object.ObjectResources);
            //}

            return Context.GetVariable(VariableName);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            String VariableName;
            if (ScriptExpression != null)
            {
                VariableName = ScriptExpression.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpression)).GetString();
            }
            else
            {
                VariableName = this.VariableName;
            }

            if (ScriptResources.Enums.TryGetValue(VariableName, out EnumScriptRootContent Enum))
            {
                return new EnumValueContainer(VariableName, Enum.Options);
            }

            ValueContainerBase Result = Context.GetVariable(VariableName);
            if (Result != null)
            {
                return Result;
            }

            throw new EmptyValueException(Name);
        }

        public override ScriptCommandBase ToCommand(BookmarkableFileReader BFR, String ParserPath)
        {
            String Connector = ParserUtils.GetConnectorCommand(BFR);
            ParserUtils.AssertToken(Connector, BFR.Peek(), ParserPath + " / [Connector]");

            switch (Connector)
            {
                case ScriptCommandBase.SetComponentName:
                    //if (VariableName != null)
                    //{
                    return SetVariableScriptCommand.Parse(VariableName, BFR);
                //}
                //return SetVariableScriptCommand.Parse(ScriptExpression, BFR);
                case ScriptCommandBase.SetMissingComponentName:
                    return SetMissingVariableScriptCommand.Parse(VariableName, BFR);
                case ScriptCommandBase.SetEmptyComponentName:
                    return SetEmptyVariableScriptCommand.Parse(VariableName, BFR);
                case ScriptCommandBase.SetMissingEmptyComponentName:
                    return SetMissingEmptyVariableScriptCommand.Parse(VariableName, BFR);
                case ScriptCommandBase.SelfIncrementComponentName:
                    return new SelfAddScriptCommand(null, VariableName, null, null);
                case ScriptCommandBase.SelfDecrementComponentName:
                    return new SelfSubtractScriptCommand(null, VariableName, null, null);
            }

            throw new ConnectorCommandUnknownException(Connector);
        }
    }
}