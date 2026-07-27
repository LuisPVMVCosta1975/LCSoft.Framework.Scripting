namespace LCSoft.Framework.Scripting.Classes
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptClause;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent;

    internal static class Parser
    {
        internal static void FromFile(Script Script, String ScriptFileName)
        {
            BookmarkableFileReader BFR = new BookmarkableFileReader(ScriptFileName);

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            while (!BFR.IsEOF())
            {
                Boolean IsClause = ParseDispatchClause(Script, BFR);
                if (IsClause)
                {
                    ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                    continue;
                }

                ScriptRootContentBase RootContent = ParseDispatchRootContent(BFR);
                if (RootContent == null)
                {
                    throw new Exception("null root content");
                }
                Script.AddContent(RootContent);

                ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            }
        }

        private static Boolean ParseDispatchClause(Script Script, BookmarkableFileReader BFR)
        {
            BFR.CreateBookmark();

            String Token = ParserUtils.GetToken(BFR);
            if (Token == "")
            {
                BFR.RestoreBookmark();
                return false;
            }

            switch (Token)
            {
                case IncludeScriptClause.ComponentName:
                    BFR.DiscardBookmark();
                    IncludeScriptClause.Parse(Script, BFR);
                    return true;
            }

            BFR.RestoreBookmark();

            return false;
        }

        private static ScriptRootContentBase ParseDispatchRootContent(BookmarkableFileReader BFR)
        {
            BFR.CreateBookmark();

            String Token = ParserUtils.GetToken(BFR);
            switch (Token)
            {
                case FunctionScriptRootContent.ComponentName:
                case FunctionScriptRootContent.ComponentName1:
                    BFR.DiscardBookmark();
                    return FunctionScriptRootContent.Parse(BFR);
                case TypeScriptRootContent.ComponentName:
                    BFR.DiscardBookmark();
                    return TypeScriptRootContent.Parse(BFR);
                case EnumScriptRootContent.ComponentName:
                    BFR.DiscardBookmark();
                    return EnumScriptRootContent.Parse(BFR);
                case ObjectScriptRootContent.ComponentName:
                case ObjectScriptRootContent.ComponentName1:
                    BFR.DiscardBookmark();
                    return ObjectScriptRootContent.Parse(BFR);
            }

            BFR.RestoreBookmark();

            return ParserUtils.ParseCommand(BFR, "[Script]");
        }
    }
}