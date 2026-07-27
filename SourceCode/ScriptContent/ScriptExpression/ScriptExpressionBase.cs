namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.ParseTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand;
    using LCSoft.Framework.Scripting.ValueContainer;

    public abstract class ScriptExpressionBase : IScriptExpression
    {
        public const String ComponentType = "Expression";

        public abstract String GetImplementationType();

        public abstract ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources);
        public abstract ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name);

        public virtual ScriptCommandBase ToCommand(BookmarkableFileReader BFR, String ParserPath)
        {
            throw new ExpressionOutOfContextException(GetImplementationType(), ParserPath);
        }

        //internal static ValueContainerBase Evaluate(this IScriptExpression Target, Context Context, ScriptResources ScriptResources, String Name)
        //{
        //    ValueContainerBase Result = Target.EvaluateElement(Context, ScriptResources);
        //    if (Result != null)
        //    {
        //        return Result;
        //    }

        //    throw new EmptyValueException(Name);
        //}
    }
}