namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class CreateAssemblyClassScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "CreateAssemblyClass";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateAssemblyClassScriptExpression Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '"', ComponentSignature);
            String AsssemblyFile = ParserUtils.GetString(BFR, ComponentSignature + " / " + nameof(AsssemblyFile) + " [String Literal]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ',', ComponentSignature + " / [Parameter Separator]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '"', ComponentSignature);
            String ClassFullName = ParserUtils.GetString(BFR, ComponentSignature + " / " + nameof(ClassFullName) + " [String Literal]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new CreateAssemblyClassScriptExpression(AsssemblyFile, ClassFullName);
        }

        internal String Assembly;
        internal String Class;

        public override String GetImplementationType() => ComponentSignature;

        public CreateAssemblyClassScriptExpression(String Assembly, String Class)
        {
            this.Assembly = Assembly;
            this.Class = Class;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            return new ClassReferenceValueContainer(System.Reflection.Assembly.LoadFrom(Assembly).GetType(Class));
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            return new ClassReferenceValueContainer(System.Reflection.Assembly.LoadFrom(Assembly).GetType(Class));
        }
    }
}