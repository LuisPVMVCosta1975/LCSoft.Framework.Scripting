namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class CreateFunctionScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "CreateFunction";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateFunctionScriptExpression Parse(BookmarkableFileReader BFR)
        {
            FunctionScriptRootContent Function = ParserUtils.ParseFunctionContent("Anonymous", BFR, ComponentSignature);

            return new CreateFunctionScriptExpression(Function);
        }

        internal FunctionScriptRootContent Function;

        public override String GetImplementationType() => ComponentSignature;

        public CreateFunctionScriptExpression(FunctionScriptRootContent Function)
        {
            this.Function = Function;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return new FunctionValueContainer(Function, Context.Global, ScriptResources);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            return new FunctionValueContainer(Function, Context.Global, ScriptResources);
        }
    }
}