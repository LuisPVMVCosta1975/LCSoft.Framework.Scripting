namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;

    public class RunScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "Run";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static RunScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<ScriptCommandBase> CodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(CodeBlock) + " [Code Block]");

            return new RunScriptCommand(CodeBlock);
        }

        internal List<ScriptCommandBase> CodeBlock;

        public RunScriptCommand(List<ScriptCommandBase> CodeBlock)
        {
            this.CodeBlock = CodeBlock;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            ExecutionResult ElementResult = RunTimeUtils.RunBlock(CodeBlock, Context, ScriptResources);

            if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Break)
            {
                return ElementResult.EndBlock();
            }
            if (ElementResult.CancelationFlag != ExecutionResult.CancelationMode.None)
            {
                return ElementResult;
            }

            return ExecutionResult.None();
        }
    }
}