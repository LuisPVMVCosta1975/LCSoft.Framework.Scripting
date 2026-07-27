namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;

    public class ContinueScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "Continue";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static ContinueScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new ContinueScriptCommand();
        }

        public ContinueScriptCommand()
        {
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            return ExecutionResult.Continue;
        }
    }
}