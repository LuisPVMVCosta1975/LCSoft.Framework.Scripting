namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;

    public class WhileScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "While";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static WhileScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Predicate = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Predicate) + " [Expression]");
            //ParserUtils.AssertExpression(Predicate, BFR.Peek(), ComponentSignature + " / " + nameof(Predicate) + " (Condition)");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<ScriptCommandBase> CodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(CodeBlock) + " [Code Block]");

            return new WhileScriptCommand(Predicate, CodeBlock);
        }

        internal IScriptExpression Predicate;
        internal List<ScriptCommandBase> CodeBlock;

        public WhileScriptCommand(IScriptExpression Predicate, List<ScriptCommandBase> CodeBlock)
        {
            this.Predicate = Predicate;
            this.CodeBlock = CodeBlock;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            while (Predicate.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Predicate)).GetBoolean())
            {
                ExecutionResult ElementResult = RunTimeUtils.RunBlock(CodeBlock, Context, ScriptResources);

                if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Break)
                {
                    return ElementResult.EndBlock();
                }
                if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Continue)
                {
                    continue;
                }
                if (ElementResult.CancelationFlag != ExecutionResult.CancelationMode.None)
                {
                    return ElementResult;
                }
            }

            return ExecutionResult.None();
        }
    }
}