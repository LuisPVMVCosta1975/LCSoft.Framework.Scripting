namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class CreateLambdaScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "CreateLambda";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateLambdaScriptExpression Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Expression = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Expression) + " [Expression]");
            //ParserUtils.AssertExpression(Value, BFR.Peek(), ComponentSignature + " / " + nameof(Value) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new CreateLambdaScriptExpression(Expression);
        }

        internal IScriptExpression Expression;

        public override String GetImplementationType() => ComponentSignature;

        public CreateLambdaScriptExpression(IScriptExpression Expression)
        {
            this.Expression = Expression;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return new LambdaValueContainer(Expression);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            return new LambdaValueContainer(Expression);
        }
    }
}