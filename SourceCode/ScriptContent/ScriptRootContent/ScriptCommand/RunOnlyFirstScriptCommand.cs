namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class RunOnlyFirstScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "RunOnlyFirst";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static RunOnlyFirstScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String VariableName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(VariableName, BFR.Peek(), ComponentName + " / " + nameof(VariableName) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<ScriptCommandBase> CodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(CodeBlock) + " [Code Block]");

            return new RunOnlyFirstScriptCommand(VariableName, CodeBlock);
        }

        internal String VariableName;
        internal List<ScriptCommandBase> CodeBlock;

        public RunOnlyFirstScriptCommand(String VariableName, List<ScriptCommandBase> CodeBlock)
        {
            this.VariableName = VariableName;
            this.CodeBlock = CodeBlock;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase ControlFlag = Context.GetVariable(VariableName); //TODO: tech debt: GetVariableContext
            if (ControlFlag == ValueContainerBase.Empty)
            {
                throw new EmptyValueException(ComponentSignature + " / " + nameof(ControlFlag));
            }

            if (ControlFlag.GetBoolean())
            {
                Context.SetVariable(VariableName, ValueContainerBase.False);
                return Execute(Context, ScriptResources);
            }

            return ExecutionResult.None();
        }

        private ExecutionResult Execute(Context Context, ScriptResources ScriptResources)
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