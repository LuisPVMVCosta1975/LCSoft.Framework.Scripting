namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand
{
    using System;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;

    public abstract class ScriptCommandBase : ScriptRootContentBase
    {
        public new const String ComponentType = "Command";

        public const String SetComponentName = "=";
        public const String SetMissingComponentName = "?=";
        public const String SetEmptyComponentName = "_=";
        public const String SetMissingEmptyComponentName = "?_=";

        public const String SelfIncrementComponentName = "++";
        public const String SelfDecrementComponentName = "--";
        public const String SelfAddComponentName = "+="; //TODO: implement
        public const String SelfSubtractComponentName = "-="; //TODO: implement
        public const String SelfAndComponentName = "&="; //TODO: implement
        public const String SelfOrComponentName = "|="; //TODO: implement
        public const String SelfXOrComponentName = "^="; //TODO: implement
        public const String SelfXAndComponentName = "!="; //TODO: implement

        internal abstract ExecutionResult RunElement(Context Context, ScriptResources ScriptResources);
    }
}