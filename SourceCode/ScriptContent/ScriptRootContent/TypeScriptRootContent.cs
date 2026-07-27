namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;

    public class TypeScriptRootContent : ScriptRootContentBase
    {
        public const String ComponentName = "Type";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        public static TypeScriptRootContent Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String TypeName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(TypeName, BFR.Peek(), ComponentSignature + " / " + nameof(TypeName) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<String> Properties = ParserUtils.ParseListOfTokens(BFR, ComponentSignature + " / " + nameof(Properties) + " [List Of Identifiers]", "[Identifier]");
            ParserUtils.AssertListOfTokens(Properties, ComponentSignature + " / " + nameof(Properties) + " [List Of Identifiers]");

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new TypeScriptRootContent(TypeName, Properties);
        }

        public String Name;
        public List<String> Properties;

        public TypeScriptRootContent(String Name, List<String> Properties)
        {
            this.Name = Name;
            this.Properties = Properties;
        }
    }
}