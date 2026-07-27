namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;

    public class OnErrorScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "OnError";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static OnErrorScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<ScriptCommandBase> TryCodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(TryCodeBlock) + " [Code Block]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String Token = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(Token, "Catch", BFR.Peek(), ComponentSignature + " / Catch [Token]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String VariableName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(VariableName, BFR.Peek(), ComponentName + " / " + nameof(VariableName) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<ScriptCommandBase> CatchCodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(CatchCodeBlock) + " [Code Block]");

            return new OnErrorScriptCommand(TryCodeBlock, VariableName, CatchCodeBlock);
        }

        internal List<ScriptCommandBase> TryCodeBlock;
        internal String VariableName;
        internal List<ScriptCommandBase> CatchCodeBlock;

        public OnErrorScriptCommand(List<ScriptCommandBase> TryCodeBlock, String VariableName, List<ScriptCommandBase> CatchCodeBlock)
        {
            this.TryCodeBlock = TryCodeBlock;
            this.VariableName = VariableName;
            this.CatchCodeBlock = CatchCodeBlock;
        }

        internal override ExecutionResult RunElement(Context ParentContext, ScriptResources ScriptResources)
        {
            try
            {
                ExecutionResult ElementResult = RunTimeUtils.RunBlock(TryCodeBlock, ParentContext, ScriptResources);

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
            catch (Exception Ex)
            {
                Context LocalContext = ParentContext.EnterChildContext();

                try
                {
                    LocalContext.SetVariable(VariableName, Ex, null);

                    ExecutionResult ElementResult = RunTimeUtils.RunBlock(CatchCodeBlock, LocalContext, ScriptResources);
                    if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Break)
                    {
                        return ElementResult.EndBlock();
                    }
                    if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Fail)
                    {
                        throw;
                    }
                    if (ElementResult.CancelationFlag != ExecutionResult.CancelationMode.None)
                    {
                        return ElementResult;
                    }

                    return ExecutionResult.None();
                }
                finally
                {
                    LocalContext.LeaveContext();
                }
            }
        }
    }
}