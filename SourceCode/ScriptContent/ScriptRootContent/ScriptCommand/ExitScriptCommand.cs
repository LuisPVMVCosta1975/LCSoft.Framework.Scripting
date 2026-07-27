namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.Internal;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class ExitScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "Exit";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static ExitScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            IScriptExpression Value = ParserUtils.ParseExpression(BFR, ComponentSignature + " / " + nameof(Value) + " [Expression]");
            //ParserUtils.AssertExpression(Value, BFR.Peek(), ComponentSignature + " / " + nameof(Value) + " [Expression]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new ExitScriptCommand(Value);
        }

        internal IScriptExpression Value;

        public ExitScriptCommand(IScriptExpression Value)
        {
            this.Value = Value;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            throw new ExitInternalException(Value == null ? ValueContainerBase.Empty : Value.EvaluateElement(Context, ScriptResources));
        }
    }
}