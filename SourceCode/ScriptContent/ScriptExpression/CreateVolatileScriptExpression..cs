namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class CreateVolatileScriptExpression : ScriptExpressionBase
    {
        public const string ComponentName = "CreateVolatile";
        public const string ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateVolatileScriptExpression Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Expression = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Expression) + " [Expression]");
            //ParserUtils.AssertExpression(Value, BFR.Peek(), ComponentSignature + " / " + nameof(Value) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new CreateVolatileScriptExpression(Expression);
        }

        internal IScriptExpression Expression;

        public override String GetImplementationType() => ComponentSignature;

        public CreateVolatileScriptExpression(IScriptExpression Expression)
        {
            this.Expression = Expression;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return new VolatileValueContainer(Expression.EvaluateElement(Context, ScriptResources));
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, string Name)
        {
            return new VolatileValueContainer(Expression.EvaluateElement(Context, ScriptResources));
        }
    }
}