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

    public class CreateTypeScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "CreateType";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateTypeScriptExpression Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            if (BFR.Peek() == ')')
            {
                BFR.Advance();
                return new CreateTypeScriptExpression();
            }

            String TypeName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(TypeName, BFR.Peek(), ComponentSignature + " / " + nameof(TypeName) + " [Identifier]");

            List<IScriptExpression> PropertyValues;
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            if (BFR.Peek() == ',')
            {
                BFR.Advance();

                ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                PropertyValues = ParserUtils.ParseListOfExpressions(BFR, ComponentSignature + " / " + nameof(PropertyValues) + " [List Of Expressions]", ')');
                ParserUtils.AssertListOfExpressions(PropertyValues, ComponentSignature + " / " + nameof(PropertyValues) + " [List Of Expressions]");
            }
            else
            {
                PropertyValues = null;
            }

            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new CreateTypeScriptExpression(TypeName, PropertyValues);
        }

        internal String TypeName;
        internal List<IScriptExpression> PropertyValues;

        public override String GetImplementationType() => ComponentSignature;

        public CreateTypeScriptExpression()
        {
        }
        public CreateTypeScriptExpression(String TypeName)
        {
            this.TypeName = TypeName;
        }
        public CreateTypeScriptExpression(String TypeName, List<IScriptExpression> PropertyValues)
        {
            this.TypeName = TypeName;
            this.PropertyValues = PropertyValues;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            Dictionary<String, ValueContainerBase> Result = new Dictionary<String, ValueContainerBase>();

            if (TypeName != null)
            {
                int Index = 0;
                int PropertyCount = PropertyValues?.Count ?? 0;
                foreach (String Attribute in GetType(ScriptResources.Types, TypeName).Properties)
                {
                    if (Index < PropertyCount)
                    {
                        Result.Add(Attribute, PropertyValues[Index].EvaluateElement(Context, ScriptResources));
                    }
                    else
                    {
                        Result.Add(Attribute, null);
                    }
                    Index++;
                }
            }

            return new AttributeListValueContainer(Result, TypeName != null);
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            Dictionary<String, ValueContainerBase> Result = new Dictionary<String, ValueContainerBase>();

            if (TypeName != null)
            {
                int Index = 0;
                int PropertyCount = PropertyValues?.Count ?? 0;
                foreach (String Attribute in GetType(ScriptResources.Types, TypeName).Properties)
                {
                    if (Index < PropertyCount)
                    {
                        Result.Add(Attribute, PropertyValues[Index].EvaluateElement(Context, ScriptResources));
                    }
                    else
                    {
                        Result.Add(Attribute, null);
                    }
                    Index++;
                }
            }

            return new AttributeListValueContainer(Result, TypeName != null);
        }

        private TypeScriptRootContent GetType(Dictionary<string, TypeScriptRootContent> ScriptTypes, String TypeName)
        {
            if (ScriptTypes.TryGetValue(TypeName, out TypeScriptRootContent Type))
            {
                return Type;
            }

            throw new TypeNotFoundException(TypeName);
        }
    }
}