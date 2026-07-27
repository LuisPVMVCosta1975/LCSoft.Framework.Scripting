namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;

    public class RepeatScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "Repeat";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static RepeatScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<ScriptCommandBase> CodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(CodeBlock) + " [Code Block]");

            return new RepeatScriptCommand(CodeBlock);
        }

        internal List<ScriptCommandBase> CodeBlock;

        public RepeatScriptCommand(List<ScriptCommandBase> CodeBlock)
        {
            this.CodeBlock = CodeBlock;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            for (; ; )
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
        }
    }
}