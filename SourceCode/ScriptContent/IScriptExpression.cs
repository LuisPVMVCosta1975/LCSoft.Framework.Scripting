namespace LCSoft.Framework.Scripting.ScriptContent
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand;
    using LCSoft.Framework.Scripting.ValueContainer;

    public interface IScriptExpression
    {
        ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources);
        ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name);

        ScriptCommandBase ToCommand(BookmarkableFileReader BFR, String ParserPath);
    }
}