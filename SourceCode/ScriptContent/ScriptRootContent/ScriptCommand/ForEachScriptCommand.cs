namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class ForEachScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "ForEach";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static ForEachScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String IteratorName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(IteratorName, BFR.Peek(), ComponentName + " / " + nameof(IteratorName) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ',', ComponentSignature + " / [Parameter Separator]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Collection = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Collection) + " [Expression]");
            //ParserUtils.AssertExpression(Value, BFR.Peek(), ComponentSignature + " / " + nameof(Value) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<ScriptCommandBase> CodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(CodeBlock) + " [Code Block]");

            return new ForEachScriptCommand(IteratorName, Collection, CodeBlock);
        }

        internal String IteratorName;
        internal IScriptExpression Collection;
        internal List<ScriptCommandBase> CodeBlock;

        public ForEachScriptCommand(String IteratorName, IScriptExpression Collection, List<ScriptCommandBase> CodeBlock)
        {
            this.IteratorName = IteratorName;
            this.Collection = Collection;
            this.CodeBlock = CodeBlock;
        }

        internal override ExecutionResult RunElement(Context ParentContext, ScriptResources ScriptResources)
        {
            ValueContainerBase Values = Collection.EvaluateElement(ParentContext, ScriptResources, ComponentSignature + " / " + nameof(Collection));

            Context LocalContext = ParentContext.EnterChildContext();

            try
            {
                foreach (ValueContainerBase IteratorValue in Values.Enumerate())
                {
                    LocalContext.SetVariable(IteratorName, IteratorValue);

                    ExecutionResult ElementResult = RunTimeUtils.RunBlock(CodeBlock, LocalContext, ScriptResources);

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
            finally
            {
                LocalContext.LeaveContext();
            }
        }
    }
}