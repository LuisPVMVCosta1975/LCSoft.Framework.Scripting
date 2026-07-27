namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class IIfScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "IIf";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static IIfScriptExpression Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression ScriptExpression = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(ScriptExpression) + " [Expression]");
            //ParserUtils.AssertExpression(Value, BFR.Peek(), ComponentSignature + " / " + nameof(Value) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ',', ComponentSignature + " / [Parameter Separator]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression ExpressionTrue = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(ExpressionTrue) + " [Expression]");
            //ParserUtils.AssertExpression(Value, BFR.Peek(), ComponentSignature + " / " + nameof(Value) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ',', ComponentSignature + " / [Parameter Separator]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression ExpressionFalse = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(ExpressionTrue) + " [Expression]");
            //ParserUtils.AssertExpression(Value, BFR.Peek(), ComponentSignature + " / " + nameof(Value) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new IIfScriptExpression(ScriptExpression, ExpressionTrue, ExpressionFalse);
        }

        internal IScriptExpression ScriptExpression;
        internal IScriptExpression ExpressionTrue;
        internal IScriptExpression ExpressionFalse;

        public override String GetImplementationType() => ComponentSignature;

        public IIfScriptExpression(IScriptExpression Condition, IScriptExpression ExpressionTrue, IScriptExpression ExpressionFalse)
        {
            ScriptExpression = Condition;
            this.ExpressionTrue = ExpressionTrue;
            this.ExpressionFalse = ExpressionFalse;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            if (ScriptExpression.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpression)).GetBoolean())
            {
                return ExpressionTrue.EvaluateElement(Context, ScriptResources);
            }

            return ExpressionFalse.EvaluateElement(Context, ScriptResources);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            ValueContainerBase Result;
            if (ScriptExpression.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(ScriptExpression)).GetBoolean())
            {
                Result = ExpressionTrue.EvaluateElement(Context, ScriptResources);
            }
            else
            {
                Result = ExpressionFalse.EvaluateElement(Context, ScriptResources);
            }
            if (Result != null)
            {
                return Result;
            }

            throw new EmptyValueException(Name);
        }
    }
}