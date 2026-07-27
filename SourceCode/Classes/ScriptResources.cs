namespace LCSoft.Framework.Scripting.Classes
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent;

    public class ScriptResources
    {
        internal Dictionary<String, FunctionScriptRootContent> Functions;
        internal Dictionary<String, TypeScriptRootContent> Types;
        internal Dictionary<String, EnumScriptRootContent> Enums;
        internal Dictionary<String, ObjectScriptRootContent> Objects;

        public ScriptResources()
        {
            Functions = new Dictionary<String, FunctionScriptRootContent>();
            Types = new Dictionary<String, TypeScriptRootContent>();
            Enums = new Dictionary<String, EnumScriptRootContent>();
            Objects = new Dictionary<String, ObjectScriptRootContent>();
        }
        public ScriptResources(Dictionary<String, FunctionScriptRootContent> Functions, Dictionary<String, TypeScriptRootContent> Types)
        {
            this.Functions = Functions;
            this.Types = Types;
            this.Enums = new Dictionary<String, EnumScriptRootContent>();
            this.Objects = new Dictionary<String, ObjectScriptRootContent>();
        }
    }
}