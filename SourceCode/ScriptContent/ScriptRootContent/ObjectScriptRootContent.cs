namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.ParseTime.Repeated;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class ObjectScriptRootContent : ScriptRootContentBase
    {
        public const String ComponentName = "Object";
        public const String ComponentName1 = "Obj";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        public static ObjectScriptRootContent Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String ObjectName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(ObjectName, BFR.Peek(), ComponentSignature + " / " + nameof(ObjectName) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '{', ComponentSignature);

            Dictionary<String, LiteralValueContainerBase> Fields = null;
            Dictionary<String, TypeScriptRootContent> Types = null;
            FunctionScriptRootContent InitFunc = null;
            Dictionary<String, FunctionScriptRootContent> PrivFuncs = null;
            Dictionary<String, FunctionScriptRootContent> PubFuncs = null;

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            while (!ParserUtils.IsTerminatorOrEOF(BFR.Peek(), '}'))
            {
                String Token = ParserUtils.GetToken(BFR);
                switch (Token)
                {
                    case "InitFunc":
                        if (InitFunc != null)
                        {
                            throw new InitFuncObjectRepeatedException(ObjectName);
                        }
                        InitFunc = ParserUtils.ParseFunctionContent("InitFunc", BFR, ComponentSignature + " / InitFunc [Function]");
                        ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                        break;
                    case "PubFunc":
                        if (PubFuncs == null)
                        {
                            PubFuncs = new Dictionary<String, FunctionScriptRootContent>();
                        }
                        if (PrivFuncs == null)
                        {
                            PrivFuncs = new Dictionary<String, FunctionScriptRootContent>();
                        }
                        FunctionScriptRootContent Func = ParserUtils.ParseFunctionContent(null, BFR, ComponentSignature + " / PubFunc [Function]");
                        PrivFuncs.Add(Func.Name, Func);
                        PubFuncs.Add(Func.Name, Func);
                        ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                        break;
                    case "PrivFunc":
                        if (PrivFuncs == null)
                        {
                            PrivFuncs = new Dictionary<String, FunctionScriptRootContent>();
                        }
                        Func = ParserUtils.ParseFunctionContent(null, BFR, ComponentSignature + " / PrivFunc [Function]");
                        PrivFuncs.Add(Func.Name, Func);
                        ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                        break;
                    case "Type":
                        if (Types == null)
                        {
                            Types = new Dictionary<String, TypeScriptRootContent>();
                        }
                        TypeScriptRootContent Type = TypeScriptRootContent.Parse(BFR);
                        Types.Add(Type.Name, Type);
                        ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                        break;
                    default:
                        if (Fields == null)
                        {
                            Fields = new Dictionary<String, LiteralValueContainerBase>();
                        }
                        (String FieldName, LiteralValueContainerBase FieldValue) = ParserUtils.GetInitializedToken(Token, BFR, ComponentSignature + " / Field [Initialized Token]");
                        Fields.Add(FieldName, FieldValue);
                        ParserUtils.IgnoreNonBreakingWhiteSpaces(BFR);
                        ParserUtils.AssertStatmentTerminador(BFR, BFR.Peek(), ComponentSignature + " / Field [Literal]");
                        break;
                }
            }

            ParserUtils.AssertChar(BFR.Read(), '}', ComponentSignature);

            return new ObjectScriptRootContent(ObjectName, Fields, Types, InitFunc, PrivFuncs, PubFuncs);
        }

        public String Name;
        public Dictionary<String, LiteralValueContainerBase> Fields;
        //public Dictionary<String, TypeScriptRootContent> Types;
        public FunctionScriptRootContent InitializationFunction;
        public ScriptResources ObjectResources;
        //public Dictionary<String, FunctionScriptRootContent> PrivateFunctions;
        public Dictionary<String, FunctionScriptRootContent> PublicFunctions;

        public ObjectScriptRootContent(String Name, Dictionary<String, LiteralValueContainerBase> Fields, Dictionary<String, TypeScriptRootContent> Types, FunctionScriptRootContent InitializationFunction, Dictionary<String, FunctionScriptRootContent> PrivateFunctions, Dictionary<String, FunctionScriptRootContent> PublicFunctions)
        {
            this.Name = Name;
            this.Fields = Fields;
            //this.Types = Types;
            this.InitializationFunction = InitializationFunction;
            this.ObjectResources = new ScriptResources(PrivateFunctions, Types);
            //this.PrivateFunctions = PrivateFunctions;
            this.PublicFunctions = PublicFunctions;
        }
    }
}