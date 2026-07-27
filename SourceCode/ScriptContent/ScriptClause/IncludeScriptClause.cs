namespace LCSoft.Framework.Scripting.ScriptContent.ScriptClause
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;

    internal class IncludeScriptClause : ScriptClauseBase
    {
        public const String ComponentName = "Include";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        public static void Parse(Script Script, BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreNonBreakingWhiteSpaces(BFR);
            String ScriptFileName = ParserUtils.GetLine(BFR);
            ParserUtils.AssertLine(ScriptFileName, BFR.Peek(), ComponentSignature + " / " + nameof(ScriptFileName) + " [Escaped Text]");

            Parser.FromFile(Script, ScriptFileName);
        }
    }
}