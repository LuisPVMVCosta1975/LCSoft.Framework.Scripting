namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;

    public class DoWhileScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "DoWhile";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static DoWhileScriptCommand Parse(BookmarkableFileReader BFR)
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

            return new DoWhileScriptCommand(Predicate, CodeBlock);
        }

        internal IScriptExpression Predicate;
        internal List<ScriptCommandBase> CodeBlock;

        public DoWhileScriptCommand(IScriptExpression Predicate, List<ScriptCommandBase> CodeBlock)
        {
            this.Predicate = Predicate;
            this.CodeBlock = CodeBlock;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            do
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
            while (Predicate.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Predicate)).GetBoolean());

            return ExecutionResult.None();
        }
    }
}