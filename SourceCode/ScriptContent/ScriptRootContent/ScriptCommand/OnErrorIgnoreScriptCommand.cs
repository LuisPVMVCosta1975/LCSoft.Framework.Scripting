namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.Internal;

    public class OnErrorIgnoreScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "OnErrorIgnore";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static OnErrorIgnoreScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<ScriptCommandBase> CodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(CodeBlock) + " [Code Block]");

            return new OnErrorIgnoreScriptCommand(CodeBlock);
        }

        internal List<ScriptCommandBase> CodeBlock;

        public OnErrorIgnoreScriptCommand(List<ScriptCommandBase> CodeBlock)
        {
            this.CodeBlock = CodeBlock;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            try
            {
                ExecutionResult ElementResult = RunTimeUtils.RunBlock(CodeBlock, Context, ScriptResources);

                if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Break)
                {
                    ElementResult.EndBlock();
                }
                if (ElementResult.CancelationFlag != ExecutionResult.CancelationMode.None)
                {
                    return ElementResult;
                }

                return ExecutionResult.None();
            }
            catch (ExitInternalException)
            {
                throw;
            }
            catch (Exception)
            {
            }

            return ExecutionResult.None();
        }
    }
}