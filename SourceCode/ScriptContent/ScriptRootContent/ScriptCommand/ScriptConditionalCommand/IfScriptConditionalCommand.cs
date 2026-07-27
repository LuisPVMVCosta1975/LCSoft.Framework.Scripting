namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand.ScriptConditionalCommand
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class IfScriptConditionalCommand : ScriptConditionalCommandBase
    {
        public const String ComponentName = "If";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static IfScriptConditionalCommand Parse(BookmarkableFileReader BFR, Boolean IsElse)
        {
            if (!IsElse)
            {
                ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

                ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            }

            IScriptExpression Predicate = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Predicate) + " [Expression]");
            //ParserUtils.AssertExpression(ScriptExpression, BFR.Peek(), ComponentSignature + " / " + nameof(ScriptExpression) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<ScriptCommandBase> TrueCodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(TrueCodeBlock) + " [Code Block]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            BFR.CreateBookmark();
            String Token = ParserUtils.GetToken(BFR);
            if (Token != "Else")
            {
                BFR.RestoreBookmark();
                return new IfScriptConditionalCommand(Predicate, TrueCodeBlock);
            }

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            if (BFR.Peek() == ')')
            {
                BFR.Advance();

                ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                List<ScriptCommandBase> FalseCodeBlock = ParserUtils.GetCodeBlock(BFR, ComponentSignature + " / " + nameof(TrueCodeBlock) + " [Code Block]");


                return new IfScriptConditionalCommand(Predicate, TrueCodeBlock, FalseCodeBlock);
            }

            return new IfScriptConditionalCommand(Predicate, TrueCodeBlock, IfScriptConditionalCommand.Parse(BFR, true));
        }

        internal IScriptExpression Predicate;
        internal List<ScriptCommandBase> TrueClause;
        internal ScriptConditionalCommandBase ConditionalCommandFalseClause;
        internal List<ScriptCommandBase> CommandsFalseClause;

        public IfScriptConditionalCommand(IScriptExpression Predicate, List<ScriptCommandBase> TrueClause, ScriptConditionalCommandBase FalseClause)
        {
            this.Predicate = Predicate;
            this.TrueClause = TrueClause;
            ConditionalCommandFalseClause = FalseClause;
        }
        public IfScriptConditionalCommand(IScriptExpression Predicate, List<ScriptCommandBase> TrueClause, List<ScriptCommandBase> FalseClause)
        {
            this.Predicate = Predicate;
            this.TrueClause = TrueClause;
            CommandsFalseClause = FalseClause;
        }
        public IfScriptConditionalCommand(IScriptExpression Predicate, List<ScriptCommandBase> TrueClause)
        {
            this.Predicate = Predicate;
            this.TrueClause = TrueClause;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            ValueContainerBase ValueContainer = Predicate.EvaluateElement(Context, ScriptResources, ComponentSignature + " / " + nameof(Predicate));

            ExecutionResult ElementResult = Execute(ValueContainer.GetBoolean(), Context, ScriptResources);

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

        private ExecutionResult Execute(Boolean ConditionResult, Context Context, ScriptResources ScriptResources)
        {
            if (ConditionResult)
            {
                return RunTimeUtils.RunBlock(TrueClause, Context, ScriptResources);
            }
            else if (ConditionalCommandFalseClause != null)
            {
                return ConditionalCommandFalseClause.RunElement(Context, ScriptResources);
            }
            else if (CommandsFalseClause != null)
            {
                return RunTimeUtils.RunBlock(CommandsFalseClause, Context, ScriptResources);
            }

            return ExecutionResult.None();
        }
    }
}