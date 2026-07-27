namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class RunAlternatedScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "RunAlternated";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static RunAlternatedScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String VariableName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(VariableName, BFR.Peek(), ComponentName + " / " + nameof(VariableName) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<ScriptCommandBase> MainCodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(MainCodeBlock) + " [Code Block]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            BFR.CreateBookmark();
            String Token = ParserUtils.GetToken(BFR);
            if (Token == "Else")
            {
                BFR.DiscardBookmark();

                ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

                ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

                ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                List<ScriptCommandBase> ElseCodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(ElseCodeBlock) + " [Code Block]");

                return new RunAlternatedScriptCommand(VariableName, MainCodeBlock, ElseCodeBlock);
            }
            else
            {
                BFR.RestoreBookmark();
                return new RunAlternatedScriptCommand(VariableName, MainCodeBlock);
            }
        }

        internal String VariableName;
        internal List<ScriptCommandBase> MainCodeBlock;
        internal List<ScriptCommandBase> ElseCodeBlock;

        public RunAlternatedScriptCommand(String VariableName, List<ScriptCommandBase> MainCodeBlock)
        {
            this.VariableName = VariableName;
            this.MainCodeBlock = MainCodeBlock;
        }
        public RunAlternatedScriptCommand(String VariableName, List<ScriptCommandBase> MainCodeBlock, List<ScriptCommandBase> ElseCodeBlock)
        {
            this.VariableName = VariableName;
            this.MainCodeBlock = MainCodeBlock;
            this.ElseCodeBlock = ElseCodeBlock;
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
                return Execute(MainCodeBlock, Context, ScriptResources);
            }
            else
            {
                Context.SetVariable(VariableName, ValueContainerBase.True);
                if (ElseCodeBlock != null)
                {
                    return Execute(ElseCodeBlock, Context, ScriptResources);
                }
            }

            return ExecutionResult.None();
        }

        private ExecutionResult Execute(List<ScriptCommandBase> ScriptCommands, Context Context, ScriptResources ScriptResources)
        {
            ExecutionResult ElementResult = RunTimeUtils.RunBlock(ScriptCommands, Context, ScriptResources);

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