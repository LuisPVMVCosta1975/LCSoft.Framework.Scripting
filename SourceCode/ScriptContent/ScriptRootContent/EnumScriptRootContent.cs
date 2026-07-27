namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class EnumScriptRootContent : ScriptRootContentBase
    {
        public const String ComponentName = "Enum";
        public const String ComponentSignature = ComponentName + " [" + ComponentType + "]";

        public static readonly EnumScriptRootContent DataTypesEnum = new EnumScriptRootContent("DataTypesEnum");

        static EnumScriptRootContent()
        {
            DataTypesEnum.Options.Add("Boolean", ValueContainerBase.BooleanDataType);
            DataTypesEnum.Options.Add("Char", ValueContainerBase.CharDataType);
            DataTypesEnum.Options.Add("DateTime", ValueContainerBase.DateTimeDataType);
            DataTypesEnum.Options.Add("Double", ValueContainerBase.DoubleDataType);
            DataTypesEnum.Options.Add("Int32", ValueContainerBase.Int32DataType);
            DataTypesEnum.Options.Add("Int64", ValueContainerBase.Int64DataType);
            DataTypesEnum.Options.Add("Single", ValueContainerBase.SingleDataType);
            DataTypesEnum.Options.Add("String", ValueContainerBase.StringDataType);
            DataTypesEnum.Options.Add("TimeSpan", ValueContainerBase.TimeSpanDataType);

            DataTypesEnum.Options.Add("Null", ValueContainerBase.NullDataType);

            DataTypesEnum.Options.Add("AttributeList", ValueContainerBase.AttributeListDataType);
            DataTypesEnum.Options.Add("CancelationToken", ValueContainerBase.CancelationTokenDataType);
            DataTypesEnum.Options.Add("ClassReference", ValueContainerBase.ClassReferenceDataType);
            DataTypesEnum.Options.Add("Delegate", ValueContainerBase.DelegateDataType);
            DataTypesEnum.Options.Add("Function", ValueContainerBase.FunctionDataType);
            DataTypesEnum.Options.Add("Lambda", ValueContainerBase.LambdaDataType);
            DataTypesEnum.Options.Add("Lazy", ValueContainerBase.LazyDataType);
            DataTypesEnum.Options.Add("ObjectReference", ValueContainerBase.ObjectReferenceDataType);
            DataTypesEnum.Options.Add("Object", ValueContainerBase.ObjectDataType);
            DataTypesEnum.Options.Add("Semaphore", ValueContainerBase.SemaphoreDataType);
            DataTypesEnum.Options.Add("SpawnToken", ValueContainerBase.SpawnTokenDataType);
            DataTypesEnum.Options.Add("Stringer", ValueContainerBase.StringerDataType);
            DataTypesEnum.Options.Add("Timer", ValueContainerBase.TimerDataType);
            DataTypesEnum.Options.Add("ValueList", ValueContainerBase.ValueListDataType);
            DataTypesEnum.Options.Add("Volatile", ValueContainerBase.VolatileDataType);

            DataTypesEnum.Options.Add("Enum", ValueContainerBase.EnumDataType);
        }

        public static EnumScriptRootContent Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            String EnumName = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(EnumName, BFR.Peek(), ComponentSignature + " / " + nameof(EnumName) + " [Identifier]");

            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '{', ComponentSignature + " / [Code Block Start]");

            Dictionary<String, LiteralValueContainerBase> Properties = ParserUtils.ParseListOfInitializedTokens(BFR, ComponentSignature + " / Property [Initialized Token]");
            ParserUtils.AssertListOfInitializedTokens(Properties, ComponentSignature);

            ParserUtils.AssertChar(BFR.Read(), '}', ComponentSignature + " / [Code Block End]");

            return new EnumScriptRootContent(EnumName, Properties);
        }

        public String Name;
        public Dictionary<String, LiteralValueContainerBase> Options;

        public EnumScriptRootContent(String Name)
        {
            this.Name = Name;
            Options = new Dictionary<String, LiteralValueContainerBase>();
        }
        public EnumScriptRootContent(String Name, Dictionary<String, LiteralValueContainerBase> Options)
        {
            this.Name = Name;
            this.Options = Options;
        }
    }
}