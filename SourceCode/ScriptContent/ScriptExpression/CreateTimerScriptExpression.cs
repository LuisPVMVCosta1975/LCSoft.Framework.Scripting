namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class CreateTimerScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "CreateTimer";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateTimerScriptExpression Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String FunctionName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(FunctionName, BFR.Peek(), ComponentName + " / " + nameof(FunctionName) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new CreateTimerScriptExpression(FunctionName);
        }

        internal String FunctionName;

        public override String GetImplementationType() => ComponentSignature;

        public CreateTimerScriptExpression(String FunctionName)
        {
            this.FunctionName = FunctionName;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return new TimerValueContainer(FunctionName, Context.Global, ScriptResources);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            return new TimerValueContainer(FunctionName, Context.Global, ScriptResources);
        }
    }
}