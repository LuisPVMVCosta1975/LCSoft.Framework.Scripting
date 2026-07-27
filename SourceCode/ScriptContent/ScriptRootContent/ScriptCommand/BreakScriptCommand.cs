namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;

    public class BreakScriptCommand : ScriptCommandBase
    {
        public const String ComponentName = "Break";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static BreakScriptCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            Int64 Count = ParserUtils.GetInt64(BFR, ComponentSignature + " / " + nameof(Count) + " [Int64 Literal]");
            ParserUtils.AssertPositive(Count, ComponentSignature + " / " + nameof(Count) + " [Int64 Literal]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new BreakScriptCommand(Count);
        }

        internal Int64 Count;

        public BreakScriptCommand(Int64 Count)
        {
            this.Count = Count;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            //return (ValueContainerBase.Empty,= ExecutionResult.CancelationMode.Break, Count);
            return ExecutionResult.Break(Count);

        }
    }
}