namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class CreateSemaphoreScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "CreateSemaphore";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateSemaphoreScriptExpression Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new CreateSemaphoreScriptExpression();
        }

        public override String GetImplementationType() => ComponentSignature;

        public CreateSemaphoreScriptExpression()
        {
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return new SemaphoreValueContainer();
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            return new SemaphoreValueContainer();
        }
    }
}