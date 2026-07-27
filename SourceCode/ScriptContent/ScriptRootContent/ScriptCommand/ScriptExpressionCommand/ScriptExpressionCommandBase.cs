namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand.ScriptExpressionCommand
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public abstract class ScriptExpressionCommandBase : ScriptCommandBase, IScriptExpression
    {
        public new const String ComponentType = "Expression Command";

        public ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return RunElement(Context, ScriptResources).Value;
        }
        public ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            ExecutionResult ElementResult = RunElement(Context, ScriptResources);
            if (ElementResult.Value != null)
            {
                return ElementResult.Value;
            }

            throw new EmptyValueException(Name);
        }

        public ScriptCommandBase ToCommand(BookmarkableFileReader BFR, String ParserPath)
        {
            return this;
        }
    }
}