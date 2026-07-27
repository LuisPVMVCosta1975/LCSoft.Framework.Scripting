namespace LCSoft.Framework.Scripting.ScriptContent.ScriptExpression
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class CreateAssemblyObjectScriptExpression : ScriptExpressionBase
    {
        public const String ComponentName = "CreateAssemblyObject";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static CreateAssemblyObjectScriptExpression Parse(BookmarkableFileReader BFR)
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

            return new CreateAssemblyObjectScriptExpression(AsssemblyFile, ClassFullName, Parameters);
        }

        internal String Assembly;
        internal String Class;
        internal List<IScriptExpression> Parameters;

        public override String GetImplementationType() => ComponentSignature;

        public CreateAssemblyObjectScriptExpression(String Assembly, String Class, List<IScriptExpression> Parameters)
        {
            this.Assembly = Assembly;
            this.Class = Class;
            this.Parameters = Parameters;
        }

        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources)
        {
            Type Type = System.Reflection.Assembly.LoadFrom(Assembly).GetType(Class);
            Object[] InvokeParameters;
            Type[] InvokeTypes;
            if (Parameters != null)
            {
                InvokeParameters = new Object[Parameters.Count];
                InvokeTypes = new Type[Parameters.Count];
                ValueContainerBase Obj;
                for (int i = 0; i < Parameters.Count; i++)
                {
                    Obj = Parameters[i].EvaluateElement(Context, ScriptResources);
                    InvokeParameters[i] = Obj.GetUnspecified();
                    InvokeTypes[i] = Obj.GetUnderlyingType();
                }
            }
            else
            {
                InvokeParameters = null;
                InvokeTypes = Type.EmptyTypes;
            }
            ConstructorInfo CI = Type.GetConstructor(InvokeTypes);
            if (CI == null)
            {
                if ((InvokeTypes?.Length ?? 0) == 0)
                {
                    throw new OperationNotFoundException(Type.Name + ":Constructor");
                }
                else
                {
                    AdvancedStringBuilder ASB = new AdvancedStringBuilder(", ");
                    foreach (Type InvokeType in InvokeTypes)
                    {
                        ASB.Append(InvokeType.Name);
                    }
                    throw new OperationNotFoundException(Type.Name + ":Constructor(" + ASB.ToString() + ")");
                }
            }

            return new ObjectReferenceValueContainer(CI.Invoke(InvokeParameters));
        }
        public override ValueContainerBase EvaluateElement(Context Context, ScriptResources ScriptResources, String Name)
        {
            Type Type = System.Reflection.Assembly.LoadFrom(Assembly).GetType(Class);
            Object[] InvokeParameters;
            Type[] InvokeTypes;
            if (Parameters != null)
            {
                InvokeParameters = new Object[Parameters.Count];
                InvokeTypes = new Type[Parameters.Count];
                ValueContainerBase Obj;
                for (int i = 0; i < Parameters.Count; i++)
                {
                    Obj = Parameters[i].EvaluateElement(Context, ScriptResources);
                    InvokeParameters[i] = Obj.GetUnspecified();
                    InvokeTypes[i] = Obj.GetUnderlyingType();
                }
            }
            else
            {
                InvokeParameters = null;
                InvokeTypes = Type.EmptyTypes;
            }
            ConstructorInfo CI = Type.GetConstructor(InvokeTypes);
            if (CI == null)
            {
                if ((InvokeTypes?.Length ?? 0) == 0)
                {
                    throw new OperationNotFoundException(Type.Name + ":Constructor");
                }
                else
                {
                    AdvancedStringBuilder ASB = new AdvancedStringBuilder(", ");
                    foreach (Type InvokeType in InvokeTypes)
                    {
                        ASB.Append(InvokeType.Name);
                    }
                    throw new OperationNotFoundException(Type.Name + ":Constructor(" + ASB.ToString() + ")");
                }
            }

            return new ObjectReferenceValueContainer(CI.Invoke(InvokeParameters)); //Deveria ser Box?
        }
    }
}