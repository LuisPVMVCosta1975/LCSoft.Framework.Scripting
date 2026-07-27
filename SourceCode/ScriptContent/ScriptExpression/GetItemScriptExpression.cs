namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.ParseTime.Unknown;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class GetItemScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "GetItem";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static GetItemScriptExpression Parse(IScriptExpression Expression, BookmarkableFileReader BFR)
        {
            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            //ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature);

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Key = ParserUtils.ParseExpression(BFR, ComponentSignature);
            ParserUtils.AssertExpression(Key, BFR.Peek(), ComponentSignature + " / " + nameof(Key) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new GetItemScriptExpression(Expression, Key);
        }

        internal IScriptExpression Source;
        internal IScriptExpression Key;

        public override String GetImplementationType() => ComponentSignature;

        public GetItemScriptExpression(IScriptExpression Source, IScriptExpression Key)
        {
            this.Source = Source;
            this.Key = Key;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase SourceValueContainer = Source.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Source));
            ValueContainerBase KeyValueContainer = Key.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Key));
            return SourceValueContainer.GetItem(KeyValueContainer);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            ValueContainerBase SourceValueContainer = Source.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Source));
            ValueContainerBase KeyValueContainer = Key.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Key));

            ValueContainerBase Result = SourceValueContainer.GetItem(KeyValueContainer);
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
                    return SetItemScriptCommand.Parse(Source, Key, BFR);
                case ScriptCommandBase.SelfIncrementComponentName:
                    return new SelfAddScriptCommand(Source, null, Key, null);
                case ScriptCommandBase.SelfDecrementComponentName:
                    return new SelfSubtractScriptCommand(Source, null, Key, null);
            }

            throw new ConnectorCommandUnknownException(Connector);
        }
    }
}