namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.ParseTime.Unknown;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class GetPropertyScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "GetProperty";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal IScriptExpression Source;
        internal String AttributeName;

        public override String GetImplementationType() => ComponentSignature;

        public GetPropertyScriptExpression(IScriptExpression Source, String AttributeName)
        {
            this.Source = Source;
            this.AttributeName = AttributeName;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase Result = Source.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Source));
            return Result.GetProperty(AttributeName);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            ValueContainerBase Result = Source.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Source));

            Result = Result.GetProperty(AttributeName);
            if (Result != null)
            {
                return Result;
            }

            throw new EmptyValueException(Name);
        }

        public override ScriptCommandBase ToCommand(BookmarkableFileReader BFR, String ParserPath)
        {
            String Connector = ParserUtils.GetConnectorCommand(BFR);
            ParserUtils.AssertToken(Connector, BFR.Read(), ParserPath + " / [Connector]");

            switch (Connector)
            {
                case ScriptCommandBase.SetComponentName:
                    return SetPropertyScriptCommand.Parse(Source, AttributeName, BFR);
                case ScriptCommandBase.SelfIncrementComponentName:
                    return new SelfAddScriptCommand(Source, AttributeName, null, null);
                case ScriptCommandBase.SelfDecrementComponentName:
                    return new SelfSubtractScriptCommand(Source, AttributeName, null, null);
            }

            throw new ConnectorCommandUnknownException(Connector);
        }
    }
}