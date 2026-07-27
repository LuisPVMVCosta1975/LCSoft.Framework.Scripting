namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class CreateObjectScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "CreateObject";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateObjectScriptExpression Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            String ObjectName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(ObjectName, BFR.Peek(), ComponentSignature + " / " + nameof(ObjectName) + " [Identifier]");

            List<IScriptExpression> Parameters;
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            if (BFR.Peek() == ',')
            {
                BFR.Advance();

                ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                Parameters = ParserUtils.ParseListOfExpressions(BFR, ComponentSignature + " / " + nameof(Parameters) + " [List Of Expressions]", ')');
                ParserUtils.AssertListOfExpressions(Parameters, ComponentSignature + " / " + nameof(Parameters) + " [List Of Expressions]");
            }
            else
            {
                Parameters = null;
            }

            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new CreateObjectScriptExpression(ObjectName, Parameters);
        }

        internal String ObjectName;
        internal List<IScriptExpression> Parameters;

        public override String GetImplementationType() => ComponentSignature;

        public CreateObjectScriptExpression(String ObjectName)
        {
            this.ObjectName = ObjectName;
        }
        public CreateObjectScriptExpression(String ObjectName, List<IScriptExpression> Parameters)
        {
            this.ObjectName = ObjectName;
            this.Parameters = Parameters;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            ObjectScriptRootContent Object = GetObject(ScriptResources.Objects, ObjectName);
            return new ObjectValueContainer(Context, ScriptResources, Object, Parameters);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            ObjectScriptRootContent Object = GetObject(ScriptResources.Objects, ObjectName);
            return new ObjectValueContainer(Context, ScriptResources, Object, Parameters);
        }

        private ObjectScriptRootContent GetObject(Dictionary<string, ObjectScriptRootContent> ScriptObjects, String ObjectName)
        {
            if (ScriptObjects.TryGetValue(ObjectName, out ObjectScriptRootContent Object))
            {
                return Object;
            }

            throw new ObjectNotFoundException(ObjectName);
        }
    }
}