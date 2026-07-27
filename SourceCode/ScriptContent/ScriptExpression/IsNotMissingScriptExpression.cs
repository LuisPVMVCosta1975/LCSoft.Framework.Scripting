namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class IsNotMissingScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "IsNotMissing";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static IsNotMissingScriptExpression Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String VariableName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(VariableName, BFR.Peek(), ComponentName + " / " + nameof(VariableName) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new IsNotMissingScriptExpression(VariableName);
        }

        private readonly String VariableName;

        public override String GetImplementationType() => ComponentSignature;

        public IsNotMissingScriptExpression(String VariableName)
        {
            this.VariableName = VariableName;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return (Context.CheckVariable(VariableName) ? ValueContainerBase.True : ValueContainerBase.False);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            return (Context.CheckVariable(VariableName) ? ValueContainerBase.True : ValueContainerBase.False);
        }
    }
}