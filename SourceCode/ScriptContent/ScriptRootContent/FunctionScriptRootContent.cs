namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand;

    public class FunctionScriptRootContent : ScriptRootContentBase
    {
        public const String ComponentName = "Function";
        public const String ComponentName1 = "Func";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        public static FunctionScriptRootContent Parse(BookmarkableFileReader BFR)
        {
            return ParserUtils.ParseFunctionContent(null, BFR, ComponentSignature);
        }

        public String Name;
        public List<String> Parameters;
        public List<ScriptCommandBase> ScriptCommands;

        public FunctionScriptRootContent(String Name, List<String> Parameters, List<ScriptCommandBase> ScriptCommands)
        {
            this.Name = Name;
            this.Parameters = Parameters;
            this.ScriptCommands = ScriptCommands;
        }
    }
}