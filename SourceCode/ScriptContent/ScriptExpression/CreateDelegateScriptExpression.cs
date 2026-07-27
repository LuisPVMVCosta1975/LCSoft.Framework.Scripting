namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class CreateDelegateScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "CreateDelegate";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateDelegateScriptExpression Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String FunctionName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(FunctionName, BFR.Peek(), ComponentName + " / " + nameof(FunctionName) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new CreateDelegateScriptExpression(FunctionName);
        }

        internal String FunctionName;

        public override String GetImplementationType() => ComponentSignature;

        public CreateDelegateScriptExpression(String FunctionName)
        {
            this.FunctionName = FunctionName;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return new DelegateValueContainer(FunctionName, Context.Global, ScriptResources);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            return new DelegateValueContainer(FunctionName, Context.Global, ScriptResources);
        }
    }
}