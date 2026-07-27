namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;

    public class FailScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "Fail";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static FailScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new FailScriptCommand();
        }

        public FailScriptCommand()
        {
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            return ExecutionResult.Fail;
        }
    }
}