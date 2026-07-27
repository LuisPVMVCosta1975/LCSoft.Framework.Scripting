namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class CreateListScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "CreateList";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateListScriptExpression ParseShortcut(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<IScriptExpression> Values = ParserUtils.ParseListOfExpressions(BFR, ComponentSignature + " / " + nameof(Values) + " [List Of Expressions]", ']');

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ']', ComponentSignature + " / [Value List End]");

            return new CreateListScriptExpression(Values, true);
        }
        internal static CreateListScriptExpression ParseComplete(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            List<IScriptExpression> Values = ParserUtils.ParseListOfExpressions(BFR, ComponentSignature + " / " + nameof(Values) + " [List Of Expressions]", ')');

            //ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new CreateListScriptExpression(Values, false);
        }

        internal readonly List<IScriptExpression> Values;
        internal readonly Boolean IsShortcut;

        public override String GetImplementationType() => ComponentSignature;

        public CreateListScriptExpression(List<IScriptExpression> Values, Boolean IsShortcut)
        {
            this.Values = Values;
            this.IsShortcut = IsShortcut;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            List<ValueContainerBase> Result = new List<ValueContainerBase>();

            if (Values != null)
            {
                foreach (IScriptExpression Value in Values)
                {
                    Result.Add(Value.EvaluateElement(Context, ScriptResources));
                }
            }

            return new ValueListValueContainer(Result, IsShortcut);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            List<ValueContainerBase> Result = new List<ValueContainerBase>();

            if (Values != null)
            {
                foreach (IScriptExpression Value in Values)
                {
                    Result.Add(Value.EvaluateElement(Context, ScriptResources));
                }
            }

            return new ValueListValueContainer(Result, IsShortcut);
        }
    }
}