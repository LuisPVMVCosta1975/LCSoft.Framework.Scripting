namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class CreateLazyScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "CreateLazy";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateLazyScriptExpression Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Expression = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Expression) + " [Expression]");
            //ParserUtils.AssertExpression(Value, BFR.Peek(), ComponentSignature + " / " + nameof(Value) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new CreateLazyScriptExpression(Expression);
        }

        internal IScriptExpression Expression;

        public override String GetImplementationType() => ComponentSignature;

        public CreateLazyScriptExpression(IScriptExpression Expression)
        {
            this.Expression = Expression;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return new LazyValueContainer(Expression);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            return new LazyValueContainer(Expression);
        }
    }
}